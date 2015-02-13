using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using GameLogical.GameEnitity.AI ;
using GameEvent;
using GameLogical.GameSkill ;
using GameLogical.GameSkill.Buff ;
using GameLogical.GameSkill.Skill;
using common ;
using GameLogical.GameLevel ;

namespace GameLogical.GameEnitity{
	public class CEnemyPet : CCreature
	{
		public      FightCreatureData	 	 m_data ;
		public      StateMachine<CEnemyPet>  m_stateMachine ;
		public		StateMachine<CEnemyPet>  m_skillStateMachine ;
		//protected	PetBrain			 	 m_brain		;
		public   	PetAIDataBass		 	 m_petAIData;
		public      GameObject               m_object       ;
		public      GameObject				 m_bloodBar     ;
		//public      GameObject				 m_mpBar		;
		
		public		GameObject				 talkBlink		;//宠物说话
		private		float					 talkTime = -1.0f;//说话时间
		private		const	float			 talkMaxTime = 3;//说话显示时间

		public		EffectData				 m_effectData	;
		public      CCreature                m_targetCreature;
		
		//skill
		public      List<int>          		 m_skillList    ;
		public		int						 m_mainSkillId	;
		public		float					 m_mainSkillCd	;
		public		float					 m_mainSkillFullTime ;
		public      int						 m_curUsingSkill;
		//buff
		public 		List<int>				 m_buffList ;
		
		public      Vector3[]				 m_attackPosArr;
		public		List<int>				 m_beLockCreatureIDList ;
		
		public		float					 m_sharkRed    ;
		
		public		float					 m_sharkTotalTime;
		public		float					 m_sharkCurTime  ;
		
		public 		bool					 m_isCrit		;

		private 	Dictionary<string,Object> m_objectList	;

		public	void SetColor(Color color)
		{
			if(m_object != null)
				m_object.transform.FindChild(gameGlobal.CREATURE).gameObject.renderer.material.color = color ;
		}

		public void Init(FightCreatureData data){
			m_data = data ;
			
			m_effectData = new EffectData();
			
			//skill
			m_skillList = new List<int>();

			//buff
			m_buffList = new List<int>();
			
			//attack position list
			m_attackPosArr = new Vector3[5];
			//lock creature list
			m_beLockCreatureIDList = new List<int>();
			
			//
			m_sharkRed = -1.0f ;
			
			m_sharkTotalTime = 0.6f ;

			m_objectList = new Dictionary<string, Object>();
		}

		public void InitSkill(){
			SkillMoudleData mainSkillMoudleData = (SkillMoudleData)fileMgr.GetInstance().GetData(m_data.skillList[0],CsvType.CSV_TYPE_SKILL);

			m_mainSkillId = SkillMgr.GetInstance().CreateSkill(SkillType.SKILL_TYPE_PET,m_data.skillList[0],GetId());
			m_mainSkillCd = 0.0f ;
			m_mainSkillFullTime = mainSkillMoudleData.coolDown * 0.001f;

			for(int i = 1; i<m_data.skillList.Count; ++i){
				SkillMoudleData skillMoudleData = (SkillMoudleData)fileMgr.GetInstance().GetData(m_data.skillList[i],CsvType.CSV_TYPE_SKILL);
				if(skillMoudleData.environment == 1){
					if(GameLevelMgr.GetInstance().m_levelType != LevelType.LEVEL_TYPE_PVP){
						continue ;
					}
				}
				else if(skillMoudleData.environment == 3){
					if(GameLevelMgr.GetInstance().m_levelType == LevelType.LEVEL_TYPE_PVP){
						continue ;
					}
				}

				if(skillMoudleData.activateType == 1){
					int id = SkillMgr.GetInstance().CreateSkill(SkillType.SKILL_TYPE_ENEMY_PET,m_data.skillList[i],GetId());
					m_skillList.Add(id);
				}
				else if(skillMoudleData.activateType == 2){
					SingleBuffCreateData singleBuff = new SingleBuffCreateData() ;
					singleBuff.buffModuleID = skillMoudleData.buffer	;
					singleBuff.srcCreatureID = m_data.id ;
					singleBuff.destCreatureID= m_data.id ;
					singleBuff.rangeType = BuffRangeType.BUFF_RANGE_PASSTIVITY_FIGHT ;
					CBuffMgr.GetInstance().CreateBuff(singleBuff);
				}
				else if(skillMoudleData.activateType == 3){
					SingleBuffCreateData singleBuff = new SingleBuffCreateData() ;
					singleBuff.buffModuleID = skillMoudleData.buffer	;
					singleBuff.buffRate     = skillMoudleData.buffRate  ;
					singleBuff.srcCreatureID = m_data.id ;
					singleBuff.destCreatureID= m_data.id ;
					singleBuff.rangeType = BuffRangeType.BUFF_RANGE_PASSTIVITY ;
					CBuffMgr.GetInstance().CreateBuff(singleBuff);
				}
			}
		}

