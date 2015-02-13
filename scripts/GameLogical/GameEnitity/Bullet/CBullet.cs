using UnityEngine;
using System.Collections;
using GameEvent ;
using GameLogical.GameEnitity.AI ;
namespace GameLogical.GameEnitity{
	public class CBullet : CCreature
	{
		public  			BulletData				m_data 				;
		public			 	GameObject				m_renderObject 		;
		public   		    StateMachine<CBullet>   m_stateMachine 		;
		public 				Vector3					m_lastGoPos			;
		//protected			MonsterNormalBrain		m_brain				;
		
		public	void SetColor(Color color)
		{
			if(m_renderObject != null)
				m_renderObject.transform.FindChild("creature").gameObject.renderer.material.color = color ;
		}

		public void Init(BulletData data){
			m_data = data ;

		}
		
		public void Think(){
			
		}
		public void Update(float deltaTime){
			if(m_stateMachine != null){
				m_stateMachine.Update(deltaTime);
			}
		}
		public void OnMessage(EventMessageBase message){
			m_stateMachine.OnMessage(message);
		}
		public EnitityType GetEnitityType(){
			return EnitityType.ENITITY_TYPE_BULLET ;
		}
		
		public AIState  GetEnitityAiState(){
			CStateBase<CBullet> state = m_stateMachine.GetState();
			return state.GetState();
		}
		
		public void Release(){
			MonoBehaviour.Destroy( m_renderObject ) ;
			m_renderObject = null ;
		}
		public GameObject GetRenderObject(){
			return m_renderObject ;
		}
		public void Play(string name,WrapMode mode){
			Transform trans = m_renderObject.transform.FindChild("creature");
			if(trans != null){
				trans.animation.wrapMode = mode ;
				trans.gameObject.animation.Play(name);
			}
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
		public void	SetHp(int hp){
			
		}
		public EffectData GetEffectData(){
			return null ;
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
			return -1 ;
		}

		public void LoadObjectCallBack(Object ob, MGResouce.LoadCreatureData tran){
			if(ob != null){
				m_renderObject = MonoBehaviour.Instantiate(ob) as GameObject ;
				m_renderObject.transform.position = tran.pos ;

				m_stateMachine = new StateMachine<CBullet>(this);
				m_stateMachine.SetState(new BulletMove());

				Play("effect",WrapMode.Loop);
			}
		}
	}
}


