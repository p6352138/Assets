using UnityEngine;
using System.Collections;
using GameEvent;
using System.Collections.Generic;

namespace GameLogical.GameEnitity.AI
{
	public class PetPursueState: CStateBase<CPet>{
		protected static PetPursueState instance;
		public void Release(){
			
		}
		public void Enter(CPet type){
			//talk
			/*PetMoudleData  petMoudleData = (PetMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID, common.CsvType.CSV_TYPE_PET);
			
			type.TalkLv(petMoudleData.talkLvInSpeed, petMoudleData.talkIDInSpeed);*/
			
			type.Play("walk",WrapMode.Loop);
			Think(type);
		}
		public void Execute(CPet type, float time){
			type.m_petAIData.time += time ;
			if(type.m_petAIData.time >= AICommon.AI_THINK_DELTA_TIME){
				
				Think(type);
				type.m_petAIData.time = 0.0f ;
			}
			Action(type,time);
		}

		public void Think(CPet type){
			if(type.m_targetCreature!=null && type.m_targetCreature.GetRenderObject()!=null){
				float disX = type.GetRenderObject().transform.position.x - type.m_targetCreature.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_BE_ATTACK_FORWARD).position.x ;
				float dis  = Mathf.Abs(disX) ;
				disX = type.GetRenderObject().transform.position.x - type.m_targetCreature.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_BE_ATTACK_BACK).position.x ;
				if(dis > Mathf.Abs(disX)){
					dis = Mathf.Abs(disX) ;
				}
				//float disY = type.GetRenderObject().transform.position.y - type.m_targetCreature.GetRenderObject().transform.position.y ;
				//float dis  = Mathf.Abs(disX) + Mathf.Abs(disY);
				//trun aroud
				if(disX < 0){
					type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT).localRotation = Quaternion.LookRotation(Vector3.forward) ;
					Vector3 pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition ;
					pos.z = -0.5f ;
					type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition = pos ;

					pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT).localPosition ;
					pos.z = -0.2f ;
					type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT).localPosition = pos ;
				}
				else{
					type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT).localRotation = Quaternion.LookRotation(Vector3.back) ;
					Vector3 pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition ;
					pos.z = 0.5f ;
					type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition = pos ;

					pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT).localPosition ;
					pos.z = 0.2f ;
					type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT).localPosition = pos ;
				}
				
				//on the attack area .change to attack state
				if(dis < type.attackArea){
					type.m_stateMachine.ChangeState(PetAttackState.getInstance());
				}
				else if(type.m_targetCreature.GetRenderObject().transform.position.x > AICommon.AI_PET_MOVE_MAX_X)
				{
					type.m_stateMachine.ChangeState(PetMoveState.getInstance());
				}
			}else{
				type.m_stateMachine.ChangeState(PetMoveState.getInstance());
			}
		}

		public void Action(CPet type, float time){
			if(type.m_targetCreature!=null && type.m_targetCreature.GetRenderObject()!=null){
				Vector3 disVec = type.m_targetCreature.GetRenderObject().transform.position - type.GetRenderObject().transform.position ;
				if(Mathf.Abs( disVec.x ) < AICommon.AI_ATTACK_X_GAP){
					disVec.x = 0.0f ;
				}
				disVec = disVec.normalized * time * type.speed  + type.GetRenderObject().transform.position;
				disVec.z = disVec.y ;
				type.GetRenderObject().transform.position = disVec ;
			}
		}

		public void Exit(CPet type){
			
		}
		public void OnMessage(CPet type, EventMessageBase data){
			/*if(data.eventMessageModel == EventMessageModel.eEventMessageModel_Enitity){
				if((EnitityAction)data.eventMessageAction == EnitityAction.ENITITY_ACTION_LOCK_PET){
					EventMessageLockPet lockPetMessage = data as EventMessageLockPet ;
					CCreature creaure = EnitityMgr.GetInstance().GetEnitity(lockPetMessage.lockMonsterID) ;
					if(creaure != null){
						type.m_targetCreature = creaure ;
					}
				}
			}*/
		}
		public AIState  GetState(){
			return AIState.AI_STATE_PURSUE ;
		}
		
		public static PetPursueState getInstance(){
			if(instance==null){instance = new PetPursueState();}
			return instance;
		}
	}
}

