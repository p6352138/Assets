using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using GameEvent ;
using GameMessgeHandle;
using GameLogical.GameLevel;
using GameLogical.Guide;

namespace GameLogical.GameEnitity.AI
{
	/**
	 * move to the city gate state
	 * **/
	public class MonsterMoveState: CStateBase<CMonster>{
		protected static MonsterMoveState instance;
		public void Release(){
			
		}
		public void Enter(CMonster type){
			//type.TalkLv(monstermoudle.talkLvInMove, monstermoudle.talkIDInMove);
			
			type.Play("walk",WrapMode.Loop);
			type.m_targetCreature = null;

			type.m_monsterAIData.time = 0.0f ;
		}
		public void Execute(CMonster type, float time){
			type.m_monsterAIData.time += time ;
			if(type.m_monsterAIData.time >= AICommon.AI_THINK_DELTA_TIME){

				Think(type);
				type.m_monsterAIData.time = 0.0f ;
			}
			Action(type,time);
		}

		void Think(CMonster type){

			List<CCreature> petList = EnitityMgr.GetInstance().GetPetList();
			if(petList.Count > 0){
				CCreature targetPet = null;
				float dis = float.MaxValue ;
				float tempDis ;
				float tempDisX;
				float tempDisY;
				MonsterMoudleData monstermoudle = (MonsterMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID,
				                                                                                          common.CsvType.CSV_TYPE_MONSTER);
				//find the nestest target on eye shot
				for(int i = 0; i<petList.Count; ++i){
					//not come back
					if(petList[i].GetRenderObject().transform.position.x >= type.GetRenderObject().transform.position.x + AICommon.AI_MONSTER_COME_BACK_DISTANCE)
						continue ;
					//tempDis = Vector3.Distance(petList[i].GetRenderObject().transform.position,type.GetRenderObject().transform.position) ;
					tempDisX = petList[i].GetRenderObject().transform.position.x - type.GetRenderObject().transform.position.x ;
					tempDisY = petList[i].GetRenderObject().transform.position.y - type.GetRenderObject().transform.position.y ;
					tempDis  = Mathf.Abs(tempDisX) ;
					//tempDis = Mathf.Abs(tempDisY) ;
					//in eye shoot
					if(tempDis < type.m_data.eyeShotArea && tempDis < dis && Mathf.Abs(tempDisY) < AICommon.AI_ATTACK_Y_GAP ){
						if(petList[i].GetEnitityAiState() != AIState.AI_STATE_WEAK && petList[i].GetEnitityAiState() != AIState.AI_STATE_DEATH)
						{
							CPet pet = (CPet)petList[i] ;
							if(monstermoudle.profession != 14){
								if(pet.CanBeLock()){
									targetPet = petList[i] ;
									dis = tempDis ;
								}
							}
							else{
								targetPet = petList[i] ;
								dis = tempDis ;
							}
						}
					}
				}
				//find one
				if(targetPet!=null){
					//MonsterPursueStateData pursueData = new MonsterPursueStateData();
					//pursueData.targetObjectId = targetPet.GetId() ;
					CPet pet = (CPet)targetPet ;
					pet.AddAttack(type.id);
					EventMessageLockPet lockPetMessage = new EventMessageLockPet() ;
					lockPetMessage.lockPetID = targetPet.GetId();
					lockPetMessage.lockMonsterID = type.id ;

					EnitityMgr.GetInstance().OnMessage(lockPetMessage);
					type.m_targetCreature = targetPet;       //set the target
					type.m_stateMachine.ChangeState(MonsterPursueState.getInstance());
					return;
				}

				if(NewPlayerGuide.isGuide
				   &&NewPlayerGuide.curGuide==907
				   &&monstermoudle.strength>1
				   &&type.GetRenderObject().transform.position.x <110.0f
				   )
				{
					GuideStopMessage stop_play_message = new GuideStopMessage();
					EventMgr.GetInstance().OnEventMgr(stop_play_message);
					NewPlayerGuide.curGuide = 908;
					GuideCheck2Message msg = new GuideCheck2Message();
					msg.guideStep = NewPlayerGuide.curGuide;
					NewPlayerGuide.GetInstance().OnMessage(msg);
				}

				if(NewPlayerGuide.curGuide==100&&NewPlayerGuide.isGuide
				   &&GameLevelMgr.GetInstance().m_curWave==1
				   &&NewPlayerGuide.isGuide&&type.GetRenderObject().transform.position.x <110.0f)
				{
					//GuideStopMessage stop_play_message = new GuideStopMessage();
					//EventMgr.GetInstance().OnEventMgr(stop_play_message);
					
					GuideCheck2Message msg = new GuideCheck2Message();
					//GameLevelMgr.GetInstance().m_curWave==1;
					msg.guideStep = 101;
					NewPlayerGuide.GetInstance().OnMessage(msg);

					gameGlobal.g_fightSceneUI.CutCoolDownTime(1000.0f,3);
				}

				if(NewPlayerGuide.curGuide==103&&GameLevelMgr.GetInstance().m_curWave==2
				   &&NewPlayerGuide.isGuide&&type.GetRenderObject().transform.position.x <110.0f)
				{
					//GuideStopMessage stop_play_message = new GuideStopMessage();
					//EventMgr.GetInstance().OnEventMgr(stop_play_message);

					GuideCheck2Message msg = new GuideCheck2Message();


					msg.guideStep = 104;
					
					NewPlayerGuide.GetInstance().OnMessage(msg);
					gameGlobal.g_fightSceneUI.CutCoolDownTime(1000.0f,2);

				}
			}

			if(type.GetRenderObject().transform.position.x < 10){
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT).localRotation = Quaternion.LookRotation(Vector3.forward) ;
				//type.GetRenderObject().transform.localRotation = Quaternion.LookRotation(Vector3.forward);
				Vector3 pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition ;
				pos.z = -0.5f ;
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition = pos ;

				pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT).localPosition ;
				pos.z = -0.2f ;
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT).localPosition = pos ;
			}
			else{
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT).localRotation = Quaternion.LookRotation(Vector3.back) ;
				Vector3 pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition ;
				pos.z = 0.5f ;
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_SHADOW).localPosition = pos ;

				pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT).localPosition ;
				pos.z = 0.5f ;
				type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT).localPosition = pos ;
			}
		}

		void Action(CMonster type, float time){
			CCreature creature = null;
			creature = EnitityMgr.GetInstance().city;
			CCity city = EnitityMgr.GetInstance().city ;
			float distance = type.GetRenderObject().transform.position.x - (15 - (type.m_monsterAIData.wayIndex) * 2.0f) ;

			
			if(creature.GetEnitityAiState() == AIState.AI_STATE_DOOR_NARMOL){
				if( distance < type.attackArea){
					if(type.m_data.attackType == AttackType.ATTACK_TYPE_FAR){
						if(distance < type.attackArea - 7){
							type.m_targetCreature = creature;
							type.m_stateMachine.ChangeState(MonsterAttackCityState.getInstance());//attack the city gate
						}
						else{
							type.GetRenderObject().transform.position += Vector3.left * time * type.monsterSpeed ;
						}
					}
					else{
						type.m_targetCreature = creature;
						type.m_stateMachine.ChangeState(MonsterAttackCityState.getInstance());//attack the city gate
					}
					
				}
				else{
					type.GetRenderObject().transform.position += Vector3.left * time * type.monsterSpeed ;
				}
			}
			else if(creature.GetEnitityAiState() == AIState.AI_STATE_DOOR_BREAK){
				if( distance < 2.0f){
					type.m_stateMachine.ChangeState(MonsterEscapeState.getInstance());//catch the girl
				}
				else{
					type.GetRenderObject().transform.position += Vector3.left * time * type.monsterSpeed ;
				}
			}
		}

		public void Exit(CMonster type){
			//type.Play("stand",WrapMode.Loop);
		}
		public void OnMessage(CMonster type, EventMessageBase data){
		}
			
		public AIState  GetState(){
			return AIState.AI_STATE_MOVE ;
		}
		public static MonsterMoveState getInstance(){
			if(instance ==null){
				instance = new MonsterMoveState();
			}
			return instance;
		}
	}
}

