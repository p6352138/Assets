using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameEvent;


namespace GameLogical.GameEnitity.AI
{
	public class PlayerMoveState: CStateBase<CPlayer>{
		protected static PlayerMoveState instance;
		public void Release(){
			
		}
		public void Enter(CPlayer type){
			
			type.Play("stand",WrapMode.Loop);
		}
		public void Execute(CPlayer type, float time){
			List<CCreature> monsterList = EnitityMgr.GetInstance().GetMonsterList();
			float dis = float.MaxValue ;
			if(monsterList.Count > 0){
				CCreature targetMonster = null;
				float tempDis ;
				//find the nestest target on eye shot
				for(int i = 0; i<monsterList.Count; ++i){
					if(monsterList[i].GetEnitityAiState() == AIState.AI_STATE_WEAK)
						continue ;
					tempDis = Vector3.Distance(monsterList[i].GetRenderObject().transform.position,type.GetRenderObject().transform.position) ;
					if(tempDis<type.eyeShotArea && tempDis < dis){
						dis = tempDis ;
						targetMonster = monsterList[i] ;
					}
				}
				//find one
				if(targetMonster!=null){
					//MonsterPursueStateData pursueData = new MonsterPursueStateData();
					//pursueData.targetObjectId = targetPet.GetId() ;
					type.m_targetCreature = targetMonster;       //set the target
					type.m_stateMachine.ChangeState(PlayerPursueState.getInstance());
					return;
				}
			}
			PetPatrolAIData data = type.m_petAIData as PetPatrolAIData ;
			Vector3 destPos = data.patrolPathList[0] ;
			dis = Vector3.Distance(destPos,type.GetRenderObject().transform.position);
			Vector3 moveVec = destPos - type.renderObject.gameObject.transform.position;

			if(moveVec.x > 0){
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE).localRotation = Quaternion.LookRotation(Vector3.back) ;
				//type.GetRenderObject().transform.localRotation = Quaternion.LookRotation(Vector3.forward);
				Vector3 pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition ;
				pos.z = -0.5f ;
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition = pos ;
			}else if(moveVec.x < 0){
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE).localRotation = Quaternion.LookRotation(Vector3.forward) ;
				//type.GetRenderObject().transform.localRotation = Quaternion.LookRotation(Vector3.back);
				
				//type.talkBlink.transform.localRotation = Quaternion.LookRotation(Vector3.back);
				
				Vector3 pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition ;
				pos.z = 0.5f ;
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition = pos ;
			}
			type.renderObject.gameObject.transform.position += moveVec.normalized * type.speed * time;
			Vector3 typePos = type.GetRenderObject().transform.position ;
			typePos.z = typePos.y/80.0f - 1;
			type.GetRenderObject().transform.position = typePos ;
			
			if(dis<0.5f){
				type.m_stateMachine.ChangeState(PlayerStandState.getInstance());

			}
		}
		public void Exit(CPlayer type){
			
		}
		public void OnMessage(CPlayer type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_STAND ;
		}
		
		public static PlayerMoveState getInstance(){
			if(instance==null){instance = new PlayerMoveState();}
			return instance;
		}
	}
}

