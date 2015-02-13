using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AppUtility;
using GameEvent;
using common ;
using GameLogical.Building ;
using GameLogical.GameLevel ;
using GameLogical.GameSkill;
using GameLogical.GameEnitity.AI ;

namespace GameLogical.GameEnitity{
	class EnitityMgr : Singleton<EnitityMgr>, IEvent
	{
		protected	List<CCreature>	m_allEnitityMap ;
		protected	List<CCreature>	m_enemyMap		;
		protected	List<CCreature>	m_partnerMap	;
		protected	List<CCreature>	m_womenMap		;
		protected   List<CCreature> m_bulletMap		;
		protected 	List<CCreature> m_backUpPetMap  ;
		protected	CCity						m_city			;
		protected	CPlayer						m_player		;
		protected	CEnemyPlayer				m_EnemyPlayer	;
		
		static	protected	int		creatureIndex				;
		static  public	    float[] SPEED_TYPE 					;
		//static  public		float	AI_THINK_TIME	=	0.3f	;

		private		int						m_energy			;

		public 		int						m_AttackLockCount ;
		public		int						m_curLockCount	  ;
		public		bool					m_needReset		  ;

		public		int						m_staticScene = 0  ;

		/*public int Energy{
			set{
				m_energy = value;
				if(GameLevelMgr.GetInstance().m_levelType == LevelType.LEVEL_TYPE_PVP){
					gameGlobal.g_PvPFightSceneUI.SetEnergy(m_energy);
				}
				else{
					gameGlobal.g_fightSceneUI.SetEnergy(m_energy);
				}

			}
			get{
				return m_energy;
			}
		}*/
		
		//static protected int m_indexID = 10;
		
		//////////////////////////////////interface////////////////////////////////////////
		public void Init(){
			m_allEnitityMap = new List<CCreature>();
			m_enemyMap		= new List<CCreature>();
			m_partnerMap	= new List<CCreature>();
			m_womenMap      = new List<CCreature>();
			m_bulletMap		= new List<CCreature>();
			m_backUpPetMap  = new List<CCreature>();
			EventMgr.GetInstance().AddLinsener(this);
			
			SPEED_TYPE = new float[4] {24.0f,20.0f,16.0f,48.0f} ;
		}
		
		public CCreature CreateEnitity(EnitityType type,object moudleID){
			CCreature creature = null ;
			creatureIndex++ ;
			switch(type){
			case EnitityType.ENITITY_TYPE_MONSTER:{
				CMonster monster = CreateMonster((CreateMonsterData)moudleID);
				common.debug.GetInstance().AppCheckSlow(monster);
				m_allEnitityMap.Add(monster);
				m_enemyMap.Add(monster);
				creature = monster ;
				//monster.InitSkill();
			}
				break ;
				
			case EnitityType.ENITITY_TYPE_CITY:{
				CCity city = CreateCity((int)moudleID);
				common.debug.GetInstance().AppCheckSlow(city);
				m_allEnitityMap.Add(city);
				m_city = city ;
				creature = city ;
			}
				break ;
				
			case EnitityType.ENITITY_TYPE_PET:{
				CPet pet = CreatePet((CreaturePetData)moudleID);
				common.debug.GetInstance().AppCheckSlow(pet);
				m_allEnitityMap.Add(pet);
				m_partnerMap.Add(pet);
				m_partnerMap[m_partnerMap.Count - 1] = m_allEnitityMap[m_allEnitityMap.Count - 1] ;
				creature = pet ;
				//pet.InitSkill();
			}
				break ;
				
			case EnitityType.ENITITY_TYPE_WOMEN:{
				CWomen women = CreateWomen((int)moudleID);
				common.debug.GetInstance().AppCheckSlow(women);
				m_allEnitityMap.Add(women);
				m_womenMap.Add(women);
				creature = women ;
			}
				break ;
				
			case EnitityType.ENITITY_TYPE_CHARACTER:{
				m_player	=	CreatePlayer(GameDataCenter.GetInstance().playerData); 
				common.debug.GetInstance().AppCheckSlow(m_player);
				m_allEnitityMap.Add(m_player);
				creature = m_player ;
				m_player.InitSkill();
				/*m_player.GetRenderObject().transform.position = GameLogical.GameLevel.GameLevelMgr.GetInstance().m_petBrithPointArr[4] ;
				PetPatrolAIData patrolData = new PetPatrolAIData();
				patrolData.patrolPathList[0] = m_player.GetRenderObject().transform.position;
				m_player.m_petAIData = patrolData ;*/
			}
				break ;

			case EnitityType.ENITITY_TYPE_PVP_CHARACTER:{
				m_player	=	CreatePlayer(GameDataCenter.GetInstance().playerData); 
				common.debug.GetInstance().AppCheckSlow(m_player);
				m_allEnitityMap.Add(m_player);
				creature = m_player ;
				m_player.InitPvPSkill();

			}
				break ;

			case EnitityType.ENITITY_TYPE_ENEMY_CHARACTER:{
				m_EnemyPlayer = CreateEnemyPlayer() ;
				common.debug.GetInstance().AppCheckSlow(m_EnemyPlayer);
				m_allEnitityMap.Add(m_EnemyPlayer);
				creature = m_player ;
				m_EnemyPlayer.InitPvPSkill();
			}
				break ;
				
			case EnitityType.ENITITY_TYPE_BULLET:{
				BulletData data = (BulletData)moudleID ;
				CBullet	bullet =	CreateBullet(data); 
				common.debug.GetInstance().AppCheckSlow(bullet);
				m_bulletMap.Add(bullet);
				creature = bullet ;
			}
				break;
				
			case EnitityType.ENITITY_TYPE_ENEMY_PET:{
				//PetDto pet = (CreaturePetData)moudleID ;
				CEnemyPet monster = CreateEnemyPet((CreaturePetData)moudleID) ;
				common.debug.GetInstance().AppCheckSlow(monster);
				m_allEnitityMap.Add(monster);
				m_enemyMap.Add(monster);
				creature = monster ;
				//monster.InitSkill();
			}
				break ;
			case EnitityType.ENITYTY_TYPE_BACK_UP_PET:{
				CPet backUpPet = CreateBackUpPet((CreaturePetData)moudleID) ;
				common.debug.GetInstance().AppCheckSlow(backUpPet);
				m_allEnitityMap.Add(backUpPet);
				m_backUpPetMap.Add(backUpPet);
				m_backUpPetMap[m_backUpPetMap.Count - 1] = m_allEnitityMap[m_allEnitityMap.Count - 1] ;
				creature = backUpPet ;
				//backUpPet.InitSkill();
			}
				break ;
			}
			return creature ;
		}
		
