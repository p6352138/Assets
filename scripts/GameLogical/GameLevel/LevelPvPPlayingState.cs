using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using GameLogical.GameEnitity.AI ;
using GameEvent ;
using GameLogical.GameEnitity ;
using GameMessgeHandle ;

namespace GameLogical.GameLevel{
	public class LevelPvPPlayingState : CStateBase<Object>
	{
		protected static LevelPvPPlayingState instance;
			
		public void Release(){
			
		}
		public void Enter(Object type){
			
		}
		public void Execute(Object type, float time){
			
		}
		public void Exit(Object type){
			
		}
		public void OnMessage(Object type, EventMessageBase data){
			if(data.eventMessageModel == EventMessageModel.eEventMessageModel_Enitity){
				switch((EnitityAction)data.eventMessageAction){
				case EnitityAction.ENITITY_ACTION_DEATH:{
					EventMessageDeathEnd deathMessage = data as EventMessageDeathEnd ;
					int id = int.Parse(deathMessage.ob.name);
					CCreature creature = EnitityMgr.GetInstance().GetEnitity(id);
					//pet death
					if(creature.GetEnitityType() == EnitityType.ENITITY_TYPE_PET){
						CCreature petCreature = null;
						for(int i = 0 ; i<EnitityMgr.GetInstance().GetPetList().Count; ++i){
							petCreature = EnitityMgr.GetInstance().GetPetList()[i] ;
							if(petCreature.GetEnitityAiState() != AIState.AI_STATE_DEATH){
								return ;
							}
						}

						//all death
						//if(EnitityMgr.GetInstance().GetPetList().Count == 1){
							//lose
						if(GameDataCenter.GetInstance().pvpType == 1){
							Dictionary<string,object> dic = new Dictionary<string, object>();
							dic.Add("win",2);
							dic.Add("fighterId",GameDataCenter.GetInstance().pvpPlayerInfo.playerId);
							main.SendNetMessage(LevelMessageRegister.FIGHT_PVP_RESULT,dic);
						}
						else if(GameDataCenter.GetInstance().pvpType == 2){
							GameLevel.GameLevelMgr.GetInstance().m_isEnd = true ;
							GameDataCenter.GetInstance().pvpResult.win = 2 ;
							GameDataCenter.GetInstance().pvpResult.gold= 0 ;
							GameDataCenter.GetInstance().pvpResult.exp = 0 ;
							GameLevel.GameLevelMgr.GetInstance().m_levelStateMachin.ChangeState(LevelPvPEndState.getInstance());
							//gameGlobal.g_LevelResultUI.Show(ResultType.RESULT_TYPE_PVP);
						}
						for(int i = 0; i< EnitityMgr.GetInstance().GetPetList().Count; ++i){
							CPet pet = EnitityMgr.GetInstance().GetPetList()[i] as CPet ;
							if(pet.GetEnitityAiState() != AIState.AI_STATE_DEATH){
								pet.m_stateMachine.ChangeState(PetStandState.getInstance());
							}
							
						}
						
						for(int i = 0; i< EnitityMgr.GetInstance().GetMonsterList().Count; ++i){
							CEnemyPet monster = EnitityMgr.GetInstance().GetMonsterList()[i] as CEnemyPet ;
							monster.m_stateMachine.ChangeState(EnemyPetStandState.getInstance());
						}

					}
					//enemy pet death
					else if(creature.GetEnitityType() == EnitityType.ENITITY_TYPE_ENEMY_PET){
						if(EnitityMgr.GetInstance().GetMonsterList().Count == 1){
							//win
							if(GameDataCenter.GetInstance().pvpType == 1){
								Dictionary<string,object> dic = new Dictionary<string, object>();
								dic.Add("win",1);
								dic.Add("fighterId",GameDataCenter.GetInstance().pvpPlayerInfo.playerId);
								main.SendNetMessage(LevelMessageRegister.FIGHT_PVP_RESULT,dic);
							}
							else if(GameDataCenter.GetInstance().pvpType == 2){
								GameLevel.GameLevelMgr.GetInstance().m_isEnd = true ;
								GameDataCenter.GetInstance().pvpResult.win = 1 ;
								GameDataCenter.GetInstance().pvpResult.gold= 0 ;
								GameDataCenter.GetInstance().pvpResult.exp = 0 ;
								GameLevel.GameLevelMgr.GetInstance().m_levelStateMachin.ChangeState(LevelPvPEndState.getInstance());
								
							}

							for(int i = 0; i< EnitityMgr.GetInstance().GetPetList().Count; ++i){
								CPet pet = EnitityMgr.GetInstance().GetPetList()[i] as CPet ;
								if(pet.GetEnitityAiState() != AIState.AI_STATE_DEATH){
									pet.m_stateMachine.ChangeState(PetStandState.getInstance());
								}
								
							}
							
							for(int i = 0; i< EnitityMgr.GetInstance().GetMonsterList().Count; ++i){
								CEnemyPet monster = EnitityMgr.GetInstance().GetMonsterList()[i] as CEnemyPet ;
								monster.m_stateMachine.ChangeState(EnemyPetStandState.getInstance());
							}
						}
					}


				}
					break ;
					
				/*case EnitityAction.ENITITY_ACTION_FIGHT:{
					EnitityMgr.GetInstance().Energy += 1 ;
					GameDataCenter.GetInstance().pvpPlayerInfo.engry += 1 ;
				}
					break ;*/
				}
			}
			
		}
		public AIState  GetState(){
			return 0;
		}
		
		public static LevelPvPPlayingState getInstance(){
			if(instance == null) instance = new LevelPvPPlayingState();
			return instance;
		}
	}
}


