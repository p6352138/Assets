using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using GameEvent ;
using GameLogical ;
using GameLogical.GameSkill ;
using GameLogical.GameSkill.Skill ;

namespace GameLogical.GameEnitity.AI{
	public class EnemyPlayerAutoSkillState : CStateBase<CEnemyPlayer>{
		protected static EnemyPlayerAutoSkillState instance;
		public void Release(){
			
		}
		public void Enter(CEnemyPlayer type){
		}
		public void Execute(CEnemyPlayer type, float time){
		}
		public void Exit(CEnemyPlayer type){
			
		}
		public void OnMessage(CEnemyPlayer type, EventMessageBase data){
			//int totalBlood = 0;
			//int curBlood   = 0;
			/*List<CCreature> petList = EnitityMgr.GetInstance().GetPetList();
			for(int i = 0; i < petList.Count; ++i){
				CPet pet =  petList[i]  as CPet;
				//totalBlood += pet.m_data.maxBlood ;
				//curBlood   += pet.m_data.blood    ;
			}
			
			float percent = (float)curBlood / (float)totalBlood ;
			if(percent > 0.2){
				
			}
			else
			{
				
			}*/
			
			/*for(int i = 0; i < GameDataCenter.GetInstance().petFightPackData.playerSkillIds.Count; ++i){
				SkillSimpleDto skillDto = GameDataCenter.GetInstance().GetPlayerSkillData(GameDataCenter.GetInstance().petFightPackData.playerSkillIds[i]);
				CSkillBass skill = SkillMgr.GetInstance().GetSkill(skillDto.id);
				if(skill.isFreezed == false){
					int ran = Random.Range(0,100);
					if(ran < 30){
						if(skill.canUse()){
							SkillDataBass skillData = skill.GetSkillData() ;
							SkillMoudleData skillMoudleData = (SkillMoudleData)common.fileMgr.GetInstance().GetData( skillData.moudleID ,common.CsvType.CSV_TYPE_SKILL);
							if(skillMoudleData.useObject == 1){
								CCreature creature = EnitityMgr.GetInstance().FindDangerousMonster();
								if(creature != null){
									skill.useSkill(creature.GetId());
								}
							}
							else if(skillMoudleData.useObject == 2){
								
							}
							
						}
					}
				}
			}*/
		}
		public AIState  GetState(){
			return AIState.AI_STATE_AUTO_SKILL ;
		}
		public static EnemyPlayerAutoSkillState getInstance(){
			if(instance ==null){
				instance = new EnemyPlayerAutoSkillState();
			}
			
			return instance;
		}
	}
}

