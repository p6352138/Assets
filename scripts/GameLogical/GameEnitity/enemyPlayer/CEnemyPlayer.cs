using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameLogical.GameEnitity.AI ;
using GameEvent ;
using GameLogical.GameSkill ;
using GameLogical.GameSkill.Skill;
using GameLogical.GameLevel ;
using common ;
using Bass2D;
using GameLogical.GameSkill.Buff ;

namespace GameLogical.GameEnitity{
	public class CEnemyPlayer : CCreature{
		
		public      	FightCreatureData	 	 m_data ;
		public			EffectData				 m_effectData	;
		public	 		GameObject 			renderObject ;
		//protected 		PlayerData 			m_data ;
		public		List<int> 			m_skillList	 ;
		public		int					m_destID	 ;
		public			int					 m_curSelectSkillIndex = -1;
		public			int					 m_usingSkillIndex = -1 ;
		public      CCreature                m_targetCreature;
		public   	EnemyPlayerAIDataBass	 m_enemyPlayerAIData;

		public			int				m_skillAnger;
		public			int				m_buffAnger;

		float 		appearTime 		= 	-1.0f;
		float 		appearMaxTime 	= 	8f;
		
		public   StateMachine<CEnemyPlayer>   	m_stateMachine ;

		private List<CCreature> selectEnityMsg = new List<CCreature>();
		public			Vector3			skillPos = new Vector3();

		public			Vector3			playerStandPos = new Vector3(100f, 2f, 0.31f);

		public enum EnemyPlayerAIState{
			Static = 0,
			Fight
		}

		public EnemyPlayerAIState	playerState	 = EnemyPlayerAIState.Static;

		public	void SetColor(Color color)
		{
			if(renderObject != null)
				renderObject.transform.FindChild(gameGlobal.CREATURE).gameObject.renderer.material.color = color ;
		}

		public int attack{
			get{
				return m_data.attack ;
			}
		}

		/// <summary>
		/// Init the specified data.
		/// </summary>
		/// <param name='data'>
		/// Data.
		/// </param>
		public void Init(FightCreatureData data){
			m_skillList = new List<int>();
			m_data = data ;
			m_effectData = new EffectData();
			//int id = SkillMgr.GetInstance().CreateSkill(1,m_data.id);
			//m_skillList.Add(id);

			string[] reslut = GameDataCenter.GetInstance().pvpPlayerInfo.angerSkill.Split(',');
			m_skillAnger = SkillMgr.GetInstance().CreateSkill(SkillType.SKILL_TYPE_ENEMY_PLAYER, 
			                                                  int.Parse(reslut[0]), m_data.id);
			m_buffAnger = int.Parse(reslut[1]);

			m_enemyPlayerAIData = new EnemyPlayerAIDataBass();
		}


		public void InitPvPSkill(){
			/*
			for(int i = 0; i < GameDataCenter.GetInstance().pvpOtherFightPackData.playerSkillIds.Count; ++i){
				//SkillSimpleDto skillDto = GameDataCenter.GetInstance().GetPlayerSkillData(GameDataCenter.GetInstance().petFightPackData.playerSkillIds[i]);
				int id = SkillMgr.GetInstance().CreateSkill(SkillType.SKILL_TYPE_ENEMY_PLAYER,GameDataCenter.GetInstance().pvpOtherFightPackData.playerSkillIds[i].id,m_data.id);
				m_skillList.Add(id) ;
			}*/
		}

		public void LoadObjectCallBack(Object ob, MGResouce.LoadCreatureData tran){
			if(ob != null){
				renderObject = MonoBehaviour.Instantiate(ob) as GameObject ;
				renderObject.name = m_data.id.ToString();
				
				renderObject.SetActive(true);
				
				renderObject.transform.localPosition = playerStandPos;
				m_stateMachine = new StateMachine<CEnemyPlayer>(this);
				playerState = EnemyPlayerAIState.Static;
				DisAppear();
				
			}
		}

