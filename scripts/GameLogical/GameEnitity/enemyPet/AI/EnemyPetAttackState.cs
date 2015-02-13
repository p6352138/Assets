using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using GameEvent ;
using GameLogical.GameSkill ;
using GameLogical.GameSkill.Skill ;

namespace GameLogical.GameEnitity.AI{
	public class EnemyPetAttackState : CStateBase<CEnemyPet>
	{
		protected static EnemyPetAttackState instance;
		
		public void Release(){
			
		}

		public int GetSkillID ()
		{
			int random = Random.Range(0,100);
			
			SkillLvMoudleData skilllvData = (SkillLvMoudleData)common.fileMgr.GetInstance().GetData(1, 
				common.CsvType.CSV_TYPE_SKILLLV);
				
			int lv = 0;
			int id = 0;
			for(int i = 0; i < skilllvData.skillLvList.Count; i++)
			{
				lv += skilllvData.skillLvList[i];
				if(lv > random)
				{
					id = i + 1;
					return id;
				}
			}
			return 0;
		}
		
		public void Enter(CEnemyPet type){
			type.m_data.curAttackCD = -1.0f ;
			//List<CPetSkill> skills =  type.GetCanUseSkillList();
			List<EnemySkillLimit> skillDic = type.GetCanUseSkillList2();
			//have skill
			if(skillDic.Count > 0)
			{
				CEnemyPetSkill oneSkill = null;
				int skillID = GetSkillID ();
				if(skillID != 0)
					oneSkill = type.GetSkillByIndex(skillID);
				
				if(oneSkill != null && type.IsSkillEffect(oneSkill))
				{
					type.Play("skill2",WrapMode.Once);
					type.m_curUsingSkill = oneSkill.GetSkillData().id ;
					//talk
					//PetMoudleData  petMoudleData = (PetMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID, common.CsvType.CSV_TYPE_PET);
		
					//type.TalkLv(petMoudleData.talkLvInSkill, petMoudleData.talkIDInSkill);
				}
				//attack
				else
				{
					type.Play("attack",WrapMode.Once);
					
					//talk
					//PetMoudleData  petMoudleData = (PetMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID, common.CsvType.CSV_TYPE_PET);
			
					//type.TalkLv(petMoudleData.talkLvInAttack, petMoudleData.talkIDInAttack);
				}
			}
			//attack
			else{
				type.Play("attack",WrapMode.Once);
				
				//talk
				//PetMoudleData  petMoudleData = (PetMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID, common.CsvType.CSV_TYPE_PET);
			
				//type.TalkLv(petMoudleData.talkLvInAttack, petMoudleData.talkIDInAttack);
			}
		}
		
