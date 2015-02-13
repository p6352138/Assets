using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using AppUtility;
using GameEvent ;
using GameLogical.GameEnitity;
using common ;
using GameLogical.GameEnitity.AI ;
using GameLogical.GameSkill.Skill ;
using GameLogical.GameSkill.Buff  ;

namespace GameLogical.GameLevel{
	class GameLevelMgr : Singleton<GameLevelMgr>, IEvent
	{
		public GameObject	m_SceneObject	;
		//public GameObject   m_smoothObject  ;
		public Vector3[]	m_monsterBrithPointArr	;
		public int[,]       m_monsterTeamPoint	=	{
			{1,3,5,6,4,7,9,10,13,14,17,18,16,19},
			{1,3,5,6,9,10,13,14,12,15,17,18,16,19},
			{1,2,5,6,4,7,9,10,8,11,13,14,17,18},
			{1,3,5,7,9,10,8,11,13,14,12,15,17,18},
			{1,2,4,6,9,10,13,14,12,15,17,18,16,19},
			{1,2,5,6,9,10,13,14,12,15,17,18,16,19}
		};
		public static int   TEAM_POINT_NUM = 6 ;
		public Vector3[]	m_petBrithPointArr		;
		public Vector3[]	m_enemyPetBrithPoinArr	;
		public List<List<CCreature>> m_monsterList ;
		public int			m_escapeNum		;
		public int			m_monsterNum	;
		public int 			m_killMonsterNum;
		public List<int>    m_killMonsterIdList ;
		public int          m_curWave		;
		public int          m_curWaveMonsterNum ;
		public int 			m_levelID		;
		
		public bool         m_isEnd         ;
		
		public int 			m_bossHp		= -1;
		public int 			m_bossLastHp	= -1;
		
		public string 		m_taskId		= "";
		
		public StateMachine<Object> m_levelStateMachin;
		
		public bool			m_canSmooth		= false ;

		public int			m_bossLine		= -1	;

		public bool			m_isStop		= false ;

		public bool 		m_isSkip		= false ;

		public LevelType    m_levelType		= LevelType.LEVEL_TYPE_NULL;

		public bool			m_isFristTime	;

		public int			m_pvpCreatureTotalNum = 0 ;


		public void Init(){
			EventMgr.GetInstance().AddLinsener(this);
			m_monsterBrithPointArr = new Vector3[4] ;
			m_monsterBrithPointArr[0] = new Vector3(125.0f,55.0f,55.0f);
			m_monsterBrithPointArr[1] = new Vector3(125.0f,45.0f,45.0f);
			m_monsterBrithPointArr[2] = new Vector3(125.0f,35.0f,35.0f);
			m_monsterBrithPointArr[3] = new Vector3(125.0f,25.0f,25.0f);

			
			m_petBrithPointArr = new Vector3[4] ;
			m_petBrithPointArr[2] = new Vector3(35.5f,55.0f,55.0f);
			m_petBrithPointArr[3] = new Vector3(20.0f,20.0f,20.0f);
			m_petBrithPointArr[0] = new Vector3(48.5f,45.0f,45f);
			m_petBrithPointArr[1] = new Vector3(43f,30.0f,30.0f);

			m_enemyPetBrithPoinArr = new Vector3[4] ;
			m_enemyPetBrithPoinArr[2] = new Vector3(94.5f,55.0f,55.0f);
			m_enemyPetBrithPoinArr[3] = new Vector3(105.0f,20.0f,20.0f);
			m_enemyPetBrithPoinArr[0] = new Vector3(81.5f,45.0f,45.0f);
			m_enemyPetBrithPoinArr[1] = new Vector3(87,30.0f,30.0f);

			m_monsterList = new List<List<CCreature>>();
			
			m_isEnd = false ;
			
			m_levelStateMachin = new StateMachine<Object>(null);
			
			m_killMonsterIdList = new List<int>();
		}
		
		//
		public void Update(float deltaTime){
			if(m_isStop == true)
				return ;

			if(this.m_levelStateMachin!=null){
				this.m_levelStateMachin.Update(deltaTime);
			}
		}
		
		//
		public void OnMessage(EventMessageBase message){
			if(message.eventMessageModel == EventMessageModel.eEventMessageModel_Level){
				if(message.eventMessageAction == (int)LevelMessageAction.LEVEL_MESSAGE_ACTION_GUIDE_STOP){
					m_isStop = !m_isStop ;
				}
			}
			this.m_levelStateMachin.OnMessage(message);
		}
		
		//
		public void LoadLevel(int levelID){
			if(levelID != -1){
				m_levelID = levelID ;
				this.m_levelStateMachin.SetState(LevelBeginState.getInstance());
			}
			else{
				m_levelID = levelID ;
				this.m_levelStateMachin.SetState(LevelBeginState.getInstance());
			}
		}
		
		public void LoadPvP(){
			m_levelType = LevelType.LEVEL_TYPE_PVP ;
			this.m_levelStateMachin.SetState(LevelPvPBeginState.getInstance());
		}
		
		//end game
		public void EndGame(){
			if(m_isEnd)
				return ;
			this.m_levelStateMachin.ChangeState(LevelEndState.getInstance());
		}

		public void DestroySceneBG()
		{
			if(m_SceneObject!=null){
				MonoBehaviour.Destroy(m_SceneObject);
				m_SceneObject = null ;
			}
		}
		
		public void DestroyLevel(){
			EnitityMgr.GetInstance().DestroyEnitityAll();
//			MonoBehaviour.Destroy(m_SceneObject);
//			m_SceneObject = null;

			EventMessageBase message = new EventMessageBase() ;
			message.eventMessageModel = EventMessageModel.eEVentMessageModel_Smooth ;
			message.eventMessageAction= (int)SmoothAction.SMOOTH_DESTROY ;
			//CLineSmoothMgr.GetInstance().OnMessage(message);

			//gameGlobal.g_PvPFightSceneUI.
//			gameGlobal.g_fightSceneUI.hide();
//			gameGlobal.g_PvPFightSceneUI.hide();
			m_monsterList = new List<List<CCreature>>();
			m_killMonsterIdList.Clear();
			m_escapeNum	= 0	;
			m_monsterNum = 0	;
			m_killMonsterNum = 0;
			m_levelID = 0		;
			m_curWave = 0 ;
			m_curWaveMonsterNum = 0 ;
			m_bossLastHp = -1 ;
			m_bossHp = -1 ;
			m_taskId = "" ;
			m_bossLine = -1 ;
			m_isEnd = false ;
			m_isSkip = false;
			m_levelType = LevelType.LEVEL_TYPE_NULL ;
			m_isFristTime = false ;
			this.m_levelStateMachin.SetState(null);
			Time.timeScale = 1.0f ;
			//skill
			SkillMgr.GetInstance().Clear();
			//buff
			CBuffMgr.GetInstance().Clear();

			MGResouce.BundleMgr.Instance.CleanCreature();

			m_pvpCreatureTotalNum = 0 ;

			GameDataCenter.GetInstance().m_curSelectSkillIndex = -1 ;
		}
	}
}


