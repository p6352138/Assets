using UnityEngine;
using System.Collections;
using GameEvent;

namespace GameLogical.GameEnitity.AI{

	
	/// <summary>
	/// pet stand state.
	/// </summary>
	public class CPetStandState : CStateBase<CPet>{
		public void Release(){
			
		}
		public void Enter(CPet type){
			type.Play("stand",WrapMode.Loop);
		}
		public void Execute(CPet type, float time){
			type.Think();
		}
		public void Exit(CPet type){
			
		}
		public void OnMessage(CPet type, EventMessageBase data){
	/*		switch((EnitityAction)data.eventMessageAction){
				// real attack
			case EnitityAction.ENITITY_ACTION_FIGHT:{
				EventMessageFight fightMessage = data as EventMessageFight ;
				common.debug.GetInstance().AppCheckSlow(fightMessage);
				CCreature scrCreature = EnitityMgr.GetInstance().GetEnitity(fightMessage.scrCreatureId);
				CCreature destCreature = EnitityMgr.GetInstance().GetEnitity(fightMessage.destCreatureId);
				if(scrCreature == null || destCreature == null){
					type.Think();
					return ;
				}
				
				// some one hurt me
				if(destCreature == type){
					type.BeAttack();
					EnitityType enitityType = destCreature.GetEnitityType();
					
					//
					switch(enitityType){
					case EnitityType.ENITITY_TYPE_MONSTER:{
						CMonster scrMonster = scrCreature as CMonster ;
						common.debug.GetInstance().AppCheckSlow(scrMonster);
						type.blood -= scrMonster.attack ;
						//death
						if(type.blood <= 0){
							CPetDeathState deathState = new CPetDeathState();
							type.SetState(deathState);
						}
						//fight back
						else{
							
						}
					}
						break;
					}
				}
			}
				break ;
			}*/
		}
		public AIState  GetState(){
			return AIState.AI_STATE_STAND ;
		}
	}
	
	/// <summary>
	/// pet move state.
	/// </summary>
	public class CPetMoveState : CStateBase<CPet>{
		protected			Vector3			m_moveVec			;
		protected			PetMoveAIData   m_data 				;
		
		/// <summary>
		/// Enter the state ,set data and init some object
		/// </summary>
		/// <param name='monster'>
		/// monster 
		/// </param>
		public void Enter(CPet pet){
			pet.Play("walk",WrapMode.Loop);
		}
		
		
		/// <summary>
		/// Release this instance.
		/// </summary>
		public void Release(){
			m_data = null ;
		}
		
		/// <summary>
		/// Execute the specified monster and time.
		/// </summary>
		/// <param name='monster'>
		/// Type.
		/// </param>
		/// <param name='monster'>
		/// Time.
		/// </param>
		public void Execute(CPet pet, float time){
			m_data = pet.m_petAIData as PetMoveAIData;
			CCreature destObject = EnitityMgr.GetInstance().GetEnitity(m_data.destObjectId);
			// move to pos
			if(destObject == null){
				pet.Think();
				return ;
			}

			m_moveVec = EnitityMgr.GetInstance().GetEnitityPos(pet.id,m_data.destObjectId) - pet.m_object.gameObject.transform.position;
			
			if(m_moveVec.sqrMagnitude < 0.1f){
				pet.Think();
			}
			else{
				if(m_moveVec.x > 0){
					pet.GetRenderObject().transform.localRotation = Quaternion.LookRotation(Vector3.forward);
				}else if(m_moveVec.x < 0){
					pet.GetRenderObject().transform.localRotation = Quaternion.LookRotation(Vector3.back);
				}
				pet.m_object.gameObject.transform.position += m_moveVec.normalized * pet.speed * time;
			}
		}
		
		/// <summary>
		/// Exit the specified monster.
		/// </summary>
		/// <param name='monster'>
		/// Type.
		/// </param>
		public void Exit(CPet pet){
			//pet.Think();
			//Release();
		}
		
