using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using GameLogical.GameEnitity.AI ;
using GameEvent;
using GameLogical.GameSkill ;
using GameLogical.GameSkill.Buff ;
//using UnityEditor;
using GameLogical;
using common;
using GameLogical.GameSkill.Skill ;
using GameLogical.GameLevel ;
using GameLogical.Guide;
namespace GameLogical.GameEnitity{

	
	public class CMonster : CCreature
	{
		public   FightCreatureData    			 m_data ;
		public   StateMachine<CMonster>   m_stateMachine ;
		//protected	MonsterNormalBrain		 m_brain		;
		public   	MonsterAIDataBass		 m_monsterAIData;
		public      GameObject               m_object       ;
		public      GameObject				 m_bloodBar     ;
		public		GameObject				 talkBlink		;//说话
		public		GameObject				 nameBlink		;//怪物名字
		private		float					 talkTime = -1.0f;//说话时间
		private		const	float			 talkMaxTime = 3;//说话显示时间
		//public 		ArrayList				 m_eyeShotObjectList ;
		//public		ArrayList				 m_attackAreaObjectList ;
		public		EffectData				 m_effectData	;
		public      CCreature                m_targetCreature;
		public 		List<int>				 m_buffList ;
		
		//skill
		public      List<int>          		 m_skillList    ;
		public      int						 m_curUsingSkill;
		
		public 		float					 m_sharkRed ;
		
		public		float					 m_sharkTotalTime;
		public		float					 m_sharkCurTime  ;
		
		public      int        				 rewardID	;
		public		bool					 m_isCrit	;

		private		Dictionary<string,Object> m_objectList ;
		
		/// <summary>
		/// Use this for initialization.
		/// </summary>
		/// <param name='data'>
		/// MonsterData.
		/// </param>
		public void Init (FightCreatureData data)
		{
			m_data = data ;
			
			m_effectData = new EffectData();
			
			m_buffList = new List<int>();
			
			m_sharkRed = -1.0f ;
			
			m_sharkTotalTime = 0.6f ;
			
			m_skillList = new List<int>();
			
			rewardID = -1 ;

			m_objectList = new Dictionary<string, Object>();
		}

		public void InitSkill(){
			MonsterMoudleData moudleData = (MonsterMoudleData)fileMgr.GetInstance().GetData(m_data.moudleID,CsvType.CSV_TYPE_MONSTER);
			
			for(int i = 0; i<moudleData.beSkillList.Count; ++i){
				SkillMoudleData skillMoudleData = (SkillMoudleData)fileMgr.GetInstance().GetData(moudleData.beSkillList[i],CsvType.CSV_TYPE_SKILL);
				
				SingleBuffCreateData singleBuff = new SingleBuffCreateData() ;
				singleBuff.buffModuleID = skillMoudleData.buffer	;
				singleBuff.buffRate     = skillMoudleData.buffRate  ;
				singleBuff.srcCreatureID = m_data.id ;
				singleBuff.destCreatureID= m_data.id ;
				singleBuff.rangeType = BuffRangeType.BUFF_RANGE_PASSTIVITY_FIGHT ;
				CBuffMgr.GetInstance().CreateBuff(singleBuff);
			}
			for(int i = 0; i<moudleData.skillList.Count; ++i){
				int id = SkillMgr.GetInstance().CreateSkill(SkillType.SKILL_TYPE_MONSTER,moudleData.skillList[i],GetId());
				m_skillList.Add(id);
			}
		}

