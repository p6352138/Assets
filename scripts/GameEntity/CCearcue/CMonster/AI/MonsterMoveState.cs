using UnityEngine;
using System.Collections;
using GameEntity;
using GameLogic.Navigation;

namespace GameLogic.AI
{
    public class MonsterMoveState : CStateBase<CMonster> 
    {

        protected static MonsterIdelState instance;

        public void Release()
        {
            ;
        }

        public void Enter(CMonster type)
        {
            type.Play(MonsterAnimation.RUN, WrapMode.Loop);
        }

        public void Execute(CMonster type, float time)
        {
            if (NavigationMgr.GetInstance().GetGrid().GetDistance(CCearcueMgr.GetInstance().player.PositionInPathGrid, type.PositionInPathGrid) > CMonsterCommon.eyeArea)
            {
                type.m_stateMachine.ChangeState(MonsterIdelState.GetInstance());
            }
            else if (NavigationMgr.GetInstance().GetGrid().GetDistance(CCearcueMgr.GetInstance().player.PositionInPathGrid, type.PositionInPathGrid) <= CMonsterCommon.AttackArea)
            {
                type.m_stateMachine.ChangeState(MonsterIdelState.GetInstance());
            }
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
            return AIState.AI_STATE_MOVE;
        }

        public static MonsterIdelState GetInstance()
        {
            if (instance == null)
                instance = new MonsterIdelState();
            return instance;
        }
    }
}
