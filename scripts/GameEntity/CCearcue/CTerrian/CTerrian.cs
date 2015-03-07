using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameLogic.Navigation;
using GameLogic.AI;

namespace GameEntity{
	public class CTerrian : CCreature {

		#region private Properties
		private int m_id;
		private GameObject m_go;

		private Terrain terrain;
		private List<Bounds> m_BoundsList; 
		#endregion

		#region public Fields
		public StateMachine<CTerrian> m_stateMachine;
		#endregion

		public CTerrian(int id,GameObject ob){
			m_BoundsList = new List<Bounds>();

			m_id = id;
			m_go = ob;

			terrain = m_go.GetComponent<Terrain>();
			BoxCollider box;
			MeshCollider mesh;

			for (int i = 0;i<m_go.transform.FindChild("prop").childCount;i++){
				box = m_go.transform.FindChild("prop").GetChild(i).GetComponent<BoxCollider>();

				if(box == null){
					mesh = m_go.transform.FindChild("prop").GetChild(i).GetComponent<MeshCollider>();
					if(mesh != null){
						m_BoundsList.Add(mesh.bounds);
					}
				}
				else{
					m_BoundsList.Add(box.bounds);
				}
			}

			NavigationMgr.GetInstance().InitPathData(m_BoundsList);
		}

		#region public function
		public Vector3 GetTerrianPosion(){
			return m_go.transform.localPosition;
		}

		public List<Bounds> GetTerrianBounds(){
			return m_BoundsList;
		}
		#endregion

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
			;
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

		
		public EnitityType GetEnitityType()
		{
			return EnitityType.ENTITY_TERRIAN;
		}
		
		#endregion
	}
}