		public void LoadObjectCallBack(Object ob, MGResouce.LoadCreatureData tran){
			if(ob != null){
				m_object = MonoBehaviour.Instantiate(ob) as GameObject;
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
		}

		public void LoadOtherObjectCallBack(MGResouce.LoadUIData data){
			m_objectList.Add(data.name,data.ob);
			if(m_objectList.Count == 6){
				FinishLoad();
			}
		}

		void FinishLoad(){
			MonsterMoudleData moudleData = (MonsterMoudleData)fileMgr.GetInstance().GetData(m_data.moudleID,CsvType.CSV_TYPE_MONSTER);
			
			//nlood
			GameObject obj = m_objectList["BloodYellowBar"] as GameObject ;//gameGlobal.g_rescoureMgr.GetGameObjectResource("object/UI/SceneUI/BloodBar") as GameObject;
			m_bloodBar = GameObject.Instantiate(obj) as GameObject;
			m_bloodBar.transform.parent = m_object.transform.FindChild(gameGlobal.OBJECT_POIN_TOP) ;
			m_bloodBar.transform.localPosition = Vector3.zero ;
			//m_bloodBar.transform.localScale = Vector3.one ;
			m_bloodBar.name = "BloodBar" ;

			InitSkill(); 
			if(moudleData.strength == 2)
			{
				m_bloodBar.transform.FindChild("Background").GetComponent<UISprite>().spriteName = "UI_zhandou_040";
				m_bloodBar.transform.FindChild("Background").GetComponent<UISprite>().width  = 90 ;
				m_bloodBar.transform.FindChild("Background").GetComponent<UISprite>().height = 18 ;
				//m_bloodBar.transform.FindChild("Background").localScale = new Vector3(102, 18, 1);
				m_bloodBar.transform.FindChild("Background").localPosition = new Vector3(-46, 0, 0);
			}
			if(moudleData.strength >= 3)
			{
				m_bloodBar.transform.FindChild("Background").GetComponent<UISprite>().spriteName = "UI_zhandou_041";
				m_bloodBar.transform.FindChild("Background").GetComponent<UISprite>().width  = 110 ;
				m_bloodBar.transform.FindChild("Background").GetComponent<UISprite>().height = 30 ;
				//m_bloodBar.transform.FindChild("Background").localScale = new Vector3(118, 24, 1);
				m_bloodBar.transform.FindChild("Background").localPosition = new Vector3(-63, 4, 0);
				
				/*Transform shader  = m_object.transform.FindChild(gameGlobal.CREATURE_SHADOW);
				ob = gameGlobal.g_rescoureMgr.GetGameObjectResource("petShader");
				GameObject petShader = GameObject.Instantiate(ob) as GameObject;
				petShader.transform.parent = shader.parent;
				petShader.transform.localScale = shader.localScale;
				petShader.transform.localPosition = new Vector3( shader.localPosition.x , shader.localPosition.y, -1);
				petShader.name = "shadow";
				UISprite shaderSprite = petShader.GetComponent<UISprite>();
				
				shaderSprite.spriteName = "UI_zhandou_039";*/
			}
			
			
			//boss
			if(moudleData.strength >= 2)
			{
				//名字
				GameObject ob3 = m_objectList["talk"] as GameObject ;//gameGlobal.g_rescoureMgr.GetGameObjectResource("object/UI/SceneUI/talk") as GameObject;
				nameBlink = GameObject.Instantiate(ob3) as GameObject;

				nameBlink.transform.localScale = Vector3.one * 0.125f;
				nameBlink.transform.FindChild("Sprite").gameObject.SetActive(false);
				nameBlink.transform.parent = m_object.transform.FindChild(gameGlobal.OBJECT_POIN_TOP) ;
				nameBlink.transform.localPosition = new Vector3(-0.0f,0f,0.0f) ;
				
				UILabel talkLabel = nameBlink.transform.FindChild("talkStr").GetComponent("UILabel") as UILabel;
				talkLabel.fontSize = 25;
				talkLabel.width = 128;
				talkLabel.transform.localPosition = new Vector3(0, 26, -2);

				//MonsterMoudleData monstData = fileMgr.GetInstance().GetData(this.m_data.moudleID, 
				//                                                            CsvType.CSV_TYPE_MONSTER) as MonsterMoudleData;
				
				
				talkLabel.text = UICommon.colorString[moudleData.strength] + moudleData.name;
			}
			
			//说话
			GameObject ob2 = m_objectList["talk"] as GameObject ;//gameGlobal.g_rescoureMgr.GetGameObjectResource("object/UI/SceneUI/talk") as GameObject;
			talkBlink = GameObject.Instantiate(ob2) as GameObject;
			
			talkBlink.transform.localScale = Vector3.one * 0.15f;
			talkBlink.transform.parent = m_object.transform.FindChild(gameGlobal.OBJECT_POIN_TOP) ;
			talkBlink.transform.localPosition = new Vector3(-50.0f,0.0f,0.0f) ;
			NGUITools.SetActive(talkBlink, false);

			m_monsterAIData = new MonsterAIDataBass();
			m_stateMachine = new StateMachine<CMonster>(this);

			//talk
			if(moudleData.talkList.Count != 0){
				m_stateMachine.SetState(MonsterTalkMoveState.getInstance());
			}
			else{
				if(moudleData.profession == 15){
					m_stateMachine.SetState(MonsterNoLookMove.getInstance());
				}
				else{
					m_stateMachine.SetState(MonsterMoveState.getInstance());
				}
			}
		}

		/// <summary>
		/// Update the specified deltaTime.
		/// </summary>
		/// <param name='deltaTime'>
		/// Delta time.
		/// </param>
		public void Update (float deltaTime)
		{
			if(m_stateMachine != null){
				m_stateMachine.Update(deltaTime);
			}

			
			if(m_sharkRed != -1.0f){
				m_sharkRed += deltaTime ;
				Vector3 vec = Vector3.right * 50.0f * Time.deltaTime;
				if(m_object != null){
					m_object.transform.FindChild(gameGlobal.CREATURE_ROOT).localPosition += vec;
				}

				if(m_sharkRed >= 0.166f){
					if(m_object != null){
						m_object.transform.FindChild(gameGlobal.CREATURE).gameObject.renderer.material.color = Color.white ;
						m_sharkRed = -1.0f ;
						m_object.transform.FindChild(gameGlobal.CREATURE_ROOT).localPosition = Vector3.zero ;
						Transform trans = m_object.transform.FindChild(gameGlobal.CREATURE);
					}
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
		
		public void TalkLv(List<int> talkLv, List<int> talkID)
		{
			//talk
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
		
		public void talkPlay()
		{
			if(!NGUITools.GetActive(talkBlink))
			{
				//talkBlink.GetComponent<TalkBlink>().SetSize();
				NGUITools.SetActive(talkBlink, true);

				if(talkBlink.transform.FindChild("expression") != null)
					talkBlink.transform.FindChild("expression").FindChild("creature").animation.Play("effect");
			}
			talkTime = 0.0f;
		}
		
		public void talkStop()
		{
			if(NGUITools.GetActive(talkBlink))
				NGUITools.SetActive(talkBlink, false);
		}
		
		/// <summary>
		/// ai think what to do.
		/// </summary>
		public void Think(){
		//	m_brain.ThinkAbout();
		}
		
		/// <summary>
		/// Play animation.
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
		
		public void Shark(){
			Transform trans = m_object.transform.FindChild(gameGlobal.CREATURE);
			trans.gameObject.SetActive(!trans.gameObject.activeSelf);
		}
		
		public void SharkRed(){
			if(m_sharkRed == -1.0f){
				m_sharkRed = 0.0f ;
				m_object.transform.FindChild(gameGlobal.CREATURE).gameObject.renderer.material.color = new Color(0.94f,0.21f,0.21f,1.0f) ;
			}
		}
		
		//
		public void Stop(){
			Transform trans = m_object.transform.FindChild(gameGlobal.CREATURE);
			trans.gameObject.animation.Stop();
		}
		
		/// <summary>
		/// Bes the attack.
		/// </summary>
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
				MuscClip.MusicClipMgr.GetInstance().MusicClips(audioPath);
			}

			GameObject effectOb = RescourseMgr.GetInstance().GetGameObjectResource("effect");
			GameObject effectSceneOb =	MonoBehaviour.Instantiate(effectOb) as GameObject;
			effectSceneOb.transform.localPosition = GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_BODY).transform.position;
			effectSceneOb.transform.FindChild("creature").animation.Play("effect");
			//SharkRed();
		}
		
		//count fight
		void CountFight(EventMessageFight fight){
			CCreature creature = EnitityMgr.GetInstance().GetEnitity(fight.scrCreatureId);
			if(creature!=null){
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
				
				else if(creature.GetEnitityType()==EnitityType.ENITITY_TYPE_CITY){
					//miss
					//int random = Random.Range(0,100);
					int duck = m_data.duck + m_effectData.duck + m_data.duck * m_effectData.duckPrecent / 100 ;
					/*if(random < duck){
						GameObject ob = gameGlobal.g_rescoureMgr.GetGameObjectResource("JumpMiss") ;//gameGlobal.g_rescoureMgr.GetGameObjectResource("object/UI/SceneUI/JumpMiss") as GameObject;
						GameObject sceneOb = MonoBehaviour.Instantiate(ob) as GameObject;
						sceneOb.transform.parent = GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_HEAD) ;
						sceneOb.transform.localScale = Vector3.one ;
						this.BeAttack(fight.audioName,fight.beEffectName);
						return ;
					}
					else{*/
						CCity city = creature as CCity;
						this.blood -= city.attack ;
						this.BeAttack(fight.audioName,fight.beEffectName);
					//}
					
				}

				else if(creature.GetEnitityType()==EnitityType.ENITITY_TYPE_CHARACTER)
				{
					int duck = m_data.duck + m_effectData.duck + m_data.duck * m_effectData.duckPrecent / 100 ;
					CPlayer player = creature as CPlayer;
					this.blood -= player.attack ;
					this.BeAttack(fight.audioName,fight.beEffectName);
				}
			}
		}
		
		/// <summary>
		/// Sets the ai state.
		/// </summary>
		/// <param name='state'>
		/// State.
		/// </param>
		public void SetState(CStateBase<CMonster> state){
			CStateBase<CMonster> perState = m_stateMachine.GetState();
			perState.Release();
			perState = null ;
			m_stateMachine.SetState(state);
		}
		
		/// <summary>
		/// Executes the state again.
		/// </summary>
		public void ExecuteStateAgain(){
			CStateBase<CMonster> perState = m_stateMachine.GetState();
			m_stateMachine.SetState(perState);
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
						//change smooth elenment 
						if(rewardID != -1){
							EventMessageSmoothChangeBox changeBoxMessage = new EventMessageSmoothChangeBox();
							changeBoxMessage.rewardID = rewardID ;
							//CLineSmoothMgr.GetInstance().OnMessage(changeBoxMessage);
						}
						
						MonsterMoudleData moudleData = (MonsterMoudleData)fileMgr.GetInstance().GetData(m_data.moudleID,CsvType.CSV_TYPE_MONSTER) ;
						if(moudleData.strength >= 3){
							GameLevel.GameLevelMgr.GetInstance().m_bossLastHp = 0 ;
						}
						EnitityMgr.GetInstance().DestroyEnitity(this);
						return;
					}
				}
				//real fight
				else if(EnitityAction.ENITITY_ACTION_FIGHT == action){
					EventMessageFight fight = message as EventMessageFight ;
					//be hurt
					if(fight.destCreatureId == m_data.id){
						CountFight(fight);
						if(blood == 0){
							if(GameLevelMgr.GetInstance().m_levelType != LevelType.LEVEL_TYPE_PVP){
								MonsterMoudleData moudleData = (MonsterMoudleData)fileMgr.GetInstance().GetData(m_data.moudleID,CsvType.CSV_TYPE_MONSTER) ;
								if(moudleData.strength == 1){
									gameGlobal.g_fightSceneUI.CutCoolDownTime(0.5f,m_data.seat);
								}
								else if(moudleData.strength == 2){
									gameGlobal.g_fightSceneUI.CutCoolDownTime(1f,m_data.seat);
								}
								else if(moudleData.strength == 3){
									gameGlobal.g_fightSceneUI.CutCoolDownTime(2f,m_data.seat);
								}

							}
						}
					}
	
				}
				if(m_stateMachine != null){
					m_stateMachine.OnMessage(message);
				}
			}
			
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
		/// Gets the eye shot list.
		/// </summary>
		/// <returns>
		/// The eye shot list.
		/// </returns>
		/*public ArrayList GetEyeShotList(){
			return m_eyeShotObjectList ;
		}
		
		/// <summary>
		/// Gets the attack area list.
		/// </summary>
		/// <returns>
		/// The attack area list.
		/// </returns>
		public ArrayList GetAttackAreaList(){
			return m_attackAreaObjectList ;
		}*/
		