		public void DestroyEnitity(CCreature creature){
			EnitityType enitityType = creature.GetEnitityType();
			switch(enitityType){
				//monster
			case EnitityType.ENITITY_TYPE_MONSTER:{
				CMonster monster = creature as CMonster ;
				common.debug.GetInstance().AppCheckSlow(monster);
				if(m_allEnitityMap.Contains(monster) && m_enemyMap.Contains(monster)){
					MonsterMoudleData date = fileMgr.GetInstance().GetData(monster.m_data.moudleID, CsvType.CSV_TYPE_MONSTER) as MonsterMoudleData;
					/*switch(date.strength)
					{
					case 1:
						Energy += 10;
						break;
					case 2:
						Energy += 15;
						break;
					case 3:
						Energy += 20;
						break;
					}*/

					m_allEnitityMap.Remove(monster);
					m_enemyMap.Remove(monster);
					monster.Release();
					monster = null ;
				}
#if DEBUG
				else{
					common.debug.GetInstance().Error("Remove entity error:" + monster.id) ;
				}
#endif
			}
				break ;
			case EnitityType.ENITITY_TYPE_CITY:{
				m_allEnitityMap.Remove(m_city);
				m_city.Release();
				m_city = null ;
			}
				break;
			case EnitityType.ENITITY_TYPE_CHARACTER:{
				m_allEnitityMap.Remove(m_player);
				m_player.Release();
				m_player = null ;
			}
				break;

			case EnitityType.ENITITY_TYPE_ENEMY_CHARACTER:{
				m_allEnitityMap.Remove(m_EnemyPlayer);
				m_EnemyPlayer.Release();
				m_EnemyPlayer = null ;
			}
				break ;
			case EnitityType.ENITITY_TYPE_PET:{
				CPet pet = creature as CPet ;
				common.debug.GetInstance().AppCheckSlow(pet);
				if(m_allEnitityMap.Contains(pet) && m_partnerMap.Contains(pet)){
					m_allEnitityMap.Remove(pet);
					m_partnerMap.Remove(pet);
					pet.Release();
					pet = null ;
				}
				
			}
				break ;
			case EnitityType.ENITITY_TYPE_WOMEN:{
				CWomen women = creature as CWomen ;
				common.debug.GetInstance().AppCheckSlow(women);
				if(m_allEnitityMap.Contains(women) && m_womenMap.Contains(women)){
					m_allEnitityMap.Remove(women);
			
					m_womenMap.Remove(women);
					women.Release();
					women = null ;
				}
			}
				break ;
				
			case EnitityType.ENITITY_TYPE_BULLET:{
				CBullet bullet = creature as CBullet ;
				common.debug.GetInstance().AppCheckSlow(bullet);
				if(m_bulletMap.Contains(bullet)){
					m_bulletMap.Remove(bullet);
					bullet.Release();
					bullet = null ;
				}
			}
				break ;
				
			case EnitityType.ENITITY_TYPE_ENEMY_PET:{
				
				CEnemyPet monster = creature as CEnemyPet ;
				common.debug.GetInstance().AppCheckSlow(monster);
				if(m_allEnitityMap.Contains(monster) && m_enemyMap.Contains(monster)){
					m_allEnitityMap.Remove(monster);
					m_enemyMap.Remove(monster);
					monster.Release();
					monster = null ;
				}
			}
				break ;

			case EnitityType.ENITYTY_TYPE_BACK_UP_PET:{
				CPet monster = creature as CPet ;
				common.debug.GetInstance().AppCheckSlow(monster);
				if(m_allEnitityMap.Contains(monster) && m_backUpPetMap.Contains(monster)){
					m_allEnitityMap.Remove(monster);
					m_backUpPetMap.Remove(monster);
					monster.Release();
					monster = null ;
				}
			}
				break ;
			}
		}
		
		public void DestroyEnitityAll(){
			for(int i = m_allEnitityMap.Count - 1 ; i >= 0 ; --i){
				DestroyEnitity(m_allEnitityMap[i]);
			}
			for(int i = m_bulletMap.Count - 1; i>=0; --i){
				DestroyEnitity(m_bulletMap[i]);
			}
			/*foreach(CCreature temp in m_allEnitityMap.Values){
				DestroyEnitity(temp);
			}*/
			m_allEnitityMap.Clear();
			m_enemyMap.Clear();	
			m_partnerMap.Clear();
			m_womenMap.Clear();
			m_bulletMap.Clear();
			m_city = null ;
			m_player=null ;
			creatureIndex=0 ;
			m_AttackLockCount = 0 ;

			m_staticScene = 0 ;
			Time.timeScale = 1.0f ;
			common.common.blackScreen(false);

			m_needReset = false ;
			Resources.UnloadUnusedAssets();
			//gc
			System.GC.Collect();
		}

		public void ChangePet(CPet scrPet, CPet destPet){
			int scrSeat = scrPet.m_data.seat ;
			int destSeat= destPet.m_data.seat ;
			scrPet.m_data.seat = destSeat ;
			destPet.m_data.seat= scrSeat  ;

			if(scrSeat > 3){
				m_partnerMap.Add(scrPet);
				m_backUpPetMap.Remove(scrPet);
			}
			else{
				m_backUpPetMap.Add(scrPet);
				m_partnerMap.Remove(scrPet);
			}
			
			if(destSeat > 3){
				m_partnerMap.Add(destPet);
				m_backUpPetMap.Remove(destPet);
			}
			else{
				m_backUpPetMap.Add(destPet);
				m_partnerMap.Remove(destPet);
			}
		}

		public void Update(float deltaTime){
			if(GameLevelMgr.GetInstance().m_isStop == true)
				return ;

			if(m_staticScene > 0){
				for(int i = m_allEnitityMap.Count - 1 ; i >= 0 ; --i){
					if(m_allEnitityMap[i].GetFightCreatureData() != null){
						if(m_allEnitityMap[i].GetFightCreatureData().isMainRole == true){
							m_allEnitityMap[i].Update(deltaTime);
						}
					}
				}

				for(int i = m_bulletMap.Count - 1 ; i >= 0 ; --i){
					m_bulletMap[i].Update(deltaTime);
					
				}
			}
			else{
				for(int i = m_allEnitityMap.Count - 1 ; i >= 0 ; --i){
					m_allEnitityMap[i].Update(deltaTime);
				}
				
				for(int i = m_bulletMap.Count - 1 ; i >= 0 ; --i){
					m_bulletMap[i].Update(deltaTime);
				}
				if(m_staticScene < 0){
					m_staticScene = 0 ;
				}
			}
		}
		