		/// <summary>
		/// Raises the message event.
		/// </summary>
		/// <param name='monster'>
		/// Type.
		/// </param>
		/// <param name='data'>
		/// Data.
		/// </param>
		public void OnMessage(CPet type, EventMessageBase data){
/*			switch((EnitityAction)data.eventMessageAction){
				// real attack
			case EnitityAction.ENITITY_ACTION_FIGHT:{
				EventMessageFight fightMessage = data as EventMessageFight ;
				common.debug.GetInstance().AppCheckSlow(fightMessage);
				CCreature scrCreature = EnitityMgr.GetInstance().GetEnitity(fightMessage.scrCreatureId);
				CCreature destCreature = EnitityMgr.GetInstance().GetEnitity(fightMessage.destCreatureId);
				if(scrCreature == null || destCreature == null){
					type.Think();
					return ;
				}
				
				// some one hurt me
				if(destCreature == type){
					type.BeAttack();
					EnitityType enitityType = destCreature.GetEnitityType();
					
					//
					switch(enitityType){
					case EnitityType.ENITITY_TYPE_MONSTER:{
						CMonster scrMonster = scrCreature as CMonster ;
						common.debug.GetInstance().AppCheckSlow(scrMonster);
						type.blood -= scrMonster.attack ;
						//death
						if(type.blood <= 0){
							CPetDeathState deathState = new CPetDeathState();
							type.SetState(deathState);
						}
						//fight back
						else{
							
						}
					}
						break;
					}
				}
			}
				break ;
			}*/
		}
		
		/// <summary>
		/// Gets the state.
		/// </summary>
		/// <returns>
		/// The state.
		/// </returns>
		public AIState GetState(){
			return AIState.AI_STATE_MOVE ;
		}
		
		/// <summary>
		/// Play the animation by name and set play mode.
		/// </summary>
		/// <param name='name'>
		/// Name.
		/// </param>
		/// <param name='mode'>
		/// Mode.
		/// </param>
		public void Play(string name,WrapMode mode){
			
		}
	}
	
	/// <summary>
	/// pet patrol state
	/// </summary>
	public class CPetPatrolState : CStateBase<CPet>{
		public void Release(){
			
		}
		public void Enter(CPet type){
			type.Play("walk",WrapMode.Loop);
		}
		public void Execute(CPet type, float time){
			PetPatrolAIData data = type.m_petAIData as PetPatrolAIData;
			Vector3 destPos = data.patrolPathList[data.destPathIndex];
			Vector3 moveVec = destPos - type.m_object.gameObject.transform.position;
			
			if(moveVec.x > 0){
				type.GetRenderObject().transform.localRotation = Quaternion.LookRotation(Vector3.forward);
			}else if(moveVec.x < 0){
				type.GetRenderObject().transform.localRotation = Quaternion.LookRotation(Vector3.back);
			}
			type.m_object.gameObject.transform.position += moveVec.normalized * type.speed * time;
			type.Think();
		}
		public void Exit(CPet type){
			
		}
		
		public void OnMessage(CPet type, EventMessageBase data){
/*			switch((EnitityAction)data.eventMessageAction){
				// real attack
			case EnitityAction.ENITITY_ACTION_FIGHT:{
				EventMessageFight fightMessage = data as EventMessageFight ;
				common.debug.GetInstance().AppCheckSlow(fightMessage);
				CCreature scrCreature = EnitityMgr.GetInstance().GetEnitity(fightMessage.scrCreatureId);
				CCreature destCreature = EnitityMgr.GetInstance().GetEnitity(fightMessage.destCreatureId);
				if(scrCreature == null || destCreature == null){
					type.Think();
					return ;
				}
				
				// some one hurt me
				if(destCreature == type){
					type.BeAttack();
					EnitityType enitityType = destCreature.GetEnitityType();
					
					//
					switch(enitityType){
					case EnitityType.ENITITY_TYPE_MONSTER:{
						CMonster scrMonster = scrCreature as CMonster ;
						common.debug.GetInstance().AppCheckSlow(scrMonster);
						type.blood -= scrMonster.attack ;
						//death
						if(type.blood <= 0){
							CPetDeathState deathState = new CPetDeathState();
							type.SetState(deathState);
						}
						//fight back
						else{
							
						}
					}
						break;
					}
				}
			}
				break ;
			}*/
		}
		public AIState GetState(){
			return AIState.AI_STATE_PATRAL ;
		}
	}
	
