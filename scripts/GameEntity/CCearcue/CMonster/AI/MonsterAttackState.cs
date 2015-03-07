using UnityEngine;
using System.Collections;
using GameEntity;
using GameLogic.Navigation;

namespace GameLogic.AI
{
    public class MonsterAttackState : CStateBase<CMonster>
    {

		protected static MonsterAttackState instance;

        public void Release()
        {
            ;
        }

        public void Enter(CMonster type)
        {
            type.Play(MonsterAnimation.ATTACK, WrapMode.Once);
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

		public static MonsterAttackState GetInstance()
        {
            if (instance == null)
				instance = new MonsterAttackState();
            return instance;
        }
    }
}
