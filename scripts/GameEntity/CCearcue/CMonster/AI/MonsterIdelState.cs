using UnityEngine;
using System.Collections;
using GameEntity;
using GameLogic.Navigation;

namespace GameLogic.AI
{
    public class MonsterIdelState : CStateBase<CMonster> 
    {
        protected static MonsterIdelState instance;
        private float curCDTime;

        public void Release()
        {
            ;
        }

        public void Enter(CMonster type)
        {
            curCDTime = 0;
            type.Play(MonsterAnimation.IDEL, WrapMode.Loop);
        }

        public void Execute(CMonster type, float time)
        {
            if(NavigationMgr.GetInstance().GetGrid().GetDistance(CCearcueMgr.GetInstance().player.PositionInPathGrid, type.PositionInPathGrid) <= CMonsterCommon.eyeArea
			   && NavigationMgr.GetInstance().GetGrid().GetDistance(CCearcueMgr.GetInstance().player.PositionInPathGrid, type.PositionInPathGrid) > CMonsterCommon.AttackArea)
            {
                type.m_stateMachine.ChangeState(MonsterMoveState.GetInstance());
            }
            else if (NavigationMgr.GetInstance().GetGrid().GetDistance(CCearcueMgr.GetInstance().player.PositionInPathGrid, type.PositionInPathGrid) <= CMonsterCommon.AttackArea)
            {
                curCDTime += time;
                if (curCDTime >= CMonsterCommon.AttackCD)
                {
                    curCDTime = 0;
                    type.m_stateMachine.ChangeState(MonsterAttackState.GetInstance());
                }
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
