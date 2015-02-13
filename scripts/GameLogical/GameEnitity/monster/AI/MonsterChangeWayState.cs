using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using GameEvent ;
using GameLogical.GameLevel ;
using GameLogical.GameEnitity ;

namespace GameLogical.GameEnitity.AI
{
	public class MonsterChangeWayState: CStateBase<CMonster>{
		protected static MonsterChangeWayState instance;
		public void Release(){
			
		}
		public void Enter(CMonster type){
			type.Play("walk",WrapMode.Loop);
			
			//talk
			/*MonsterMoudleData monstermoudle = (MonsterMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID,
			                                                                                          common.CsvType.CSV_TYPE_MONSTER);
			type.TalkLv(monstermoudle.talkLvInSpeed, monstermoudle.talkIDInSpeed);*/
			
			type.m_monsterAIData.time = 0.0f ;
			List<CCreature> monsterList = EnitityMgr.GetInstance().GetMonsterList() ;

			if(type.m_targetCreature!=null && type.m_targetCreature.GetRenderObject()!=null){
				float distance ;

				for(int i = 0; i<monsterList.Count; ++i){
					distance = GameLevelMgr.GetInstance().m_monsterBrithPointArr[i].y - type.m_targetCreature.GetRenderObject().transform.position.y ;

				}
			}

		}
		
		public void Execute(CMonster type, float time){
			Action(type,time);
		}
		
		public void Think(CMonster type){
			//if the target in attacke
			if(type.m_targetCreature!=null && type.m_targetCreature.GetRenderObject()!=null){
				float disVec = type.m_targetCreature.GetRenderObject().transform.position.x - type.GetRenderObject().transform.position.x ;
				float disY	 = type.m_targetCreature.GetRenderObject().transform.position.y - type.GetRenderObject().transform.position.y ;

				//on the right way
				if( Mathf.Abs(disY) < AICommon.AI_ATTACK_Y_GAP )
				{
					//on the attack area, attack
					if( Mathf.Abs(disVec) < type.attackArea){
						type.m_stateMachine.ChangeState(MonsterAttackState.getInstance());
					}
					//ont on the attack area , go to pursue
					else{
						type.m_stateMachine.ChangeState(MonsterPursueState.getInstance());
					}
				}
				//change way
				else{
					float distance ;
					for(int i = 0; i<AICommon.AI_MONSTER_WAY_NUM; ++i){
						distance = GameLevelMgr.GetInstance().m_monsterBrithPointArr[i].y - type.m_targetCreature.GetRenderObject().transform.position.y ;
						if(Mathf.Abs(distance) < AICommon.AI_ATTACK_Y_GAP){
							type.m_monsterAIData.wayIndex = i ;
						}
					}
				}
			}
			else{
				type.m_stateMachine.ChangeState(MonsterMoveState.getInstance());
			}
		}
		
		public void Action(CMonster type,float time){
			float disY	 =  GameLevelMgr.GetInstance().m_monsterBrithPointArr[type.m_monsterAIData.wayIndex].y - type.GetRenderObject().transform.position.y ;
			//move y
			if(Mathf.Abs(disY) > 1.0f){
				//move up
				if(disY > 0){
					type.GetRenderObject().transform.position += Vector3.up * time * type.monsterSpeed ;
					Vector3 pos = type.GetRenderObject().transform.position ;
					pos.z = pos.y ;
					type.GetRenderObject().transform.position = pos ;
				}
				//move down
				else{
					type.GetRenderObject().transform.position += Vector3.down * time * type.monsterSpeed ;
					Vector3 pos = type.GetRenderObject().transform.position ;
					pos.z = pos.y ;
					type.GetRenderObject().transform.position = pos ;
				}
			}
			else{
				Vector3 pos = type.GetRenderObject().transform.position ;
				pos.y = GameLevelMgr.GetInstance().m_monsterBrithPointArr[type.m_monsterAIData.wayIndex].y ;
				pos.z = GameLevelMgr.GetInstance().m_monsterBrithPointArr[type.m_monsterAIData.wayIndex].z ;
				type.GetRenderObject().transform.position = pos ;
				Think(type);
			}
			
		}
		public void Exit(CMonster type){
			
		}
		public void OnMessage(CMonster type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_PATRAL ;
		}
		
		public static MonsterChangeWayState getInstance(){
			if(instance ==null){
				instance = new MonsterChangeWayState();
			}
			
			return instance;
		}
	}
}

