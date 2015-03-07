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
			type.Play (PlayerPlayAnimation.RUN, WrapMode.Loop);
		}

		public void Execute(CPlayer type, float time)
		{
			//Vector3 speed = new Vector3 (CPlayerCommon.Player_Speed * type.GetRenderObject ().transform.eulerAngles.x, 
			//                             CPlayerCommon.Player_Speed * type.GetRenderObject ().transform.eulerAngles.z,
			//                             CPlayerCommon.Player_Speed * type.GetRenderObject ().transform.eulerAngles.y);
			//type.GetRenderObject ().transform.Translate (Vector3.forward * CPlayerCommon.Player_Speed*time);
		}

		public void Exit(CPlayer type)
		{
			;
		}

		public void OnMessage(CPlayer type, EventMessageBase data)
		{
			if (data.eventMessageAction == (int)EnitityCommon.EnitityAction.ENITITY_ACTION_MOVETOP) {
				type.GetRenderObject ().transform.Translate (Vector3.forward * CPlayerCommon.Player_Speed*0.015f);
			}
			else if (data.eventMessageAction == (int)EnitityCommon.EnitityAction.ENITITY_ACTION_MOVEBOTTOM) {
				type.GetRenderObject ().transform.Translate (Vector3.back * CPlayerCommon.Player_Speed*0.015f);
			}
			else if (data.eventMessageAction == (int)EnitityCommon.EnitityAction.ENITITY_ACTION_MOVELEFT) {
				Vector3 angle = new Vector3(0,-1,0);
				type.GetRenderObject ().transform.eulerAngles += angle;
			}
			else if (data.eventMessageAction == (int)EnitityCommon.EnitityAction.ENITITY_ACTION_MOVERIGHT) {
				Vector3 angle = new Vector3(0,1,0);
				type.GetRenderObject ().transform.eulerAngles += angle;
			}
			else if (data.eventMessageAction == (int)EnitityCommon.EnitityAction.ENITITY_ACTION_MOVEOVER) {
				type.m_stateMachine.ChangeState (PlayerIdelState.GetInstance ());
			}

			//type.m_stateMachine.ChangeState (PlayerIdelState.GetInstance ());
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
