using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using GameEvent ;

namespace GameLogical.GameEnitity.AI{
	public class EnemyPetMoveState : CStateBase<CEnemyPet>
	{
		protected static EnemyPetMoveState instance;
		public void Release(){
			
		}
		public void Enter(CEnemyPet type){
			
			type.Play("walk",WrapMode.Loop);
		}
		public void Execute(CEnemyPet type, float time){
			List<CCreature> monsterList = EnitityMgr.GetInstance().GetPetList();
			float dis = float.MaxValue ;
			if(monsterList.Count > 0){
				CCreature targetMonster = null;
				float tempDis ;
				float tempDisX;
				float tempDisY;
				//find the nestest target on eye shot
				for(int i = 0; i<monsterList.Count; ++i){
					if(monsterList[i].GetRenderObject() == null || monsterList[i].GetEnitityAiState() == AIState.AI_STATE_WEAK || monsterList[i].GetEnitityAiState() == AIState.AI_STATE_DEATH)
						continue ;
					tempDisX = monsterList[i].GetRenderObject().transform.position.x - type.GetRenderObject().transform.position.x ;
					tempDisY = monsterList[i].GetRenderObject().transform.position.y - type.GetRenderObject().transform.position.y ;
					tempDis  = Mathf.Abs(tempDisX) ;
					if(tempDis<type.eyeShotArea && tempDis < dis){
						dis = tempDis ;
						targetMonster = monsterList[i] ;
					}
				}
				//find one
				if(targetMonster!=null){
					type.m_targetCreature = targetMonster;       //set the target
					type.m_stateMachine.ChangeState(EnemyPetPursueState.getInstance());
					return;
				}
			}
			PetPatrolAIData data = type.m_petAIData as PetPatrolAIData ;
			Vector3 destPos = type.GetRenderObject().transform.position ;
			destPos.x = 0.0f ;
			dis = Vector3.Distance(destPos,type.GetRenderObject().transform.position);
			Vector3 moveVec = destPos - type.m_object.gameObject.transform.position;
			
			if(moveVec.x > 0){
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT).localRotation = Quaternion.LookRotation(Vector3.forward) ;
				//type.GetRenderObject().transform.localRotation = Quaternion.LookRotation(Vector3.forward);
				Vector3 pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition ;
				pos.z = -0.5f ;
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition = pos ;

				pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT).localPosition ;
				pos.z = -0.2f ;
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT).localPosition = pos ;
			}else if(moveVec.x < 0){
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT).localRotation = Quaternion.LookRotation(Vector3.back) ;
				//type.GetRenderObject().transform.localRotation = Quaternion.LookRotation(Vector3.back);
				
				//type.talkBlink.transform.localRotation = Quaternion.LookRotation(Vector3.back);
				
				Vector3 pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition ;
				pos.z = 0.5f ;
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition = pos ;

				pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT).localPosition ;
				pos.z = 0.2f ;
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT).localPosition = pos ;
			}
			type.m_object.gameObject.transform.position += moveVec.normalized * type.speed * time;
			Vector3 typePos = type.GetRenderObject().transform.position ;
			typePos.z = typePos.y;
			type.GetRenderObject().transform.position = typePos ;
		}
		public void Exit(CEnemyPet type){
			
		}
		public void OnMessage(CEnemyPet type, EventMessageBase data){
	
		}
		public AIState  GetState(){
			return AIState.AI_STATE_MOVE ;
		}
		
		public static EnemyPetMoveState getInstance(){
			if(instance==null){instance = new EnemyPetMoveState();}
			return instance;
		}
	}
}


