using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using GameLogical.GameEnitity.AI ;
using GameEvent ;
using GameLogical.GameEnitity ;

namespace GameLogical.GameLevel{
	public class LevelPvPBeginState : CStateBase<Object>
	{
		protected static LevelPvPBeginState instance;
		public float m_delyTime = 0.0f ;
		public void Release(){}
		public void Enter(Object type){
			m_delyTime = -1.0f ;
			EnitityMgr.GetInstance().m_staticScene = 1 ;
			/*EventMessageBase createSmoothMessage = new EventMessageBase();
			createSmoothMessage.eventMessageModel = EventMessageModel.eEVentMessageModel_Smooth ;
			createSmoothMessage.eventMessageAction= 2 ;
			EventMgr.GetInstance().OnEventMgr(createSmoothMessage);*/
			gameGlobal.g_LoadingPage.show();

			
			//EnitityMgr.GetInstance().CreateEnitity(EnitityType.ENITITY_TYPE_PVP_CHARACTER,2);			
			List<PetDto> petIdList = GameDataCenter.GetInstance().pvpMyFightPackData.petDtoList ;
			CPet pet = null ;
			CreaturePetData creatureData = new CreaturePetData();
			for(int i = 0; i < petIdList.Count; ++i){
				creatureData.petDto = petIdList[i] ;
				creatureData.pos 	= GameLevel.GameLevelMgr.GetInstance().m_petBrithPointArr[i] ;
				creatureData.pos.x -= 10.0f ;
				pet = EnitityMgr.GetInstance().CreateEnitity( EnitityType.ENITITY_TYPE_PET,creatureData) as CPet;
				//pet.GetRenderObject().transform.position = GameLevel.GameLevelMgr.GetInstance().m_petBrithPointArr[i] ;
				/*PetPatrolAIData patrolData = new PetPatrolAIData();
				patrolData.patrolPathList[0].x = creatureData.pos.x;
				patrolData.patrolPathList[0].y = creatureData.pos.y;
				patrolData.patrolPathList[0].z = creatureData.pos.z;
				patrolData.patrolPathList[1] = new Vector3(37.5f,GameLevel.GameLevelMgr.GetInstance().m_petBrithPointArr[i].y,0.0f);
				patrolData.destPathIndex = 0 ;
				pet.m_petAIData = patrolData ;*/
				//pet.m_data.blood *= 1 ;
				pet.m_data.maxBlood = pet.m_data.blood ;
				GameLevelMgr.GetInstance().m_pvpCreatureTotalNum++ ;
			}


			//EnitityMgr.GetInstance().CreateEnitity(EnitityType.ENITITY_TYPE_ENEMY_CHARACTER,2);
			petIdList = GameDataCenter.GetInstance().pvpOtherFightPackData.petDtoList ;
			CEnemyPet enemy = null ;
			for(int i = 0; i < petIdList.Count; ++i){
				creatureData.petDto = petIdList[i] ;
				creatureData.pos	= GameLevel.GameLevelMgr.GetInstance().m_enemyPetBrithPoinArr[i] ;
				enemy = EnitityMgr.GetInstance().CreateEnitity( EnitityType.ENITITY_TYPE_ENEMY_PET,creatureData) as CEnemyPet;
				/*if(petIdList[i].professionId == 11 || petIdList[i].professionId == 12){
					Vector3 pos = GameLevel.GameLevelMgr.GetInstance().m_petBrithPointArr[i] ;
					pos.x += 80.0f ;
					enemy.GetRenderObject().transform.position = pos ;
				}
				else{
					Vector3 pos = GameLevel.GameLevelMgr.GetInstance().m_petBrithPointArr[i] ;
					pos.x += 90.0f ;
					enemy.GetRenderObject().transform.position = pos ;
				}*/
	
				PetPatrolAIData patrolData = new PetPatrolAIData();
				patrolData.patrolPathList[0].x = creatureData.pos.x;
				patrolData.patrolPathList[0].y = creatureData.pos.y;
				patrolData.patrolPathList[0].z = creatureData.pos.z;
				patrolData.patrolPathList[1] = new Vector3(37.5f,GameLevel.GameLevelMgr.GetInstance().m_petBrithPointArr[i].y,0.0f);
				patrolData.destPathIndex = 0 ;
				pet.m_petAIData = patrolData ;

				//enemy.m_data.blood *= 7 ;
				enemy.m_data.maxBlood = enemy.m_data.blood ;

				GameLevelMgr.GetInstance().m_pvpCreatureTotalNum++ ;
			}
			
			//UI
			//EnitityMgr.GetInstance().Energy = 0 ;
			/*GameDataCenter.GetInstance().pvpPlayerInfo.mpStone = GameDataCenter.GetInstance().pvpPlayerInfo.redEnergy 
																	+ GameDataCenter.GetInstance().pvpPlayerInfo.blueEnery 
																	+ GameDataCenter.GetInstance().pvpPlayerInfo.yellowEnergy ;*/
			//gameGlobal.g_PvPFightSceneUI.show();

			MGResouce.LoadUIData  LoginData = new MGResouce.LoadUIData();
			LoginData.packName = "FightSceneUI";
			LoginData.name = "PVPFightSceneUI";
			LoginData.scriptName = "PVPFightScenePanelUI";
			//data1.isDispose =false;
			LoginData.LoadUICallBack = gameGlobal.ShowPVPFightScenePanelUI;
			//data1.isDispose =false;
			//gameGlobal.gMailUI(data1);
			MGResouce.BundleMgr.Instance.LoadUI(LoginData);
		}

		public void Execute(Object type, float time){
			if(m_delyTime == -1.0f){
				if(GameLevelMgr.GetInstance().m_pvpCreatureTotalNum == 0){
					if(gameGlobal.g_PvPFightSceneUI != null && gameGlobal.g_PvPFightSceneUI.m_finishLoad == true){
						gameGlobal.g_PvPFightSceneUI.show();
						GameObject ob = gameGlobal.g_rescoureMgr.GetGameObjectResource("pvpScene") as GameObject;
						common.debug.GetInstance().AppCheckSlow(ob);
						ob.transform.position = new Vector3(0.0f,0.0f,80.0f);
						GameLevel.GameLevelMgr.GetInstance().m_SceneObject = MonoBehaviour.Instantiate(ob) as GameObject;
						m_delyTime = 0.0f ;

					}
				}
			}
			else{
				if(m_delyTime < 1.0f){
					m_delyTime += time ;
				}
				else{
					EnitityMgr.GetInstance().m_staticScene = 0 ;
					GameLevel.GameLevelMgr.GetInstance().m_levelStateMachin.ChangeState(LevelPvPPlayingState.getInstance());
				}
				
			}
		}

		public void Exit(Object type){
			
		}
		public void OnMessage(Object type, EventMessageBase message){

		}
		public AIState  GetState(){
			return 0;
		}
		
		public static LevelPvPBeginState getInstance(){
			if(instance == null) instance = new LevelPvPBeginState();
			return instance;
		}
	}
}


