using UnityEngine;
using System.Collections;
using GameEntity;

namespace GameLogic.AI{
	public class PlayerIdelState : CStateBase<CPlayer> {

		protected static PlayerIdelState instance;
		
		public void Release()
		{
			;
		}
		
		public void Enter(CPlayer type)
		{
			type.Play (PlayerPlayAnimation.IDEL, WrapMode.Loop);
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
		
		public static PlayerIdelState GetInstance(){
			if (instance == null)
				instance = new PlayerIdelState ();
			return instance;
		}
	}
}
