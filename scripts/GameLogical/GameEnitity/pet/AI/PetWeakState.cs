using UnityEngine;
using System.Collections;
using GameEvent;
using System.Collections.Generic;

namespace GameLogical.GameEnitity.AI{
	public class PetWeakState : CStateBase<CPet>
	{
		protected static PetWeakState instance;
		public void Release(){
			
		}
		public void Enter(CPet type){
			//talk
			/*PetMoudleData  petMoudleData = (PetMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID, common.CsvType.CSV_TYPE_PET);
			
			type.TalkLv(petMoudleData.talkLvInDie, petMoudleData.talkIDInDie);*/
			
			type.Stop();
		}
		
		public void Execute(CPet type, float time){
			type.m_sharkCurTime += time ;
			if(type.m_sharkCurTime >= 0.083f){
				type.m_sharkTotalTime -= type.m_sharkCurTime ;
				if(type.m_sharkTotalTime > 0.0f){
					type.Shark();
				}
				else{
					//relive
					int rate = Random.Range(0,100);
					if(rate < type.effectData.reLiveRate){
						type.SetHp(type.effectData.relive);
						type.m_stateMachine.ChangeState(PetStandState.getInstance());
					}
					else{
						EnitityMgr.GetInstance().m_curLockCount -= type.m_beLockCreatureIDList.Count ;
						type.m_beLockCreatureIDList.Clear();
						type.m_stateMachine.ChangeState(PetDeathState.getInstance());
						EventMessageDeathEnd message = new EventMessageDeathEnd();
						message.ob = type.GetRenderObject() ;
						EnitityMgr.GetInstance().OnMessage(message);

						/*EnitityMgr.GetInstance().Energy += 80 ;
						if(GameLevel.GameLevelMgr.GetInstance().m_levelType == GameLogical.GameLevel.LevelType.LEVEL_TYPE_PVP){
							GameDataCenter.GetInstance().pvpPlayerInfo.engry += 80 ;
						}*/
					}
				}
			}
		}
		public void Exit(CPet type){
			
		}
		public void OnMessage(CPet type, EventMessageBase data){
	
		}
		public AIState  GetState(){
			return AIState.AI_STATE_WEAK ;
		}
		
		public static PetWeakState getInstance(){
			if(instance==null){instance = new PetWeakState();}
			return instance;
		}
	}
}


