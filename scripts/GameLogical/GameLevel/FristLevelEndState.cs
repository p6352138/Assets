using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using AppUtility;
using GameEvent ;
using GameLogical.GameEnitity;
using common ;
using GameLogical.GameEnitity.AI ;

namespace GameLogical.GameLevel{
	public class FristLevelEndState: CStateBase<Object>{
		protected static FristLevelEndState instance;

		float deltaTime ;
		public void Release(){
			
		}
		public void Enter(Object type){
			List<int> talkId = new List<int>();
			talkId.Add(5008);
			//talkId.Add(5005);
			gameGlobal.g_fightSceneUI.LoadTalk(talkId);
		}

		public void Execute(Object type, float time){
			deltaTime += time ;
			if(deltaTime > 1.0f){
				if(gameGlobal.g_fightSceneUI.m_isStop == false){
					gameGlobal.GameMainShow();
					deltaTime = -1000.0f ;
					GameDataCenter.GetInstance().isFristLevel = false ;
				}
			}
		}
		public void Exit(Object type){
			
		}
		public void OnMessage(Object type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return 0;
		}
		
		public static FristLevelEndState getInstance(){
			if(instance == null) instance = new FristLevelEndState();
			return instance;
		}
	}
}