		public void LoadObjectCallBack(Object ob, MGResouce.LoadCreatureData tran){
			m_object = MonoBehaviour.Instantiate(ob) as GameObject ;
			m_object.name = m_data.id.ToString();

			m_object.transform.position = tran.pos ;
			m_object.transform.FindChild(gameGlobal.CREATURE_ROOT).localRotation = Quaternion.LookRotation(Vector3.back) ;

			MGResouce.LoadUIData loadData = new MGResouce.LoadUIData();
			loadData.packName = "FightSceneUI" ;
			loadData.name	  = "BloodYellowBar" ;
			loadData.isMask = false;
			loadData.LoadUICallBack = LoadOtherObjectCallBack ;
			MGResouce.BundleMgr.Instance.LoadUI(loadData);

			loadData = new MGResouce.LoadUIData();
			loadData.packName = "FightSceneUI" ;
			loadData.name	  = "talk" ;
			loadData.isMask = false;
			loadData.LoadUICallBack = LoadOtherObjectCallBack ;
			MGResouce.BundleMgr.Instance.LoadUI(loadData);

			loadData = new MGResouce.LoadUIData();
			loadData.packName = "FightSceneUI" ;
			loadData.name	  = "JumpMiss" ;
			loadData.isMask = false;
			loadData.LoadUICallBack = LoadOtherObjectCallBack ;
			MGResouce.BundleMgr.Instance.LoadUI(loadData);

			loadData = new MGResouce.LoadUIData();
			loadData.packName = "FightSceneUI" ;
			loadData.name	  = "JumpNumGreen" ;
			loadData.isMask = false;
			loadData.LoadUICallBack = LoadOtherObjectCallBack ;
			MGResouce.BundleMgr.Instance.LoadUI(loadData);

			loadData = new MGResouce.LoadUIData();
			loadData.packName = "FightSceneUI" ;
			loadData.name	  = "HeavyJumpNum" ;
			loadData.isMask = false;
			loadData.LoadUICallBack = LoadOtherObjectCallBack ;
			MGResouce.BundleMgr.Instance.LoadUI(loadData);

			loadData = new MGResouce.LoadUIData();
			loadData.packName = "FightSceneUI" ;
			loadData.name	  = "JumpNum" ;
			loadData.isMask = false;
			loadData.LoadUICallBack = LoadOtherObjectCallBack ;
			MGResouce.BundleMgr.Instance.LoadUI(loadData);
		}

		public void LoadOtherObjectCallBack(MGResouce.LoadUIData data){
			m_objectList.Add(data.name,data.ob) ;
			if(m_objectList.Count == 6){
				FinishLoad();
			}
		}

		void FinishLoad(){
			//blood bar
			GameObject obj = m_objectList["BloodYellowBar"] as GameObject ;//gameGlobal.g_rescoureMgr.GetGameObjectResource("object/UI/SceneUI/BloodBar") as GameObject;
			m_bloodBar = GameObject.Instantiate(obj) as GameObject;
			m_bloodBar.transform.parent = m_object.transform.FindChild(gameGlobal.OBJECT_POIN_TOP) ;
			m_bloodBar.transform.localPosition = Vector3.zero ;
			//m_bloodBar.transform.localScale = Vector3.one ;
			m_bloodBar.name = "BloodBar" ;
			
			//说话
			GameObject ob2 = m_objectList["talk"] as GameObject ;//gameGlobal.g_rescoureMgr.GetGameObjectResource("object/UI/SceneUI/talk") as GameObject;
			talkBlink = GameObject.Instantiate(ob2) as GameObject;
			
			talkBlink.transform.localScale = Vector3.one * 0.15f;
			talkBlink.transform.parent = m_object.transform.FindChild(gameGlobal.OBJECT_POIN_TOP) ;
			talkBlink.transform.localPosition = new Vector3(-50.0f,0.0f,0.0f) ;
			NGUITools.SetActive(talkBlink, false);

			m_petAIData = new PetAIDataBass();

			m_stateMachine = new StateMachine<CEnemyPet>(this);
			m_stateMachine.SetState(EnemyPetMoveState.getInstance());

			m_skillStateMachine = new StateMachine<CEnemyPet>(this);
			m_skillStateMachine.SetState(EnemyPetAutoUseSkill.getInstance());
			GameLevelMgr.GetInstance().m_pvpCreatureTotalNum-- ;

			InitSkill();
		}
		/// <summary>
		/// Update the specified deltaTime.
		/// </summary>
		/// <param name='deltaTime'>
		/// Delta time.
		/// </param>
		public void Update(float deltaTime){
			if(m_stateMachine != null){
				m_stateMachine.Update(deltaTime);
			}

			if(m_skillStateMachine != null){
				m_skillStateMachine.Update(deltaTime);
			}

			if(m_sharkRed != -1.0f){
				m_sharkRed += deltaTime ;
				if(m_sharkRed >= 0.166f){
					m_object.transform.FindChild(gameGlobal.CREATURE).gameObject.renderer.material.color = Color.white ;
					m_sharkRed = -1.0f ;
				}
			}
			
			if(talkTime != -1.0f){
				talkTime += deltaTime;
				if(talkTime > talkMaxTime){
					talkStop();
					talkTime = -1.0f;
				}
			}
		}
		
