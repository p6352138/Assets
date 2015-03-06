using UnityEngine;
using System.Collections;
using GameEntity;
using GameLogic.Navigation;

namespace GameLogic.AI
{
    public class MonsterAttackState : CStateBase<CMonster>
    {

        protected static MonsterIdelState instance;

        public void Release()
        {
            ;
        }

        public void Enter(CMonster type)
        {
            if (Random.Range(0, 9) % 2 == 0)
            {
                type.Play(MonsterAnimation.ATTACK_1, WrapMode.Once);
            }
            else
            {
                type.Play(MonsterAnimation.ATTACK_2, WrapMode.Once);
            }
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

        public static MonsterIdelState GetInstance()
        {
            if (instance == null)
                instance = new MonsterIdelState();
            return instance;
        }
    }
}
