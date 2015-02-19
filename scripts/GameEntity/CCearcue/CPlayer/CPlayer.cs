using UnityEngine;
using System.Collections;
using GameLogic.AI;

namespace GameEntity{
	public class CPlayer : CCreature {

		#region private Fields
		private int m_id;
		private GameObject m_go;
		#endregion

		#region public Fields
		public StateMachine<CPlayer> m_stateMachine;
		#endregion

		public CPlayer(int id,GameObject go)
		{
			m_id = id;
			m_go = go;
		}


		#region interface function
		/// <summary>
		/// Update the specified deltaTime.
		/// </summary>
		/// <param name='deltaTime'>
		/// Delta time.
		/// </param>
		public void Update(float deltaTime)
		{
			if(m_stateMachine != null)
				m_stateMachine.Update(deltaTime);

			//compute attack cd time 
			/*
			if(appearTime != -1.0f){
				appearTime += deltaTime;
				if(appearTime > appearMaxTime && playerState == PlayerAIState.Fight){
					DisAppear();
					appearTime = -1.0f;
				}
			}*/
		}

		/// <summary>
		/// Raises the message event.
		/// </summary>
		/// <param name="message">Message.</param>
		public void OnMessage(EventMessageBase message)
		{
			if(message.eventMessageModel == EventMessageModel.eEventMessageModel_Enitity)
			{
				//switch(message.eventMessageAction){
					//case EnitityCommon.EnitityAction.ENITITY_ACTION_SELECT_ENITITY:
				//		break;
				//}
			}
			m_stateMachine.OnMessage(message);
		}

		/// <summary>
		/// Gets the type of the enitity.
		/// </summary>
		/// <returns>
		/// The enitity type.
		/// </returns>
		//public EnitityType GetEnitityType(){
		//	return EnitityType.ENITITY_TYPE_CHARACTER ;
		//}
		
		/// <summary>
		/// Gets the state of the enitity ai.
		/// </summary>
		/// <returns>
		/// The enitity ai state.
		/// </returns>
		public AIState  GetEnitityAiState(){
			return AIState.AI_STATE_NULL ;
		}
		
		/// <summary>
		/// Release this instance.
		/// </summary>
		public void Release(){
			/*
			m_data = null;
			if(renderObject != null)
				MonoBehaviour.Destroy( renderObject ) ;
			renderObject = null ;
			
			for(int i = 0; i < m_skillList.Count; ++i){
				SkillMgr.GetInstance().RemoveSkill(m_skillList[i]);
			}
			m_skillList.Clear();
			m_skillList = null ;
			*/
		}
		
		/// <summary>
		/// Gets the render object.
		/// </summary>
		/// <returns>
		/// The render object.
		/// </returns>
		public GameObject GetRenderObject(){
			return m_go ;
		}

		public void Think()
		{
			;
		}

		//play animation state
		public void Play(string name,WrapMode mode)
		{

		}

		public int GetId()
		{
			return m_id;
		}

		#endregion
	}
}