		public void TalkLv(List<int> talkLv, List<int> talkID)
		{
			int id = 0;
			int lv = 0;
			int randNum = UnityEngine.Random.Range(1, 100);
			for(int i = 0; i < talkLv.Count; i++)
			{
				lv += talkLv[i];
				if(lv > randNum)
				{
					id = talkID[i];
					break;
				}
			}
			if(id != 0)
			{
				TalkMoudleDate talkMoudleData = (TalkMoudleDate)common.fileMgr.GetInstance().GetData(id,common.CsvType.CSV_TYPE_TALK);
				talkStr = talkMoudleData.talkString;
				talkPlay();
			}
		}
		
		public string talkStr{
			get{
				UILabel talkLabel = talkBlink.transform.FindChild("talkStr").GetComponent("UILabel") as UILabel;
				return talkLabel.text;
			}
			set{
				UILabel talkLabel = talkBlink.transform.FindChild("talkStr").GetComponent("UILabel") as UILabel;
				if(value.Length >= 5)
				{
					string talkExpression = (value.Remove(5, value.Length - 5));
					string index = talkExpression.Remove(2, 3);
					if(index == "_0")
					{
						GameObject prefabs = gameGlobal.g_rescoureMgr.GetGameObjectResource(talkExpression);
						GameObject ob = MonoBehaviour.Instantiate(prefabs) as GameObject;
						ob.transform.parent = talkBlink.transform;
						ob.transform.localScale = Vector3.one;
						ob.transform.localPosition = new Vector3(91, 26, 0);
						ob.name = "expression";
						//						ob.transform.FindChild("creature").animation.Play("effect");
						talkLabel.text = value.Remove(0, 5);
					}
					else
						talkLabel.text = value;
				}
				else
					talkLabel.text = value;
			}
		}
		
		public void talkPlay()
		{
			if(!NGUITools.GetActive(talkBlink))
			{
				NGUITools.SetActive(talkBlink, true);
				talkBlink.GetComponent<TalkBlink>().SetSize();
			}
			talkTime = 0.0f;
		}
		
		public void talkStop()
		{
			if(NGUITools.GetActive(talkBlink))
				NGUITools.SetActive(talkBlink, false);
		}
		
		
		/// <summary>
		/// Raises the message event.
		/// </summary>
		/// <param name='message'>
		/// Message.
		/// </param>
		public void OnMessage(EventMessageBase message){
			if(message.eventMessageModel == EventMessageModel.eEventMessageModel_Enitity){
				EnitityAction action = (EnitityAction)message.eventMessageAction;
				//death
				if(EnitityAction.ENITITY_ACTION_DEATH == action){
					EventMessageDeathEnd deathMessage = message as EventMessageDeathEnd ;
					int id = int.Parse(deathMessage.ob.name);
					if(id == m_data.id){
						EnitityMgr.GetInstance().DestroyEnitity(this);
						return;				
					}
					
				}
				//weak
				else if(EnitityAction.ENITITY_ACTION_WEAK == action){
					EventMessageWeak weakMessage = (EventMessageWeak)message ;
					if(m_beLockCreatureIDList.Contains(weakMessage.id)){
						m_beLockCreatureIDList.Remove(weakMessage.id);
					}
				}
				//real fight
				else if(EnitityAction.ENITITY_ACTION_FIGHT == action){
					EventMessageFight fight = message as EventMessageFight ;
					//be hurt
					if(fight.destCreatureId == m_data.id){
						CountFight(fight);
					}
				}
				if(this.m_stateMachine!=null){
					this.m_stateMachine.OnMessage(message);
				}
			}
		}
		
		
		/// <summary>
		/// Gets the type of the enitity.
		/// </summary>
		/// <returns>
		/// The enitity type.
		/// </returns>
		public EnitityType GetEnitityType(){
			return EnitityType.ENITITY_TYPE_ENEMY_PET ;
		}
		
		/// <summary>
		/// Gets the state of the enitity ai.
		/// </summary>
		/// <returns>
		/// The enitity ai state.
		/// </returns>
		public AIState  GetEnitityAiState(){
			CStateBase<CEnemyPet> state ;
			if(m_stateMachine != null){
				state = m_stateMachine.GetState();
				return state.GetState();
			}
			else{
				return AIState.AI_STATE_NULL ;
			}

		}
		
