using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using GameEvent ;
using common ;
using GameLogical.GameSkill ;
using GameLogical.GameSkill.Skill ;
using GameLogical.GameSkill.Buff  ;
namespace GameLogical.GameEnitity.AI{
	public class EnemyPetAutoUseSkill : CStateBase<CEnemyPet>
	{
		protected static EnemyPetAutoUseSkill instance;
		public void Release(){
			
		}
		public void Enter(CEnemyPet type){
			type.m_petAIData.time = 0.0f ;
		}
		public void Execute(CEnemyPet type, float time){
			type.m_petAIData.time += time ;
			type.m_mainSkillCd += time ;
			if(type.m_petAIData.time >= AICommon.AI_THINK_DELTA_TIME){
				type.m_petAIData.time = 0.0f ;
				Action(type,time);
			}
		}

		void Action(CEnemyPet type, float time){
			//MonsterMoudleData moudleData = fileMgr.GetInstance().GetData(type.m_data.moudleID,CsvType.CSV_TYPE_MONSTER) as MonsterMoudleData;
			if(type.m_mainSkillCd >= type.m_mainSkillFullTime + type.GetEffectData().cutCoolDownTime){
				type.m_mainSkillCd = 0.0f ;
				
				CSkillBass skill = SkillMgr.GetInstance().GetSkill(type.m_mainSkillId);
				List<CCreature> petSelectList = new List<CCreature>();
				SkillMoudleData skillMoudleData = fileMgr.GetInstance().GetData(skill.GetSkillData().moudleID,CsvType.CSV_TYPE_SKILL) as SkillMoudleData;
				EventMessageEnititySelect selectMessage = new EventMessageEnititySelect();
				//range skill
				if(skill.GetSkillData().isRange == true){
					if(skillMoudleData.useObject == 1){
						List<CCreature> tempPetList = EnitityMgr.GetInstance().GetPetList() ;
						for(int i = 0; i<tempPetList.Count; ++i){
							if(tempPetList[i].GetEnitityAiState() != AIState.AI_STATE_DEATH || tempPetList[i].GetEnitityAiState() != AIState.AI_STATE_WEAK){
								petSelectList.Add(tempPetList[i]) ;
							}
						}
					}
					else{
						//remove debuff
						if(skillMoudleData.skillType == (int)SkillWorkType.SKILL_WORK_TYPE_REMOVE_DEBUFF){
							List<CCreature> tempList = EnitityMgr.GetInstance().GetMonsterList();
							for(int i = 0; i<tempList.Count; ++i){
								CEnemyPet enemyPet = tempList[i] as CEnemyPet ;
								for(int j = 0; j<enemyPet.m_buffList.Count; ++j){
									BuffDataBass buffData = CBuffMgr.GetInstance().GetBuffData(enemyPet.m_buffList[i]);
									BuffMoudleData buffMoudleData = fileMgr.GetInstance().GetData(buffData.moudleId,CsvType.CSV_TYPE_BUFF) as BuffMoudleData ;
									if(buffMoudleData.hurtType == 2){
										petSelectList = EnitityMgr.GetInstance().GetMonsterList();
										
										selectMessage.scrId = type.GetId() ;
										selectMessage.id = petSelectList;
										selectMessage.pos = new Vector3(50.0f, 40.0f, 80.0f);
										skill.useSkill(selectMessage);
										return ;
									}
								}
							}
							return ;
						}
						else{
							List<CCreature> tempMonsterList = EnitityMgr.GetInstance().GetMonsterList() ;
							for(int i = 0; i<tempMonsterList.Count; ++i){
								if(tempMonsterList[i].GetEnitityAiState() != AIState.AI_STATE_DEATH || tempMonsterList[i].GetEnitityAiState() != AIState.AI_STATE_WEAK){
									petSelectList.Add(tempMonsterList[i]) ;
								}
							}

						}
					}
				}
				else{
					if(skillMoudleData.useObject == 1){
						if(skillMoudleData.skillType == (int)SkillWorkType.SKILL_WORK_TYPE_CUT_BLOOD || skillMoudleData.skillType == (int)SkillWorkType.SKILL_WORK_TYPE_ATTACK){
							CCreature dest = type.FindLeastBlood(EnitityType.ENITITY_TYPE_PET);
							petSelectList.Add(dest);
						}
						else if(skillMoudleData.skillType == (int)SkillWorkType.SKILL_WORK_TYPE_CUT_BLOOD_PERCENT ){
							CCreature dest = type.FindMostBlood(EnitityType.ENITITY_TYPE_PET);
							petSelectList.Add(dest);
						}
					}
					else if(skillMoudleData.useObject == 2){
						CCreature dest = type.FindSuitCreature(skillMoudleData);
						if(dest != null){
							petSelectList.Add(dest);
						}
						else{
							return ;
						}
					}
					else if(skillMoudleData.useObject == 3){
						petSelectList.Add(type);
					}
				}
				
				
				
				selectMessage.scrId = type.GetId() ;
				selectMessage.id = petSelectList;
				selectMessage.pos = new Vector3(50.0f, 40.0f, 80.0f);
				
				
				skill.useSkill(selectMessage);
			}
		}

		public void Exit(CEnemyPet type){
			
		}
		public void OnMessage(CEnemyPet type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_AUTO_SKILL ;
		}
		public static EnemyPetAutoUseSkill getInstance(){
			if(instance ==null){
				instance = new EnemyPetAutoUseSkill();
			}
			
			return instance;
		}
	}
}

