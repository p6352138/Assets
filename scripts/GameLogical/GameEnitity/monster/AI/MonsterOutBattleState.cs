using UnityEngine;
using System.Collections;
using GameEvent ;

namespace GameLogical.GameEnitity.AI
{
	public class MonsterOutBattleState : CStateBase<CMonster>
	{
		protected static MonsterOutBattleState instance;
		public void Release(){
			
		}
		public void Enter(CMonster type){
			type.m_monsterAIData.time = 0.0f ;
			
			type.Play("stand",WrapMode.Loop);
		}
		
		public void Execute(CMonster type, float time){
			type.m_monsterAIData.time += time ;
			if(type.m_monsterAIData.time >= 1.0f){
				type.SetState(MonsterMoveState.getInstance());
			}
		}
		
		public void Exit(CMonster type){
			
		}
		public void OnMessage(CMonster type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_STAND ;
		}
		public static MonsterOutBattleState getInstance(){
			if(instance ==null){
				instance = new MonsterOutBattleState();
			}
			
			return instance;
		}
	}
}


