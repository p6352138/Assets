using UnityEngine;
using System.Collections;

namespace GameEntity{
	public enum PlayerPlayAnimation
	{
		IDEL,
		WALK,
		RUN,
		JUMP,
	}

	public static class CPlayerCommon
	{
		public static readonly float Player_Speed = 2.0f;
	}
}
