using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using AppUtility;
using GameEvent ;
using GameLogical.GameEnitity;
using common ;
using GameLogical.GameEnitity.AI ;

namespace GameLogical.GameLevel{
	public class LevelBeginState: CStateBase<Object>{
		protected static LevelBeginState instance;
		
		public void Release(){
			
		}
		public void Enter(Object type){
			gameGlobal.g_LoadingPage.show();
			/*for(int i=0;i<gameGlobal.g_fightSceneUI.transform.parent.childCount;i++){
				if(!gameGlobal.g_fightSceneUI.transform.parent.GetChild(i).name.Equals("FightSceneUI")){
					MonoBehaviour.Destroy(gameGlobal.g_fightSceneUI.transform.parent.GetChild(i).gameObject);
				}
			}*/
			Resources.UnloadUnusedAssets();
			//EnitityMgr.GetInstance().Energy = 0;imageId

			for(int i = 0; i<GameDataCenter.GetInstance().pointsList.Count; ++i){
				if(GameDataCenter.GetInstance().pointsList[i].id == GameDataCenter.GetInstance().levelData.id){
					if(GameDataCenter.GetInstance().pointsList[i].isHaveAttack == 0){
						GameLevelMgr.GetInstance().m_isFristTime = true ;
					}
				}
			}

			ResourceMoudleData resData = fileMgr.GetInstance().GetData(GameDataCenter.GetInstance().levelData.imageId,CsvType.CSV_TYPE_RESOUCE) as ResourceMoudleData ;
			GameObject ob = gameGlobal.g_rescoureMgr.GetGameObjectResource(resData.path) as GameObject;
			common.debug.GetInstance().AppCheckSlow(ob);
			GameLevel.GameLevelMgr.GetInstance().m_SceneObject = MonoBehaviour.Instantiate(ob) as GameObject;
			GameLevel.GameLevelMgr.GetInstance().m_SceneObject.transform.position = new Vector3(0.0f,0.0f,85.0f);
			//city
			EnitityMgr.GetInstance().CreateEnitity(EnitityType.ENITITY_TYPE_CITY,1);
			
			//character
			//EnitityMgr.GetInstance().CreateEnitity(EnitityType.ENITITY_TYPE_CHARACTER,2);
			
			List<PointNpcDto> pointList = GameDataCenter.GetInstance().levelData.npcs ;
			//UI
			/*gameGlobal.g_fightSceneUI.SetWomenNum(10);
			gameGlobal.g_fightSceneUI.maxWave = pointList.Count ;
			gameGlobal.g_fightSceneUI.curWave = 0 ;
			gameGlobal.g_fightSceneUI.StartWave();
			
			gameGlobal.g_fightSceneUI.maxBlood = EnitityMgr.GetInstance().city.blood ;
			gameGlobal.g_fightSceneUI.SetBloodProgressBar(gameGlobal.g_fightSceneUI.maxBlood);*/
			
			
			
			List<PetDto> petIdList = GameDataCenter.GetInstance().petFightPackData.petDtoList ;
			CPet pet = null ;
			CreaturePetData creatureData = new CreaturePetData();
			PetMoudleData petMoudleData  ;
			for(int i = 0; i < petIdList.Count; ++i){
				creatureData.petDto = petIdList[i] ;
				petMoudleData = fileMgr.GetInstance().GetData(creatureData.petDto.betConfigId,CsvType.CSV_TYPE_PET) as PetMoudleData;
				if(petMoudleData.attackLockCount == 0){
					EnitityMgr.GetInstance().m_AttackLockCount++ ;
				}
				else{
					EnitityMgr.GetInstance().m_AttackLockCount += petMoudleData.attackLockCount ;
				}

				creatureData.pos 	= GameLevel.GameLevelMgr.GetInstance().m_petBrithPointArr[petIdList[i].seat] ;
				pet = EnitityMgr.GetInstance().CreateEnitity( EnitityType.ENITITY_TYPE_PET,creatureData) as CPet;
			}

			petIdList = GameDataCenter.GetInstance().petFightPackData.sparePetList ;
			for(int i = 0; i<petIdList.Count; ++i){
				creatureData.petDto = petIdList[i] ;
				//creatureData.pos 	= GameLevel.GameLevelMgr.GetInstance().m_petBrithPointArr[petIdList[i].seat] ;
				pet = EnitityMgr.GetInstance().CreateEnitity( EnitityType.ENITYTY_TYPE_BACK_UP_PET,creatureData) as CPet;
			}

			MGResouce.LoadUIData  LoginData = new MGResouce.LoadUIData();
			LoginData.packName = "FightSceneUI";
			LoginData.name = "FightSceneUI";
			LoginData.scriptName = "FightScenePanelUI";
			LoginData.LoadUICallBack = gameGlobal.ShowFightScenePanelUI;
			LoginData.isMask = false ;
			MGResouce.BundleMgr.Instance.LoadUI(LoginData);
		}
		public void Execute(Object type, float time){
			//GameLevel.GameLevelMgr.GetInstance().m_levelStateMachin.ChangeState(LevelPlayingState.getInstance());
		}
		public void Exit(Object type){
			
		}
		public void OnMessage(Object type, EventMessageBase data){

		}
		public AIState  GetState(){
			return AIState.AI_STATE_STAND;
		}
		
		public static LevelBeginState getInstance(){
			if(instance == null) instance = new LevelBeginState();
			return instance;
		}
	}
}

