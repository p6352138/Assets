using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameEvent;

namespace GameLogical.GameEnitity.AI
{
	public class PetPatrolState: CStateBase<CPet>{
		protected static PetPatrolState instance;
		public void Release(){
			
		}
		public void Enter(CPet type){
			//talk
			/*PetMoudleData  petMoudleData = (PetMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID, common.CsvType.CSV_TYPE_PET);
			
			type.TalkLv(petMoudleData.talkLvInSleep, petMoudleData.talkIDInSleep);*/
			
			type.Play("walk",WrapMode.Loop);
			type.m_targetCreature=null;
		}
		public void Execute(CPet type, float time){
			//find enemy
			//had find attack target 
			//CCreature target = null;
			//int targetID;
			//had find move to target 
			/*if(type.GetEyeShotList().Count > 0){
				targetID =  (int)type.GetEyeShotList()[0] ;
				target = EnitityMgr.GetInstance().GetEnitity(targetID); 
				if(target!=null){
					type.m_targetCreature = target;
					type.m_stateMachine.ChangeState(PetPursueState.getInstance());
					return ;
				}
			}
			
			PetPatrolAIData data = type.m_petAIData as PetPatrolAIData ;
			Vector3 destPos = data.patrolPathList[data.destPathIndex] ;
			float dis = Vector3.Distance(destPos,type.GetRenderObject().transform.position);
			Vector3 moveVec = destPos - type.m_object.gameObject.transform.position;
			
			if(moveVec.x > 0){
				type.GetRenderObject().transform.localRotation = Quaternion.LookRotation(Vector3.forward);
			}else if(moveVec.x < 0){
				type.GetRenderObject().transform.localRotation = Quaternion.LookRotation(Vector3.back);
			}
			type.m_object.gameObject.transform.position += moveVec.normalized * type.speed * time;
			if(dis<0.5f){
				if(data.destPathIndex < data.patrolPathList.Length - 1){
					data.destPathIndex ++ ;
				}
				else{
					data.destPathIndex = 0 ;
				}
			}*/
			
		List<CCreature> monsterList = EnitityMgr.GetInstance().GetMonsterList();
			float dis = float.MaxValue ;
			if(monsterList.Count > 0){
				CCreature targetMonster = null;
				float tempDis ;
				//find the nestest target on eye shot
				for(int i = 0; i<monsterList.Count; ++i){
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
					type.m_stateMachine.ChangeState(PetPursueState.getInstance());
					return;
				}
			}
			PetPatrolAIData data = type.m_petAIData as PetPatrolAIData ;
			Vector3 destPos = data.patrolPathList[data.destPathIndex] ;
			dis = Vector3.Distance(destPos,type.GetRenderObject().transform.position);
			Vector3 moveVec = destPos - type.m_object.gameObject.transform.position;
			
			if(moveVec.x > 0){
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT).localRotation = Quaternion.LookRotation(Vector3.forward);
				//type.GetRenderObject().transform.localRotation = Quaternion.LookRotation(Vector3.forward);
			}else if(moveVec.x < 0){
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT).localRotation = Quaternion.LookRotation(Vector3.back);
				//type.GetRenderObject().transform.localRotation = Quaternion.LookRotation(Vector3.back);
			}
			type.m_object.gameObject.transform.position += moveVec.normalized * type.speed * time;
			if(dis<0.5f){
				if(data.destPathIndex < data.patrolPathList.Length - 1){
					data.destPathIndex ++ ;
				}
				else{
					data.destPathIndex = 0 ;
				}
			}
		}
		public void Exit(CPet type){
			
		}
		public void OnMessage(CPet type, EventMessageBase data){
	
		}
		public AIState  GetState(){
			return AIState.AI_STATE_STAND ;
		}
		
		public static PetPatrolState getInstance(){
			if(instance==null){instance = new PetPatrolState();}
			return instance;
		}
	}
}

