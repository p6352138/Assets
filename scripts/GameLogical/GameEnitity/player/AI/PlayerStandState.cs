using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameEvent;

namespace GameLogical.GameEnitity.AI
{
	public class PlayerStandState: CStateBase<CPlayer>{
		protected static PlayerStandState instance;
		public void Release(){
			
		}
		public void Enter(CPlayer type){
			type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE).localRotation = Quaternion.LookRotation(Vector3.back);
			//type.GetRenderObject().transform.localRotation = Quaternion.LookRotation(Vector3.forward);
			
			//type.talkBlink.transform.localRotation = Quaternion.LookRotation(Vector3.forward);
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
					if(monsterList[i] == null || monsterList[i].GetRenderObject() == null)
						continue ;
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
				}
			}
		}
		public void Exit(CPlayer type){
			
		}
		public void OnMessage(CPlayer type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_STAND ;
		}
		
		public static PlayerStandState getInstance(){
			if(instance==null){instance = new PlayerStandState();}
			return instance;
		}
	}
}

