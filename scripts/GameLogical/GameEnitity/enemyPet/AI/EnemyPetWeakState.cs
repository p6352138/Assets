using UnityEngine;
using System.Collections;
using GameEvent ;

namespace GameLogical.GameEnitity.AI{
	public class EnemyPetWeakState : CStateBase<CEnemyPet>
	{
		protected static EnemyPetWeakState instance;
		public void Release(){
			
		}
		public void Enter(CEnemyPet type){
			//type.Play("death",WrapMode.Once);
			type.Stop();
			EventMessageWeak message = new EventMessageWeak();
			message.id = type.id ;
			EnitityMgr.GetInstance().OnMessage(message);
			
		}
		
		public void Execute(CEnemyPet type, float time){
			type.m_sharkCurTime += time ;
			if(type.m_sharkCurTime >= 0.083f){
				type.m_sharkTotalTime -= type.m_sharkCurTime ;
				if(type.m_sharkTotalTime > 0.0f){
					type.Shark();
					type.m_sharkCurTime = 0.0f ;
				}
				else{
					type.m_stateMachine.ChangeState(EnemyPetDeathState.getInstance());
					EventMessageDeathEnd message = new EventMessageDeathEnd();
					message.ob = type.GetRenderObject() ;
					EnitityMgr.GetInstance().OnMessage(message);

					//EnitityMgr.GetInstance().Energy += 60 ;
					//GameDataCenter.GetInstance().pvpPlayerInfo.engry += 80 ;
				}
			}
			
		}
		
		public void Exit(CEnemyPet type){
			
		}
		public void OnMessage(CEnemyPet type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_WEAK ;
		}
		public static EnemyPetWeakState getInstance(){
			if(instance ==null){
				instance = new EnemyPetWeakState();
			}
			
			return instance;
		}
	}
}


