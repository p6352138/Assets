using UnityEngine;
using System.Collections;
using GameEvent ;

namespace GameLogical.GameEnitity.AI{
	public class MonsterTalkMoveState : CStateBase<CMonster>
	{
		protected static MonsterTalkMoveState instance;
		public void Release(){
			
		}
		
		public void Enter(CMonster type){
			type.m_monsterAIData.time = 0.0f ;
			type.Play("walk",WrapMode.Loop);
			type.m_targetCreature = null;
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
			if(type.GetRenderObject().transform.position.x < 100){
				MonsterMoudleData monsterData = common.fileMgr.GetInstance().GetData(type.m_data.moudleID,common.CsvType.CSV_TYPE_MONSTER) as MonsterMoudleData ;
				gameGlobal.g_fightSceneUI.LoadTalk(monsterData.talkList);
				if(monsterData.profession == 15){
					type.SetState(MonsterNoLookMove.getInstance());
				}
				else{
					type.SetState(MonsterMoveState.getInstance());
				}

			}
		}

		void Action(CMonster type,float time){
			type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT).localRotation = Quaternion.LookRotation(Vector3.back) ;
			type.GetRenderObject().transform.position += Vector3.left * time * type.monsterSpeed ;
		}
		
		public void Exit(CMonster type){
			//type.m_stateMachine
			//type.m_stateMachine.ChangeState(type.m_stateMachine.GetPreviosState());
		}
		
		public void OnMessage(CMonster type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_MOVE ;
		}
		
		public static MonsterTalkMoveState getInstance(){
			if(instance==null){instance = new MonsterTalkMoveState();}
			return instance;
		}
	}
}


