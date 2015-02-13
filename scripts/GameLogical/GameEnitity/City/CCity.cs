using UnityEngine;
using System.Collections;
using GameEvent ;
using GameLogical.GameEnitity.AI;

namespace GameLogical.GameEnitity{
	public class CCity : CCreature
	{
		public 		GameObject				m_renderObject 		;
		protected	CityData				m_data				;
		protected 	ArrayList				m_eyeShotObjectList ;
		protected	ArrayList				m_attackAreaObjectList ;
		protected   bool					m_isMove ;
		protected   CityAIDataBass			m_cityAIData   ;
		protected   StateMachine<CCity>		m_stateMachine ;
		protected	EffectData 				m_effectData   ;
		public   	Vector3[]				m_ArrowPos	   ;
		public 		float					m_coolDown	   ;
		public		float					m_curCoolDown1  ;
		public		float					m_curCoolDown2  ;
		public		Transform				m_leftPaoTran  ;
		public		Transform				m_rightPaoTran ;
		
		public	void SetColor(Color color)
		{
			if(m_renderObject != null)
				m_renderObject.transform.FindChild("creature").gameObject.renderer.material.color = color ;
		}

		/// <summary>
		/// Init the specified data.
		/// </summary>
		/// <param name='data'>
		/// Data.
		/// </param>
		public void Init (CityData data)
		{
			m_data = data ;
			m_data.gateMaxBlood = data.gateBlood ;
			m_effectData = new EffectData();
			//m_data.
		}

		public void LoadObjectCallBack(Object ob, MGResouce.LoadCreatureData data){

			m_renderObject = MonoBehaviour.Instantiate(ob) as GameObject;
			m_renderObject.transform.position = data.pos ;

			if(m_data.attack == 0){
				m_curCoolDown1 = -1.0f ;
			}
			else{
				m_ArrowPos = new Vector3[2] ;
				m_ArrowPos[0] = m_renderObject.transform.FindChild("arrowPoint/1").position ;
				m_ArrowPos[1] = m_renderObject.transform.FindChild("arrowPoint/2").position ;
				m_curCoolDown1 = 0.0f ;
				m_curCoolDown2 = -1.5f;
				m_coolDown = 3.0f ;
			}

			m_renderObject.name = m_data.id.ToString(); 
			//load gun

			ResourceMoudleData resData = common.fileMgr.GetInstance().GetData(m_data.moudleID + 10,common.CsvType.CSV_TYPE_RESOUCE) as ResourceMoudleData ;

			MGResouce.LoadCreatureData loadData = new MGResouce.LoadCreatureData() ;
			loadData.fun = this.LoadGunCallBack ;
			loadData.name= resData.name ;
			loadData.packName = resData.packagePath ;
			MGResouce.BundleMgr.Instance.LoadCity(loadData);
			common.debug.GetInstance().AppCheckSlow(ob);
		}

		public void LoadGunCallBack(Object ob, MGResouce.LoadCreatureData data){
			GameObject jiantaOb1 = MonoBehaviour.Instantiate(ob) as GameObject;
			jiantaOb1.name = "pao" ;
			jiantaOb1.transform.parent = m_renderObject.transform.FindChild("arrowPoint/1");
			jiantaOb1.transform.localPosition = Vector3.zero ;
			jiantaOb1.transform.localScale = Vector3.one ;
			GameObject jiantaOb2 = MonoBehaviour.Instantiate(ob) as GameObject;
			jiantaOb2.name = "pao";
			jiantaOb2.transform.parent = m_renderObject.transform.FindChild("arrowPoint/2");
			jiantaOb2.transform.localPosition = Vector3.zero ;
			jiantaOb2.transform.localScale = Vector3.one ;

			m_leftPaoTran = m_renderObject.transform.FindChild("arrowPoint/1/pao") ;
			m_rightPaoTran= m_renderObject.transform.FindChild("arrowPoint/2/pao") ;

			CCityNormalState state = new CCityNormalState();
			CityNormalStateData normalStateData = new CityNormalStateData();
			m_cityAIData = normalStateData ;

			m_stateMachine = new StateMachine<CCity>(this);
			m_stateMachine.SetState(state);
		}

		/// <summary>
		/// Think this instance.
		/// </summary>
		public void Think(){
			
		}
		
		/// <summary>
		/// Update the specified deltaTime.
		/// </summary>
		/// <param name='deltaTime'>
		/// Delta time.
		/// </param>
		public void Update(float deltaTime){
			/*if(m_isMove)
				m_renderObject.transform.position += Vector3.back * 0.01f ;
			else
				m_renderObject.transform.position += Vector3.forward * 0.01f ;
			m_isMove = !m_isMove ;*/
			if(m_stateMachine != null){
				m_stateMachine.Update(deltaTime);
			}
		}
		
