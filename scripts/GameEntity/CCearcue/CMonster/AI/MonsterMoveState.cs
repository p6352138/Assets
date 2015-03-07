using UnityEngine;
using System.Collections;
using GameEntity;
using GameLogic.Navigation;

namespace GameLogic.AI
{
    public class MonsterMoveState : CStateBase<CMonster> 
    {
		protected static MonsterMoveState instance;

        public void Release()
        {
            ;
        }

        public void Enter(CMonster type)
        {
			type.GetRenderObject ().transform.LookAt (CCearcueMgr.GetInstance ().player.GetRenderObject ().transform.localPosition);
            type.Play(MonsterAnimation.RUN, WrapMode.Loop);
        }

        public void Execute(CMonster type, float time)
        {
			type.GetRenderObject ().transform.LookAt (CCearcueMgr.GetInstance ().player.GetRenderObject ().transform.localPosition);
			type.GetRenderObject ().transform.Translate (Vector3.forward * CMonsterCommon.MoveSpeed*time);

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

		public static MonsterMoveState GetInstance()
        {
            if (instance == null)
				instance = new MonsterMoveState();
            return instance;
        }
    }
}
