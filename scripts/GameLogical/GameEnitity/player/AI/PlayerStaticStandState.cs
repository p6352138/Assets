using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameEvent;

namespace GameLogical.GameEnitity.AI
{
	public class PlayerStaticStandState: CStateBase<CPlayer>{
		protected static PlayerStaticStandState instance;
		public void Release(){
			
		}
		public void Enter(CPlayer type){
			type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT).localRotation = Quaternion.LookRotation(Vector3.forward);
			//type.GetRenderObject().transform.localRotation = Quaternion.LookRotation(Vector3.forward);
			
			//type.talkBlink.transform.localRotation = Quaternion.LookRotation(Vector3.forward);
			type.Play("stand",WrapMode.Loop);
		}
		public void Execute(CPlayer type, float time){

		}
		public void Exit(CPlayer type){
			
		}
		public void OnMessage(CPlayer type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_STATIC_STAND ;
		}
		
		public static PlayerStaticStandState getInstance(){
			if(instance==null){instance = new PlayerStaticStandState();}
			return instance;
		}
	}
}

