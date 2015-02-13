using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using AppUtility;
using GameEvent ;
using GameLogical.GameEnitity;
using GameLogical.Guide;
using common ;
using GameLogical.GameEnitity.AI ;

namespace GameLogical.GameLevel{
	public class LevelPlayingState: CStateBase<Object>{
		protected static LevelPlayingState instance;
		
		public void Release(){
			
		}
		public void Enter(Object type){
			Time.timeScale = 1;
			//GameLevelMgr.GetInstance().m_isStop = false ;

			GameLevelMgr.GetInstance().m_taskId = GameDataCenter.GetInstance().levelData.taskId ;
			//monster wave 1
			if(GameLevelMgr.GetInstance().m_curWave == 0){
				List<PointNpcDto> pointList = GameDataCenter.GetInstance().levelData.npcs ;
				GameLevelMgr.GetInstance().m_curWave = 1 ;
				MonsterPointCreateMessage message = new MonsterPointCreateMessage();
				message.pointData = pointList[0] ;
				EventMgr.GetInstance().OnEventMgr(message);
			}
		}
		public void Execute(Object type, float time){
			
		}
		public void Exit(Object type){
			
		}
		public void OnMessage(Object type, EventMessageBase message){
			if(message.eventMessageModel == EventMessageModel.eEventMessageModel_Level){
				switch((LevelMessageAction)message.eventMessageAction){
				case LevelMessageAction.LEVEL_MESSAGE_ACTION_CREATE_MONSTER :{
					MonsterPointCreateMessage monsterPointMessage = message as MonsterPointCreateMessage ;
					List<NpcDto> npcList = monsterPointMessage.pointData.npcList ;
					
					/*for(int i = 0; i<4; ++i){
						List<CCreature> creature = new List<CCreature>();
						GameLevel.GameLevelMgr.GetInstance().m_monsterList.Add(creature);
					}*/

					int index = 0 ;
					//List<Vector3> posList = CountPosition(npcList);
					List<NpcDto> sortList = SortMonsterTeam(npcList);
					List<Vector3> posList = FindTreamPostion(sortList);
					GameLevelMgr.GetInstance().m_curWaveMonsterNum = 0 ;
					EnitityMgr.GetInstance().m_curLockCount = 0 ;
					//MonsterMoudleData monsterMoudleData ;
					for(int i = 0; i<sortList.Count; ++i){
						NpcDto npcDto = sortList[i] ;
						GameLevelMgr.GetInstance().m_curWaveMonsterNum += npcDto.number ;
						for(int j = 0; j<npcDto.number; ++j){
							CreateMonsterData createMonsterData = new CreateMonsterData();
							createMonsterData.moudleID = npcDto.npcId ;
							createMonsterData.rewardID = npcDto.rewardId ;
							createMonsterData.lastHp   = npcDto.lastHp ;
							createMonsterData.pos	   = posList[index];
							index++ ;
							CMonster creature = EnitityMgr.GetInstance().CreateEnitity( EnitityType.ENITITY_TYPE_MONSTER,createMonsterData ) as CMonster;
						}
					}
					
					/*for(int i = 0; i<4; ++i){
						GameLevel.GameLevelMgr.GetInstance().m_monsterList[i].Clear();
						GameLevelMgr.GetInstance().m_bossLine = -1 ;
					}*/
					
					GameLevel.GameLevelMgr.GetInstance().m_monsterNum += GameLevel.GameLevelMgr.GetInstance().m_curWaveMonsterNum ;
				}
					break ;
				}
				
			}
			
			//Enitity message
			else if(message.eventMessageModel == EventMessageModel.eEventMessageModel_Enitity){
				switch((EnitityAction)message.eventMessageAction){
				case EnitityAction.ENITITY_ACTION_ESCAPE:{
					EventMessageMonsterEscape escapeMessage = message as EventMessageMonsterEscape ;
					
					CMonster creature =  (CMonster)EnitityMgr.GetInstance().GetEnitity(escapeMessage.id);
					
					MonsterMoudleData moudleData = (MonsterMoudleData)fileMgr.GetInstance().GetData(creature.m_data.moudleID,CsvType.CSV_TYPE_MONSTER);

					//super boss
					/*if(moudleData.strength == 5){
						GameLevel.GameLevelMgr.GetInstance().m_escapeNum += 10 ;
						gameGlobal.g_fightSceneUI.SetWomenNum(GameDataCenter.GetInstance().levelData.girlCount - GameLevel.GameLevelMgr.GetInstance().m_escapeNum);
					}*/
					//boss

					if(moudleData.strength == 5){
						GameLevel.GameLevelMgr.GetInstance().m_escapeNum += 10 ;
						gameGlobal.g_fightSceneUI.SetWomenNum(GameDataCenter.GetInstance().levelData.girlCount - GameLevel.GameLevelMgr.GetInstance().m_escapeNum);
					}
					else if(moudleData.strength >= 3){
						GameLevel.GameLevelMgr.GetInstance().m_escapeNum += 1 ;
						gameGlobal.g_fightSceneUI.SetWomenNum(GameDataCenter.GetInstance().levelData.girlCount - GameLevel.GameLevelMgr.GetInstance().m_escapeNum);
					}
					//elite 
					else if(moudleData.strength == 2){
						GameLevel.GameLevelMgr.GetInstance().m_escapeNum += 1 ;
						gameGlobal.g_fightSceneUI.SetWomenNum(GameDataCenter.GetInstance().levelData.girlCount - GameLevel.GameLevelMgr.GetInstance().m_escapeNum);
					}
					//normal
					else{
						GameLevel.GameLevelMgr.GetInstance().m_escapeNum ++ ;
						gameGlobal.g_fightSceneUI.SetWomenNum(GameDataCenter.GetInstance().levelData.girlCount - GameLevel.GameLevelMgr.GetInstance().m_escapeNum);
						GameLevelMgr.GetInstance().m_curWaveMonsterNum-- ;
					}
						
					
					if(GameLevel.GameLevelMgr.GetInstance().m_escapeNum >= GameDataCenter.GetInstance().levelData.girlCount){
						//EndGame();
						GameLevel.GameLevelMgr.GetInstance().m_levelStateMachin.ChangeState(LevelEndState.getInstance());
					}
					
					if(GameLevelMgr.GetInstance().m_curWaveMonsterNum <= 0){
						List<PointNpcDto> pointList = GameDataCenter.GetInstance().levelData.npcs ;
						//next wave
						if(GameLevelMgr.GetInstance().m_curWave < pointList.Count){
							gameGlobal.g_fightSceneUI.SetWaveNumText(++gameGlobal.g_fightSceneUI.curWave);

						}
						//last wave , end game
						else{
							GameLevel.GameLevelMgr.GetInstance().m_levelStateMachin.ChangeState(LevelEndState.getInstance());
						}
						
					}

				}
					break ;
				case EnitityAction.ENITITY_ACTION_DEATH:{
					EventMessageDeathEnd deathMessage = message as EventMessageDeathEnd ;
					int id = int.Parse(deathMessage.ob.name);
					CCreature creature = EnitityMgr.GetInstance().GetEnitity(id);
					if(creature.GetEnitityType() == EnitityType.ENITITY_TYPE_MONSTER){
						CMonster monster = creature as CMonster ;
						GameLevel.GameLevelMgr.GetInstance().m_killMonsterNum++ ;
						GameLevel.GameLevelMgr.GetInstance().m_killMonsterIdList.Add(monster.getMonsterData().moudleID);
						GameLevelMgr.GetInstance().m_curWaveMonsterNum-- ;
						/*if(GameLevel.GameLevelMgr.GetInstance().m_escapeNum + GameLevel.GameLevelMgr.GetInstance().m_killMonsterNum >= GameLevel.GameLevelMgr.GetInstance().m_monsterNum){
							//EndGame();
							GameLevel.GameLevelMgr.GetInstance().m_levelStateMachin.ChangeState(LevelEndState.getInstance());
						}*/
						if(GameLevelMgr.GetInstance().m_curWaveMonsterNum <= 0){
							//last wave end game
							if(GameLevelMgr.GetInstance().m_curWave == GameDataCenter.GetInstance().levelData.npcs.Count){
								GameLevel.GameLevelMgr.GetInstance().m_levelStateMachin.ChangeState(LevelEndState.getInstance());
							}
							//next wave
							else{
								/*List<PointNpcDto> pointList = GameDataCenter.GetInstance().levelData.npcs ;
								GameLevelMgr.GetInstance().m_curWave++ ;
								GameLevelMgr.GetInstance().m_curWaveMonsterNum = pointList[GameLevelMgr.GetInstance().m_curWave - 1].npcList.Count ;
								MonsterPointCreateMessage messagePointCreate = new MonsterPointCreateMessage();
								messagePointCreate.pointData = pointList[GameLevelMgr.GetInstance().m_curWave - 1] ;
								GameLevel.GameLevelMgr.GetInstance().m_monsterNum+=pointList[GameLevelMgr.GetInstance().m_curWave - 1].npcList.Count ;
								EventMgr.GetInstance().OnEventMgr(messagePointCreate);*/
								gameGlobal.g_fightSceneUI.SetWaveNumText(++gameGlobal.g_fightSceneUI.curWave);
							}
						}
					}
					//check pet all death
					else if(creature.GetEnitityType() == EnitityType.ENITITY_TYPE_PET){
						List<CCreature> fightPetList = EnitityMgr.GetInstance().GetPetList();
						for(int i = 0; i< fightPetList.Count; ++i){
							if(fightPetList[i].GetEnitityAiState() != AIState.AI_STATE_DEATH){
								return ;
							}
						}

						if(GameDataCenter.GetInstance().levelData.changePetCount != 0){
							fightPetList = EnitityMgr.GetInstance().GetBackupPetList();
							for(int i = 0; i< fightPetList.Count; ++i){
								if(fightPetList[i].GetFightCreatureData().blood > 0){
									return ;
								}
							}
						}

						//check is last wave
						if(gameGlobal.g_fightSceneUI.curWave != GameDataCenter.GetInstance().levelData.npcs.Count){
							GameLevel.GameLevelMgr.GetInstance().m_escapeNum += 10 ;
							gameGlobal.g_fightSceneUI.SetWomenNum(0);
						}
						else{
							List<CCreature> monsterList = EnitityMgr.GetInstance().GetMonsterList();
							MonsterMoudleData monsterMoudleData ; 
							for(int i = 0; i<monsterList.Count; ++i){
								monsterMoudleData = fileMgr.GetInstance().GetData(monsterList[i].GetFightCreatureData().moudleID,CsvType.CSV_TYPE_PET) as MonsterMoudleData;
								if(monsterMoudleData.strength > 1){
									GameLevel.GameLevelMgr.GetInstance().m_escapeNum += 10 ;
									gameGlobal.g_fightSceneUI.SetWomenNum(0);
									break ;
								}
								GameLevel.GameLevelMgr.GetInstance().m_escapeNum++ ;
								//gameGlobal.g_fightSceneUI.SetWomenNum(0);
							}
						}

						GameLevel.GameLevelMgr.GetInstance().m_levelStateMachin.ChangeState(LevelEndState.getInstance());
					}
				}
					break ;

				
				}
			}
			else if(message.eventMessageModel == EventMessageModel.eEventMessageModel_PLAY_STATE){
				GameLevel.GameLevelMgr.GetInstance().m_levelStateMachin.ChangeState(LevelPauseState.getInstance());
			}
		}
		public AIState  GetState(){
			return AIState.AI_STATE_PATRAL;
		}
		
