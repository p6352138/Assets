using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameEvent;


namespace GameLogical.GameEnitity.AI
{
	public class PetMoveState: CStateBase<CPet>{
		protected static PetMoveState instance;
		public void Release(){
			
		}
		public void Enter(CPet type){
			type.m_petAIData.time = 0.0f ;
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
			List<CCreature> monsterList = EnitityMgr.GetInstance().GetMonsterList();
			if(monsterList.Count > 0){
				CCreature targetMonster = null;
				float tempDis ;
				float tempDisX;
				float tempDisY;
				float dis = float.MaxValue ;
				//find the nestest target on eye shot
				for(int i = 0; i<monsterList.Count; ++i){
					if(monsterList[i].GetRenderObject() == null || monsterList[i].GetEnitityAiState() == AIState.AI_STATE_WEAK)
						continue ;
					tempDisX = monsterList[i].GetRenderObject().transform.position.x - type.GetRenderObject().transform.position.x ;
					tempDis  = Mathf.Abs(tempDisX)  ;
					tempDisY = monsterList[i].GetRenderObject().transform.position.y - type.GetRenderObject().transform.position.y ;
					tempDis  = Mathf.Abs(tempDisX) + Mathf.Abs(tempDisY) * 0.5f ;
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
					type.m_stateMachine.ChangeState(PetPursueState.getInstance());
					return;
				}
			}
		}

		public void Action(CPet type, float time){
			PetPatrolAIData data = type.m_petAIData as PetPatrolAIData ;
			Vector3 destPos = data.patrolPathList[0] ;
			float dis = float.MaxValue ;
			dis = destPos.x - type.GetRenderObject().transform.position.x ;
			//dis  = Vector3.Distance(destPos,type.GetRenderObject().transform.position);
			//Vector3 moveVec = destPos - type.m_object.gameObject.transform.position;
			if(dis > 0){
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT).localRotation = Quaternion.LookRotation(Vector3.back) ;
				//type.GetRenderObject().transform.localRotation = Quaternion.LookRotation(Vector3.forward);
				Vector3 pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition ;
				pos.z = -0.5f ;
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition = pos ;

				pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT).localPosition ;
				pos.z = -0.2f ;
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT).localPosition = pos ;
				
				type.m_object.gameObject.transform.position +=  Vector3.right * type.speed * time;
			}else if(dis < 0){
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT).localRotation = Quaternion.LookRotation(Vector3.forward) ;
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
			return AIState.AI_STATE_STAND ;
		}
		
		public static PetMoveState getInstance(){
			if(instance==null){instance = new PetMoveState();}
			return instance;
		}
	}
}