		public void OnMessage(EventMessageBase message){
			if(message.eventMessageModel == EventMessageModel.eEventMessageModel_Enitity){
				EnitityAction action = (EnitityAction)message.eventMessageAction ;
				switch(action){
				case EnitityAction.ENITITY_ACTION_CREATE:{
					EventMessageCreateEnitity data = message as EventMessageCreateEnitity ;
#if DEBUG
					common.debug.GetInstance().AppCheckSlow(data);
#endif
					CreateEnitity(data.type,data.moudleID);
					
				}
					break;
					//real fight
				case EnitityAction.ENITITY_ACTION_FIGHT:{
					EventMessageFight fightMessage = message as EventMessageFight ;
					common.debug.GetInstance().AppCheckSlow(fightMessage);
					CCreature destCreature = EnitityMgr.GetInstance().GetEnitity(fightMessage.destCreatureId);
					CCreature scrCreature = EnitityMgr.GetInstance().GetEnitity(fightMessage.scrCreatureId);
					if(destCreature != null && scrCreature != null){
						destCreature.OnMessage(fightMessage);
						scrCreature.OnMessage(fightMessage);
//						if(destCreature.GetEnitityType() == EnitityType.ENITITY_TYPE_PET)
//						{
//							Energy ++;
//						}
//						else if(scrCreature.GetEnitityType() == EnitityType.ENITITY_TYPE_PET)
//						{
//							Energy++;
//						}
					}
				}
					break;
					
					//trigger fight
				case EnitityAction.ENITITY_ACTION_FIGHT_SATRT:{
					EventMessageFightStart fightMessage = message as EventMessageFightStart ;
					CCreature creature = FindEnitity(fightMessage.ob);
					if(creature != null){
						creature.OnMessage(message);
					}
					/*int id = int.Parse(fightMessage.ob.name);
					if(m_allEnitityMap.ContainsKey(id)){
						m_allEnitityMap[id].OnMessage(message);
					}*/
				}
					break;
				case EnitityAction.ENITITY_ACTION_FIGHT_FINISH:{
					EventMessageFightEnd fightMessage = message as EventMessageFightEnd ;
					CCreature creature = FindEnitity(fightMessage.ob);
					if(creature != null){
						creature.OnMessage(message);
					}
					/*int id = int.Parse(fightMessage.ob.name);
					if(m_allEnitityMap.ContainsKey(id)){
						m_allEnitityMap[id].OnMessage(message);
					}*/
				}
					break ;
				case EnitityAction.ENITITY_ACTION_DEATH:{
					EventMessageDeathEnd deathMessage = message as EventMessageDeathEnd ;
					
					
					int id = int.Parse(deathMessage.ob.name);
					
					foreach(CCreature temp in m_allEnitityMap){
						if(temp.GetId() != id)
							temp.OnMessage(message);
					}
					
					GameLevel.GameLevelMgr.GetInstance().OnMessage(message);
					
					CCreature creature = FindEnitity(deathMessage.ob);
					if(creature != null){
						creature.OnMessage(message);
					}
					
					/*if(m_allEnitityMap.ContainsKey(id)){
						m_allEnitityMap[id].OnMessage(message);
					}*/
				}
					break ;
					
				case EnitityAction.ENITITY_ACTION_ENTER_COLLIDER:{
					EventMessageEnterCollider enterMessage = message as EventMessageEnterCollider ;
					CCreature creature = FindEnitity(enterMessage.scrObject);
					if(creature != null){
						creature.OnMessage(message);
					}
					
					/*int id = int.Parse(enterMessage.scrObject.name);
					if(m_allEnitityMap.ContainsKey(id)){
						m_allEnitityMap[id].OnMessage(message);
					}*/
				}
					break ;
					
				case EnitityAction.ENITITY_ACTION_EXIT_COLLIDER:{
					EventMessageExitCollider enterMessage = message as EventMessageExitCollider ;
					CCreature creature = FindEnitity(enterMessage.scrObject);
					if(creature != null){
						creature.OnMessage(message);
					}
					/*int id = int.Parse(enterMessage.scrObject.name);
					if(m_allEnitityMap.ContainsKey(id)){
						m_allEnitityMap[id].OnMessage(message);
					}*/
					break ;
				}
					
				case EnitityAction.ENITITY_ACTION_ESCAPE:{
						//GameLevel.GameLevelMgr.GetInstance().OnMessage(message);
					for(int i = 0; i<m_womenMap.Count; ++i){
						m_womenMap[i].OnMessage(message);
					}
					}
					break ;

				case EnitityAction.ENITITY_ACTION_BE_DRAW:{
					for(int i = 0; i<m_womenMap.Count; ++i){
						m_womenMap[i].OnMessage(message);
					}
				}
					break ;
					
				case EnitityAction.ENITITY_ACTION_SELECT_ENITITY:{
					EventMessageEnititySelect selectMessage = message as EventMessageEnititySelect ;
					if(m_player != null){
						m_player.OnMessage(message);
//						Energy++;
					}
				}
					break ;
					
				case EnitityAction.ENITYTY_ACTION_SKILL:{
					EventMessageSkill skillMessage = message as EventMessageSkill ;
					CCreature creature = FindEnitity(skillMessage.ob);
					if(creature != null){
						creature.OnMessage(message);
					}
				}
					break ;

				

				case EnitityAction.ENITYTY_ACTION_SKILL_START:{
					EventMessageSkillStart skillMessage = message as EventMessageSkillStart ;
					CCreature creature = FindEnitity(skillMessage.ob);
					if(creature != null){
						creature.OnMessage(message);
					}
				}
					break ;

				case EnitityAction.ENITITY_ACTION_START_ANGRY_SKILL:{
					EventMessageEnitityStartAngrySkill startAngrySkill = message as EventMessageEnitityStartAngrySkill ;
					CCreature creature = GetEnitity(startAngrySkill.scrId);
					if(creature != null){
						creature.OnMessage(message);
					}
					//m_staticScene++ ;
				}
					break ;
					
				case EnitityAction.ENITITY_ACTION_SKILL_FINISH:{
					EventMessageSkillEnd skillEndMessage = message as EventMessageSkillEnd ;
					CCreature creature = FindEnitity(skillEndMessage.ob);
					if(creature != null){
						creature.OnMessage(message);
					}
				}
					break ;
					
				case EnitityAction.ENITITY_ACTION_WEAK:{
					for(int i = 0; i<m_partnerMap.Count; ++i){
						m_partnerMap[i].OnMessage(message);
					}
				}
					break;

				case EnitityAction.ENITITY_ACTION_LOCK_PET:{
					EventMessageLockPet lockPetMessage = message as EventMessageLockPet ;
					CCreature creature = GetEnitity(lockPetMessage.lockPetID);
					if(creature != null){
						creature.OnMessage(lockPetMessage);
					}
				}
					break ;

				case EnitityAction.ENITITY_ACTION_CHANGE_PET:{
					EventMessageChangePet changePetMessage = message as EventMessageChangePet ;
					CCreature creature = GetEnitity(changePetMessage.scrIndex);
					if(creature != null){
						creature.OnMessage(changePetMessage);
					}
				}
					break ;
				default:
					break;
				}
			}
			else if(message.eventMessageModel == EventMessageModel.eEventMessageModel_Level){
				
				//MonsterPointCreateMessage
				switch( (LevelMessageAction)message.eventMessageAction){
				case LevelMessageAction.LEVEL_MESSAGE_ACTION_AUTO_SKILL:{
					m_player.OnMessage(message);
				}
					break ;
				//case LevelMessageAction.LEVEL_MESSAGE_ACTION_CREATE_MONSTER:{
					
				//}
				//	break ;
				}
			}
			else if(message.eventMessageModel == EventMessageModel.eEVentMessageModel_Smooth){
				if(message.eventMessageAction == (int)SmoothAction.SMOOTH_FINISH){
//					m_player.OnMessage(message);
				}
				else if(message.eventMessageAction == (int)SmoothAction.SMOOTH_SUM)
				{
					EventMessageSmoothSum data = message as EventMessageSmoothSum;
			//		Energy += data.red;	
			//		Energy += data.blue;
			//		Energy += data.green;
				}
			}

			else if(message.eventMessageModel == EventMessageModel.eEventMessageModel_Skill){
				switch((SkillEventMessageAction)message.eventMessageAction){
					case SkillEventMessageAction.SKILL_ACTION_USE_SKILL:{
						EventMessageUseSkill useSkill = message as EventMessageUseSkill ;
						CCreature creature = GetEnitity(useSkill.id);
						if(creature != null){
							creature.OnMessage(message);
						}
					}
						break ;
				}

			}
			/*else if(message.eventMessageModel == EventMessageModel.eEventMessageModel_Skill)
			{
				if(message.eventMessageAction == (int)SkillEventMessageAction.SKILL_ACTION_USE_SKILL )
				{
				}
			}*/
			//MonsterPointCreateMessage
		}
		
