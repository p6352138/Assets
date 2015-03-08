using UnityEngine;
using System.Collections;
using GameLogic.AI;
using GameLogic.Navigation;

namespace GameEntity{
	public class CPlayer : CCreature {

		#region private Fields
		private int m_id;
		private GameObject m_go;
		private Animation m_animation;
		#endregion

		#region private Properties
		private int blood;
		private int[] attackArea;
		#endregion

		#region public Fields
		public StateMachine<CPlayer> m_stateMachine;

        public int PositionInPathGrid
        {
            get
            {
                return NavigationMgr.GetInstance().GetGrid().GetPathNodeIndex(m_go.transform.localPosition);
            }
        }
		#endregion

		public CPlayer(int id,GameObject go)
		{
			m_id = id;
			m_go = go;

			m_animation = m_go.GetComponent<Animation> ();
			if (m_animation == null) {
				Debug.LogError("the animation component is null!");
			}

			m_stateMachine = new StateMachine<CPlayer> (this);
			m_stateMachine.SetState (PlayerIdelState.GetInstance());

			blood = CPlayerCommon.PlayerBlood;
			attackArea = new int[3];
		}

		#region public function
		public int[] GetAttackArea(){
			int[] testArea = new int[8];

			int curIndex = NavigationMgr.GetInstance ().GetGrid ().GetPathNodeIndex (m_go.transform.localPosition);
			int column = NavigationMgr.GetInstance ().GetGrid ().GetColumn (curIndex);
			int row = NavigationMgr.GetInstance ().GetGrid ().GetRow (curIndex);

			testArea[0] = NavigationMgr.GetInstance ().GetGrid ().GetCellIndex (column - 1,row - 1);
			testArea[1] = NavigationMgr.GetInstance ().GetGrid ().GetCellIndex (column    ,row - 1);
			testArea[2] = NavigationMgr.GetInstance ().GetGrid ().GetCellIndex (column + 1,row - 1);
			testArea[3] = NavigationMgr.GetInstance ().GetGrid ().GetCellIndex (column + 1,row 	  );
			testArea[4] = NavigationMgr.GetInstance ().GetGrid ().GetCellIndex (column + 1,row + 1);
			testArea[5] = NavigationMgr.GetInstance ().GetGrid ().GetCellIndex (column    ,row + 1);
			testArea[6] = NavigationMgr.GetInstance ().GetGrid ().GetCellIndex (column - 1,row + 1);
			testArea[7] = NavigationMgr.GetInstance ().GetGrid ().GetCellIndex (column - 1,row    );

			int eulerIndex = (int)(m_go.transform.eulerAngles.y / 45);
			int testIndex = 5;

			for (int i = 0; i<3; i++) {

				if(eulerIndex + i >= 8)
				{
					testIndex = eulerIndex + i - 8;
				}
				else{
					testIndex = eulerIndex + i;
				}
				attackArea[i] = testArea[testIndex];
			}			

			return attackArea;
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
			if(message.eventMessageModel == EventMessageModel.eEventMessageModel_PLAY_MOVE_STATE)
			{
				m_stateMachine.ChangeState(PlayerWalkState.GetInstance());
				m_stateMachine.OnMessage(message);
			}
			else if(message.eventMessageModel == EventMessageModel.eEventMessageModel_PLAY_STATE)
			{
				if(message.eventMessageAction == (int)EnitityCommon.EnitityAction.ENITITY_ACTION_FIGHT_FINISH)
				{
					m_stateMachine.ChangeState(PlayerIdelState.GetInstance());
					m_stateMachine.OnMessage(message);
				}
				else{
					blood -= message.eventMessageAction;

					if(blood <= 0)
					{
						m_stateMachine.ChangeState(PlayerDeathState.GetInstance());
						m_stateMachine.OnMessage(message);
					}
					else
					{
						m_stateMachine.ChangeState(PlayerInjuerState.GetInstance());
						m_stateMachine.OnMessage(message);
					}

					Debug.Log("ai a ~~~~~~~~~ blood == " + blood.ToString());
				}
			}
			else if(message.eventMessageModel == EventMessageModel.eEventMessageModel_PLAY_ATTACK_STATE)
			{
				m_stateMachine.ChangeState(PlayerAttackState.GetInstance());
			}
		}

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
			m_animation.wrapMode = mode;
			m_animation.Play (name);
		}

		public void Play(PlayerPlayAnimation type,WrapMode mode)
		{
			string name = "";
			m_animation.wrapMode = mode;

			if (type == PlayerPlayAnimation.IDEL) {
				name = "idle";
			}
			else if (type == PlayerPlayAnimation.ATTACK) {
				name = "attack";
			}
			else if (type == PlayerPlayAnimation.RUN) {
				name = "run";
			}
			else if (type == PlayerPlayAnimation.INJURT) {
				name = "injurt";
			}
			else if (type == PlayerPlayAnimation.DEATH) {
				name = "death";
			}

			m_animation.Play (name);
		}
		
		public int GetId()
		{
			return m_id;
		}

		
		public EnitityType GetEnitityType()
		{
			return EnitityType.ENTITY_PLAYER;
		}
		#endregion
	}
}
