using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameEvent;

namespace GameLogical.GameEnitity.AI
{
	public class PetStandState: CStateBase<CPet>{
		protected static PetStandState instance;
		public void Release(){
			
		}
		public void Enter(CPet type){
			type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT).localRotation = Quaternion.LookRotation(Vector3.forward);
			//type.GetRenderObject().transform.localRotation = Quaternion.LookRotation(Vector3.forward);

			Vector3 pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition ;
			pos.z = -0.5f ;
			type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition = pos ;

			//type.talkBlink.transform.localRotation = Quaternion.LookRotation(Vector3.forward);
			type.Play("stand",WrapMode.Loop);
			type.m_petAIData.time = 0.0f ;
		}

		public void Execute(CPet type, float time){
			type.m_petAIData.time += time ;
			if(type.m_petAIData.time >= AICommon.AI_THINK_DELTA_TIME * 0.5f){
				Think(type);
				type.m_petAIData.time = 0.0f ;
			}
		}

		public void Think(CPet type){
			List<CCreature> monsterList = EnitityMgr.GetInstance().GetMonsterList();
			float dis = float.MaxValue ;
			if(monsterList.Count > 0){
				CCreature targetMonster = null;
				float tempDis ;
				float disX ;
				float disY ;
				//find the nestest target on eye shot
				for(int i = 0; i<monsterList.Count; ++i){
					if(monsterList[i] == null || monsterList[i].GetRenderObject() == null)
						continue ;
					if(monsterList[i].GetEnitityAiState() == AIState.AI_STATE_WEAK)
						continue ;
					if(monsterList[i].GetRenderObject().transform.position.x > 100)
						continue ;
					disX = monsterList[i].GetRenderObject().transform.position.x - type.GetRenderObject().transform.position.x ;
					disY = monsterList[i].GetRenderObject().transform.position.y - type.GetRenderObject().transform.position.y ;
					tempDis = Mathf.Abs(disX) + Mathf.Abs(disY) * 0.5f;
					//tempDis = Mathf.Abs(disX) ;
					if(tempDis < dis){
						dis = tempDis ;
						targetMonster = monsterList[i] ;
					}
				}
				
				Vector3 pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition ;
				pos.z = -0.5f ;
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition = pos ;
				//find one
				if(targetMonster!=null){
					//MonsterPursueStateData pursueData = new MonsterPursueStateData();
					//pursueData.targetObjectId = targetPet.GetId() ;
					type.m_targetCreature = targetMonster;       //set the target
					type.m_stateMachine.ChangeState(PetPursueState.getInstance());
				}
			}
		}
		public void Exit(CPet type){
			
		}
		public void OnMessage(CPet type, EventMessageBase data){
			if(data.eventMessageModel == EventMessageModel.eEventMessageModel_Enitity){
				if((EnitityAction)data.eventMessageAction == EnitityAction.ENITITY_ACTION_LOCK_PET){
					EventMessageLockPet lockPetMessage = data as EventMessageLockPet ;
					CCreature creaure = EnitityMgr.GetInstance().GetEnitity(lockPetMessage.lockMonsterID) ;
					if(creaure != null){
						type.m_targetCreature = creaure ;
						type.m_stateMachine.ChangeState(PetPursueState.getInstance());
					}
				}
			}
		}
		public AIState  GetState(){
			return AIState.AI_STATE_STAND ;
		}
		
		public static PetStandState getInstance(){
			if(instance==null){instance = new PetStandState();}
			return instance;
		}
	}
}

