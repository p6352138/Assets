using UnityEngine;
using System.Collections;
using GameEvent ;

namespace GameLogical.GameEnitity.AI{
	public class EnemyPetStandState : CStateBase<CEnemyPet>
	{
		protected static EnemyPetStandState instance;
		public void Release(){
			
		}
		public void Enter(CEnemyPet type){
			type.Play("stand",WrapMode.Loop);
		}
		
		public void Execute(CEnemyPet type, float time){

		}
		
		public void Exit(CEnemyPet type){
			
		}
		public void OnMessage(CEnemyPet type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_STAND ;
		}
		public static EnemyPetStandState getInstance(){
			if(instance ==null){
				instance = new EnemyPetStandState();
			}
			
			return instance;
		}
	}
}


