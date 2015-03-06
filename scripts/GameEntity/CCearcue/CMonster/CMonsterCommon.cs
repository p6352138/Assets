using UnityEngine;
using System.Collections;

namespace GameEntity
{
    public enum MonsterAnimation
    {
        IDEL,
        ATTACK_1,
        ATTACK_2,
        DEATH,
        INJURT,
        RUN,
    }

    public static class CMonsterCommon
    {
        public static readonly int eyeArea = 5;
        public static readonly int AttackArea = 1;
        public static readonly float AttackCD = 0.5f;

    }
}