		public void Execute(CEnemyPet type, float time){
			if(type.m_data.curAttackCD != -1.0f){
				
			
				type.m_data.curAttackCD += time ;
				if(type.m_data.curAttackCD >= type.m_data.attackCD){
					if(type.m_targetCreature!=null && type.m_targetCreature.GetRenderObject()!=null){
						if(type.m_targetCreature.GetEnitityAiState() != AIState.AI_STATE_WEAK && type.m_targetCreature.GetEnitityAiState() != AIState.AI_STATE_DEATH){
							float dis = type.m_targetCreature.GetRenderObject().transform.position.x - type.GetRenderObject().transform.position.x ;
							float disY = type.m_targetCreature.GetRenderObject().transform.position.y - type.GetRenderObject().transform.position.y ;
							if(Mathf.Abs(dis) > type.attackArea){
								type.m_stateMachine.ChangeState(EnemyPetMoveState.getInstance());
							}
							else{
								type.m_data.curAttackCD = -1.0f ;
								List<CEnemyPetSkill> skills =  type.GetCanUseSkillList();
								//have skill
								if(skills.Count > 0){
									//int random = Random.Range(0,100);
									//can use skill
									CEnemyPetSkill oneSkill = null;
									int skillID = GetSkillID ();
									if(skillID != 0)
										oneSkill = type.GetSkillByIndex(skillID);
									
									if(oneSkill != null && type.IsSkillEffect(oneSkill))
									{
										type.Play("skill2",WrapMode.Once);
										type.m_curUsingSkill = oneSkill.GetSkillData().id ;
										//talk
										//PetMoudleData  petMoudleData = (PetMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID, common.CsvType.CSV_TYPE_PET);
							
										//type.TalkLv(petMoudleData.talkLvInSkill, petMoudleData.talkIDInSkill);
									}
									//attack
									else{
										type.Play("attack",WrapMode.Once);
									}
								}
								//attack
								else{
									type.Play("attack",WrapMode.Once);
								}
							}
						}
						else{
							type.m_stateMachine.ChangeState(EnemyPetMoveState.getInstance());
						}
					}else{
						type.m_stateMachine.ChangeState(EnemyPetMoveState.getInstance());
					}
				}
			}
		}
		public void Exit(CEnemyPet type){
			
		}
		public void OnMessage(CEnemyPet type, EventMessageBase data){
			EnitityAction action = (EnitityAction)data.eventMessageAction;
			if(action == EnitityAction.ENITITY_ACTION_FIGHT){
				
				
				
			}
			//attack finish
			else if(action == EnitityAction.ENITITY_ACTION_FIGHT_FINISH){

				type.Play("stand",WrapMode.Loop);
				
				//type.Play("attack",WrapMode.Once);
			}
			//attack
			else if(action == EnitityAction.ENITITY_ACTION_FIGHT_SATRT){
				EventMessageFightStart fightStartMessage = (EventMessageFightStart)data ;
				if(type.m_targetCreature==null || type.m_targetCreature.GetRenderObject()==null){
					return;
				}
				
				//near
				if(type.m_data.attackType == AttackType.ATTACK_TYPE_NEAR){
					EventMessageFight message = new EventMessageFight();
					message.scrCreatureId = type.id ;
					message.destCreatureId= type.m_targetCreature.GetId() ;
					message.audioName = fightStartMessage.audioName ;
					message.beEffectName = fightStartMessage.beEffectName ;
					EventMgr.GetInstance().OnEventMgr(message);
				}
				//far
				else if(type.m_data.attackType == AttackType.ATTACK_TYPE_FAR){
					PetMoudleData petMoudleData = common.fileMgr.GetInstance().GetData(type.m_data.moudleID,common.CsvType.CSV_TYPE_PET) as PetMoudleData ;
					BulletData bulletData = new BulletData();
					bulletData.scrID = type.GetId();
					bulletData.destID= type.m_targetCreature.GetId() ;
					if(petMoudleData.AttackEffectID != -1){
						bulletData.effectID = petMoudleData.AttackEffectID * 10 + 2;
						bulletData.effectEndID = petMoudleData.AttackEffectID * 10 + 3 ;
					}
					else{
						bulletData.effectID = 400032;
						bulletData.effectEndID = 400033 ;
						common.debug.GetInstance().Error("attack effect id error pet id:" + petMoudleData.ID);
					}
					bulletData.audioPath = fightStartMessage.audioName ; 

					bulletData.pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ATTACK).position ;

					CCreature creatureBullet = EnitityMgr.GetInstance().CreateEnitity(EnitityType.ENITITY_TYPE_BULLET,bulletData);
					//creatureBullet.GetRenderObject().transform.position = type.GetRenderObject().transform.position ;
				}
				type.m_data.curAttackCD = 0.0f ;
			}
			else if(action == EnitityAction.ENITYTY_ACTION_SKILL){
				if(type.m_curUsingSkill != -1 ){
					if(type.m_targetCreature != null){
						CSkillBass skill = SkillMgr.GetInstance().GetSkill(type.m_curUsingSkill);
						skill.useSkill(type.m_targetCreature.GetId());
					}
				}
			}
			else if(action == EnitityAction.ENITITY_ACTION_SKILL_FINISH){
				type.m_data.curAttackCD = 0.0f ;
				type.Play("stand",WrapMode.Loop);
			}
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_ACTTACK ;
		}
		
		public static EnemyPetAttackState getInstance(){
			if(instance==null){instance = new EnemyPetAttackState();}
			return instance;
		}
	}
}


