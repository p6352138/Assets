using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using GameEvent;
using GameLogical.GameSkill.Skill ;
using GameLogical.GameSkill ;

namespace GameLogical.GameEnitity.AI
{
	public class PlayerAttackState: CStateBase<CPlayer>{
		protected static PlayerAttackState instance;
		
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
		
		public int GetSkill(List<SkillLimit> skillDic)
		{
			int totalWeight = 0;
			for(int i = 0; i < skillDic.Count; i++)
			{
				totalWeight += skillDic[i].pelSkill.GetSkillData().rate;
			}
			
			int random = Random.Range(0,totalWeight);
			int lv = 0;
			for(int i = 0; i < skillDic.Count; i++)
			{
				lv += skillDic[i].pelSkill.GetSkillData().rate;
				if(lv > random)
				{
					return skillDic[i].pelSkill.GetSkillData().id;
				}
			}
			return 0;
		}
		
		public void Enter(CPlayer type){
			type.m_data.curAttackCD = -1.0f ;
			//List<CPetSkill> skills =  type.GetCanUseSkillList();
			//List<SkillLimit> skillDic = type.GetCanUseSkillList2();
			//have skill
			/*if(skillDic.Count > 0)
			{
				int random = Random.Range(0,100);
				CPetSkill oneSkill = null;
				int skillID = GetSkill (skillDic);
				if(skillID != 0)
					oneSkill = type.GetSkillByID(skillID);
				
				if(oneSkill != null && type.IsSkillEffect(oneSkill) && random < 25 )
				{
					type.Play("skill1",WrapMode.Once);
					type.m_curUsingSkill = oneSkill.GetSkillData().id ;
					//talk
					PetMoudleData  petMoudleData = (PetMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID, common.CsvType.CSV_TYPE_PET);
					
					type.TalkLv(petMoudleData.talkLvInSkill, petMoudleData.talkIDInSkill);
				}
				//attack
				else
				{
					type.Play("attack",WrapMode.Once);
					
					//talk
					PetMoudleData  petMoudleData = (PetMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID, common.CsvType.CSV_TYPE_PET);
					
					type.TalkLv(petMoudleData.talkLvInAttack, petMoudleData.talkIDInAttack);
				}
			}*/
			//attack
			//else
			{
				type.Play("skill2",WrapMode.Once);
				
				//talk
				//PetMoudleData  petMoudleData = (PetMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID, common.CsvType.CSV_TYPE_PET);
				
				//type.TalkLv(petMoudleData.talkLvInAttack, petMoudleData.talkIDInAttack);
			}
		}
		
