using UnityEngine;
using System.Collections;
using GameEvent ;
using GameLogical ;

namespace GameLogical.GameEnitity.AI{
	public class MonsterStoneState : CStateBase<CMonster>
	{
		protected static MonsterStoneState instance;
		public void Release(){
			
		}
		
		public void Enter(CMonster type){
			type.Stop();
		}
		
		public void Execute(CMonster type, float time){
			
		}
		
		public void Exit(CMonster type){
			//type.m_stateMachine
			//type.m_stateMachine.ChangeState(type.m_stateMachine.GetPreviosState());
		}
		
		public void OnMessage(CMonster type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_STONE ;
		}
		
		public static MonsterStoneState getInstance(){
			if(instance==null){instance = new MonsterStoneState();}
			return instance;
		}
	}

}