		/// <summary>
		/// Release this instance.
		/// </summary>
		public void Release(){
			m_data = null;
			m_stateMachine.Release();
			m_stateMachine = null ;

			m_skillStateMachine.Release();
			m_skillStateMachine = null ;
			//m_brain	= null	;
			m_petAIData = null;
			MonoBehaviour.Destroy( m_object ) ;
			m_object = null ;

			MGResouce.BundleMgr.Instance.UnloadBundle("FightSceneUI","BloodYellowBar");
			MGResouce.BundleMgr.Instance.UnloadBundle("FightSceneUI","talk");
			MGResouce.BundleMgr.Instance.UnloadBundle("FightSceneUI","JumpMiss");
			MGResouce.BundleMgr.Instance.UnloadBundle("FightSceneUI","JumpNumGreen");
			MGResouce.BundleMgr.Instance.UnloadBundle("FightSceneUI","HeavyJumpNum");
			MGResouce.BundleMgr.Instance.UnloadBundle("FightSceneUI","JumpNum");
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
		/// Think about what to do
		/// change ai state
		/// </summary>
		public void Think(){
			//m_brain.ThinkAbout();
		}
		
		/// <summary>
		/// Sets the state.
		/// </summary>
		/// <param name='state'>
		/// State.
		/// </param>
		public void SetState(CStateBase<CEnemyPet> state){
			CStateBase<CEnemyPet> perState = m_stateMachine.GetState();
			perState.Release();
			perState = null ;
			m_stateMachine.SetState(state);
		}
		
		
		
				/// <summary>
		/// Executes the state again.
		/// </summary>
		public void ExecuteStateAgain(){
			CStateBase<CEnemyPet> perState = m_stateMachine.GetState();
			m_stateMachine.SetState(perState);
		}
		
		/// <summary>
		/// Play the animation
		/// </summary>
		/// <param name='name'>
		/// Name.
		/// </param>
		/// <param name='mode'>
		/// Mode.
		/// </param>
		public void Play(string name, WrapMode mode){
			Transform trans = m_object.transform.FindChild(gameGlobal.CREATURE);
			trans.animation.wrapMode = mode ;
			trans.gameObject.animation.Play(name);
		}
		
		public void Stop(){
			Transform trans = m_object.transform.FindChild(gameGlobal.CREATURE);
			trans.gameObject.animation.Stop();
		}
		
		public void Shark(){
			Transform trans = m_object.transform.FindChild(gameGlobal.CREATURE);
			trans.gameObject.SetActive(!trans.gameObject.activeSelf) ;
		}
		
		public void SharkRed(){
			if(m_sharkRed == -1.0f){
				m_sharkRed = 0.0f ;
				m_object.transform.FindChild(gameGlobal.CREATURE).gameObject.renderer.material.color = Color.red ;
			}
		}
		
		public void BeAttack(string audioPath,string beEffectPath){
			if(beEffectPath != null && beEffectPath != ""){
				GameObject ob = gameGlobal.g_rescoureMgr.GetGameObjectResource(beEffectPath) as GameObject;
				GameObject sceneOb = MonoBehaviour.Instantiate(ob) as GameObject;
				Vector3 scale = sceneOb.transform.localScale ;
				sceneOb.transform.parent = GetRenderObject().transform ;
				sceneOb.transform.localPosition =  Vector3.zero ;
				sceneOb.transform.localScale = scale;
			}
			if(audioPath != null && audioPath != ""){
				MuscClip.MusicClipMgr.GetInstance().MusicClips(audioPath,GetRenderObject());
			}
			
			GameObject effectOb = RescourseMgr.GetInstance().GetGameObjectResource("effect");
			GameObject effectSceneOb =	MonoBehaviour.Instantiate(effectOb) as GameObject;
			effectSceneOb.transform.localPosition = GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_BODY).transform.position;
			effectSceneOb.transform.FindChild("creature").animation.Play("effect");
			SharkRed();
			
			/*int random = Random.Range(0,100);
			if(random < 10){
				common.common.shakeCamera();
			}*/
		}
		
		
		void CountFight(EventMessageFight fight){
			CCreature creature = EnitityMgr.GetInstance().GetEnitity(fight.scrCreatureId);
			if(creature.GetEnitityType()==EnitityType.ENITITY_TYPE_PET){//be attacked by the pet
				CPet pet = creature as CPet;
				//miss
				int random ;
				random = Random.Range(0,1000);
				if(pet.m_effectData.mustBingo == false){
					int duck = pet.m_data.spell + pet.m_effectData.mp + (int)(pet.m_data.spell * pet.m_effectData.mpPercent * 0.01f) - m_data.duck - m_effectData.duck - (int)(m_data.duck * m_effectData.duckPrecent * 0.01f) ;
					if(random > duck){
						GameObject ob = m_objectList["JumpMiss"] as GameObject ;//gameGlobal.g_rescoureMgr.GetGameObjectResource("object/UI/SceneUI/JumpMiss") as GameObject;
						GameObject sceneOb = MonoBehaviour.Instantiate(ob) as GameObject;
						sceneOb.transform.parent = GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_HEAD) ;
						sceneOb.transform.localScale = Vector3.one ;
						sceneOb.transform.localPosition = Vector3.zero ;
						this.BeAttack(fight.audioName,fight.beEffectName);
						return ;
					}
				}
				
				
				//count attack
				float bassAttack = (pet.m_data.attack + pet.m_effectData.attack) * (1.0f + pet.m_effectData.attackPrecent * 0.01f);
				
				if(pet.GetEffectData().realHurt == true){
					pet.GetEffectData().changeAttack -= m_effectData.changeHurt ;
				}
				
				//wave hurt
				if(m_effectData.waveHurtPercentMin != 0){
					random = Random.Range(m_effectData.waveHurtPercentMin,m_effectData.waveHurtPercentMax);
					bassAttack *= (random/100.0f) ;
				}
				
				//change hurt
				bassAttack += pet.GetEffectData().changeAttack + m_effectData.changeHurt ;
				if(bassAttack < 0)
					bassAttack = 0 ;
				float changePercent = (float)(pet.GetEffectData().changeAttackPercent + m_effectData.changeHurtPercent)/ 100.0f ;
				if(changePercent != 0){
					bassAttack += bassAttack * changePercent  ;
					if(bassAttack < 0)
						bassAttack = 0 ;
				}
				
				//camp
				//ri
				int petCamp = pet.GetFightCreatureData().camp ;
				if(m_data.camp == 1 ){
					if(petCamp == 1){
						bassAttack *= 1.0f + (pet.GetEffectData().changeSunAttackPercent + m_effectData.changeSunHurtPercent) * 0.01f ;
						bassAttack += pet.GetEffectData().changeSunAttack + m_effectData.changeSunAttack ;
					}
					else if(petCamp == 2){
						bassAttack *= FightCommon.BE_REFRAIN_COEFFICIENT + (pet.GetEffectData().changeSunAttackPercent + m_effectData.changeMoonHurtPercent) * 0.01f ;
						bassAttack += pet.GetEffectData().changeSunAttack + m_effectData.changeMoonHurt ;
					}
					else if(petCamp == 3){
						bassAttack *= FightCommon.REFRAIN_COEFFICIENT + (pet.GetEffectData().changeSunAttackPercent + m_effectData.changeSunHurtPercent) * 0.01f ;
						bassAttack += pet.GetEffectData().changeSunAttack + m_effectData.changeSunHurt ;
					}
				}
				//yue
				else if(m_data.camp == 2){
					if(petCamp == 1 ){
						bassAttack *= FightCommon.REFRAIN_COEFFICIENT + (pet.GetEffectData().changeMoonAttackPercent + m_effectData.changeSunHurtPercent) * 0.01f ;
						bassAttack += pet.GetEffectData().changeMoonAttack + m_effectData.changeSunAttack ;
					}
					else if(petCamp == 2){
						bassAttack *= 1.0f + (pet.GetEffectData().changeMoonAttackPercent + m_effectData.changeMoonHurtPercent) * 0.01f ;
						bassAttack += pet.GetEffectData().changeMoonAttack + m_effectData.changeMoonHurt ;
					}
					else if(petCamp == 3){
						bassAttack *= FightCommon.BE_REFRAIN_COEFFICIENT + (pet.GetEffectData().changeMoonAttackPercent + m_effectData.changeSunHurtPercent) * 0.01f ;
						bassAttack += pet.GetEffectData().changeMoonAttack + m_effectData.changeSunHurt ;
					}
				}
				//xin
				else if(m_data.camp == 3){
					if(petCamp == 1){
						bassAttack *= FightCommon.BE_REFRAIN_COEFFICIENT + (pet.GetEffectData().changeStarAttackPercent + m_effectData.changeSunHurtPercent) * 0.01f ;
						bassAttack += pet.GetEffectData().changeStarAttack + m_effectData.changeSunAttack ;
					}
					else if(petCamp == 2){
						bassAttack *= FightCommon.REFRAIN_COEFFICIENT + (pet.GetEffectData().changeStarAttackPercent + m_effectData.changeMoonHurtPercent) * 0.01f ;
						bassAttack += pet.GetEffectData().changeStarAttack + m_effectData.changeMoonHurt ;
					}
					else if(petCamp == 3){
						bassAttack *= 1.0f + (pet.GetEffectData().changeStarAttackPercent + m_effectData.changeSunHurtPercent) * 0.01f ;
						bassAttack += pet.GetEffectData().changeStarAttack + m_effectData.changeSunHurt ;
					}
				}
				
				//double hurt
				random = Random.Range(0,1000);
				if(random <= crit){
					bassAttack *= 2 ;
					bassAttack *= 1.0f + pet.GetEffectData().critHurtPercent * 0.01f ;
					bassAttack += pet.GetEffectData().critHurt ;
					m_isCrit = true ;
				}
				
				//can not hurt
				if(effectData.invincibility == true){
					return ;
				}
				
				//one hurt
				random = Random.Range(0,100);
				if(random < effectData.oneHurt){
					bassAttack = 1 ;
					blood -= (int)bassAttack ;
					
					this.BeAttack(fight.audioName,fight.beEffectName);
					return ;
				}
				
				
				//
				//count hurt
				blood -= (int)bassAttack ;
				
				this.BeAttack(fight.audioName,fight.beEffectName);
				
				//buff message
				EventMessageAttack attackMessage = new EventMessageAttack() ;
				attackMessage.hurt = (int)bassAttack;
				attackMessage.scrID= fight.scrCreatureId ;
				attackMessage.destID=fight.destCreatureId;
				CBuffMgr.GetInstance().OnMessage(attackMessage);
			}
		}
		
