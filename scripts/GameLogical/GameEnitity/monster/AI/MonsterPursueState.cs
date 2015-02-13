using UnityEngine;
using System.Collections;
using GameEvent ;

namespace GameLogical.GameEnitity.AI
{
	public class MonsterPursueState: CStateBase<CMonster>{
		protected static MonsterPursueState instance;
		public void Release(){
			
		}
		public void Enter(CMonster type){
			type.Play("walk",WrapMode.Loop);
			
			//talk
			/*MonsterMoudleData monstermoudle = (MonsterMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID,
				common.CsvType.CSV_TYPE_MONSTER);
			type.TalkLv(monstermoudle.talkLvInSpeed, monstermoudle.talkIDInSpeed);*/

			type.m_monsterAIData.time = 0.0f ;
			Think(type);
		}
		
		public void Execute(CMonster type, float time){
			type.m_monsterAIData.time += time ;
			if(type.m_monsterAIData.time >= AICommon.AI_THINK_DELTA_TIME){
				Think(type);
				type.m_monsterAIData.time = 0.0f ;
			}
			Action(type,time);
		}

		public void Think(CMonster type){
			//if the target in attacke
			if(type.m_targetCreature!=null && type.m_targetCreature.GetRenderObject()!=null){
				float disVec = type.m_targetCreature.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_BE_ATTACK_FORWARD).position.x - type.GetRenderObject().transform.position.x ;
				float disVecX = Mathf.Abs(disVec);
				disVec = type.m_targetCreature.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_BE_ATTACK_BACK).position.x - type.GetRenderObject().transform.position.x ;
				if(disVecX > Mathf.Abs(disVec)){
					disVecX = Mathf.Abs(disVec) ;
				}
				float disY	 = type.m_targetCreature.GetRenderObject().transform.position.y - type.GetRenderObject().transform.position.y ;

				//turn around
				if(disVec > 0){
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
				if(disVecX < type.attackArea )
				{
					if( Mathf.Abs(disY) < AICommon.AI_ATTACK_Y_GAP){
						type.m_stateMachine.ChangeState(MonsterAttackState.getInstance());
					}
					else{
						type.m_stateMachine.ChangeState(MonsterChangeWayState.getInstance());
					}
				}

			}
			else{
				type.m_stateMachine.ChangeState(MonsterMoveState.getInstance());
			}
		}

		public void Action(CMonster type,float time){
			float disVec = type.m_targetCreature.GetRenderObject().transform.position.x - type.GetRenderObject().transform.position.x ;
			float disY	 = type.m_targetCreature.GetRenderObject().transform.position.y - type.GetRenderObject().transform.position.y ;
			//move x
			if(Mathf.Abs(disVec) >= AICommon.AI_ATTACK_X_GAP){
				//move forward
				if(disVec > 0){
					type.GetRenderObject().transform.position += Vector3.right * time * type.monsterSpeed ;
				}
				//move back
				else{
					type.GetRenderObject().transform.position -= Vector3.right * time * type.monsterSpeed ;
				}
			}
		}

		public void Exit(CMonster type){
			
		}
		public void OnMessage(CMonster type, EventMessageBase data){


		}
		public AIState  GetState(){
			return AIState.AI_STATE_PURSUE ;
		}
		
		public static MonsterPursueState getInstance(){
			if(instance ==null){
				instance = new MonsterPursueState();
			}
			
			return instance;
		}
	}
}

