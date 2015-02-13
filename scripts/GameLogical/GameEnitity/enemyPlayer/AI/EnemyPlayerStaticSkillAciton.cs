using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameEvent;
using GameLogical.GameSkill;
using GameLogical.GameSkill.Skill;
using common;

namespace GameLogical.GameEnitity.AI
{
	public class EnemyPlayerStaticSkillAciton: CStateBase<CEnemyPlayer>{
		protected static EnemyPlayerStaticSkillAciton instance;
		public void Release(){
			
		}
		public void Enter(CEnemyPlayer type){
			type.Play("skill2",WrapMode.Once);
		}
		public void Execute(CEnemyPlayer type, float time){

		}
		public void Exit(CEnemyPlayer type){
			
		}
		public void OnMessage(CEnemyPlayer type, EventMessageBase data){
			EnitityAction action = (EnitityAction)data.eventMessageAction;
			if(action == EnitityAction.ENITITY_ACTION_FIGHT){

			}
			//attack finish
			else if(action == EnitityAction.ENITITY_ACTION_FIGHT_FINISH){
				type.m_stateMachine.SetState(EnemyPlayerStaticStandState.getInstance());

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
					type.m_usingSkillIndex = type.m_curSelectSkillIndex ;
					EventMessageEnititySelect eventData = new EventMessageEnititySelect() ;

					//remedy
					if(skillMoudleData.skillType == (int)SkillWorkType.SKILL_WORK_TYPE_REMEDY){
						CCreature creature = type.FindLeastBlood(EnitityType.ENITITY_TYPE_ENEMY_PET);
						if(creature != null){
							List<CCreature> creatureList = new List<CCreature>();
							creatureList.Add(creature);
							eventData.id = creatureList;
							eventData.pos = creature.GetRenderObject().transform.position ;
							skill.useSkill(eventData);
						}

					}
					else if(skillMoudleData.skillType == (int)SkillWorkType.SKILL_WORK_TYPE_ATTACK){
						CCreature creature = null ;
						if(skillMoudleData.useObject == 1){
							if(skillMoudleData.range == 1){
								creature = type.FindLeastBlood(EnitityType.ENITITY_TYPE_PET);
								List<CCreature> creatureList = new List<CCreature>();
								creatureList.Add(creature);
								eventData.id = creatureList ;
							}
							else{
								eventData.id = EnitityMgr.GetInstance().GetPetList();
							}

						}
						else if(skillMoudleData.useObject == 2){
							if(skillMoudleData.range == 1){
								creature = type.FindLeastBlood(EnitityType.ENITITY_TYPE_ENEMY_PET);
								List<CCreature> creatureList = new List<CCreature>();
								creatureList.Add(creature);
								eventData.id = creatureList ;
							}
							else{
								eventData.id = EnitityMgr.GetInstance().GetMonsterList();
							}

						}
						eventData.pos = new Vector3(60.0f,40.0f,0.0f) ;
						skill.useSkill(eventData);
					}
					else{
						CCreature creature = type.FindSuitCreature(skillMoudleData);
						if(creature != null){
							List<CCreature> creatureList = new List<CCreature>();
							creatureList.Add(creature);
							eventData.id = creatureList;
							eventData.pos = creature.GetRenderObject().transform.position ;
							skill.useSkill(eventData);
						}
					}
					type.m_destID = -1 ;
					type.m_usingSkillIndex = -1 ;
					type.m_curSelectSkillIndex = -1	;
					if(type.playerState == CEnemyPlayer.EnemyPlayerAIState.Static)
						type.m_stateMachine.SetState(EnemyPlayerStaticSkillAciton.getInstance());
				}

			}
			else if(action == EnitityAction.ENITITY_ACTION_SKILL_FINISH){
				type.m_stateMachine.SetState(EnemyPlayerStaticStandState.getInstance());
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
		
		public static EnemyPlayerStaticSkillAciton getInstance(){
			if(instance==null){instance = new EnemyPlayerStaticSkillAciton();}
			return instance;
		}
	}
}

