using UnityEngine;
using System.Collections;
using GameEvent ;

namespace GameLogical.GameEnitity.AI
{
	//relax state
	public class MonsterStandState : CStateBase<CMonster>{
		protected static MonsterStandState instance;
		public void Release(){
			
		}
		public void Enter(CMonster type){
			type.Play("stand",WrapMode.Loop);
		}
		public void Execute(CMonster type, float time){
			
		}
		public void Exit(CMonster type){
			
		}
		public void OnMessage(CMonster type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_STAND ;
		}
		public static MonsterStandState getInstance(){
			if(instance ==null){
				instance = new MonsterStandState();
			}
			
			return instance;
		}
	}
}

