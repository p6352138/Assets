using UnityEngine;
using System.Collections;
using GameLogical.GameEnitity.AI ;
using GameEvent ;
using GameLogical.GameEnitity ;

namespace GameLogical.GameLevel{
	public class LevelPvPEndState : CStateBase<Object>
	{
		protected static LevelPvPEndState instance;
			
		public void Release(){
			
		}
		public void Enter(Object type){
			GameDataCenter.GetInstance().m_resultData.m_type = ResultType.RESULT_TYPE_PVP ;
			gameGlobal.ResultShow();
//			gameGlobal.g_LevelResultUI.Show(ResultType.RESULT_TYPE_PVP);
			for(int i = 0; i< EnitityMgr.GetInstance().GetPetList().Count; ++i){
				CPet pet = EnitityMgr.GetInstance().GetPetList()[i] as CPet ;
				pet.m_stateMachine.ChangeState(PetStandState.getInstance());
			}

			for(int i = 0; i< EnitityMgr.GetInstance().GetMonsterList().Count; ++i){
				CEnemyPet monster = EnitityMgr.GetInstance().GetMonsterList()[i] as CEnemyPet ;
				monster.m_stateMachine.ChangeState(EnemyPetStandState.getInstance());
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
		
		public static LevelPvPEndState getInstance(){
			if(instance == null) instance = new LevelPvPEndState();
			return instance;
		}		
	}
}


