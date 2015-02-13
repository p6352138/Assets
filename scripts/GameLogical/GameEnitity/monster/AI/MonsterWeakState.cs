using UnityEngine;
using System.Collections;
using GameEvent ;


namespace GameLogical.GameEnitity.AI
{
	public class MonsterWeakState: CStateBase<CMonster>{
		protected static MonsterWeakState instance;
		public void Release(){
			
		}
		public void Enter(CMonster type){
			//type.Play("death",WrapMode.Once);
			type.Stop();
			EventMessageWeak message = new EventMessageWeak();
			message.id = type.id ;
			EnitityMgr.GetInstance().OnMessage(message);
			
			//talk
			/*MonsterMoudleData monstermoudle = (MonsterMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID,
				common.CsvType.CSV_TYPE_MONSTER);
			type.TalkLv(monstermoudle.talkLvInDie, monstermoudle.talkIDInDie);*/
		}
		
		public void Execute(CMonster type, float time){
			type.m_sharkCurTime += time ;
			if(type.m_sharkCurTime >= 0.083f){
				type.m_sharkTotalTime -= type.m_sharkCurTime ;
				if(type.m_sharkTotalTime > 0.0f){
					type.Shark();
					type.m_sharkCurTime = 0.0f ;
				}
				else{
					//relive
					int rate = Random.Range(0,100);
					if(rate < type.effectData.reLiveRate){
						type.SetHp(type.effectData.relive);
					}
					else{
						type.m_stateMachine.ChangeState(MonsterDeadState.getInstance());
						EventMessageDeathEnd message = new EventMessageDeathEnd();
						message.ob = type.GetRenderObject() ;
						EnitityMgr.GetInstance().OnMessage(message);
					}
				}
			}
			
		}
		
		public void Exit(CMonster type){
			
		}
		public void OnMessage(CMonster type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_WEAK ;
		}
		public static MonsterWeakState getInstance(){
			if(instance ==null){
				instance = new MonsterWeakState();
			}
			
			return instance;
		}
	}
}

