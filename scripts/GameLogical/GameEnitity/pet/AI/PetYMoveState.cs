using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameEvent;


namespace GameLogical.GameEnitity.AI
{
	public class PetYMoveState: CStateBase<CPet>{
		protected static PetYMoveState instance;
		public void Release(){
			
		}
		public void Enter(CPet type){
			type.m_petAIData.time = 0.0f ;
			type.Play("walk",WrapMode.Loop);
			Think(type);
		}
		public void Execute(CPet type, float time){
			type.m_petAIData.time += time ;
			if(type.m_petAIData.time >= AICommon.AI_THINK_DELTA_TIME){
				Think(type);
				type.m_petAIData.time = 0.0f ;
			}
			Action(type,time);
		}
		
		public void Think(CPet type){

		}
		
		public void Action(CPet type, float time){
			if(type.m_targetCreature.GetRenderObject().transform.position.y < type.GetRenderObject().transform.position.y)
				type.GetRenderObject().transform.position += (new Vector3(0, -1, 0)) * time * type.speed ;
			else
				type.GetRenderObject().transform.position += (new Vector3(0, 1, 0)) * time * type.speed ;
			float tempDisY = type.m_targetCreature.GetRenderObject().transform.position.y - type.GetRenderObject().transform.position.y ;

			if(Mathf.Abs(tempDisY) < 0.5f){
				type.m_stateMachine.ChangeState(PetAttackState.getInstance());
			}
			Vector3 pos = type.GetRenderObject().transform.position;
			type.GetRenderObject().transform.position = new Vector3(pos.x, pos.y, pos.y);
		}
		
		public void Exit(CPet type){
			
		}
		public void OnMessage(CPet type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_STAND ;
		}
		
		public static PetYMoveState getInstance(){
			if(instance==null){instance = new PetYMoveState();}
			return instance;
		}
	}
}