		//////////////////////////////////public////////////////////////////////////////
		public  void FindNearCreature(Vector3 vPos,ref CCreature ob, ref float dis){
			float tempDis = int.MaxValue ;
			float compareDis;
			foreach(CCreature temp in m_allEnitityMap){
				compareDis = Vector3.Distance(temp.GetRenderObject().transform.position,vPos) ;
				if(compareDis < tempDis){
					ob = temp ;
					tempDis = compareDis ;
					dis = compareDis ;
				}
			}
		}
		
		public  void FindNearCreature(CCreature scrObj,ref CCreature ob, ref float dis){
			//Vector3 vPos  = scrObj.GetRenderObject().transform.position ;
			float tempDis = int.MaxValue ;
			float compareDis;
			foreach(CCreature temp in m_allEnitityMap){
				if(temp == scrObj)
					continue ;
				compareDis = Vector3.Distance(temp.GetRenderObject().transform.position,scrObj.GetRenderObject().transform.position) ;
				if(compareDis < tempDis){
					ob = temp ;
					tempDis = compareDis ;
					dis = compareDis ;
				}
			}
		}
		
		
		public void FindNearCreature(CCreature scrObj,EnitityType type,ref CCreature ob, ref float dis){
			//Vector3 vPos  = scrObj.GetRenderObject().transform.position ;
			float tempDis = int.MaxValue ;
			float compareDis;
			//find in monster
			if(type == EnitityType.ENITITY_TYPE_MONSTER){
				foreach(CCreature temp in m_enemyMap){
					if(temp == scrObj)
						continue ;
					compareDis = Vector3.Distance(temp.GetRenderObject().transform.position,scrObj.GetRenderObject().transform.position) ;
					if(compareDis < tempDis){
						ob = temp ;
						tempDis = compareDis ;
						dis = compareDis ;
					}
				}
			}
			//find in partner
			else{
				foreach(CCreature temp in m_partnerMap){
					if(temp == scrObj)
						continue ;
					compareDis = Vector3.Distance(temp.GetRenderObject().transform.position,scrObj.GetRenderObject().transform.position) ;
					if(compareDis < tempDis){
						ob = temp ;
						tempDis = compareDis ;
						dis = compareDis ;
					}
				}
			}
		}

		public void FindNearCreature(Vector3 vPos, EnitityType type, ref CCreature ob, ref float dis){
			float tempDis = int.MaxValue ;
			float compareDis;
			//find in monster
			if(type == EnitityType.ENITITY_TYPE_MONSTER){
				foreach(CCreature temp in m_enemyMap){
					compareDis = Vector3.Distance(temp.GetRenderObject().transform.position,vPos) ;
					if(compareDis < tempDis){
						ob = temp ;
						tempDis = compareDis ;
						dis = compareDis ;
					}
				}
			}
			//find in partner
			else{
				foreach(CCreature temp in m_partnerMap){
					compareDis = Vector3.Distance(temp.GetRenderObject().transform.position,vPos) ;
					if(compareDis < tempDis){
						ob = temp ;
						tempDis = compareDis ;
						dis = compareDis ;
					}
				}
			}
		}

		public void FindNearCreatureXY(Vector3 vPos, EnitityType type, ref CCreature ob, ref float dis){
			float tempDis = int.MaxValue ;
			float compareDis;
			float disX ;
			float disY ;
			//find in monster
			if(type == EnitityType.ENITITY_TYPE_MONSTER){
				foreach(CCreature temp in m_enemyMap){
					if(temp.GetRenderObject() != null){
						disX = temp.GetRenderObject().transform.position.x - vPos.x ;
						disY = temp.GetRenderObject().transform.position.y - vPos.y ;
						compareDis = Mathf.Abs(disX) + Mathf.Abs(disY);
						if(compareDis < tempDis){
							ob = temp ;
							tempDis = compareDis ;
							dis = compareDis ;
						}
					}
				}
			}
			//find in partner
			else{
				foreach(CCreature temp in m_partnerMap){
					disX = temp.GetRenderObject().transform.position.x - vPos.x ;
					disY = temp.GetRenderObject().transform.position.y - vPos.y ;
					compareDis = Mathf.Abs(disX) + Mathf.Abs(disY);
					if(compareDis < tempDis){
						ob = temp ;
						tempDis = compareDis ;
						dis = compareDis ;
					}
				}
			}
		}
		
