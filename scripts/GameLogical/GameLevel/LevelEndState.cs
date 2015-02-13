using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using AppUtility;
using GameEvent ;
using GameLogical.GameEnitity;
using common ;
using GameLogical.GameEnitity.AI ;

namespace GameLogical.GameLevel{
	public class LevelEndState: CStateBase<Object>{
		protected static LevelEndState instance;
		
		public void Release(){
			
		}
		public void Enter(Object type){
			if(GameDataCenter.GetInstance().isFristLevel == true){
				GameLevel.GameLevelMgr.GetInstance().m_levelStateMachin.ChangeState(FristLevelEndState.getInstance());
				return ;
			}

			if(GameLevelMgr.GetInstance().m_isSkip == true)
			{
				int pointId = GameLevelMgr.GetInstance().m_levelID ;
				Dictionary<string,object> messageData = new Dictionary<string, object>();
				messageData.Add("pointId",pointId);
				main.SendNetMessage(GameMessgeHandle.LevelMessageRegister.SKIP_LEVEL_RESULT,messageData) ;
				return ;
			}


			if(GameLevelMgr.GetInstance().m_levelType == LevelType.LEVEL_TYPE_PVE_NORMAL_POINT){
				EndGameDto result = new EndGameDto();
				result.loseNpc = GameLevel.GameLevelMgr.GetInstance().m_killMonsterNum ;
				result.pointId = GameLevel.GameLevelMgr.GetInstance().m_levelID ;
				result.robWoman= GameLevel.GameLevelMgr.GetInstance().m_escapeNum ;
				//result.energy  = CLineSmoothMgr.GetInstance().buleStone + CLineSmoothMgr.GetInstance().greenStone + CLineSmoothMgr.GetInstance().redStone ;
				result.gemCount= result.energy ;
				if(GameLevel.GameLevelMgr.GetInstance().m_escapeNum<GameDataCenter.GetInstance().levelData.girlCount){
					result.isWin = 1 ;
				}
				else{
					result.isWin = 2 ;
				}
				
				/*for(int i = 0; i<CLineSmoothMgr.GetInstance().m_rewardList.Count; ++i){
					result.rewardList.Add(CLineSmoothMgr.GetInstance().m_rewardList[i]);
				}*/
				
				result.npcList = GameLevel.GameLevelMgr.GetInstance().m_killMonsterIdList ;
				result.cityLastHp = EnitityMgr.GetInstance().city.blood ;
				Dictionary<string,object> data = new Dictionary<string, object>();
				data.Add("result",result.structToDic());
				main.SendNetMessage(GameMessgeHandle.LevelMessageRegister.LEVEL_RESULT,data);
				GameLevel.GameLevelMgr.GetInstance().m_isEnd = true ;
			}
			else if(GameLevelMgr.GetInstance().m_levelType == LevelType.LEVEL_TYPE_PVE_MONSTER_INVASION){
				EventResultDto eventResultDto = new EventResultDto(null);
				eventResultDto.eventId = GameDataCenter.GetInstance().cruSelectEvent.id ;
				eventResultDto.playerId= GameDataCenter.GetInstance().cruSelectEvent.playerId ;
				/*for(int i = 0; i<CLineSmoothMgr.GetInstance().m_rewardList.Count; ++i){
					eventResultDto.rewardList.Add(CLineSmoothMgr.GetInstance().m_rewardList[i]);
				}*/
				
				eventResultDto.hp = GameLevelMgr.GetInstance().m_bossHp;
				if(eventResultDto.hp != -1){
					//
					if(GameLevelMgr.GetInstance().m_bossLastHp != 0){
						List<CCreature> monsterList = EnitityMgr.GetInstance().GetMonsterList();
						for(int i = 0; i<monsterList.Count; ++i){
							CMonster monster =  monsterList[i] as CMonster ;
							MonsterMoudleData moudleData = (MonsterMoudleData)fileMgr.GetInstance().GetData(monster.m_data.moudleID,CsvType.CSV_TYPE_MONSTER);
							if(moudleData.strength >= 3){
								if(monster.m_data.blood > 0){
									eventResultDto.lastHp = monster.m_data.blood ;
								}
								else{
									eventResultDto.lastHp = 0 ;
								}
								
								break ;
							}
						}
					}
					//
					else{
						eventResultDto.lastHp = 0 ;
					}
					
				}
				//never see the boss
				else{
					eventResultDto.lastHp = -1 ;
				}
				
				eventResultDto.npcList = GameLevel.GameLevelMgr.GetInstance().m_killMonsterIdList ;
				Dictionary<string,object> data = new Dictionary<string, object>();
				data.Add("result",eventResultDto.structToDic());
				main.SendNetMessage(GameMessgeHandle.LevelMessageRegister.EVENT_ATTACK_RESULT,data);
				GameLevel.GameLevelMgr.GetInstance().m_isEnd = true ;
			}
			else if(GameLevelMgr.GetInstance().m_levelType == LevelType.LEVEL_TYPE_PVE_TASK_NPC){
				EndGameDto result = new EndGameDto();
				result.loseNpc = GameLevel.GameLevelMgr.GetInstance().m_killMonsterNum ;
				result.pointId = GameLevel.GameLevelMgr.GetInstance().m_levelID ;
				result.robWoman= GameLevel.GameLevelMgr.GetInstance().m_escapeNum ;
				if(GameLevel.GameLevelMgr.GetInstance().m_escapeNum<GameDataCenter.GetInstance().levelData.girlCount){
					result.isWin = 1 ;
				}
				else{
					result.isWin = 2 ;
				}
				
				/*for(int i = 0; i<CLineSmoothMgr.GetInstance().m_rewardList.Count; ++i){
					result.rewardList.Add(CLineSmoothMgr.GetInstance().m_rewardList[i]);
				}*/
				
				result.npcList = GameLevel.GameLevelMgr.GetInstance().m_killMonsterIdList ;
				result.cityLastHp = EnitityMgr.GetInstance().city.blood ;
				result.taskId = GameLevelMgr.GetInstance().m_taskId ;

				//result.energy  = CLineSmoothMgr.GetInstance().buleStone + CLineSmoothMgr.GetInstance().greenStone + CLineSmoothMgr.GetInstance().redStone ;
				//result.gemCount= result.energy ;

				Dictionary<string,object> data = new Dictionary<string, object>();
				data.Add("result",result.structToDic());
				//data.Add("taskId",GameLevelMgr.GetInstance().m_taskId);
				main.SendNetMessage(GameMessgeHandle.LevelMessageRegister.TASK_NPC_RESULT,data);
				GameLevel.GameLevelMgr.GetInstance().m_isEnd = true ;
			}
			else if(GameLevelMgr.GetInstance().m_levelType == LevelType.LEVEL_TYPE_PVE_CARBON_POINT){
				EndGameDto result = new EndGameDto();
				result.loseNpc = GameLevel.GameLevelMgr.GetInstance().m_killMonsterNum ;
				result.pointId = GameLevel.GameLevelMgr.GetInstance().m_levelID ;
				result.robWoman= GameLevel.GameLevelMgr.GetInstance().m_escapeNum ;
				//result.energy  = CLineSmoothMgr.GetInstance().buleStone + CLineSmoothMgr.GetInstance().greenStone + CLineSmoothMgr.GetInstance().redStone ;
				result.gemCount= result.energy ;
				if(GameLevel.GameLevelMgr.GetInstance().m_escapeNum<GameDataCenter.GetInstance().levelData.girlCount){
					result.isWin = 1 ;
				}
				else{
					result.isWin = 2 ;
				}
				
				/*for(int i = 0; i<CLineSmoothMgr.GetInstance().m_rewardList.Count; ++i){
					result.rewardList.Add(CLineSmoothMgr.GetInstance().m_rewardList[i]);
				}*/
				
				result.npcList = GameLevel.GameLevelMgr.GetInstance().m_killMonsterIdList ;
				result.cityLastHp = EnitityMgr.GetInstance().city.blood ;
				Dictionary<string,object> data = new Dictionary<string, object>();
				data.Add("result",result.structToDic());
				main.SendNetMessage(GameMessgeHandle.PlayerMessageRegister.CARBON_RESULT,data);
				GameLevel.GameLevelMgr.GetInstance().m_isEnd = true ;
			}
			
		}
		public void Execute(Object type, float time){
			
		}
		public void Exit(Object type){
			
		}
		public void OnMessage(Object type, EventMessageBase data){
	
		}
		public AIState  GetState(){
			return 0;
		}
		
		public static LevelEndState getInstance(){
			if(instance == null) instance = new LevelEndState();
			return instance;
		}
	}
}

