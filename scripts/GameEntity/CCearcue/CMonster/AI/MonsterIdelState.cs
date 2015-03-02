using UnityEngine;
using System.Collections;
using GameEntity;

namespace GameLogic.AI
{
    public class MonsterIdelState : CStateBase<CMonster> 
    {
        protected static MonsterIdelState instance;

        public void Release()
        {
            ;
        }

        public void Enter(CMonster type)
        {
            type.Play(MonsterAnimation.IDEL, WrapMode.Loop);
        }

        public void Execute(CMonster type, float time)
        {
            ;
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
            return AIState.AI_STATE_STAND;
        }

        public static MonsterIdelState GetInstance()
        {
            if (instance == null)
                instance = new MonsterIdelState();
            return instance;
        }
    }
}
