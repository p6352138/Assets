using UnityEngine;
using System.Collections;
using GameEntity;

namespace GameLogic.AI
{
    public class MonsterDeathState : CStateBase<CMonster>
    {
        protected static MonsterDeathState instance;

        public void Release()
        {
            ;
        }

        public void Enter(CMonster type)
        {
            type.Play(MonsterAnimation.DEATH, WrapMode.Once);
        }

        public void Execute(CMonster type, float time)
        {

        }

        public void Exit(CMonster type)
        {
            ;
        }

        public void OnMessage(CMonster type, EventMessageBase data)
        {
            ;
        }

        public AIState GetState()
        {
            return AIState.AI_STATE_ATTACK;
        }

        public static MonsterDeathState GetInstance()
        {
            if (instance == null)
                instance = new MonsterDeathState();
            return instance;
        }
    }
}