		public GameObject yingzi;
		public void Appear()
		{
			/*appearTime = 0f;
			playerState = EnemyPlayerAIState.Fight;

			renderObject.transform.FindChild("root").gameObject.SetActive(false);

			GameObject prefabs = gameGlobal.g_rescoureMgr.GetGameObjectResource("shanxianqian");
			GameObject ob = MonoBehaviour.Instantiate(prefabs) as GameObject;
			ob.transform.localPosition = this.GetRenderObject().transform.localPosition;
			ob.transform.FindChild("creature").animation.wrapMode = WrapMode.Loop ;
			ob.transform.FindChild("creature").animation.Play("effect");
			ShanXian spShanXian = ob.transform.FindChild("creature").GetComponent("ShanXian") as ShanXian;
			spShanXian.type = EnitityType.ENITITY_TYPE_ENEMY_CHARACTER ;

			int k = GameDataCenter.GetInstance().playerData.professionId;

			GameObject prefab = gameGlobal.g_rescoureMgr.GetGameObjectResource("yingzi" + 
			              GameDataCenter.GetInstance().pvpPlayerInfo.sex +
			              GameDataCenter.GetInstance().pvpPlayerInfo.professionId);
			yingzi = MonoBehaviour.Instantiate(prefab) as GameObject;
			yingzi.name = "yingzi";
			//yingzi.transform.parent = GetRenderObject().transform.FindChild(gameGlobal.CREATURE);
			yingzi.transform.position = GetRenderObject().transform.FindChild(gameGlobal.CREATURE).position;
			yingzi.transform.rotation =  GetRenderObject().transform.FindChild(gameGlobal.CREATURE).rotation ;
			//yingzi.transform.rotation = Quaternion.LookRotation(Vector3.back) ;
			//yingzi.transform.FindChild("yingzi").localPosition = new Vector3(0, 0, -1);

			List<CCreature> petlist = EnitityMgr.GetInstance().GetMonsterList();
			for(int i = 0; i < petlist.Count; i++)
			{
				SingleBuffCreateData buffData = new SingleBuffCreateData();
				List<int> buffList = new List<int>();
				buffList.Add(m_buffAnger);
				buffData.buffModuleID = buffList;
				buffData.rangeType = BuffRangeType.BUFF_RANGE_SINGLE ;
				buffData.srcCreatureID = m_data.id;
				CEnemyPet pet = petlist[i] as CEnemyPet;
				if(pet.GetEnitityAiState() == AIState.AI_STATE_DEATH)
					continue ;
				buffData.destCreatureID= pet.id;
				CBuffMgr.GetInstance().CreateBuff(buffData);
			}*/

//			yingzi.transform.FindChild("yingzi").GetComponent<UISprite>().MakePixelPerfect();
		}

		public void DestroyYingzi()
		{
			if(yingzi != null)
			{
				yingzi.gameObject.SetActive(false);
				MonoBehaviour.Destroy(yingzi.gameObject);
				yingzi = null;
			}
		}

		public void ShanXian()
		{
			if(yingzi != null)
			{
				yingzi.gameObject.SetActive(false);
				MonoBehaviour.Destroy(yingzi.gameObject);
				yingzi = null;
			}
			renderObject.transform.localPosition = new Vector3(100, 42, 42);
			GetRenderObject().transform.FindChild("root").gameObject.SetActive(false);
			
			GameObject prefabs = gameGlobal.g_rescoureMgr.GetGameObjectResource("shanxianhou");
			GameObject ob = MonoBehaviour.Instantiate(prefabs) as GameObject;
			ob.transform.localPosition = this.GetRenderObject().transform.localPosition;
			ob.transform.FindChild("creature").animation.wrapMode = WrapMode.Loop ;
			ob.transform.FindChild("creature").animation.Play("effect");
			ShanXian spShanXian = ob.transform.FindChild("creature").GetComponent("ShanXian") as ShanXian;
			spShanXian.type = EnitityType.ENITITY_TYPE_ENEMY_CHARACTER ;

			//renderObject.transform.localPosition = new Vector3(43, 52, 0.31f);
		}

		public void ShanXianHou()
		{
			/*if(yingzi != null)
			{
				MonoBehaviour.Destroy(yingzi.gameObject);
				yingzi = null;
			}*/
			appearTime = 0f;
			renderObject.SetActive(true);

			if(playerState == EnemyPlayerAIState.Fight)
			{
				renderObject.transform.localPosition = new Vector3(100, 42, 42);
			}
			renderObject.transform.FindChild("root").gameObject.SetActive(true);

			appearTime = 0f;
			if(playerState == EnemyPlayerAIState.Static)
				m_stateMachine.SetState(EnemyPlayerStaticStandState.getInstance());
			else
				m_stateMachine.SetState(EnemyPlayerStaticSkillState.getInstance());
		}

		public void DisAppear()
		{
			playerState = EnemyPlayerAIState.Static;
			renderObject.SetActive(true);

			m_stateMachine.SetState(EnemyPlayerStaticStandState.getInstance());
			Play("stand",WrapMode.Loop);
			appearTime = -1.0f;
			
			renderObject.transform.localPosition = playerStandPos;

			//renderObject.SetActive(false);
		}
		/// <summary>
		/// Think this instance.
		/// </summary>
		public void Think(){
			
		}

