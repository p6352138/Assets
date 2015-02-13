using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using AppUtility;
using GameEvent ;
using GameLogical.GameEnitity;
using common ;
using GameLogical.GameEnitity.AI ;

namespace GameLogical.GameLevel
{
	public class LevelPauseState: CStateBase<Object>{
		protected static LevelPauseState instance;
		
		public void Release(){
			
		}
		public void Enter(Object type){
			Time.timeScale = 0;
			//GameLevelMgr.GetInstance().m_isStop = true ;
		}
		public void Execute(Object type, float time){
			
		}
		public void Exit(Object type){
			
		}
		public void OnMessage(Object type, EventMessageBase message){
			if(message.eventMessageModel == EventMessageModel.eEventMessageModel_PLAY_STATE){
				GameLevel.GameLevelMgr.GetInstance().m_levelStateMachin.ChangeState(LevelPlayingState.getInstance());
			}
		}
		public AIState  GetState(){
			return AIState.AI_STATE_STONE ;
		}
		
		public static LevelPauseState getInstance(){
			if(instance == null) instance = new LevelPauseState();
			return instance;
		}
	}
}

