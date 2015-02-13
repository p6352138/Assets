using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameEvent;

namespace GameLogical.GameEnitity.AI
{
	public class PetChangePetDownState : CStateBase<CPet>
	{
		protected static PetChangePetDownState instance;
		public void Release(){
			
		}
		public void Enter(CPet type){
			type.m_petAIData.time = 0.0f ;
			type.Stop();

			CPet changePet = EnitityMgr.GetInstance().GetEnitity(type.m_changePetID) as CPet;
			PetMoudleData moudleData = common.fileMgr.GetInstance().GetData(changePet.m_data.moudleID,common.CsvType.CSV_TYPE_PET) as PetMoudleData;
			if(gameGlobal.g_fightSceneUI != null)
			{
				if(moudleData.attackLockCount == 0){
					GameObject sceneOb = MonoBehaviour.Instantiate( gameGlobal.g_fightSceneUI.m_objList["shanxianqian"] ) as GameObject ;
					sceneOb.transform.position = type.GetRenderObject().transform.position ;
					sceneOb.transform.FindChild("creature").animation.Play("effect");
				}
				else{
					GameObject sceneOb = MonoBehaviour.Instantiate( gameGlobal.g_fightSceneUI.m_objList["shanxianqian2"] ) as GameObject ;
					sceneOb.transform.position = type.GetRenderObject().transform.position ;
					sceneOb.transform.FindChild("creature").animation.Play("effect");
				}

			}
			
		}
		public void Execute(CPet type, float time){
			type.m_petAIData.time += time ;
			if(type.m_petAIData.time >= 0.2f){
				Action(type,time);
			}

		}
		
		public void Think(CPet type){

		}
		
		public void Action(CPet type, float time){
			EventMessageChangePetUp changePetUpMessage = new EventMessageChangePetUp();
			changePetUpMessage.pos = type.GetRenderObject().transform.position ;
			CPet changePet = EnitityMgr.GetInstance().GetEnitity(type.m_changePetID) as CPet;
			changePet.OnMessage(changePetUpMessage);
			type.GetRenderObject().transform.position = new Vector3(-1000.0f,0.0f,0.0f);
			type.SetState(PetSleepState.getInstance());

			EnitityMgr.GetInstance().ChangePet(type,changePet);

			List<CCreature> monsterList = EnitityMgr.GetInstance().GetMonsterList();
			for(int i = 0; i < monsterList.Count; ++i){
				CMonster monster = monsterList[i] as CMonster ;
				if(monster.m_targetCreature != null){
					if(monster.m_targetCreature.GetId() == type.GetId()){
						monster.m_targetCreature = null ;
						monster.SetState(MonsterOutBattleState.getInstance());
					}
				}

			}

			type.ClearAttack();
		}
		
		public void Exit(CPet type){

			type.m_changePetID = -1 ;
		}
		public void OnMessage(CPet type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_CHANGE_PET_DOWN ;
		}
		
		public static PetChangePetDownState getInstance(){
			if(instance==null){instance = new PetChangePetDownState();}
			return instance;
		}
	}
}


