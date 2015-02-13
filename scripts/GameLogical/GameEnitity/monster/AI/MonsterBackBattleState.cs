using UnityEngine;
using System.Collections;
using GameEvent ;


namespace GameLogical.GameEnitity.AI
{
	public class MonsterBackBattleState: CStateBase<CMonster>{
		protected static MonsterBackBattleState instance;
		public void Release(){
			
		}
		public void Enter(CMonster type){
		
		}
		public void Execute(CMonster type, float time){
			
			
		}
		public void Exit(CMonster type){
			
		}
		public void OnMessage(CMonster type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_ACTTACK_CITY ;
		}
		public static MonsterBackBattleState getInstance(){
			if(instance ==null){
				instance = new MonsterBackBattleState();
			}
			
			return instance;
		}
	
	}
}



