using UnityEngine;
using System.Collections;
using GameEvent ;
using GameLogical.GameEnitity ;
using GameLogical.GameLevel ;

namespace GameLogical.GameEnitity.AI
{
	public class MonsterMoveAwayState : CStateBase<CMonster>
	{
		protected static MonsterMoveAwayState instance;
		public void Release(){
			
		}
		public void Enter(CMonster type){
			type.Play("walk",WrapMode.Loop);
		}
		
		public void Execute(CMonster type, float time){
			Action(type,time);
		}
		
		public void Think(CMonster type){
		}
		
		public void Action(CMonster type,float time){
			if(type.GetRenderObject().transform.position.x > 110)
			{
				type.m_stateMachine.ChangeState(MonsterMoveState.getInstance());
			}

			float dis = type.m_targetCreature.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_BE_ATTACK_FORWARD).position.x - type.GetRenderObject().transform.position.x ;
			float disVecX = Mathf.Abs(dis);
			dis = type.m_targetCreature.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_BE_ATTACK_BACK).position.x - type.GetRenderObject().transform.position.x ;
			if(disVecX > Mathf.Abs(dis)){
				disVecX = Mathf.Abs(dis) ;
			}
			//move y
			if(disVecX < AICommon.AI_ATTACK_X_GAP){
				//move up
				if(dis > 0){
					type.GetRenderObject().transform.position += Vector3.left * time * type.monsterSpeed ;
					Vector3 pos = type.GetRenderObject().transform.position ;
					pos.z = pos.y ;
					type.GetRenderObject().transform.position = pos ;
				}
				//move down
				else{
					type.GetRenderObject().transform.position += Vector3.right * time * type.monsterSpeed ;
					Vector3 pos = type.GetRenderObject().transform.position ;
					pos.z = pos.y ;
					type.GetRenderObject().transform.position = pos ;
				}
			}
			else{
				type.m_stateMachine.ChangeState(MonsterAttackState.getInstance());
			}
			
		}
		public void Exit(CMonster type){
			
		}
		public void OnMessage(CMonster type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_ACTTACK ;
		}
		
		public static MonsterMoveAwayState getInstance(){
			if(instance ==null){
				instance = new MonsterMoveAwayState();
			}
			
			return instance;
		}
	}
}


