using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using GameEvent ;
using GameMessgeHandle;
using GameLogical.GameLevel;

namespace GameLogical.GameEnitity.AI{
	public class MonsterNoLookMove : CStateBase<CMonster>
	{
		protected static MonsterNoLookMove instance;
		public void Release(){
			
		}
		public void Enter(CMonster type){
			
			
			//talk
			/*MonsterMoudleData monstermoudle = (MonsterMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID,
				common.CsvType.CSV_TYPE_MONSTER);
			type.TalkLv(monstermoudle.talkLvInMove, monstermoudle.talkIDInMove);*/
			
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
		public static MonsterNoLookMove getInstance(){
			if(instance ==null){
				instance = new MonsterNoLookMove();
			}
			return instance;
		}
	}
}


