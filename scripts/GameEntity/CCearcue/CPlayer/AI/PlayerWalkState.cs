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
			type.Play (PlayerPlayAnimation.WALK, WrapMode.Loop);
		}

		public void Execute(CPlayer type, float time)
		{
			//Vector3 speed = new Vector3 (CPlayerCommon.Player_Speed * type.GetRenderObject ().transform.eulerAngles.x, 
			//                             CPlayerCommon.Player_Speed * type.GetRenderObject ().transform.eulerAngles.z,
			//                             CPlayerCommon.Player_Speed * type.GetRenderObject ().transform.eulerAngles.y);
			type.GetRenderObject ().transform.Translate (Vector3.forward * CPlayerCommon.Player_Speed*time);
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