		/// <summary>
		/// Gets the identifier.
		/// </summary>
		/// <returns>
		/// The identifier.
		/// </returns>
		public int GetId(){
			return m_data.id ;
		}
		
		///////////////////////////set get data/////////////////////////
		//get attack
		public int attack{
			get{
				int bassAttack = m_data.attack + m_effectData.attack + (int)(m_data.attack * m_effectData.attackPrecent * 0.01f);
				int random = Random.Range(0,1000);
				if(random <= crit){
					bassAttack *= 2 ;
				}
				return bassAttack;
			}
		}
		
		
		public int crit{
			get{
				return m_data.crit + m_effectData.crit + m_data.crit * m_effectData.critPrecent / 1000 ;
			}
		}
		
		public int maxBlood{
			get{
				return m_data.maxBlood + m_effectData.hpMax;
			}
		}
		
		//blood
		public int blood{
			get{
				return m_data.blood + m_data.maxBlood * m_effectData.hpPercent ;
			}
			
			set{
				//jump tect
				int maxBlood = m_data.maxBlood + m_data.maxBlood * m_effectData.hpPercent / 100 + m_effectData.hpMax;
				int tect ;//= value - m_data.blood - m_data.maxBlood * m_effectData.hpPercent / 100;
				//m_data.blood = value ;
				if(value < maxBlood){
					tect = value - m_data.blood - m_data.maxBlood * m_effectData.hpPercent / 100;
					m_data.blood = value ;
				}
				else{
					tect = maxBlood - m_data.blood - m_data.maxBlood * m_effectData.hpPercent / 100;
				}
				
				if(tect == 0)
					return ;
				
				GameObject ob ;//= gameGlobal.g_rescoureMgr.GetGameObjectResource("JumpNum") ;//gameGlobal.g_rescoureMgr.GetGameObjectResource("object/UI/SceneUI/JumpNum") as GameObject;
				GameObject sceneOb;// = MonoBehaviour.Instantiate(ob) as GameObject;
				TweenFont font ;//= sceneOb.GetComponent("TweenFont") as TweenFont;
				//font.text = tect.ToString() ;
				if(tect > 0){
					ob = m_objectList["JumpNumGreen"] as GameObject ;//gameGlobal.g_rescoureMgr.GetGameObjectResource("object/UI/SceneUI/JumpNum") as GameObject;
					sceneOb = MonoBehaviour.Instantiate(ob) as GameObject;
					sceneOb.transform.parent = GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_HEAD) ;
					sceneOb.transform.localScale = Vector3.one ;
					sceneOb.transform.localPosition = Vector3.zero ;
					font = sceneOb.GetComponent("TweenFont") as TweenFont;
					font.text = tect.ToString() ;
					UILabel label = sceneOb.transform.FindChild("Label").GetComponent("UILabel") as UILabel;
					label.color = new Color32(17,255,15,255);
				}
				
				if(tect<0){
					if(m_isCrit == true){
						ob = m_objectList["HeavyJumpNum"] as GameObject ;//gameGlobal.g_rescoureMgr.GetGameObjectResource("object/UI/SceneUI/HeavyJumpNum") as GameObject;
						sceneOb = MonoBehaviour.Instantiate(ob) as GameObject;
						sceneOb.transform.parent = GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_HEAD) ;
						sceneOb.transform.localScale = Vector3.one ;
						sceneOb.transform.localPosition = Vector3.zero ;
						font = sceneOb.GetComponent("TweenFont") as TweenFont;
						font.text = tect.ToString() ;
						m_isCrit = false ;
					}
					else{
						ob = m_objectList["JumpNum"] as GameObject ;//gameGlobal.g_rescoureMgr.GetGameObjectResource("JumpNum") ;//gameGlobal.g_rescoureMgr.GetGameObjectResource("object/UI/SceneUI/JumpNum") as GameObject;
						sceneOb = MonoBehaviour.Instantiate(ob) as GameObject;
						sceneOb.transform.parent = GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_HEAD) ;
						sceneOb.transform.localScale = Vector3.one ;
						sceneOb.transform.localPosition = Vector3.zero ;
						font = sceneOb.GetComponent("TweenFont") as TweenFont;
						font.text = tect.ToString() ;
					}
					
				}
				
				
				UISliderAnim slider = m_bloodBar.GetComponent("UISliderAnim") as UISliderAnim;
				slider.sliderValue = (float)m_data.blood / (float)maxBlood ;
				
				//death
				if(m_data.blood + m_data.maxBlood * m_effectData.hpPercent <= 0){
					m_data.blood = 0 ;
					this.m_stateMachine.ChangeState(EnemyPetWeakState.getInstance());
				}
				
			}
		}
		
		//eye shot area
		public float eyeShotArea{
			get{
				return m_data.eyeShotArea ;
			}
			
		}
		
		//get attack area
		public float attackArea{
			get{
				return m_data.attackArea ;
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
				return m_data.moveSpeed + m_effectData.speed + m_data.moveSpeed * m_effectData.speedPercent / 100;
			}
		}
		
		//get state
		public AIState aiState{
			get{
				CStateBase<CEnemyPet> state = m_stateMachine.GetState();
				return state.GetState();
			}
		}
		
		//get spell
		public int spell{
			get{
				return m_data.spell + m_effectData.mp + m_data.spell * m_effectData.mpPercent / 100;
			}
			set{
				m_data.spell = value ;
				if(m_data.spell < 0)
					m_data.spell = 0 ;
				else if(m_data.spell > m_data.maxSpell){
					m_data.spell = m_data.maxSpell ;
				}
			}
		}
		
		//effect data
		public	EffectData	effectData{
			get{
				return m_effectData ;
			}
		}
		
		public EffectData GetEffectData(){
			return m_effectData ;
		}
		
		public FightCreatureData GetFightCreatureData(){
			return m_data ;
		}
		
		//set hp
		public void SetHp(int hp){
			blood += hp ;
		}
		
		//buff
		public bool AddBuff(int buffMoudleID,int buffID){
			for(int i = 0; i< m_buffList.Count; ++i){
				BuffDataBass buffData = CBuffMgr.GetInstance().GetBuffData(m_buffList[i]) ;
				if(buffData != null){
					BuffMoudleData data = (BuffMoudleData)common.fileMgr.GetInstance().buffCsvData.dataDic[buffData.moudleId] ;
					BuffMoudleData addData = (BuffMoudleData)common.fileMgr.GetInstance().buffCsvData.dataDic[buffMoudleID] ;
					if(data.type == addData.type){
						if(data.strength > addData.strength)
							return false ;
						else{
							CBuffMgr.GetInstance().DestroyBuff(data.id);
						}
					}
				}
			}
			m_buffList.Add(buffID);
			return true ;
		}

		public int CheckBuff(int buffMoudleID){
			BuffDataBass buffData ;
			for(int i = 0; i<m_buffList.Count; ++i){
				buffData = CBuffMgr.GetInstance().GetBuffData(m_buffList[i]);
				if(buffData!=null){
					if(buffData.moudleId == buffMoudleID){
						return m_buffList[i] ;
					}
				}
			}

			return -1 ;

		}
		
		public void DelBuff(int buffID){
			if(m_buffList.Contains(buffID))
				m_buffList.Remove(buffID);
		}
		
		//attack lock
		public int GetAttackNum(){
			return m_beLockCreatureIDList.Count ;
		}
		
		public bool CanBeLock(){
			PetMoudleData moudleData = (PetMoudleData)common.fileMgr.GetInstance().GetData(m_data.moudleID,common.CsvType.CSV_TYPE_PET) ;
			if(m_beLockCreatureIDList.Count < moudleData.attackLockCount){
				return true ;
			}
			return false ;
		}
		
		public void AddAttack(int id){
			m_beLockCreatureIDList.Add(id) ;
		}
		
		public List<CEnemyPetSkill> GetCanUseSkillList(){
			List<CEnemyPetSkill> skills = new List<CEnemyPetSkill>();
			for(int i = 0; i < m_skillList.Count; ++i){
				CEnemyPetSkill skill = (CEnemyPetSkill)SkillMgr.GetInstance().GetSkill(m_skillList[i]);
				
				if(!skill.canUse())
					continue;
				//SkillMoudleData skillData = (SkillMoudleData)fileMgr.GetInstance().GetData(skill.GetSkillData().moudleID,CsvType.CSV_TYPE_SKILL);
				skills.Add(skill);
			}
			return skills ;
		}
		
		public List<EnemySkillLimit> GetCanUseSkillList2(){
			List<EnemySkillLimit> skills = new List<EnemySkillLimit>();
			for(int i = 0; i < m_skillList.Count; ++i){
				CEnemyPetSkill skill = (CEnemyPetSkill)SkillMgr.GetInstance().GetSkill(m_skillList[i]);
				
				if(!skill.canUse())
					continue;
				
				int moudleID = skill.GetSkillData().moudleID;
				SkillMoudleData skillData = (SkillMoudleData)common.fileMgr.GetInstance().GetData(moudleID,
					CsvType.CSV_TYPE_SKILL);
				
				EnemySkillLimit skilllimit = new EnemySkillLimit();
				skilllimit.pelSkill = skill;
				skilllimit.environment = skillData.environment;
				skilllimit.skillLimitType = skillData.activateLimitType;
				skilllimit.skillLimitValue = skillData.activateLimitVaule;

				if(skillData.environment == 1){
					if(GameLevelMgr.GetInstance().m_levelType != LevelType.LEVEL_TYPE_PVP){
						continue ;
					}
				}
				else if(skillData.environment == 3){
					if(GameLevelMgr.GetInstance().m_levelType == LevelType.LEVEL_TYPE_PVP){
						continue ;
					}
				}
				
				float bloodPrenct = (float)blood / (float)maxBlood * 100;
				float cityBloodPrenct = 0.0f; 
				if(EnitityMgr.GetInstance().city != null){
					cityBloodPrenct = EnitityMgr.GetInstance().city.blood / EnitityMgr.GetInstance().city.maxBlood * 100 ;
				}
				
				if(skilllimit.skillLimitType == 1){
					if(bloodPrenct > skillData.activateLimitVaule && skillData.activateLimitVaule != -1){
						continue ;
					}
				}
				else if(skilllimit.skillLimitType == 4){
					if(bloodPrenct < skillData.activateLimitVaule && skillData.activateLimitVaule != -1){
						continue ;
					}
				}
				else if(skilllimit.skillLimitType == 2){
					if(cityBloodPrenct > skillData.activateLimitVaule && skillData.activateLimitVaule != -1){
						continue ;
					}
				}
				else if(skilllimit.skillLimitType == 3){
					int lastWome = GameDataCenter.GetInstance().levelData.girlCount - GameLevelMgr.GetInstance().m_escapeNum ;
					if(lastWome > skillData.activateLimitVaule && skillData.activateLimitVaule != -1){
						continue ;
					}
				}
				
				skills.Add(skilllimit);
			}
			return skills ;
		}
		
		public CEnemyPetSkill GetSkillByIndex(int index){
			CEnemyPetSkill returnSkill = null ;
			for(int i = 0; i < m_skillList.Count; ++i){
				CEnemyPetSkill skill = (CEnemyPetSkill)SkillMgr.GetInstance().GetSkill(m_skillList[i]);
				int moudleID = skill.GetSkillData().moudleID;
				SkillMoudleData skillData = (SkillMoudleData)common.fileMgr.GetInstance().GetData(moudleID,
					CsvType.CSV_TYPE_SKILL);
				
				if(skillData.strength == index){
					returnSkill = skill ;
				}
			}
			return returnSkill ;
		}
		
		public bool IsSkillEffect(CEnemyPetSkill skill)
		{
			int moudleID = skill.GetSkillData().moudleID;
			SkillMoudleData skillData = (SkillMoudleData)common.fileMgr.GetInstance().GetData(moudleID,
				CsvType.CSV_TYPE_SKILL);
			
			if(!skill.canUse())
				return false;
				
			bool isEffect = false;
			switch(skillData.activateType)
			{
			case 1:
				float bloodPercent1 = (float)blood / maxBlood * 100;
				if(skillData.activateLimitVaule > bloodPercent1 )
					isEffect = true;
				
				break;
			case 2:
				if(skillData.activateLimitVaule > GameLevel.GameLevelMgr.GetInstance().m_escapeNum)
				{
					isEffect = true;
				}
				break;
			case 3:
				float bloodPercent = (float)EnitityMgr.GetInstance().city.blood / (float)EnitityMgr.GetInstance().city.maxBlood * 100;
				if(skillData.activateLimitVaule > bloodPercent)
				{
					isEffect = true;
				}
				break;
			case -1:
				isEffect = true;
				break;
			default:
				break;
			}
				
			return isEffect ;
		}
		
		public void SetCrit(bool isCrit){
			m_isCrit = isCrit ;
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

		public CCreature FindMostBlood(EnitityType type){
			List<CCreature> enemyPetList = null ; 
			if(type == EnitityType.ENITITY_TYPE_ENEMY_PET){
				enemyPetList = EnitityMgr.GetInstance().GetMonsterList() ;
			}
			else if(type == EnitityType.ENITITY_TYPE_PET){
				enemyPetList = EnitityMgr.GetInstance().GetPetList() ;
			}
			
			int tempBlood = int.MinValue ;
			CCreature creature = null ;
			for(int i = 0; i<enemyPetList.Count; ++i){
				if(enemyPetList[i].GetFightCreatureData().blood > tempBlood){
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

	public class EnemySkillLimit
	{
		public CEnemyPetSkill pelSkill;
		public int environment = 2;
		public int skillLimitType = -1;
		public int skillLimitValue = -1;
	}
}