		public List<CCreature> FindAreaCreatureList(CCreature scrObj,EnitityType type, float dis){
			//Vector3 vPos  = scrObj.GetRenderObject().transform.position ;
			float compareDis;
			List<CCreature> creatureList = new List<CCreature>();
			//find in monster
			if(type == EnitityType.ENITITY_TYPE_MONSTER){
				foreach(CCreature temp in m_enemyMap){
					if(temp == scrObj)
						continue ;
					compareDis = Vector3.Distance(temp.GetRenderObject().transform.position,scrObj.GetRenderObject().transform.position) ;
					if(compareDis < dis){
						creatureList.Add(temp);
					}
				}
			}
			//find in partner
			else{
				foreach(CCreature temp in m_partnerMap){
					if(temp == scrObj)
						continue ;
					compareDis = Vector3.Distance(temp.GetRenderObject().transform.position,scrObj.GetRenderObject().transform.position) ;
					if(compareDis < dis){
						creatureList.Add(temp);
					}
				}
			}
			
			return creatureList ;
		}
		
		
		public CMonster FindDangerousMonster(){
			float far =  float.MinValue ;
			CCreature creatrue = null ;
			for(int i = 0; i<m_enemyMap.Count ; ++i){
				if(m_enemyMap[i].GetEnitityAiState() == GameLogical.GameEnitity.AI.AIState.AI_STATE_ESCAPE){
					if(m_enemyMap[i].GetRenderObject().transform.position.x > far){
						far = m_enemyMap[i].GetRenderObject().transform.position.x ;
						creatrue = m_enemyMap[i] ;
					}	
				}
			}
			
			if(creatrue == null){
				far = float.MaxValue ;
				for(int i = 0; i<m_enemyMap.Count ; ++i){
					if(m_enemyMap[i].GetRenderObject() == null)
						continue ;
					if(m_enemyMap[i].GetRenderObject().transform.position.x < far && m_enemyMap[i].GetEnitityAiState() != GameLogical.GameEnitity.AI.AIState.AI_STATE_WEAK){
						far = m_enemyMap[i].GetRenderObject().transform.position.x ;
						creatrue = m_enemyMap[i] ;
					}	
				}
			}
			
			return creatrue as CMonster;
		}
		
		public CCreature FindEnitity(GameObject ob){
			try{
				int id = int.Parse( ob.name ) ;
				return GetEnitity(id);
			}
			catch{
				return null ;
			}
			
			
			//return null ;
		}
		
		public CCreature GetEnitity(int id){
			foreach(CCreature temp in m_allEnitityMap){
				if(temp.GetId() == id)
					return temp ;
			}
			/*if(m_allEnitityMap.ContainsKey(id)){
				return m_allEnitityMap[id] ;
			}*/
			return null ;
		}
		
		public Vector3 GetEnitityPos(int scrIndex, int destIndex){
			CCreature scrCreature = GetEnitity(scrIndex);
			CCreature destCreature= GetEnitity(destIndex);
			if(scrCreature != null && destCreature != null){
				if(scrCreature.GetRenderObject().transform.position.x > destCreature.GetRenderObject().transform.position.x){
					Vector3 reslut = destCreature.GetRenderObject().transform.position ;
					reslut.x += 1.0f ;
					return reslut ;
				}
				else{
					Vector3 reslut = destCreature.GetRenderObject().transform.position ;
					reslut.x -= 1.0f ;
					return reslut ;
				}
			}
			else{
				common.debug.GetInstance().Error("GetEnitityPos can not find enitity: scrIndex" + scrIndex + " destIndex" + destIndex);
				return Vector3.zero ;
			}
			/*if(m_allEnitityMap.(scrIndex) && m_allEnitityMap.ContainsKey(destIndex)){
				if(m_allEnitityMap[scrIndex].GetRenderObject().transform.position.x > m_allEnitityMap[destIndex].GetRenderObject().transform.position.x){
					Vector3 reslut = m_allEnitityMap[destIndex].GetRenderObject().transform.position ;
					reslut.x += 1.0f ;
					return reslut ;
				}
				else{
					Vector3 reslut = m_allEnitityMap[destIndex].GetRenderObject().transform.position ;
					reslut.x -= 1.0f ;
					return reslut ;
				}
			}
			else{
				common.debug.GetInstance().Error("GetEnitityPos can not find enitity: scrIndex" + scrIndex + " destIndex" + destIndex);
				return Vector3.zero ;
			}*/

		}
		//////////////////////////////////  get data   ////////////////////////////////////////
		public CCity city{
			get{
				return m_city ;
			}
		}
		
		public CPlayer palyer{
			get{
				return m_player ;
			}
		}

		public CEnemyPlayer enemyPlayer{
			get{
				return m_EnemyPlayer ;
			}
		}
		
		public List<CCreature> GetMonsterList(){
			return m_enemyMap ;
		}
		
		public List<CCreature> GetPetList(){
			return m_partnerMap ;
		}

		public List<CCreature> GetBackupPetList(){
			return m_backUpPetMap ;
		}

		public bool IsFullLock(){
			if(m_curLockCount >= m_AttackLockCount)
				return true ;
			else
				return false ;
		}
		/*public PetDto GetPetData(string id){
			for(int i = 0; i<GameDataCenter.GetInstance().petList.Count; ++i){
				if(GameDataCenter.GetInstance().petList[i].id == id)
					return null ;
					//return GameDataCenter.GetInstance().petList[i] ;
			}
			return null ;
		}*/
		