		/// <summary>
		/// Raises the message event.
		/// </summary>
		/// <param name='message'>
		/// Message.
		/// </param>
		public void OnMessage(EventMessageBase message){
			switch((EnitityAction)message.eventMessageAction){
				case EnitityAction.ENITITY_ACTION_FIGHT:{
					EventMessageFight fightMessage = message as EventMessageFight ;
					common.debug.GetInstance().AppCheckSlow(fightMessage);
					CCreature scrCreature = EnitityMgr.GetInstance().GetEnitity(fightMessage.scrCreatureId);
					CCreature destCreature = EnitityMgr.GetInstance().GetEnitity(fightMessage.destCreatureId);
					// some one hurt me
					if(destCreature == this){
						BeAttack();
						EnitityType enitityType = scrCreature.GetEnitityType();
						
						//
						switch(enitityType){
						case EnitityType.ENITITY_TYPE_MONSTER:{
							CMonster scrPet = scrCreature as CMonster ;
							common.debug.GetInstance().AppCheckSlow(scrPet);
							m_data.gateBlood -= scrPet.attack ;
							if(m_data.gateBlood < 0)
								m_data.gateBlood = 0 ;
							gameGlobal.g_fightSceneUI.SetBloodProgressBar(m_data.gateBlood);
							//death
							if(m_data.gateBlood <= 0){
								if(GetEnitityAiState() == AIState.AI_STATE_DOOR_NARMOL){
									CCityDoorBreakState state = new CCityDoorBreakState();
									CityDoorBreakStateData doorBreakStateData = new CityDoorBreakStateData();
									//m_cityAIData = new CityNormalStateData();
									m_cityAIData = doorBreakStateData ;
									m_stateMachine.SetState(state);
									GameObject gateOb = m_renderObject.transform.Find("gate").gameObject;
									GameObject.Destroy(gateOb);
								}
							}
							//fight back
							else{
								
							}
						}
							break;
						}
					}
				}
				break ;
			}

		}
		
		/// <summary>
		/// Gets the render object.
		/// </summary>
		/// <returns>
		/// The render object.
		/// </returns>
		public GameObject GetRenderObject(){
			return m_renderObject ;
		}
		
		/// <summary>
		/// Gets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		public int id{
			get{
				return m_data.id ;
			}
		}
		
		public int blood{
			get{
				return m_data.gateBlood ;
			}
		}
		
		public int maxBlood{
			get{
				return m_data.gateMaxBlood ;
			}
		}
		
		public int attack{
			get{
				return m_data.attack ;
			}
		}
		
		public int level{
			get{
				return m_data.level ;
			}
		}
		
		/// <summary>
		/// Gets the type of the enitity.
		/// </summary>
		/// <returns>
		/// The enitity type.
		/// </returns>
		public EnitityType GetEnitityType(){
			return EnitityType.ENITITY_TYPE_CITY ;
		}
		
		//
		/// <summary>
		/// Gets the near attack position
		/// </summary>
		public Vector3 GetAttackPos(Vector3 scrPos){
			float dis = float.MaxValue ;
			float temp ;
			Vector3 pos = Vector3.one;
			Transform trans = m_renderObject.transform.FindChild("AttackPoint");
			for(int i = 0; i < trans.childCount; ++i){
				temp = scrPos.y - trans.GetChild(i).position.y;
				if(temp < 0.0f)
					temp = -temp ;
				if(temp < dis){
					dis = temp ;
					pos = trans.GetChild(i).position ;
				}
			}
			
			return pos ;
		}
		
		
		public void Release(){
			m_data = null;
			m_stateMachine.Release();
			m_stateMachine = null ;
			MonoBehaviour.Destroy( m_renderObject ) ;
			m_renderObject = null ;
			
			/*if(m_eyeShotObjectList.Count != 0)
				m_eyeShotObjectList.RemoveRange(0,m_eyeShotObjectList.Count);
			m_eyeShotObjectList = null ;
			
			if(m_attackAreaObjectList.Count != 0)
				m_attackAreaObjectList.RemoveRange(0,m_attackAreaObjectList.Count);
			m_attackAreaObjectList = null ;*/
		}
		
		/// <summary>
		/// Gets the state of the enitity ai.
		/// </summary>
		/// <returns>
		/// The enitity ai state.
		/// </returns>
		public AIState GetEnitityAiState(){
			CStateBase<CCity> state = m_stateMachine.GetState();
			return state.GetState(); 
		}
		
		public int GetId(){
			return m_data.id ;
		}
		
		public void Play(string name , WrapMode mode){
			Transform trans = m_renderObject.transform.FindChild("creature");
			trans.animation.wrapMode = mode ;
			trans.gameObject.animation.Play(name);
		}
		
		public ArrayList GetEyeShotList(){
			return m_eyeShotObjectList ;
		}
		
		public ArrayList GetAttackAreaList(){
			return m_attackAreaObjectList ;
		}
		
		public void BeAttack(){
			GameObject ob = gameGlobal.g_rescoureMgr.GetGameObjectResource("effect") ; //gameGlobal.g_rescoureMgr.GetGameObjectResource("object/effect/effect") as GameObject;
			GameObject sceneOb = MonoBehaviour.Instantiate(ob) as GameObject;
			sceneOb.transform.parent = GetRenderObject().transform ;
			sceneOb.transform.localPosition =  Vector3.zero ;
			sceneOb.transform.localScale = new Vector3(0.5f,0.5f,-1);//Vector3.one * 0.5f;
			sceneOb.transform.FindChild("creature").animation.wrapMode = WrapMode.Once ;
			sceneOb.transform.FindChild("creature").animation.Play("effect");
		}
		
		public EffectData GetEffectData(){
			return m_effectData ;
		}
		
		public  void	SetHp(int hp){
			m_data.gateBlood += hp ;
			if(m_data.gateBlood < 0)
				m_data.gateBlood = 0 ;
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
		public int CheckBuff(int buffMoudleID) {
			return -1 ;
		}
	}
}


