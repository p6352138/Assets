using UnityEngine;
using System.Collections;
using GameEntity;

namespace GameLogic.AI{
	public class PlayerDeathState : CStateBase<CPlayer> {

		protected static PlayerDeathState instance;
		
		public void Release()
		{
			;
		}
		
		public void Enter(CPlayer type)
		{
			type.Play (PlayerPlayAnimation.DEATH, WrapMode.Once);
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
		
		public static PlayerDeathState GetInstance(){
			if (instance == null)
				instance = new PlayerDeathState ();
			return instance;
		}
	}
}
