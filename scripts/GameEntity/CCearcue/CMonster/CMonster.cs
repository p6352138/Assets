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

            m_stateMachine = new StateMachine<CMonster>(this);
            m_stateMachine.SetState(MonsterIdelState.GetInstance());
        }

        public void Think()
        {

        }

        public void Update(float deltaTime)
        {
            if (m_stateMachine != null)
                m_stateMachine.Update(deltaTime);
        }
        public void OnMessage(EventMessageBase message)
        {
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
            else if (type == MonsterAnimation.ATTACK_1)
            {
                name = "attack_1";
            }
            else if (type == MonsterAnimation.ATTACK_2)
            {
                name = "attack_2";
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
