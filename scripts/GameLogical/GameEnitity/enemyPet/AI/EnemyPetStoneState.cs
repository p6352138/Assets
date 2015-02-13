using UnityEngine;
using System.Collections;
using GameEvent ;
using GameLogical ;

namespace GameLogical.GameEnitity.AI{
	public class EnemyPetStoneState : CStateBase<CEnemyPet>
	{
		protected static EnemyPetStoneState instance;
		public void Release(){
			
		}
		
		public void Enter(CEnemyPet type){
			type.Stop();
		}
		
		public void Execute(CEnemyPet type, float time){
			
		}
		
		public void Exit(CEnemyPet type){
			//type.m_stateMachine
			//type.m_stateMachine.ChangeState(type.m_stateMachine.GetPreviosState());
		}
		
		public void OnMessage(CEnemyPet type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_STONE ;
		}
		
		public static EnemyPetStoneState getInstance(){
			if(instance==null){instance = new EnemyPetStoneState();}
			return instance;
		}
	}

}