		//////////////////////////////////  Create creature   ////////////////////////////////////////
		protected CMonster CreateMonster(CreateMonsterData createData){
			if(fileMgr.GetInstance().monsterCsvData.dataDic.ContainsKey(createData.moudleID)){
				MonsterMoudleData monsterData = fileMgr.GetInstance().GetData(createData.moudleID,CsvType.CSV_TYPE_MONSTER) as MonsterMoudleData;
				ResourceMoudleData resource = fileMgr.GetInstance().GetData(monsterData.resourceID,CsvType.CSV_TYPE_RESOUCE) as ResourceMoudleData ;
				//GameObject ob = gameGlobal.g_rescoureMgr.GetGameObjectResource(resource.path) ;//gameGlobal.g_rescoureMgr.GetGameObjectResource(resource.path) as GameObject;

				CMonster   monster = new CMonster()  ;
				FightCreatureData data = new FightCreatureData() ;
				data.moudleID = createData.moudleID ;
				data.attack = monsterData.attack ;
				if(createData.lastHp == -1){
					data.blood  = monsterData.blood ;
				}
				else{
					data.blood  = createData.lastHp ;
					GameLevel.GameLevelMgr.GetInstance().m_bossHp = createData.lastHp ;
				}
				data.maxBlood = monsterData.blood ;
				
				data.moveSpeed=SPEED_TYPE[ monsterData.speed - 1];
				data.attackArea = monsterData.attackArea * 0.1f;
				data.eyeShotArea= monsterData.eyeShotArea* 0.1f;
				
				data.crit = monsterData.heavy ;
				data.duck = monsterData.dodge ;
				data.spell= monsterData.precision ;
				data.camp = monsterData.camp  ;
				
				//attack cd
				switch(monsterData.profession){
				case 11:{
					data.attackCD = 0.8f ;
					data.attackType = AttackType.ATTACK_TYPE_NEAR ;
				}
					break ;
				case 12:{
					data.attackCD = 1.8f ;
					data.attackType = AttackType.ATTACK_TYPE_NEAR ;
				}
					break ;
				case 13:{
					data.attackCD = 1.5f ;
					data.attackType = AttackType.ATTACK_TYPE_FAR ;
				}
					break ;
				case 14:{
					data.attackCD = 1.2f ;
					data.attackType = AttackType.ATTACK_TYPE_FAR ;
				}
					break ;
				case 15:{
					data.attackCD = 1.2f ;
					data.attackType = AttackType.ATTACK_TYPE_FAR ;
				}	
					break ;
				}
				
				data.id = 10000+creatureIndex ;
				monster.Init(data);

				MGResouce.LoadCreatureData loadData = new MGResouce.LoadCreatureData();
				loadData.packName = resource.packagePath ;
				loadData.name = resource.path ;
				loadData.fun  = monster.LoadObjectCallBack ;
				loadData.pos  = createData.pos ;
				MGResouce.BundleMgr.Instance.LoadCharcter(loadData);


				/*if(monsterData.strength >= 3){
					sceneOb = gameGlobal.g_rescoureMgr.GetGameObjectResource("dangerBossEffect") ;//(GameObject)gameGlobal.g_rescoureMgr.GetGameObjectResource("object/scene/dangerBossEffect");
					MuscClip.MusicClipMgr.GetInstance().MusicClips("feedback_boss");
					GameObject obj = (GameObject)MonoBehaviour.Instantiate(sceneOb);
					Vector3 effectPos = new Vector3();
					effectPos.x = 60.0f ;
					effectPos.y = 40.0f ;
					effectPos.z = -1.0f ;
					obj.transform.position = effectPos ;
				}*/
				
				
				/*if(createData.rewardID != -1){
					monster.rewardID = createData.rewardID ;
					ob = gameGlobal.g_rescoureMgr.GetGameObjectResource("box") ;//gameGlobal.g_rescoureMgr.GetGameObjectResource("object/scene/box") as GameObject;
					sceneOb = MonoBehaviour.Instantiate(ob) as GameObject;
					Vector3 scale = new Vector3();
					scale = sceneOb.transform.localScale ;
					sceneOb.transform.parent = monster.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_HEAD);
					sceneOb.transform.localScale = scale ;
					sceneOb.transform.localPosition = Vector3.one ;
				}*/
				
				return monster ;
			}
			else{
				common.debug.GetInstance().Error("Get monster moudel data failed :" + createData.moudleID);
				return null ;
			}

		}
		
		protected CEnemyPet CreateEnemyPet(CreaturePetData moudleID)
		{
			PetDto petDto = moudleID.petDto ;
			
			common.debug.GetInstance().AppCheckSlow(petDto);
			ResourceMoudleData resource = fileMgr.GetInstance().GetData(petDto.imageId,CsvType.CSV_TYPE_RESOUCE) as ResourceMoudleData ;


			
			PetMoudleData petData = fileMgr.GetInstance().GetData(petDto.betConfigId,CsvType.CSV_TYPE_PET) as PetMoudleData;
			
			CEnemyPet pet = new CEnemyPet();
			FightCreatureData data = new FightCreatureData();
			data.moudleID = petDto.betConfigId ;
			data.blood = petDto.hp ;
			data.maxBlood = petDto.hp ;
			data.attack = petDto.attack   ;
			data.moveSpeed = SPEED_TYPE[ petDto.speed - 1] ;
			data.attackArea = petData.attackArea * 0.1f;
			data.eyeShotArea= petData.eyeShotArea* 0.1f;
			data.crit = petDto.crit ;
			data.duck = petDto.duck ;
			data.spell= petDto.spell;
			data.camp = petData.camp;
				
			//skill
			for(int i = 0; i<petDto.skillDtoList.Count; ++i){
				if(petDto.skillDtoList[i].seat == 1){
					data.skillList.Add(petDto.skillDtoList[i].skillConfigId);
				}
			}

			for(int i = 0; i<petDto.skillDtoList.Count; ++i){
				if(petDto.skillDtoList[i].isOpen == 1 && petDto.skillDtoList[i].seat != 1){
					data.skillList.Add(petDto.skillDtoList[i].skillConfigId);
				}
			}
			
			//attack cd
			switch(petDto.professionId){
				case 11:{
					data.attackCD = 0.8f ;
					data.attackType = AttackType.ATTACK_TYPE_NEAR ;
				}
					break ;
				case 12:{
					data.attackCD = 1.8f ;
					data.attackType = AttackType.ATTACK_TYPE_NEAR ;
				}
					break ;
				case 13:{
					data.attackCD = 1.5f ;
					data.attackType = AttackType.ATTACK_TYPE_FAR ;
				}
					break ;
				case 14:{
					data.attackCD = 1.2f ;
					data.attackType = AttackType.ATTACK_TYPE_FAR ;
				}
					break ;
			}

			data.id = 20000+creatureIndex ; 
			data.severId = petDto.id ;
			pet.Init(data);
			MGResouce.LoadCreatureData loadData = new MGResouce.LoadCreatureData();
			loadData.packName = resource.packagePath   ;
			loadData.name 	  = resource.path 		   ;
			loadData.fun	  = pet.LoadObjectCallBack ;
			loadData.pos	  = moudleID.pos		   ;
			MGResouce.BundleMgr.Instance.LoadCharcter(loadData);

			return pet ;
		}
		
		protected CCity CreateCity(int moudleId){

			
			CCity city = new CCity();
			CityData data = new CityData();
			//BuildDto bulidDto = null ;
			int	cityImage = 53023	;
			int jiantaImage = 53033 ;
			/*for(int i = 0; i<GameDataCenter.GetInstance().buildDataList.Count; ++i){
				bulidDto = GameDataCenter.GetInstance().buildDataList[i] ;
				//city
				if(bulidDto.type == 1){
					data.gateBlood = bulidDto.hp ;
					cityImage = bulidDto.imageId ;
					data.attack = bulidDto.jiantaAttack ;
				}
				else if(bulidDto.type == 2){

					jiantaImage = bulidDto.imageId ;
				}
			}*/

			ResourceMoudleData resData = fileMgr.GetInstance().GetData(cityImage,CsvType.CSV_TYPE_RESOUCE) as ResourceMoudleData ;
			data.girlCount = 10   ;
			data.id = creatureIndex ; 
			data.moudleID = cityImage ;
			data.gateBlood= GameDataCenter.GetInstance().levelData.cityHp ;
			city.Init(data);

			MGResouce.LoadCreatureData loadData = new MGResouce.LoadCreatureData() ;
			loadData.fun = city.LoadObjectCallBack ;
			loadData.name= resData.path ;
			loadData.packName = resData.packagePath ;
			loadData.pos = new Vector3(14.5f,53.5f,82.0f);

			MGResouce.BundleMgr.Instance.LoadCity(loadData);

			return city ;
		}
		
