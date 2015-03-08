using UnityEngine;
using System.Collections;
using GameEntity;

namespace GameLogic.AI{
	public class PlayerAttackState : CStateBase<CPlayer> {

		protected static PlayerAttackState instance;
		
		public void Release()
		{
			;
		}
		
		public void Enter(CPlayer type)
		{
			type.Play (PlayerPlayAnimation.ATTACK, WrapMode.Once);
			type.GetAttackArea ();
		}
		
		public void Execute(CPlayer type, float time)
		{
			;
		}
		
		public void Exit(CPlayer type)
		{
			;
		}
		
		public void OnMessage(CPlayer type, EventMessageBase data)
		{
			;
		}
		
		public AIState GetState()
		{
			return AIState.AI_STATE_STAND;
		}
		
		public static PlayerAttackState GetInstance(){
			if (instance == null)
				instance = new PlayerAttackState ();
			return instance;
		}
	}
}
