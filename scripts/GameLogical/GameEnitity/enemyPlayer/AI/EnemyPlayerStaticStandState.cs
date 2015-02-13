using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameEvent;
using GameLogical.GameSkill ;
using GameLogical.GameSkill.Skill ;
using common ;


namespace GameLogical.GameEnitity.AI
{
	public class EnemyPlayerStaticStandState: CStateBase<CEnemyPlayer>{
		protected static EnemyPlayerStaticStandState instance;
		public void Release(){
			
		}
		public void Enter(CEnemyPlayer type){
			type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT).localRotation = Quaternion.LookRotation(Vector3.back);
			type.Play("stand",WrapMode.Loop);
		}
		public void Execute(CEnemyPlayer type, float time){
			if(GameDataCenter.GetInstance().pvpPlayerInfo.engry >= 280){
				EnitityMgr.GetInstance().enemyPlayer.Appear();
				GameDataCenter.GetInstance().pvpPlayerInfo.engry -= 280;
			}

			type.m_enemyPlayerAIData.deltaTime += Time.deltaTime ;
			//do
			if(type.m_enemyPlayerAIData.deltaTime >= 1.5f){
				type.m_enemyPlayerAIData.deltaTime = 0.0f ;
				if(type.m_enemyPlayerAIData.remedyTime > 0.0f)
					type.m_enemyPlayerAIData.remedyTime -= 1.5f ;
				int selectSkill = -1 ;
				for(int i = 0; i < type.m_skillList.Count; ++i){
					selectSkill = Random.Range(0,type.m_skillList.Count + 1) ;
					CSkillBass skill = SkillMgr.GetInstance().GetSkill(type.m_skillList[i]);
					SkillMoudleData skillMoudleData = (SkillMoudleData)fileMgr.GetInstance().GetData(skill.GetSkillData().moudleID,CsvType.CSV_TYPE_SKILL);
					if(skill.canUse()){
						//remedy
						if(skillMoudleData.skillType == (int)SkillWorkType.SKILL_WORK_TYPE_REMEDY){
							if(type.m_enemyPlayerAIData.remedyTime > 0.0f){
								continue ;
							}
						}
						type.m_curSelectSkillIndex = i ;
						type.m_stateMachine.SetState(EnemyPlayerStaticSkillAciton.getInstance());
					}
				}
			}
		}




		public void Exit(CEnemyPlayer type){
			
		}
		public void OnMessage(CEnemyPlayer type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_STATIC_STAND ;
		}
		
		public static EnemyPlayerStaticStandState getInstance(){
			if(instance==null){instance = new EnemyPlayerStaticStandState();}
			return instance;
		}
	}
}