		protected CPet CreatePet(CreaturePetData moudleID){
			
			PetDto petDto = moudleID.petDto ;
			
			common.debug.GetInstance().AppCheckSlow(petDto);
			
			ResourceMoudleData resource = fileMgr.GetInstance().GetData(petDto.imageId,CsvType.CSV_TYPE_RESOUCE) as ResourceMoudleData ;
			//GameObject ob = gameGlobal.g_rescoureMgr.GetGameObjectResource(resource.path) ;//gameGlobal.g_rescoureMgr.GetGameObjectResource(resource.path) as GameObject;


			
			PetMoudleData petData = fileMgr.GetInstance().GetData(petDto.betConfigId,CsvType.CSV_TYPE_PET) as PetMoudleData;
			
			CPet pet = new CPet();
			FightCreatureData data = new FightCreatureData();
			data.moudleID = petDto.betConfigId ;
			data.blood = petDto.hp ;
			data.maxBlood = petDto.hp ;
			data.attack = petDto.attack   ;
			data.moveSpeed = SPEED_TYPE[ petDto.speed - 1] ;
			data.attackArea = petData.attackArea * 0.1f;
			data.eyeShotArea= petData.eyeShotArea* 0.1f;
			data.crit = petDto.crit ;
			data.duck = petDto.duck ;
			data.star = petDto.quality;
			data.spell = petDto.spell ;
			data.seat  = petDto.seat  ;
			data.appHpSecond = petDto.addHpSeconds ;
			data.camp  = petData.camp ;
				
			//skill
			for(int i = 0; i<petDto.skillDtoList.Count; ++i){
				if(petDto.skillDtoList[i].seat == 1){
					data.skillList.Add(petDto.skillDtoList[i].skillConfigId);
				}
			}
			for(int i = 0; i<petDto.skillDtoList.Count; ++i){
				if(petDto.skillDtoList[i].isOpen == 1 && petDto.skillDtoList[i].seat != 1){
					data.skillList.Add(petDto.skillDtoList[i].skillConfigId);
				}
			}
			
			//attack cd
			switch(petDto.professionId){
				case 11:{
					data.attackCD = 0.8f ;
					data.attackType = AttackType.ATTACK_TYPE_NEAR ;
				}
					break ;
				case 12:{
					data.attackCD = 1.8f ;
					data.attackType = AttackType.ATTACK_TYPE_NEAR ;
				}
					break ;
				case 13:{
					data.attackCD = 1.5f ;
					data.attackType = AttackType.ATTACK_TYPE_FAR ;
				}
					break ;
				case 14:{
					data.attackCD = 1.2f ;
					data.attackType = AttackType.ATTACK_TYPE_FAR ;
				}
					break ;
			}

			data.id = 20000+creatureIndex ; 
			data.severId = petDto.id ;
			
			pet.Init(data);

			PetPatrolAIData patrolData = new PetPatrolAIData();
			patrolData.patrolPathList[0] = moudleID.pos;
			patrolData.patrolPathList[1].x = 37.5f;
			patrolData.patrolPathList[1].y = moudleID.pos.y;
			patrolData.patrolPathList[1].z = moudleID.pos.z;
			patrolData.destPathIndex = 0 ;
			pet.m_petAIData = patrolData ;

			MGResouce.LoadCreatureData loadData = new MGResouce.LoadCreatureData();
			loadData.packName = resource.packagePath   ;
			loadData.name 	  = resource.path 		   ;
			loadData.fun	  = pet.LoadObjectCallBack ;
			loadData.pos	  = moudleID.pos		   ;
			MGResouce.BundleMgr.Instance.LoadCharcter(loadData);


			return pet ;
		}

		protected CPet CreateBackUpPet(CreaturePetData moudleID){
			
			PetDto petDto = moudleID.petDto ;
			
			common.debug.GetInstance().AppCheckSlow(petDto);
			
			ResourceMoudleData resource = fileMgr.GetInstance().GetData(petDto.imageId,CsvType.CSV_TYPE_RESOUCE) as ResourceMoudleData ;
			//GameObject ob = gameGlobal.g_rescoureMgr.GetGameObjectResource(resource.path) ;//gameGlobal.g_rescoureMgr.GetGameObjectResource(resource.path) as GameObject;
			
			
			
			PetMoudleData petData = fileMgr.GetInstance().GetData(petDto.betConfigId,CsvType.CSV_TYPE_PET) as PetMoudleData;
			
			CPet pet = new CPet();
			FightCreatureData data = new FightCreatureData();
			data.moudleID = petDto.betConfigId ;
			data.blood = petDto.hp ;
			data.maxBlood = petDto.hp ;
			data.attack = petDto.attack   ;
			data.moveSpeed = SPEED_TYPE[ petDto.speed - 1] ;
			data.attackArea = petData.attackArea * 0.1f;
			data.eyeShotArea= petData.eyeShotArea* 0.1f;
			data.crit = petDto.crit ;
			data.duck = petDto.duck ;
			data.star = petDto.quality;
			data.spell = petDto.spell ;
			data.seat  = petDto.seat  ;
			data.appHpSecond = petDto.addHpSeconds ;
			data.camp  = petData.camp ;
			
			//skill
			for(int i = 0; i<petDto.skillDtoList.Count; ++i){
				if(petDto.skillDtoList[i].seat == 1){
					data.skillList.Add(petDto.skillDtoList[i].skillConfigId);
				}
			}
			for(int i = 0; i<petDto.skillDtoList.Count; ++i){
				if(petDto.skillDtoList[i].isOpen == 1 && petDto.skillDtoList[i].seat != 1){
					data.skillList.Add(petDto.skillDtoList[i].skillConfigId);
				}
			}
			
			//attack cd
			switch(petDto.professionId){
			case 11:{
				data.attackCD = 0.8f ;
				data.attackType = AttackType.ATTACK_TYPE_NEAR ;
			}
				break ;
			case 12:{
				data.attackCD = 1.8f ;
				data.attackType = AttackType.ATTACK_TYPE_NEAR ;
			}
				break ;
			case 13:{
				data.attackCD = 1.5f ;
				data.attackType = AttackType.ATTACK_TYPE_FAR ;
			}
				break ;
			case 14:{
				data.attackCD = 1.2f ;
				data.attackType = AttackType.ATTACK_TYPE_FAR ;
			}
				break ;
			}
			
			data.id = 20000+creatureIndex ; 
			data.severId = petDto.id ;
			
			pet.Init(data);
			
			PetPatrolAIData patrolData = new PetPatrolAIData();
			patrolData.patrolPathList[0] = moudleID.pos;
			patrolData.patrolPathList[1].x = 37.5f;
			patrolData.patrolPathList[1].y = moudleID.pos.y;
			patrolData.patrolPathList[1].z = moudleID.pos.z;
			patrolData.destPathIndex = 0 ;
			pet.m_petAIData = patrolData ;
			
			MGResouce.LoadCreatureData loadData = new MGResouce.LoadCreatureData();
			loadData.packName = resource.packagePath   ;
			loadData.name 	  = resource.path 		   ;
			loadData.fun	  = pet.LoadObjectCallBack ;
			loadData.pos	  = new Vector3(-1000f,0.0f,0.0f)		   ;
			MGResouce.BundleMgr.Instance.LoadCharcter(loadData);
			
			
			return pet ;
		}
		
