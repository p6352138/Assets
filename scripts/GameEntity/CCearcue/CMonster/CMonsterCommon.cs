using UnityEngine;
using System.Collections;

namespace GameEntity
{
    public enum MonsterAnimation
    {
        IDEL,
        ATTACK,
        DEATH,
        INJURT,
        RUN,
    }

    public static class CMonsterCommon
    {
		public static readonly int eyeArea = 10;
        public static readonly int AttackArea = 2;
		public static readonly float AttackCD = 0.5f;
        public static readonly float MoveSpeed = 1.0f;
        public static readonly int Boold =100;

    }
}