		public static LevelPlayingState getInstance(){
			if(instance == null) instance = new LevelPlayingState();
			return instance;
		}

		List<Vector3> CountPosition(List<NpcDto> npcList){
			NpcDto npcDto ;
			MonsterMoudleData monsterMoudleData ;
			List<Vector3> result = new List<Vector3>();
			Vector3 tempPos ;
			int bossLine  = -1;
			int bossCross = -1;
			int pointIndex ;
			List<int> lineCount = new List<int>();
			for(int i = 0; i<4; ++i){
				lineCount.Add(0);
			}

			for(int i = 0; i<npcList.Count; ++i){
				npcDto = npcList[i] ;
				monsterMoudleData = fileMgr.GetInstance().GetData(npcDto.npcId,CsvType.CSV_TYPE_MONSTER) as MonsterMoudleData ;
				for(int j = 0; j<npcDto.number; ++j){
					//boss
					if(monsterMoudleData.strength == 3){
						bossLine = Random.Range(2,4);
						bossCross= Random.Range(0,5);
						tempPos = GameLevel.GameLevelMgr.GetInstance().m_monsterBrithPointArr[bossLine] ;
						tempPos.x += bossCross * AICommon.AI_MONSTER_TEAM_GAP ;
						result.Add(tempPos);
					}
					else{
						if(GameLevelMgr.GetInstance().m_canSmooth == true){
							if(monsterMoudleData.strength >= 3){
								pointIndex = Random.Range(1,3);
							}
							else{
								pointIndex = Random.Range(0,3);
							}
							
						}
						else{
							if(monsterMoudleData.strength >= 3){
								pointIndex = Random.Range(1,3);
							}
							else{
								pointIndex = Random.Range(0,4);
							}
						}
						
						if(lineCount[pointIndex] == bossCross){
							lineCount[pointIndex]++ ;
						}
						tempPos = GameLevel.GameLevelMgr.GetInstance().m_monsterBrithPointArr[pointIndex] ;
						tempPos.x += lineCount[pointIndex] * AICommon.AI_MONSTER_TEAM_GAP ;
						lineCount[pointIndex]++ ;
						result.Add(tempPos);
					}
				}

			}

			return result ;
		}

