using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using GameEvent;
using GameLogical.GameEnitity ;

namespace GameLogical.GameEnitity.AI{
	
	public class PetPvPMoveState : CStateBase<CPet>
	{
		protected static PetPvPMoveState instance;
		public void Release(){
			
		}
		public void Enter(CPet type){
			
			type.Play("walk",WrapMode.Loop);
		}
		public void Execute(CPet type, float time){
			List<CCreature> monsterList = EnitityMgr.GetInstance().GetMonsterList();
			float dis = float.MaxValue ;
			if(monsterList.Count > 0){
				CCreature targetMonster = null;
				float tempDis ;
				//find the nestest target on eye shot
				for(int i = 0; i<monsterList.Count; ++i){
					if(monsterList[i].GetRenderObject() == null || monsterList[i].GetEnitityAiState() == AIState.AI_STATE_WEAK)
						continue ;
					tempDis = Vector3.Distance(monsterList[i].GetRenderObject().transform.position,type.GetRenderObject().transform.position) ;
					if(tempDis<type.eyeShotArea && tempDis < dis){
						dis = tempDis ;
						targetMonster = monsterList[i] ;
					}
				}
				//find one
				if(targetMonster!=null){
					type.m_targetCreature = targetMonster;       //set the target
					type.m_stateMachine.ChangeState(PetPursueState.getInstance());
					return;
				}
			}

			PetPatrolAIData data = type.m_petAIData as PetPatrolAIData ;
			Vector3 destPos = data.patrolPathList[0] ;
			dis = destPos.x - type.GetRenderObject().transform.position.x ;
			//dis  = Vector3.Distance(destPos,type.GetRenderObject().transform.position);
			//Vector3 moveVec = destPos - type.m_object.gameObject.transform.position;
			if(dis > 0){
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT).localRotation = Quaternion.LookRotation(Vector3.forward) ;
				//type.GetRenderObject().transform.localRotation = Quaternion.LookRotation(Vector3.forward);
				Vector3 pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition ;
				pos.z = -0.5f ;
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition = pos ;

				pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT).localPosition ;
				pos.z = -0.2f ;
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT).localPosition = pos ;
				
				type.m_object.gameObject.transform.position +=  Vector3.right * type.speed * time;
			}else if(dis < 0){
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT).localRotation = Quaternion.LookRotation(Vector3.back) ;
				//type.GetRenderObject().transform.localRotation = Quaternion.LookRotation(Vector3.back);
				
				//type.talkBlink.transform.localRotation = Quaternion.LookRotation(Vector3.back);
				
				Vector3 pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition ;
				pos.z = 0.5f ;
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition = pos ;

				pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT).localPosition ;
				pos.z = 0.2f ;
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT).localPosition = pos ;

				type.m_object.gameObject.transform.position -= Vector3.right * type.speed * time;
			}
			
			if(dis<0.5f){
				type.m_stateMachine.ChangeState(PetStandState.getInstance());
			}
		}
		public void Exit(CPet type){
			
		}
		public void OnMessage(CPet type, EventMessageBase data){
	
		}
		public AIState  GetState(){
			return AIState.AI_STATE_MOVE ;
		}
		
		public static PetPvPMoveState getInstance(){
			if(instance==null){instance = new PetPvPMoveState();}
			return instance;
		}
	}
}