		/// <summary>
		/// Release this instance.
		/// </summary>
		public void Release(){
			m_data = null;
			m_stateMachine.Release();
			m_stateMachine = null ;
			//m_brain	= null	;
			m_monsterAIData = null;
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
		/// Gets the identifier.
		/// </summary>
		/// <returns>
		/// The identifier.
		/// </returns>
		public int GetId(){
			if(m_data == null)
				return -1 ;
			return m_data.id ;
		}
		
		/////////////////////////////////////////// get  set  data //////////////////////////////////////////////
		//get speed
		public float monsterSpeed{
			get{
				return m_data.moveSpeed + m_effectData.speed + m_data.moveSpeed * m_effectData.speedPercent / 100;
			}
		}
		
		//get state
		public AIState aiState{
			get{
				CStateBase<CMonster> state = m_stateMachine.GetState();
				return (AIState)state.GetState();
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
		
		//get type 
		public EnitityType GetEnitityType(){
			return EnitityType.ENITITY_TYPE_MONSTER ;
		}
		
		public AIState GetEnitityAiState(){
			if(m_stateMachine != null){
				CStateBase<CMonster> state = m_stateMachine.GetState();
				return state.GetState();
			}
			else{
				return AIState.AI_STATE_NULL ;
			}
		}
		
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
		
		//crit
		public int crit{
			get{
				return m_data.crit + m_effectData.crit + m_data.attack * m_effectData.critPrecent / 1000 ;
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
				return m_data.blood + m_data.blood * m_effectData.hpPercent / 100 ;
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


				if(NewPlayerGuide.isGuide&&(NewPlayerGuide.curGuide == 112)
				   &&m_data.moudleID == 121203009 &&(maxBlood-(float)m_data.blood)>1000f)
				{
					List<int> id = new List<int>();
					id.Add(6004);
					id.Add(6005);
					GuideCheckMessage msg = new GuideCheckMessage();
					NewPlayerGuide.GetInstance().OnMessage(msg);
					gameGlobal.g_fightSceneUI.LoadTalk(id);
					/*List<CCreature> monsters = EnitityMgr.GetInstance().GetMonsterList();
					CMonster monster ;
					//monster = m_data.
					for(int i =0;i<monsters.Count;i++)
					{
						//MonsterMoudleData data = (MonsterMoudleData)fileMgr.GetInstance().GetData(monsters[i].GetFightCreatureData().moudleID,CsvType.CSV_TYPE_MONSTER);
						if(monsters[i].GetId  ==m_data.id)
						{
							//pos = monsters[i].GetRenderObject().transform.position ;
							monster = monsters[i] as CMonster ;
							monster.m_curUsingSkill = monster.m_skillList[0] ;
							Debug.Log(monster.aiState.ToString());
							monster.Play("skill2",WrapMode.Once);
							break;
						}
					}*/
				}
				if(NewPlayerGuide.isGuide&&NewPlayerGuide.curGuide == 114
				   &&EnitityMgr.GetInstance().GetPetList().Count>0
				   &&m_data.moudleID == 121203009 &&(maxBlood-(float)m_data.blood)>1000f)
				{
					List<CCreature> monsters = EnitityMgr.GetInstance().GetMonsterList();
					CMonster monster ;
					for(int i =0;i<monsters.Count;i++)
					{
						//MonsterMoudleData data = (MonsterMoudleData)fileMgr.GetInstance().GetData(monsters[i].GetFightCreatureData().moudleID,CsvType.CSV_TYPE_MONSTER);
						if(monsters[i].GetFightCreatureData().moudleID == m_data.moudleID)
						{
							//pos = monsters[i].GetRenderObject().transform.position ;
							monster = monsters[i] as CMonster ;
							monster.m_curUsingSkill = monster.m_skillList[0] ;
							Debug.Log(monster.aiState.ToString());
							monster.Play("skill2",WrapMode.Once);
							break;
						}
					}
				}
				
				if(tect == 0)
					return ;
				
				GameObject ob ;//= gameGlobal.g_rescoureMgr.GetGameObjectResource("JumpNum") ;//gameGlobal.g_rescoureMgr.GetGameObjectResource("object/UI/SceneUI/JumpNum") as GameObject;
				GameObject sceneOb;// = MonoBehaviour.Instantiate(ob) as GameObject;
				TweenFont font ;//= sceneOb.GetComponent("TweenFont") as TweenFont;
				float rangeY = Random.Range(-5.0f,5.0f);
				Vector3 pos ;
				//font.text = tect.ToString() ;
				if(tect > 0){
					ob = m_objectList["JumpNumGreen"] as GameObject ;//gameGlobal.g_rescoureMgr.GetGameObjectResource("object/UI/SceneUI/JumpNum") as GameObject;
					sceneOb = MonoBehaviour.Instantiate(ob) as GameObject;
					pos = GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_HEAD).position;
					pos.y += rangeY ;
					sceneOb.transform.position = pos;
					sceneOb.transform.parent = gameGlobal.g_curPage.gameObject.transform ;
					font = sceneOb.GetComponent("TweenFont") as TweenFont;
					font.text = tect.ToString() ;
					UILabel label = sceneOb.transform.FindChild("Label").GetComponent("UILabel") as UILabel;
					label.color = new Color32(17,255,15,255);
				}
				
				else if(tect<0){
					if(m_isCrit == true){
						ob = m_objectList["HeavyJumpNum"] as GameObject ;//gameGlobal.g_rescoureMgr.GetGameObjectResource("object/UI/SceneUI/HeavyJumpNum") as GameObject;
						sceneOb = MonoBehaviour.Instantiate(ob) as GameObject;
						pos = GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_HEAD).position;
						pos.y += rangeY ;
						sceneOb.transform.position = pos;
						sceneOb.transform.parent = gameGlobal.g_curPage.gameObject.transform ;
						font = sceneOb.GetComponent("TweenFont") as TweenFont;
						font.text = tect.ToString() ;
						m_isCrit = false ;
					}
					else{
						ob = m_objectList["JumpNum"] as GameObject ;//gameGlobal.g_rescoureMgr.GetGameObjectResource("JumpNum") ;//gameGlobal.g_rescoureMgr.GetGameObjectResource("object/UI/SceneUI/JumpNum") as GameObject;
						sceneOb = MonoBehaviour.Instantiate(ob) as GameObject;
						pos = GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_HEAD).position;
						pos.y += rangeY ;
						sceneOb.transform.position = pos;
						sceneOb.transform.parent = gameGlobal.g_curPage.gameObject.transform ;
						font = sceneOb.GetComponent("TweenFont") as TweenFont;
						font.text = tect.ToString() ;
					}

					SharkRed();
					
				}
				
				if(m_bloodBar != null){
					UISliderAnim slider = m_bloodBar.GetComponent("UISliderAnim") as UISliderAnim;
					slider.sliderValue = (float)m_data.blood / (float)maxBlood ;
				}
				
				//death
				if(m_data.blood + m_data.maxBlood * m_effectData.hpPercent <= 0){
					m_data.blood = 0 ;
					this.m_stateMachine.ChangeState(MonsterWeakState.getInstance());
				}
			}
		}
		
