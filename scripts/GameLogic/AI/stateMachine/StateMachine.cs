using UnityEngine;
using System.Collections;

namespace GameLogic.AI{
	public class StateMachine<TYPE> {

		#region protected fileds
		protected TYPE m_Object;
		protected CStateBase<TYPE> m_CurrentState;
		protected CStateBase<TYPE> m_PreviosState;
		protected CStateBase<TYPE> m_NextState;
		#endregion

		public StateMachine(TYPE type){
			m_Object = type;
			m_CurrentState = null;
		}

		public void SetState(CStateBase<TYPE> state)
		{
			m_CurrentState = state;
			if(m_CurrentState != null)
				m_CurrentState.Enter(m_Object);
		}

		// Update is called once per frame
		public void Update (float deltaTime)
		{
			if(m_CurrentState != null){
				m_CurrentState.Execute(m_Object,deltaTime);
			}
		}

		public void ChangeState(CStateBase<TYPE> state){
			m_NextState = state;
			m_PreviosState = m_CurrentState;
			if(m_PreviosState!=null){
				m_PreviosState.Exit(m_Object);
			}
			
			if(m_NextState==null){
				return ;
			}
			m_CurrentState = m_NextState;
			m_NextState.Enter(m_Object);
			
		}
		public CStateBase<TYPE> GetState(){
			return m_CurrentState ;
		}
		
		public void Release(){
			m_Object = default(TYPE) ;
			m_CurrentState.Release();
			m_CurrentState = null ;
		}
		
		public void OnMessage(EventMessageBase message){
			if(m_CurrentState!=null)m_CurrentState.OnMessage(m_Object,message);
		}
		
		public CStateBase<TYPE> GetPreviosState(){
			return m_PreviosState ;
		}
	}
}