using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameEvent;

namespace GameLogical.GameEnitity.AI
{
	public class PetSleepState : CStateBase<CPet>
	{
		protected static PetSleepState instance;
		public void Release(){
			
		}
		public void Enter(CPet type){
			type.m_petAIData.time = 0.0f ;		
		}
		public void Execute(CPet type, float time){
			type.m_petAIData.time += time ;
			if(type.m_petAIData.time >= 1.0f){
				if(type.blood != 0){
					int hp = type.m_data.appHpSecond ;
					if(type.m_effectData.changeAddHpPercent != 0){
						hp = (int)(hp * type.m_effectData.changeAddHpPercent * 0.01f) ;
					}
					type.GetFightCreatureData().blood += (int)(hp * 0.001 * type.GetFightCreatureData().maxBlood);
				}
				type.m_petAIData.time = 0.0f ;
			}
			
		}
		
		public void Think(CPet type){
			
		}
		
		public void Action(CPet type, float time){

		}
		
		public void Exit(CPet type){

		}
		public void OnMessage(CPet type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_CHANGE_SLEEP ;
		}
		
		public static PetSleepState getInstance(){
			if(instance==null){instance = new PetSleepState();}
			return instance;
		}
	}
}


