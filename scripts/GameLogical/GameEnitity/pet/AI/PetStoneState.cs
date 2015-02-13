using UnityEngine;
using System.Collections;
using GameEvent ;
using GameLogical ;

namespace GameLogical.GameEnitity.AI{
	public class PetStoneState : CStateBase<CPet>
	{
		protected static PetStoneState instance;
		public void Release(){
			
		}
		
		public void Enter(CPet type){
			if(type.GetFightCreatureData().isMainRole == true){
				type.Play("stand",WrapMode.Loop);
				type.GetFightCreatureData().isMainRole = false ;
				EnitityMgr.GetInstance().m_staticScene-- ;
				Time.timeScale = 1.0f ;
				common.common.blackScreen(false);
			}
			type.Stop();
		}
		
		public void Execute(CPet type, float time){
			
		}
		
		public void Exit(CPet type){
			//type.m_stateMachine
			//type.m_stateMachine.ChangeState(type.m_stateMachine.GetPreviosState());
		}
		
		public void OnMessage(CPet type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_STONE ;
		}
		
		public static PetStoneState getInstance(){
			if(instance==null){instance = new PetStoneState();}
			return instance;
		}
	}

}

