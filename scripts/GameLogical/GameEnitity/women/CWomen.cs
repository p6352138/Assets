using UnityEngine;
using System.Collections;
using GameLogical.GameEnitity.AI ;
using GameEvent ;

namespace GameLogical.GameEnitity{
	public class CWomen : CCreature
	{
		protected   WomenData  			 	 m_data 		;
		protected   StateMachine<CWomen>   	 m_stateMachine ;
		protected	WomenBrain			 	 m_brain		;
		public   	WomenAIDataBass		 	 m_womenAIData	;
		public      GameObject               m_object       ;

		public	void SetColor(Color color)
		{
			if(m_object != null)
				m_object.transform.FindChild(gameGlobal.CREATURE).gameObject.renderer.material.color = color ;
		}

		/// <summary>
		/// Init the specified data.
		/// </summary>
		/// <param name='data'>
		/// Data.
		/// </param>
		public void Init(WomenData data){

			//m_data = new WomenData();
			m_data = data ;

		}

		public void LoadObjectCallBack(Object ob, MGResouce.LoadCreatureData tran){
			if(ob != null){
				MGResouce.LoadWomenData loadData = tran as MGResouce.LoadWomenData ;
				m_object = MonoBehaviour.Instantiate(ob) as GameObject ;
				m_object.name = m_data.id.ToString();

				m_brain = new WomenBrain();
				m_brain.Init(this);
				m_stateMachine = new StateMachine<CWomen>(this);

				
				CWomenBeCoughtState state = new CWomenBeCoughtState();
				WomenBeCoughtStateData stateData = new WomenBeCoughtStateData();
				stateData.monsterID = loadData.monsterID ;
				this.m_womenAIData = stateData ;
				this.SetState(state);
			}
		}

		/// <summary>
		/// Think this instance.
		/// </summary>
		public void Think(){
			m_brain.ThinkAbout();
		}
		
		/// <summary>
		/// Update the specified deltaTime.
		/// </summary>
		/// <param name='deltaTime'>
		/// Delta time.
		/// </param>
		public void Update(float deltaTime){
			if(m_stateMachine != null)
				m_stateMachine.Update(deltaTime);
		}
		
		public void OnMessage(EventMessageBase message){
			if(m_brain != null){
				m_brain.OnMessage(message);
			}
		}
		
		/// <summary>
		/// Gets the type of the enitity.
		/// </summary>
		/// <returns>
		/// The enitity type.
		/// </returns>
		public EnitityType GetEnitityType(){
			return EnitityType.ENITITY_TYPE_WOMEN ;
		}
		
		public void SetState(CStateBase<CWomen> state){
			if(m_stateMachine != null){
				m_stateMachine.SetState(state);
			}
		}
		
		/// <summary>
		/// Gets the state of the enitity ai.
		/// </summary>
		/// <returns>
		/// The enitity ai state.
		/// </returns>
		public AIState  GetEnitityAiState(){
			CStateBase<CWomen> state = m_stateMachine.GetState();
			return state.GetState(); 
		}
		
		/// <summary>
		/// Release this instance.
		/// </summary>
		public void Release(){
			m_data = null;
			m_stateMachine.Release();
			m_stateMachine = null ;
			m_brain	= null	;
			m_womenAIData = null;
			MonoBehaviour.Destroy( m_object ) ;
			m_object = null ;
		}
		
		/// <summary>
		/// Gets the render object.
		/// </summary>
		/// <returns>
		/// The render object.
		/// </returns>
		public GameObject GetRenderObject(){
			return m_object ;
		}
		
		/// <summary>
		/// Play the specified name and mode.
		/// </summary>
		/// <param name='name'>
		/// Name.
		/// </param>
		/// <param name='mode'>
		/// Mode.
		/// </param>
		public void Play(string name,WrapMode mode){
			Transform trans = m_object.transform.FindChild("creature");
			trans.animation.wrapMode = mode ;
			trans.gameObject.animation.Play(name);
		}
		
		
		public ArrayList GetEyeShotList(){
			return null ;
		}
		public ArrayList GetAttackAreaList(){
			return null ;
		}
		public int  GetId(){
			return m_data.id ;
		}
		
		//
		public float womenSpeed{
			get{
				return m_data.moveSpeed ;
			}
		}
		
		public EffectData GetEffectData(){
			return null ;
		}
		
		public void SetHp(int hp){
		}
		
		public bool AddBuff(int buffMoudleID,int buffID){	
			return false ;
		}
		
		public void DelBuff(int buffID){

		}
		
		public FightCreatureData GetFightCreatureData(){
			return null ;
		}
		
		public void SetCrit(bool isCrit){

		}

		public int CheckBuff(int buffMoudleID){
			return -1;
		}
	}
}