		//eye shot area
		public float eyeShotArea{
			get{
				return 10000;//m_data.eyeShotArea ;
			}
			
		}

		//get id
		public int id{
			get{
				return m_data.id ;
			} 
		}

		//get speed
		public float speed{
			get{
				return 160;//m_data.moveSpeed + m_effectData.speed + m_data.moveSpeed * m_effectData.speedPercent / 100;
			}
		}

		//get attack area
		public float attackArea{
			get{
				return 4000;//m_data.attackArea ;
			}
		}


		public List<CCreature> SelectEnityMsg{
			set{
				selectEnityMsg.Clear();
				for(int i = 0; i<value.Count; ++i){
					selectEnityMsg.Add(value[i]);
				}

			}
			get{
				return selectEnityMsg;
			}
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

			if(appearTime != -1.0f){
				appearTime += deltaTime;
				if(appearTime > appearMaxTime && playerState == EnemyPlayerAIState.Fight){
					DisAppear();
					appearTime = -1.0f;
				}
			}
		}
		
		/// <summary>
		/// Raises the message event.
		/// </summary>
		/// <param name='message'>
		/// Message.
		/// </param>
		public void OnMessage(EventMessageBase message){
			if(message.eventMessageModel == EventMessageModel.eEventMessageModel_Enitity){
				switch((EnitityAction) message.eventMessageAction){
				//select skill target
				case EnitityAction.ENITITY_ACTION_SELECT_ENITITY:{
					EventMessageEnititySelect selectMessage = message as EventMessageEnititySelect ;
					SelectEnityMsg = selectMessage.id as List<CCreature>;
					skillPos	= selectMessage.pos;

					CSkillBass skill = SkillMgr.GetInstance().GetSkill(m_skillList[m_curSelectSkillIndex]);
					SkillMoudleData skillMoudleData = (SkillMoudleData)fileMgr.GetInstance().GetData(skill.GetSkillData().moudleID,CsvType.CSV_TYPE_SKILL);
					//if(skillMoudleData.useObject == 1){
					if(skill.canUse()){
						if(playerState == EnemyPlayerAIState.Static){
							m_stateMachine.SetState(EnemyPlayerStaticSkillAciton.getInstance());
						}
						else if(playerState == EnemyPlayerAIState.Fight){
							m_usingSkillIndex = m_curSelectSkillIndex ;
							gameGlobal.g_fightSceneUI.UseSkill(m_curSelectSkillIndex);
							
							EventMessageEnititySelect eventData = new EventMessageEnititySelect() ;
							eventData.id = SelectEnityMsg;
							eventData.pos = skillPos;
							skill.useSkill(eventData);
							m_destID = -1 ;
							m_usingSkillIndex = -1 ;
							m_curSelectSkillIndex = -1	;
						}
					}
				}
					break ;
					
				case EnitityAction.ENITITY_ACTION_SKILL_FINISH:{
					if(this.m_stateMachine!=null){
						this.m_stateMachine.OnMessage(message);
					}
					//Play("stand",WrapMode.Loop);
				}
					break ;
					
				/*case EnitityAction.ENITYTY_ACTION_SKILL:{
					if(this.m_stateMachine!=null){
						if(playerState == EnemyPlayerAIState.Fight)
							this.m_stateMachine.OnMessage(message);
					}
				}
					break ;*/
					
				case EnitityAction.ENITITY_ACTION_SELECT_SKILL:{
					EventMessageSelectSkill selectSkill = message as EventMessageSelectSkill ;
					m_curSelectSkillIndex = selectSkill.index ;
				}
					break ;
					
				case EnitityAction.ENITITY_ACTION_CANCEL_SELECT_SKILL:{
					m_curSelectSkillIndex = -1 ;
				}
					break ;
				}
			}
			else if(message.eventMessageModel == EventMessageModel.eEventMessageModel_Level){
				if(message.eventMessageAction == (int)LevelMessageAction.LEVEL_MESSAGE_ACTION_AUTO_SKILL){
					if(GameDataCenter.GetInstance().autoSkill == true){
						m_stateMachine.SetState(EnemyPlayerAutoSkillState.getInstance());
					}
					else{
						m_stateMachine.SetState(null);
					}
				}
			}
			else if(message.eventMessageModel == EventMessageModel.eEventMessageModel_Skill){
				switch((SkillEventMessageAction)message.eventMessageAction){
				case SkillEventMessageAction.SKILL_ACTION_USE_SKILL:{
					EventMessageUseSkill data = message as EventMessageUseSkill;
					if(data != null){
						GameDataCenter.GetInstance().pvpPlayerInfo.engry =  data.costMp * 2;
						//EnitityMgr.GetInstance().Energy += ;
					}
				}
					break ;
				}
			}
			m_stateMachine.OnMessage(message);
		}
		