		//eye shot area
		public float eyeShotArea{
			get{
				return m_data.eyeShotArea ;
			}
			
		}
		
		public	EffectData	effectData{
			get{
				return m_effectData ;
			}
		}
		
		
		public EffectData GetEffectData(){
			return m_effectData ;
		}
		
		public EffectData GetWeaponData(){
			return null ;
		}
		
		public  void	SetHp(int hp){
			blood += hp ;
		}
		
		public MonsterMoudleData getMonsterMoudleData(){
			return  (MonsterMoudleData)fileMgr.GetInstance().GetData(m_data.moudleID,CsvType.CSV_TYPE_MONSTER) ;
		}

		
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
		
		public void DelBuff(int buffID){
			if(m_buffList.Contains(buffID))
				m_buffList.Remove(buffID);
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

		public CMonsterSkill GetCanUseSkill(){
			//List<CMonsterSkill> skills = new List<CPetSkill>();
			for(int i = 0; i < m_skillList.Count; ++i){
				CMonsterSkill skill = (CMonsterSkill)SkillMgr.GetInstance().GetSkill(m_skillList[i]);
				
				if(skill.canUse()){
					return skill ;
				}
					continue;
				//SkillMoudleData skillData = (SkillMoudleData)fileMgr.GetInstance().GetData(skill.GetSkillData().moudleID,CsvType.CSV_TYPE_SKILL);
				//skills.Add(skill);
			}
			//return skills ;
			return null ;
		}

		public FightCreatureData getMonsterData(){
			return this.m_data;
		}
		
		public List<CMonsterSkill> GetCanUseSkillList(){
			List<CMonsterSkill> skills = new List<CMonsterSkill>();
			for(int i = 0; i < m_skillList.Count; ++i){
				CMonsterSkill skill = (CMonsterSkill)SkillMgr.GetInstance().GetSkill(m_skillList[i]);
				
				if(!skill.canUse())
					continue;
				//SkillMoudleData skillData = (SkillMoudleData)fileMgr.GetInstance().GetData(skill.GetSkillData().moudleID,CsvType.CSV_TYPE_SKILL);

				SkillMoudleData skillData = (SkillMoudleData)common.fileMgr.GetInstance().GetData(skill.GetSkillData().moudleID,
				                                                                                  CsvType.CSV_TYPE_SKILL);


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
				
				if(skillData.activateLimitType == 1){
					if(bloodPrenct > skillData.activateLimitVaule && skillData.activateLimitVaule != -1){
						continue ;
					}
				}
				else if(skillData.activateLimitType == 4){
					if(bloodPrenct < skillData.activateLimitVaule && skillData.activateLimitVaule != -1){
						continue ;
					}
				}
				else if(skillData.activateLimitType == 2){
					if(cityBloodPrenct > skillData.activateLimitVaule && skillData.activateLimitVaule != -1){
						continue ;
					}
				}
				else if(skillData.activateLimitType == 3){
					int lastWome = GameDataCenter.GetInstance().levelData.girlCount - GameLevelMgr.GetInstance().m_escapeNum ;
					if(lastWome > skillData.activateLimitVaule && skillData.activateLimitVaule != -1){
						continue ;
					}
				}

				skills.Add(skill);
			}
			return skills ;
		}
		
		public FightCreatureData GetFightCreatureData(){
			return m_data ;
		}
		
		public void SetCrit(bool isCrit){
			m_isCrit = isCrit ;
		}

		
		public	void SetColor(Color color)
		{
			if(m_object != null)
				m_object.transform.FindChild(gameGlobal.CREATURE).gameObject.renderer.material.color = color ;
		}
	}
}