	/// <summary>
	/// monster attack state.
	/// </summary>
	public class CPetAttackState : CStateBase<CPet>{
		public void Release(){
			
		}
		public void Enter(CPet type){
			type.Play("attack",WrapMode.Once);
		}
		public void Execute(CPet type, float time){
			
		}
		public void Exit(CPet type){
			
		}
		public void OnMessage(CPet type, EventMessageBase data){
/*			switch((EnitityAction)data.eventMessageAction){
				//start attack target
			case EnitityAction.ENITITY_ACTION_FIGHT_SATRT:{
				PetAttackStateData attackData = type.m_petAIData as PetAttackStateData;
				common.debug.GetInstance().AppCheckSlow(attackData);
				CCreature targetObject = EnitityMgr.GetInstance().GetEnitity(attackData.targetObjectId);
				if(targetObject != null){
					EventMessageFight fightMessage = new EventMessageFight();
					fightMessage.scrCreatureId = type.GetId() ;
					fightMessage.destCreatureId= targetObject.GetId() ;
					EventMgr.GetInstance().OnEventMgr(fightMessage);
				}
			}
				break ;
				
				//finish one time attack target
			case EnitityAction.ENITITY_ACTION_FIGHT_FINISH:{
				type.Think();
			}
				break;
				
				// real attack
			case EnitityAction.ENITITY_ACTION_FIGHT:{
				EventMessageFight fightMessage = data as EventMessageFight ;
				common.debug.GetInstance().AppCheckSlow(fightMessage);
				CCreature scrCreature = EnitityMgr.GetInstance().GetEnitity(fightMessage.scrCreatureId);
				CCreature destCreature = EnitityMgr.GetInstance().GetEnitity(fightMessage.destCreatureId);
				if(scrCreature == null || destCreature == null){
					type.Think();
					return ;
				}
				
				// some one hurt me
				if(destCreature == type){
					type.BeAttack();
					EnitityType enitityType = scrCreature.GetEnitityType();
					
					//
					switch(enitityType){
					case EnitityType.ENITITY_TYPE_MONSTER:{
						CMonster scrMonster = scrCreature as CMonster ;
						common.debug.GetInstance().AppCheckSlow(scrMonster);
						type.blood -= scrMonster.attack ;
						//death
						if(type.blood <= 0){
							CPetDeathState deathState = new CPetDeathState();
							type.SetState(deathState);
						}
						//fight back
						else{
							
						}
					}
						break;
					}
				}
				// kick some one ass
				else if(scrCreature == type){
					//type.Think();
				}
				
				//MonoBehaviour.print(fightMessage.scrCreature.GetRenderObject().name + " hit " + fightMessage.destCreature.GetRenderObject().name);
			}
				break ;
			}*/
		}
		public AIState GetState(){
			return AIState.AI_STATE_ACTTACK ;
		}
	}
	
	/// <summary>
	/// monster death state.
	/// </summary>
	public class CPetDeathState : CStateBase<CPet>{
		public float lastTime = 1.6f ;
		public float deltaTime= 0.0f ;
		public void Release(){
			
		}
		public void Enter(CPet type){
			type.Stop();
			//type.Play("death",WrapMode.Once);
		}
		
		public void Execute(CPet type, float time){
			deltaTime+=time ;
			if(deltaTime >= 0.4f){
				lastTime -= deltaTime ;
				if(lastTime > 0.0f){
					type.Shark();
				}
				else{
					EventMessageDeathEnd message = new EventMessageDeathEnd();
					message.ob = type.GetRenderObject() ;
					EnitityMgr.GetInstance().OnMessage(message);
				}
			}
		}
		public void Exit(CPet type){
			
		}
		
		public void OnMessage(CPet type, EventMessageBase data){
			/*if(data.eventMessageAction == (int)EnitityAction.ENITYTY_ACTION_DEATH){
				Release();
				EnitityMgr.GetInstance().DestroyEnitity(type) ;
			}*/
		}
		
		public AIState GetState(){
			return AIState.AI_STATE_DEATH ;
		}
	}
	
}
