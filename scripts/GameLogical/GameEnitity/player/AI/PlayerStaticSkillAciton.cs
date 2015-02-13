using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameEvent;
using GameLogical.GameSkill;
using GameLogical.GameSkill.Skill;
using common;

namespace GameLogical.GameEnitity.AI
{
	public class PlayerStaticSkillAciton: CStateBase<CPlayer>{
		protected static PlayerStaticSkillAciton instance;
		public void Release(){
			
		}
		public void Enter(CPlayer type){
			type.Play("skill2",WrapMode.Once);
		}
		public void Execute(CPlayer type, float time){

		}
		public void Exit(CPlayer type){
			
		}
		public void OnMessage(CPlayer type, EventMessageBase data){
			EnitityAction action = (EnitityAction)data.eventMessageAction;
			if(action == EnitityAction.ENITITY_ACTION_FIGHT){

			}
			//attack finish
			else if(action == EnitityAction.ENITITY_ACTION_FIGHT_FINISH){
				type.m_stateMachine.SetState(PlayerStaticStandState.getInstance());

			}
			//attack
			else if(action == EnitityAction.ENITITY_ACTION_FIGHT_SATRT){

			}
			else if(action == EnitityAction.ENITYTY_ACTION_SKILL){
					
				if(type.m_curSelectSkillIndex == -1)
					return ;
				
				CSkillBass skill = SkillMgr.GetInstance().GetSkill(type.m_skillList[type.m_curSelectSkillIndex]);
				SkillMoudleData skillMoudleData = (SkillMoudleData)fileMgr.GetInstance().GetData(skill.GetSkillData().moudleID,CsvType.CSV_TYPE_SKILL);
				//if(skillMoudleData.useObject == 1){
				if(skill.canUse()){
					//Play("skill1",WrapMode.Once);
					//m_destID = creature.GetId() ;
					type.m_usingSkillIndex = type.m_curSelectSkillIndex ;
					if(GameLevel.GameLevelMgr.GetInstance().m_levelType == GameLogical.GameLevel.LevelType.LEVEL_TYPE_PVP){
						gameGlobal.g_PvPFightSceneUI.UseSkill(type.m_curSelectSkillIndex) ;
					}
					else{
						gameGlobal.g_fightSceneUI.UseSkill(type.m_curSelectSkillIndex);
					}


					EventMessageEnititySelect eventData = new EventMessageEnititySelect() ;
					eventData.id = type.SelectEnityMsg;
					eventData.pos = type.skillPos;
					skill.useSkill(eventData);
					type.m_destID = -1 ;
					type.m_usingSkillIndex = -1 ;
					type.m_curSelectSkillIndex = -1	;
					if(type.playerState == CPlayer.PlayerAIState.Static)
						type.m_stateMachine.SetState(PlayerStaticSkillAciton.getInstance());
				}

			}
			else if(action == EnitityAction.ENITITY_ACTION_SKILL_FINISH){
				type.m_stateMachine.SetState(PlayerStaticStandState.getInstance());
			}
			else if(action == EnitityAction.ENITYTY_ACTION_SKILL_START)
			{
				GameObject prefabs=gameGlobal.g_rescoureMgr.GetGameObjectResource("shifa-431061");
				GameObject temp=NGUITools.AddChild(type.GetRenderObject().transform.FindChild("root/Ponit/skill").gameObject,prefabs);
				temp.name="TipOneButtom";
				temp.transform.localPosition =new Vector3(0, 0, 0);
				temp.transform.localScale =new Vector3(1, 1, 1);
				temp.transform.FindChild("creature").animation.Play("effect");
			}

		}
		public AIState  GetState(){
			return AIState.AI_STATE_STATIC_SKILL ;
		}
		
		public static PlayerStaticSkillAciton getInstance(){
			if(instance==null){instance = new PlayerStaticSkillAciton();}
			return instance;
		}
	}
}

