using UnityEngine;
using System.Collections;
using GameEvent ;

namespace GameLogical.GameEnitity.AI{
	/*****************************************************************************
	/// monster stand state ,control monster stand 
	*****************************************************************************/
	class CMonsterStandState : CStateBase<CMonster>{
		public void Release(){
			
		}
		public void Enter(CMonster type){
			type.Play("stand",WrapMode.Loop);
		}
		public void Execute(CMonster type, float time){
			
		}
		public void Exit(CMonster type){
			
		}
		public void OnMessage(CMonster type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_STAND ;
		}
	}
	
	/*****************************************************************************
	/// monster move state ,control monster move postion and animation
	*****************************************************************************/
	class CMonsterMoveState : CStateBase<CMonster>{
		public void Release(){
			
		}
		public void Enter(CMonster type){
			type.Play("walk",WrapMode.Loop);
		}
		public void Execute(CMonster type, float time){
			MonsterMoveStateData data = type.m_monsterAIData as MonsterMoveStateData;
#if DEBUG
			common.debug.GetInstance().AppCheckSlow(data);
#endif
			Vector3 destPos = Vector3.zero ;
			if(data.destObjectId != -1){
				CCreature creature = EnitityMgr.GetInstance().GetEnitity(data.destObjectId) ;
				if(creature != null){
					destPos = EnitityMgr.GetInstance().GetEnitityPos(type.id,data.destObjectId);
				}
				else{
					type.Think();
					return ;
				}
			}
			else{
				destPos = data.destPos ;
			}
			
			Vector3 disVec = destPos - type.GetRenderObject().transform.position ;
			if(disVec.x > 0){
				type.GetRenderObject().transform.localRotation = Quaternion.LookRotation(Vector3.back);
			}
			else{
				type.GetRenderObject().transform.localRotation = Quaternion.LookRotation(Vector3.forward);
			}
			
			if(disVec.magnitude < 0.1f){
				type.Think();
			}
			else{
				type.GetRenderObject().transform.position += disVec.normalized * time * type.monsterSpeed ;
			}
		}
		public void Exit(CMonster type){
			//type.Play("stand",WrapMode.Loop);
		}
		public void OnMessage(CMonster type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_MOVE ;
		}
	}
	
	/*****************************************************************************
	/// monster attack state ,control monster attack data and animation
	*****************************************************************************/
	class CMonsterAttackState : CStateBase<CMonster>{
		public void Release(){
			
		}
		public void Enter(CMonster type){
			type.Play("attack",WrapMode.Once);
		}
		public void Execute(CMonster type, float time){
			
		}
		public void Exit(CMonster type){
			
		}
		public void OnMessage(CMonster type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_ACTTACK ;
		}
	}
	
	/*****************************************************************************
	/// monster attack city state ,control monster attack data and animation
	*****************************************************************************/
	class CMonsterAttackCityState : CStateBase<CMonster>{
		public void Release(){
			
		}
		public void Enter(CMonster type){
			type.Play("attack",WrapMode.Once);
		}
		public void Execute(CMonster type, float time){
			
		}
		public void Exit(CMonster type){
			
		}
		public void OnMessage(CMonster type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_ACTTACK_CITY ;
		}
	}
	
	/*****************************************************************************
	/// monster weak state ,control monster weak data and animation
	*****************************************************************************/
	class CMonsterWeakState : CStateBase<CMonster>{
		public float lastTime = 1.6f ;
		public float deltaTime= 0.0f ;
		public void Release(){
			
		}
		public void Enter(CMonster type){
			//type.Play("death",WrapMode.Once);
			type.Stop();
		}
		
		public void Execute(CMonster type, float time){
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
		
		public void Exit(CMonster type){
			
		}
		public void OnMessage(CMonster type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_WEAK ;
		}
	}
	
	/*****************************************************************************
	/// monster dead state ,control monster dead data and animation
	*****************************************************************************/
	class CMonsterDeadState : CStateBase<CMonster>{
		public void Release(){
			
		}
		public void Enter(CMonster type){
			
		}
		public void Execute(CMonster type, float time){
			
		}
		public void Exit(CMonster type){
			
		}
		public void OnMessage(CMonster type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_DEATH ;
		}
	}
	
	/*****************************************************************************
	/// monster escape state ,control monster escape data and animation
	*****************************************************************************/
	class CMonsterEscapeState : CStateBase<CMonster>{
		public void Release(){
			
		}
		public void Enter(CMonster type){
			type.Play("walk",WrapMode.Loop);
			
			CCreature creature = EnitityMgr.GetInstance().CreateEnitity(EnitityType.ENITITY_TYPE_WOMEN,30001);
			CWomen women = creature as CWomen;
			common.debug.GetInstance().AppCheckSlow(women);

			CWomenBeCoughtState state = new CWomenBeCoughtState();
			WomenBeCoughtStateData stateData = new WomenBeCoughtStateData();
			stateData.monsterID = type.id ;
			women.m_womenAIData = stateData ;
			
			women.SetState(state);
		}
		
		public void Execute(CMonster type, float time){
			type.GetRenderObject().transform.localRotation = Quaternion.LookRotation(Vector3.forward);
			if(type.GetRenderObject().transform.position.x < 120.0f){
				type.GetRenderObject().transform.position += Vector3.right * time * type.monsterSpeed * 0.5f ;
			}
			else{
				type.Think();
			}
		}
		public void Exit(CMonster type){
			
		}
		public void OnMessage(CMonster type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_ESCAPE ;
		}
	}
}