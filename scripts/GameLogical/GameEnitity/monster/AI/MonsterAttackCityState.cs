using UnityEngine;
using System.Collections;
using GameEvent ;
using System.Collections.Generic;


namespace GameLogical.GameEnitity.AI
{
	public class MonsterAttackCityState: CStateBase<CMonster>{
		protected static MonsterAttackCityState instance;
		public void Release(){
			
		}
		public void Enter(CMonster type){

			List<CCreature> monsterList = EnitityMgr.GetInstance().GetMonsterList();
			float tempDis ;
			for(int i = 0; i<monsterList.Count; ++i){
				//not come back
				tempDis = Vector3.Distance(monsterList[i].GetRenderObject().transform.position,type.GetRenderObject().transform.position) ;
				if(tempDis<type.m_data.misPlace && monsterList[i].GetEnitityAiState() == AIState.AI_STATE_ACTTACK_CITY &&
				   monsterList[i].GetId() != type.GetId()){
					type.m_stateMachine.ChangeState(MonsterMisplaceState.getInstance());
					return;
				}
			}

			type.getMonsterData().curAttackCD = -1.0f ;
			type.Play("attack",WrapMode.Once);
			
			//talk
			/*MonsterMoudleData monstermoudle = (MonsterMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID,
				common.CsvType.CSV_TYPE_MONSTER);
			type.TalkLv(monstermoudle.talkLvInRobDoor, monstermoudle.talkIDInRobDoor);*/
		}

		public void Execute(CMonster type, float time){
			CCreature creature = EnitityMgr.GetInstance().city;
			if(creature.GetEnitityAiState() == AIState.AI_STATE_DOOR_NARMOL){
				if(type.getMonsterData().curAttackCD != -1.0f){
					type.getMonsterData().curAttackCD += time ;
					if(type.getMonsterData().curAttackCD >= type.getMonsterData().attackCD){
						type.getMonsterData().curAttackCD = -1.0f ;
						type.m_stateMachine.ChangeState(MonsterAttackCityState.getInstance());//attack the city gate
						
					}
				}
			}
			else if(creature.GetEnitityAiState() == AIState.AI_STATE_DOOR_BREAK){
				MonsterMoudleData moudleData = common.fileMgr.GetInstance().GetData(type.m_data.moudleID,common.CsvType.CSV_TYPE_MONSTER) as MonsterMoudleData ;
				/*if(moudleData.profession == 15){
					type.m_stateMachine.ChangeState(MonsterNoLookMove.getInstance());//catch the girl
				}
				else{
					type.m_stateMachine.ChangeState(MonsterMoveState.getInstance());//catch the girl
				}*/
				type.m_stateMachine.ChangeState(MonsterNoLookMove.getInstance());

			}
		}

		public void Exit(CMonster type){
			
		}
		public void OnMessage(CMonster type, EventMessageBase data){
			if((EnitityAction)data.eventMessageAction == EnitityAction.ENITITY_ACTION_FIGHT_FINISH){
				type.getMonsterData().curAttackCD = 0.0f ;
				type.Play("stand",WrapMode.Loop);
			}
			else if((EnitityAction)data.eventMessageAction == EnitityAction.ENITITY_ACTION_FIGHT){

			}
			else if((EnitityAction)data.eventMessageAction == EnitityAction.ENITITY_ACTION_FIGHT_SATRT){
				EventMessageFightStart fightStartMessage = (EventMessageFightStart)data ;
				if(type.m_targetCreature==null || type.m_targetCreature.GetRenderObject()==null){
					return;
				}
				
				//near
				if(type.m_data.attackType == AttackType.ATTACK_TYPE_NEAR){
					EventMessageFight message = new EventMessageFight();
					message.scrCreatureId = type.id ;
					message.destCreatureId= type.m_targetCreature.GetId() ;
					message.audioName = fightStartMessage.audioName ;
					message.beEffectName = fightStartMessage.beEffectName ;
					EventMgr.GetInstance().OnEventMgr(message);
				}
				//far
				else if(type.m_data.attackType == AttackType.ATTACK_TYPE_FAR){
					MonsterMoudleData monsterMoudleData = common.fileMgr.GetInstance().GetData(type.m_data.moudleID,common.CsvType.CSV_TYPE_MONSTER) as MonsterMoudleData ;
					
					BulletData bulletData = new BulletData();
					bulletData.scrID = type.GetId();
					bulletData.destID= type.m_targetCreature.GetId() ;
					if(monsterMoudleData.AttackEffectID != -1){
						bulletData.effectID = monsterMoudleData.AttackEffectID * 10 + 2;
						bulletData.effectEndID = monsterMoudleData.AttackEffectID * 10 + 3 ;
					}
					else{
						bulletData.effectID = 400032;
						bulletData.effectEndID = 400033 ;
						common.debug.GetInstance().Error("attack effect id error pet id:" + monsterMoudleData.ID);
					}
					bulletData.audioPath = fightStartMessage.audioName ; 

					bulletData.pos = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ATTACK).position ;
					CCreature creatureBullet = EnitityMgr.GetInstance().CreateEnitity(EnitityType.ENITITY_TYPE_BULLET,bulletData);
					//creatureBullet.GetRenderObject().transform.position = type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_BODY).position ;
					//creatureBullet.GetRenderObject().transform.FindChild("creature").animation.Play("effect");
				}
			}
		}
		public AIState  GetState(){
			return AIState.AI_STATE_ACTTACK_CITY ;
		}
		public static MonsterAttackCityState getInstance(){
			if(instance ==null){
				instance = new MonsterAttackCityState();
			}
			
			return instance;
		}
	
	}
}

