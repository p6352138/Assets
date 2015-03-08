using UnityEngine;
using System.Collections;

namespace GameEntity{
	public enum PlayerPlayAnimation
	{
		IDEL,
		DEATH,
		RUN,
		INJURT,
		ATTACK,
	}

	public static class CPlayerCommon
	{
		public static readonly float Player_Speed = 2.0f;
		public static readonly int PlayerBlood = 100;
		public static readonly int attack = 20;

	}
}