		List<Vector3> FindTreamPostion(List<NpcDto> npcList){
			int teamIndex = Random.Range(0,GameLevelMgr.TEAM_POINT_NUM) ;
			int pointIndex ;
			NpcDto npcDto ;
			Vector3 tempPos ;
			List<Vector3> result = new List<Vector3>();

			for(int i = 0; i<npcList.Count; ++i){
				npcDto = npcList[i] ;
				//monsterMoudleData = fileMgr.GetInstance().GetData(npcDto.npcId,CsvType.CSV_TYPE_MONSTER) as MonsterMoudleData ;
				for(int j = 0; j<npcDto.number; ++j){
					pointIndex = GameLevelMgr.GetInstance().m_monsterTeamPoint[teamIndex,result.Count] ;
					tempPos = GameLevel.GameLevelMgr.GetInstance().m_monsterBrithPointArr[pointIndex % 4] ;
					tempPos.x += pointIndex/4 * AICommon.AI_MONSTER_TEAM_GAP ;
					result.Add(tempPos);
				}
			}
			return result ;
		}

		List<NpcDto> SortMonsterTeam(List<NpcDto> npcList){
			NpcDto npcDto ;
			List<NpcDto> sortList = new List<NpcDto>();
			MonsterMoudleData monsterMoudleData ;
			for(int i = npcList.Count - 1; i>=0; --i){
				npcDto = npcList[i] ;
				monsterMoudleData = fileMgr.GetInstance().GetData(npcDto.npcId,CsvType.CSV_TYPE_MONSTER) as MonsterMoudleData ;
				if(monsterMoudleData.profession == 12){
					sortList.Add(npcList[i]);
					npcList.RemoveAt(i);
				}
			}
			
			for(int i = npcList.Count - 1; i>=0; --i){
				npcDto = npcList[i] ;
				monsterMoudleData = fileMgr.GetInstance().GetData(npcDto.npcId,CsvType.CSV_TYPE_MONSTER) as MonsterMoudleData ;
				if(monsterMoudleData.profession == 11){
					sortList.Add(npcList[i]);
					npcList.RemoveAt(i);
				}
			}
			
			for(int i = npcList.Count - 1; i>=0; --i){
				npcDto = npcList[i] ;
				monsterMoudleData = fileMgr.GetInstance().GetData(npcDto.npcId,CsvType.CSV_TYPE_MONSTER) as MonsterMoudleData ;
				if(monsterMoudleData.profession == 13){
					sortList.Add(npcList[i]);
					npcList.RemoveAt(i);
				}
			}
			
			for(int i = npcList.Count - 1; i>=0; --i){
				npcDto = npcList[i] ;
				monsterMoudleData = fileMgr.GetInstance().GetData(npcDto.npcId,CsvType.CSV_TYPE_MONSTER) as MonsterMoudleData ;
				if(monsterMoudleData.profession == 14){
					sortList.Add(npcList[i]);
					npcList.RemoveAt(i);
				}
			}

			for(int i = npcList.Count - 1; i>=0; --i){
				npcDto = npcList[i] ;
				monsterMoudleData = fileMgr.GetInstance().GetData(npcDto.npcId,CsvType.CSV_TYPE_MONSTER) as MonsterMoudleData ;
				if(monsterMoudleData.profession == 15){
					sortList.Add(npcList[i]);
					npcList.RemoveAt(i);
				}
			}
			return sortList ;
		}
	}
}

