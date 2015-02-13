using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameEvent;
using common ;

namespace GameLogical.GameEnitity.AI
{
	public class PetChangePetUpState : CStateBase<CPet>
	{
		protected static PetChangePetUpState instance;
		public void Release(){
			
		}
		public void Enter(CPet type){
			type.m_petAIData.time = 0.0f ;
			type.Stop();
			/*if(gameGlobal.g_fightSceneUI != null)
			{
				GameObject sceneOb = MonoBehaviour.Instantiate( gameGlobal.g_fightSceneUI.m_objList["shanxianhou"] ) as GameObject ;
				sceneOb.transform.position = type.m_changePos ;
				sceneOb.transform.FindChild("creature").animation.Play("effect");
			}*/
			
		}
		public void Execute(CPet type, float time){
			type.m_petAIData.time += time ;
			if(type.m_petAIData.time >= 0.5f){
				Action(type,time);
			}
			
		}
		
		public void Think(CPet type){
			
		}
		
		public void Action(CPet type, float time){
			type.GetRenderObject().transform.position = type.m_changePos;
			PetPatrolAIData aiData = type.m_petAIData as PetPatrolAIData ;
			aiData.patrolPathList[0] = GameLevel.GameLevelMgr.GetInstance().m_petBrithPointArr[type.m_data.seat];
			type.m_petAIData = aiData ;
			type.SetState(PetStandState.getInstance());
			type.blood += 1 ;

			if(EnitityMgr.GetInstance().m_needReset == true){
				EnitityMgr.GetInstance().m_needReset = false ;
				EnitityMgr.GetInstance().ResetPet();
			}
			else{
				PetMoudleData petData = fileMgr.GetInstance().GetData(type.m_data.moudleID,CsvType.CSV_TYPE_PET) as PetMoudleData ;
				//draw monster
				if(petData.attackLockCount != 0){
					List<CCreature> monsterList = EnitityMgr.GetInstance().GetMonsterList() ;
					List<int> drawList = new List<int>() ;
					//escape
					for(int i = 0; i<monsterList.Count; ++i){
						if(drawList.Count < petData.attackLockCount + 1){
							if(monsterList[i].GetEnitityAiState() == AIState.AI_STATE_ESCAPE && monsterList[i].GetEnitityAiState() != AIState.AI_STATE_DEATH){
								float dis = type.GetRenderObject().transform.position.x - monsterList[i].GetRenderObject().transform.position.x ;
								if(Mathf.Abs(dis) < AICommon.AI_MONSTER_BE_DRAW_DIS){
									drawList.Add(monsterList[i].GetId());
								}
							}
						}
						else{
							break ;
						}
					}

					//attack city
					for(int i = 0; i<monsterList.Count; ++i){
						if(drawList.Count < petData.attackLockCount + 1){
							if(monsterList[i].GetEnitityAiState() == AIState.AI_STATE_ACTTACK_CITY && monsterList[i].GetEnitityAiState() != AIState.AI_STATE_DEATH){
								float dis = type.GetRenderObject().transform.position.x - monsterList[i].GetRenderObject().transform.position.x ;
								if(Mathf.Abs(dis) < AICommon.AI_MONSTER_BE_DRAW_DIS){
									drawList.Add(monsterList[i].GetId());
								}
							}
						}
						else{
							break ;
						}
					}

					//attack back up
					for(int i = 0; i<monsterList.Count; ++i){
						if(drawList.Count < petData.attackLockCount + 1){
							if(monsterList[i].GetEnitityAiState() != AIState.AI_STATE_DEATH){
								CMonster monster = monsterList[i] as CMonster ;
								
								CPet target = monster.m_targetCreature as CPet ;
								if(target == null)
									continue ;
								PetMoudleData targetData = fileMgr.GetInstance().GetData(target.m_data.moudleID,CsvType.CSV_TYPE_PET) as PetMoudleData ;
								if(targetData.attackLockCount == 0){
									drawList.Add(monsterList[i].GetId());
									//target.RemoveAttack(monster.GetId());
									monster.m_targetCreature = type ;
								}
								
								/*float dis = type.GetRenderObject().transform.position.x - monsterList[i].GetRenderObject().transform.position.x ;
								if(Mathf.Abs(dis) < AICommon.AI_MONSTER_BE_DRAW_DIS){
									drawList.Add(monsterList[i].GetId());
								}*/
							}
						}
						else{
							break ;
						}
					}

					//out battle
					for(int i = 0; i<monsterList.Count; ++i){
						if(drawList.Count < petData.attackLockCount + 1){
							if(monsterList[i].GetEnitityAiState() == AIState.AI_STATE_STAND && monsterList[i].GetEnitityAiState() != AIState.AI_STATE_DEATH){
								float dis = type.GetRenderObject().transform.position.x - monsterList[i].GetRenderObject().transform.position.x ;
								if(Mathf.Abs(dis) < AICommon.AI_MONSTER_BE_DRAW_DIS){
									drawList.Add(monsterList[i].GetId());
								}
							}
						}
						else{
							break ;
						}
					}



					//attack
					for(int i = 0; i<monsterList.Count; ++i){
						if(drawList.Count < petData.attackLockCount + 1){
							if(monsterList[i].GetEnitityAiState() == AIState.AI_STATE_MOVE && monsterList[i].GetEnitityAiState() != AIState.AI_STATE_DEATH){
								float dis = type.GetRenderObject().transform.position.x - monsterList[i].GetRenderObject().transform.position.x ;
								if(Mathf.Abs(dis) < AICommon.AI_MONSTER_BE_DRAW_DIS){
									drawList.Add(monsterList[i].GetId());
								}
							}
						}
						else{
							break ;
						}
					}

					//other
					for(int i = 0; i<monsterList.Count; ++i){
						if(drawList.Count < petData.attackLockCount + 1){
							if( monsterList[i].GetEnitityAiState() != AIState.AI_STATE_DEATH){
								float dis = type.GetRenderObject().transform.position.x - monsterList[i].GetRenderObject().transform.position.x ;
								if(Mathf.Abs(dis) < AICommon.AI_MONSTER_BE_DRAW_DIS){
									drawList.Add(monsterList[i].GetId());
								}
							}
						}
						else{
							break ;
						}
					}

					for(int i = 0; i<drawList.Count; ++i){
						CMonster monster = EnitityMgr.GetInstance().GetEnitity(drawList[i]) as CMonster;

						CPet target = monster.m_targetCreature as CPet ;
						if(target != null)
							target.RemoveAttack(monster.GetId());

						monster.m_targetCreature = type ;
						monster.SetState(MonsterBeDrawState.getInstance());
						type.AddAttack(monster.GetId());
					}
				}
			}

		}
		
		public void Exit(CPet type){
			type.m_changePos = Vector3.zero;

		}
		public void OnMessage(CPet type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_CHANGE_PET_UP ;
		}
		
		public static PetChangePetUpState getInstance(){
			if(instance==null){instance = new PetChangePetUpState();}
			return instance;
		}
	}
}


