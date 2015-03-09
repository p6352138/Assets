using UnityEngine;
using System.Collections;
using GameLogic.AI;
using GameLogic.Navigation;

namespace GameEntity
{
    public class CMonster : CCreature
    {
        #region private Fields
        private int m_id;
        private GameObject m_go;
        private Animation m_animation;
		private NAnimationEvent m_aniamtionEvent;
        private int m_blood;
        #endregion

        #region public Fields
        public StateMachine<CMonster> m_stateMachine;

        public int ID { get { return m_id; } }

        public int PositionInPathGrid
        {
            get
            {
                return NavigationMgr.GetInstance().GetGrid().GetPathNodeIndex(m_go.transform.localPosition);
            }
        }
        #endregion

        public CMonster(int id,GameObject go)
        {
            m_id = id;
            m_go = go;
            m_animation = go.GetComponent<Animation>();
			m_aniamtionEvent = go.GetComponent<NAnimationEvent> ();

            m_stateMachine = new StateMachine<CMonster>(this);
            m_stateMachine.SetState(MonsterIdelState.GetInstance());

            m_blood = CMonsterCommon.Boold;
        }

        public void Think()
        {
			;
        }

        public void Update(float deltaTime)
        {
            if (m_stateMachine != null)
                m_stateMachine.Update(deltaTime);
        }
        public void OnMessage(EventMessageBase message)
        {
            if (message.eventMessageModel == EventMessageModel.eEventMessageModel_PLAY_ATTACK_STATE)
            {
                m_blood -= message.eventMessageAction;

                if (m_blood <= 0)
                {
                    m_stateMachine.ChangeState(MonsterDeathState.GetInstance());
                }
                else
                {
                    m_stateMachine.ChangeState(MonsterInjurtState.GetInstance());
                }

                Debug.Log("monster blood ="+m_blood.ToString());
            }
            else
            {
                if (message.eventMessageAction == (int)EnitityCommon.EnitityAction.ENITITY_ACTION_FIGHT_FINISH)
                {
                    m_stateMachine.ChangeState(MonsterIdelState.GetInstance());
                }
            }

            m_stateMachine.OnMessage(message);
        }
        public EnitityType GetEnitityType()
        {
            return EnitityType.ENTITY_MONSTER;
        }
        public AIState GetEnitityAiState()
        {
            if (m_stateMachine.GetState() == MonsterIdelState.GetInstance())
            {
                return AIState.AI_STATE_STAND;
            }

            return AIState.AI_STATE_NULL;
        }
        public void Release()
        {

        }
        public GameObject GetRenderObject()
        {
            return m_go;
        }
        public void Play(string name, WrapMode mode)
        {
            m_animation.wrapMode = mode;
            m_animation.Play(name);
        }

        public void Play(MonsterAnimation type,WrapMode mode)
        {
            string name = "";
            m_animation.wrapMode = mode;

            if (type == MonsterAnimation.IDEL)
            {
                name = "idle";
            }
            else if (type == MonsterAnimation.ATTACK)
            {
                name = "attack";
            }
            else if (type == MonsterAnimation.DEATH)
            {
                name = "death";
            }
            else if (type == MonsterAnimation.INJURT)
            {
                name = "injurt";
            }
            else if (type == MonsterAnimation.RUN)
            {
                name = "run";
            }

            m_animation.Play(name);
        }
        public int GetId()
        {
            return m_id;
        }
    }
}