		public void ResetPet(){
			foreach(CCreature temp in m_partnerMap){
				CPet pet = temp as CPet ;
				if(pet.GetEnitityAiState() == GameLogical.GameEnitity.AI.AIState.AI_STATE_CHANGE_PET_DOWN || pet.GetEnitityAiState() == GameLogical.GameEnitity.AI.AIState.AI_STATE_CHANGE_PET_UP){
					m_needReset = true ;
					return ;
				}
			}

			foreach(CCreature temp in m_partnerMap){
				CPet pet = temp as CPet ;
				if(pet.GetEnitityAiState() != GameLogical.GameEnitity.AI.AIState.AI_STATE_DEATH){
					pet.ResetPet();
				}
			}
		}
		
		protected CWomen CreateWomen(int moudleID){

			
			CWomen women = new CWomen();
			WomenData data = new WomenData();
			data.moveSpeed = 15.0f ;
			data.id = 30000+creatureIndex ;
			women.Init(data);
			MGResouce.LoadWomenData loadData = new MGResouce.LoadWomenData();
			loadData.fun = women.LoadObjectCallBack ;
			loadData.packName = "women" ;
			loadData.name	  = "women"	;
			loadData.monsterID=	moudleID;
			MGResouce.BundleMgr.Instance.LoadPlayer(loadData);


			return women ;
		}
		
		protected	CPlayer CreatePlayer(PlayerDto moudleID){
			//int playerMoudleID = GameDataCenter.GetInstance().playerData.professionId * 2 ;
			//PlayerMoudleData playdate = (PlayerMoudleData)fileMgr.GetInstance().GetData(playerMoudleID,CsvType.CSV_TYPE_PLAYER);

			ResourceMoudleData resource = fileMgr.GetInstance().resouceCsvData.dataDic[GameDataCenter.GetInstance().playerData.figureurl] as ResourceMoudleData;
			/*if(GameDataCenter.GetInstance().playerData.sex==1)
			{
				resource = fileMgr.GetInstance().resouceCsvData.dataDic[playdate.imageID] as ResourceMoudleData ;
			}
			else
			{
				resource = fileMgr.GetInstance().resouceCsvData.dataDic[playdate.imageID-1] as ResourceMoudleData ;
			}*/


			
			CPlayer player = new CPlayer();
			FightCreatureData data = new FightCreatureData();
			//PlayerData data = new PlayerData();


			data.enitityType = EnitityType.ENITITY_TYPE_CHARACTER ;
			data.id = 4000 + creatureIndex ;

			//player.renderObject = null ;;
			
			data.moudleID = moudleID.figureurl ;
			data.blood = 0 ;
			data.maxBlood = 0 ;
			//data.attack = moudleID.attack   ;
			data.moveSpeed = 0 ;
			data.attackArea = 0;
			data.eyeShotArea= 0;
			//data.crit = moudleID.crit ;
			data.duck = 0 ;
			player.Init(data);
			MGResouce.LoadCreatureData loadData = new MGResouce.LoadCreatureData() ;
			loadData.packName = resource.packagePath ;
			loadData.name	  = resource.path 		 ;
			loadData.fun	  = player.LoadObjectCallBack ;
			MGResouce.BundleMgr.Instance.LoadPlayer(loadData);


			return player ;
		}

		protected CEnemyPlayer CreateEnemyPlayer(){
			int playerMoudleID = GameDataCenter.GetInstance().pvpPlayerInfo.professionId * 2 ;
			PlayerMoudleData playdate = (PlayerMoudleData)fileMgr.GetInstance().GetData(playerMoudleID,CsvType.CSV_TYPE_PLAYER);
			
			ResourceMoudleData resource;
			if(GameDataCenter.GetInstance().pvpPlayerInfo.sex==1)
			{
				resource = fileMgr.GetInstance().resouceCsvData.dataDic[playdate.imageID] as ResourceMoudleData ;
			}
			else
			{
				resource = fileMgr.GetInstance().resouceCsvData.dataDic[playdate.imageID-1] as ResourceMoudleData ;
			}
			 

			CEnemyPlayer enemyPlayer = new CEnemyPlayer();
			FightCreatureData data = new FightCreatureData();

			data.enitityType = EnitityType.ENITITY_TYPE_ENEMY_CHARACTER ;
			data.id = 4000 + creatureIndex ;
			
			data.moudleID = GameDataCenter.GetInstance().pvpPlayerInfo.imageId ;
			data.blood = 0 ;
			data.maxBlood = 0 ;
			data.attack = GameDataCenter.GetInstance().pvpPlayerInfo.attack   ;
			data.moveSpeed = 0 ;
			data.attackArea = 0;
			data.eyeShotArea= 0;
			data.crit = GameDataCenter.GetInstance().pvpPlayerInfo.crit ;
			data.duck = 0 ;
			enemyPlayer.Init(data);
			MGResouce.LoadCreatureData loadData = new MGResouce.LoadCreatureData() ;
			loadData.packName = resource.packagePath ;
			loadData.name	  = resource.path 		 ;
			loadData.fun	  = enemyPlayer.LoadObjectCallBack ;
			MGResouce.BundleMgr.Instance.LoadPlayer(loadData);


			return enemyPlayer ;
		}

		protected CBullet CreateBullet(BulletData data){
			CBullet bullet = new CBullet();
			data.id = 5000 + creatureIndex ;

			if(!fileMgr.GetInstance().resouceCsvData.dataDic.ContainsKey(data.effectID)){
				common.debug.GetInstance().Error("Can not find resouce:" + data.effectID);
				return null ;
			}
			
			ResourceMoudleData resource = fileMgr.GetInstance().resouceCsvData.dataDic[data.effectID] as ResourceMoudleData ;

			MGResouce.LoadCreatureData loadData = new MGResouce.LoadCreatureData();
			loadData.packName = resource.packagePath ;
			loadData.name 	  = resource.path 		 ;
			loadData.fun	  = bullet.LoadObjectCallBack ;
			loadData.pos	  =	data.pos			 ;
			bullet.Init(data);
			MGResouce.BundleMgr.Instance.LoadBullet(loadData);

			return bullet;
		}
	}
}