		public void Execute(CPlayer type, float time){
			if(type.m_data.curAttackCD != -1.0f)
			{
				type.m_data.curAttackCD += time ;
				if(type.m_data.curAttackCD >= type.m_data.attackCD){
					if(type.m_targetCreature!=null && type.m_targetCreature.GetRenderObject()!=null){
						if(type.m_targetCreature.GetEnitityAiState() != AIState.AI_STATE_WEAK && type.m_targetCreature.GetEnitityAiState() != AIState.AI_STATE_DEATH){
							float dis = Vector3.Distance(type.m_targetCreature.GetRenderObject().transform.position,type.GetRenderObject().transform.position);
							
							if(dis > type.attackArea && dis < type.eyeShotArea){
								type.m_stateMachine.ChangeState(PlayerPursueState.getInstance());
							}
							else{
								type.m_data.curAttackCD = -1.0f ;
								//List<CPetSkill> skills =  type.GetCanUseSkillList();
								
								//List<SkillLimit> skillDic = type.GetCanUseSkillList2();
								//have skill
								/*if(skillDic.Count > 0){
									//int random = Random.Range(0,100);
									//can use skill
									//		CPetSkill oneSkill = null;
									//		int skillID = GetSkillID ();
									//		if(skillID != 0)
									//			oneSkill = type.GetSkillByIndex(skillID);
									
									int random = Random.Range(0,100);
									CPetSkill oneSkill = null;
									int skillID = GetSkill (skillDic);
									if(skillID != 0)
										oneSkill = type.GetSkillByID(skillID);
									
									if(oneSkill != null && type.IsSkillEffect(oneSkill) && random <= 25)
									{
										type.Play("skill1",WrapMode.Once);
										type.m_curUsingSkill = oneSkill.GetSkillData().id ;
										//talk
										PetMoudleData  petMoudleData = (PetMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID, common.CsvType.CSV_TYPE_PET);
										
										type.TalkLv(petMoudleData.talkLvInSkill, petMoudleData.talkIDInSkill);
									}
									//attack
									else{
										type.Play("attack",WrapMode.Once);
									}
								}*/
								//attack
								//else
								{
									type.Play("skill2",WrapMode.Once);
								}
							}
						}
						else{
							type.m_stateMachine.ChangeState(PlayerMoveState.getInstance());
						}
					}else{
						type.m_stateMachine.ChangeState(PlayerMoveState.getInstance());
					}
				}
			}
		}
		public void Exit(CPlayer type){
			
		}
		public void OnMessage(CPlayer type, EventMessageBase data){
			EnitityAction action = (EnitityAction)data.eventMessageAction;
			if(action == EnitityAction.ENITITY_ACTION_FIGHT){
				
				
				
			}
			//attack finish
			else if(action == EnitityAction.ENITITY_ACTION_FIGHT_FINISH){
				type.m_data.curAttackCD = 0.0f ;
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
					//ResourceMoudleData resData  = common.fileMgr.GetInstance().GetData(petMoudleData.attackArea,common.CsvType.CSV_TYPE_RESOUCE) as ResourceMoudleData ;
					
					//type.m_data.attackArea ;
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
					bulletData.pos = type.GetRenderObject().transform.position ;
					CCreature creatureBullet = EnitityMgr.GetInstance().CreateEnitity(EnitityType.ENITITY_TYPE_BULLET,bulletData);
					//creatureBullet.GetRenderObject().transform.position = type.GetRenderObject().transform.position ;
				}
			}
			else if(action == EnitityAction.ENITYTY_ACTION_SKILL){
				/*if(type.m_curUsingSkill != -1 ){
					if(type.m_targetCreature != null){
						CSkillBass skill = SkillMgr.GetInstance().GetSkill(type.m_curUsingSkill);
						skill.useSkill(type.m_targetCreature.GetId());
					}
				}*/

				//if(type.m_curUsingSkill != -1 ){
					if(type.m_targetCreature != null && type.m_targetCreature.GetRenderObject() != null){
					CSkillBass skill = SkillMgr.GetInstance().GetSkill(type.m_skillAnger);

					List<CCreature> petSelectList = EnitityMgr.GetInstance().GetMonsterList();
					EventMessageEnititySelect selectMessage = new EventMessageEnititySelect();
					selectMessage.id = petSelectList;
					selectMessage.pos = type.GetRenderObject().transform.localPosition;
					EventMgr.GetInstance().OnEventMgr(selectMessage);

					skill.useSkill(selectMessage);
					//type.m_data.curAttackCD = 0.0f ;
					}
				//}

				/*
				PetMoudleData petMoudleData = common.fileMgr.GetInstance().GetData(130111,common.CsvType.CSV_TYPE_PET) as PetMoudleData ;
				//ResourceMoudleData resData  = common.fileMgr.GetInstance().GetData(petMoudleData.attackArea,common.CsvType.CSV_TYPE_RESOUCE) as ResourceMoudleData ;
				
				//type.m_data.attackArea ;
				BulletData bulletData = new BulletData();
				bulletData.scrID = type.GetId();
				if(type.m_targetCreature.GetRenderObject() == null)
				{
					type.m_stateMachine.ChangeState(PlayerStandState.getInstance());
					return;
				}
				bulletData.destID= type.m_targetCreature.GetId() ;
				if( true){//petMoudleData.AttackEffectID != -1){
					bulletData.effectID = petMoudleData.AttackEffectID * 10 + 2;
					bulletData.effectEndID = petMoudleData.AttackEffectID * 10 + 3 ;
				}
				else{
					bulletData.effectID = 430002;
					bulletData.effectEndID = 430003 ;
					common.debug.GetInstance().Error("attack effect id error pet id:" + petMoudleData.ID);
				}
				
//				bulletData.audioPath = fightStartMessage.audioName ; 
				CCreature creatureBullet = EnitityMgr.GetInstance().CreateEnitity(EnitityType.ENITITY_TYPE_BULLET,bulletData);
				creatureBullet.GetRenderObject().transform.position = type.GetRenderObject().transform.position ;
				*/
			}
			else if(action == EnitityAction.ENITITY_ACTION_SKILL_FINISH){
				type.m_data.curAttackCD = 0.0f ;
				type.Play("stand",WrapMode.Loop);
			}
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_STAND ;
		}
		
		public static PlayerAttackState getInstance(){
			if(instance==null){instance = new PlayerAttackState();}
			return instance;
		}
	}
}

