using UnityEngine;
using System.Collections;
using GameEntity;

namespace GameLogic.AI{
	public class PlayerWalkState : CStateBase<CPlayer> {

		protected static PlayerWalkState instance;

		public void Release()
		{
			;
		}

		public void Enter(CPlayer type)
		{
			//type.id = 1;
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
			return AIState.AI_STATE_MOVE;
		}

		public static PlayerWalkState GetInstance(){
			if (instance == null)
				instance = new PlayerWalkState ();
			return instance;
		}
	}
}
