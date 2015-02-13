using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameEvent;
using GameLogical.GameSkill;
using GameLogical.GameSkill.Skill;

namespace GameLogical.GameEnitity.AI
{
	public class PlayerStaticSkillState: CStateBase<CPlayer>{
		protected static PlayerStaticSkillState instance;
		public void Release(){
			
		}
		public void Enter(CPlayer type){
			type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT).localRotation = Quaternion.LookRotation(Vector3.forward);
			//type.GetRenderObject().transform.localRotation = Quaternion.LookRotation(Vector3.forward);
			
			//type.talkBlink.transform.localRotation = Quaternion.LookRotation(Vector3.forward);
			type.Play("skill2",WrapMode.Once);
			type.m_data.curAttackCD = -1.0f ;
			//int temp;
			//temp = 2*GameDataCenter.GetInstance().playerData.professionId + (GameDataCenter.GetInstance().playerData.sex-1);
			//MuscClip.MusicClipMgr.GetInstance().MusicClips("skill_anger"+temp.ToString());
		}
		public void Execute(CPlayer type, float time){
			if(type.m_data.curAttackCD != -1.0f)
			{
				type.m_data.curAttackCD += time ;
				if(type.m_data.curAttackCD >= type.m_data.attackCD){
					type.m_data.curAttackCD = -1.0f ;
					{
						type.Play("skill2",WrapMode.Once);
						//int temp;
						//temp = 2*GameDataCenter.GetInstance().playerData.professionId + (GameDataCenter.GetInstance().playerData.sex-1);
						//MuscClip.MusicClipMgr.GetInstance().MusicClips("skill_anger"+temp.ToString());
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
				//if(type.m_targetCreature != null && type.m_targetCreature.GetRenderObject() != null){
				CSkillBass skill = SkillMgr.GetInstance().GetSkill(type.m_skillAnger);
				
				List<CCreature> petSelectList = EnitityMgr.GetInstance().GetMonsterList();
				EventMessageEnititySelect selectMessage = new EventMessageEnititySelect();
				selectMessage.id = petSelectList;
				selectMessage.pos = type.GetRenderObject().transform.localPosition;
				skill.useSkill(selectMessage);
				//type.m_data.curAttackCD = 0.0f ;

				//type.Play("stand", WrapMode.Loop);
				//}

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
		
		public static PlayerStaticSkillState getInstance(){
			if(instance==null){instance = new PlayerStaticSkillState();}
			return instance;
		}
	}
}