		/// <summary>
		/// Gets the type of the enitity.
		/// </summary>
		/// <returns>
		/// The enitity type.
		/// </returns>
		public EnitityType GetEnitityType(){
			return EnitityType.ENITITY_TYPE_ENEMY_CHARACTER ;
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
			m_data = null;
			if(renderObject != null)
				MonoBehaviour.Destroy( renderObject ) ;
			renderObject = null ;
			
			for(int i = 0; i < m_skillList.Count; ++i){
				SkillMgr.GetInstance().RemoveSkill(m_skillList[i]);
			}
			m_skillList.Clear();
			m_skillList = null ;
		}
		
		/// <summary>
		/// Gets the render object.
		/// </summary>
		/// <returns>
		/// The render object.
		/// </returns>
		public GameObject GetRenderObject(){
			return renderObject ;
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
			Transform trans = renderObject.transform.FindChild("root/creature");
 			trans.animation.wrapMode = mode ;
			trans.gameObject.animation.Play(name);
		}
		
		/// <summary>
		/// Gets the eye shot list.
		/// </summary>
		/// <returns>
		/// The eye shot list.
		/// </returns>
		public ArrayList GetEyeShotList(){
			return null ;
		}
		
		/// <summary>
		/// Gets the attack area list.
		/// </summary>
		/// <returns>
		/// The attack area list.
		/// </returns>
		public ArrayList GetAttackAreaList(){
			return null ;
		}
		
		/// <summary>
		/// Gets the identifier.
		/// </summary>
		/// <returns>
		/// The identifier.
		/// </returns>
		public int  GetId(){
			return m_data.id ;
		}
		
		
		public EffectData GetEffectData(){
			return m_effectData ;
		}
		
		public void SetHp(int hp){
			
		}
		
		public bool AddBuff(int buffMoudleID,int buffID){
			return false ;
		}

		public int CheckBuff(int buffMoudleID){
			return -1;
		}
		
		public void DelBuff(int buffID){
		}
		
		public FightCreatureData GetFightCreatureData(){
			return m_data ;
		}
		
		public void SetCrit(bool isCrit){

		}

		public CCreature FindLeastBlood(EnitityType type){
			List<CCreature> enemyPetList = null ; 
			if(type == EnitityType.ENITITY_TYPE_ENEMY_PET){
				enemyPetList = EnitityMgr.GetInstance().GetMonsterList() ;
			}
			else if(type == EnitityType.ENITITY_TYPE_PET){
				enemyPetList = EnitityMgr.GetInstance().GetPetList() ;
			}

			int tempBlood = int.MaxValue ;
			CCreature creature = null ;
			for(int i = 0; i<enemyPetList.Count; ++i){
				if(enemyPetList[i].GetFightCreatureData().blood < tempBlood){
					tempBlood = enemyPetList[i].GetFightCreatureData().blood ;
					creature  = enemyPetList[i] ;
				}
			}
			return creature ;
		}

		public CCreature FindSuitCreature(SkillMoudleData skillData){
			CCreature creature = null ;
			bool 	  isSame   = false;
			List<CCreature> creatureList = null ;
			if(skillData.useObject == 1){
				creatureList = EnitityMgr.GetInstance().GetPetList();
				CPet pet ;
				for(int i = 0; i<creatureList.Count; ++i){
					pet = creatureList[i] as CPet;
					for(int j = 0; j<pet.m_buffList.Count; ++j){
						if(pet.m_buffList[j] == skillData.skillType){
							isSame = true ;
							break ;
						}
					}
					if(isSame == false){
						creature = pet ;
						break ;
					}
				}
			}
			else if(skillData.useObject == 2){
				creatureList = EnitityMgr.GetInstance().GetMonsterList();
				CEnemyPet pet ;
				for(int i = 0; i<creatureList.Count; ++i){
					pet = creatureList[i] as CEnemyPet;
					for(int j = 0; j<pet.m_buffList.Count; ++j){
						if(pet.m_buffList[j] == skillData.skillType){
							isSame = true ;
							break ;
						}
					}
					if(isSame == false){
						creature = pet ;
						break ;
					}
				}
			}

			return creature ;
		}
	}
	
}


