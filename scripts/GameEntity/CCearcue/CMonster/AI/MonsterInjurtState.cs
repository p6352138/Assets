using UnityEngine;
using System.Collections;
using GameEntity;

namespace GameLogic.AI
{
    public class MonsterInjurtState : CStateBase<CMonster>
    {
        protected static MonsterInjurtState instance;

        public void Release()
        {
            ;
        }

        public void Enter(CMonster type)
        {
            type.Play(MonsterAnimation.INJURT, WrapMode.Once);
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

        public static MonsterInjurtState GetInstance()
        {
            if (instance == null)
                instance = new MonsterInjurtState();
            return instance;
        }
       
    }
}
