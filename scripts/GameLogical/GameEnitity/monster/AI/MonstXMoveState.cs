using UnityEngine;
using System.Collections;
using GameEvent ;
using GameLogical.GameSkill;
using GameLogical.GameSkill.Skill ;
using System.Collections.Generic ;
using common;


namespace GameLogical.GameEnitity.AI
{
	public class MonstXMoveState: CStateBase<CMonster>{
		protected static MonstXMoveState instance;
		
		public void Release(){
			
		}
		public void Enter(CMonster type){
			type.m_monsterAIData.time = 0.0f ;
			type.Play("walk",WrapMode.Loop);
			Think(type);
		}
		public void Execute(CMonster type, float time){
			type.m_monsterAIData.time += time ;
			if(type.m_monsterAIData.time >= AICommon.AI_THINK_DELTA_TIME){
				Think(type);
				type.m_monsterAIData.time = 0.0f ;
			}
			Action(type,time);
		}
		
		public void Think(CMonster type){
			
		}
		
		public void Action(CMonster type, float time){
			type.Play("walk",WrapMode.Loop);

			/*bool canChange =false;
			List<CCreature> monsterList = EnitityMgr.GetInstance().GetMonsterList();
			for(int i = 0; i<monsterList.Count; ++i){
				CMonster sameWayMonster = monsterList[i] as CMonster ;
				if(sameWayMonster.m_targetCreature != type.m_targetCreature || sameWayMonster.id == type.id )
					continue ;
				float distX = monsterList[i].GetRenderObject().transform.position.x - type.GetRenderObject().transform.position.x;
				float disY = monsterList[i].GetRenderObject().transform.position.y - type.GetRenderObject().transform.position.y;
				if(Mathf.Abs(distX) < AICommon.AI_MONSTER_X_MISPLACE && Mathf.Abs(disY) < AICommon.AI_MONSTER_MISPLACE
				   && sameWayMonster.GetEnitityAiState() == AIState.AI_STATE_ACTTACK)
				{
					canChange = true;
				}
			}*/

			float disX = type.m_targetCreature.GetRenderObject().transform.position.x - type.GetRenderObject().transform.position.x;
			if(type.m_data.attackType == AttackType.ATTACK_TYPE_NEAR )
			{
				if(disX > type.attackArea)
				{
					type.m_stateMachine.ChangeState(MonsterAttackState.getInstance());
					return;
				}
			}
			else
			{
				if(Mathf.Abs(disX) < type.attackArea - AICommon.AI_MONSTER_X_ATTACK_NEAR )
				{
					type.m_stateMachine.ChangeState(MonsterAttackState.getInstance());
					return;
				}
			}
			
			type.GetRenderObject().transform.position += Vector3.left * time * type.monsterSpeed ;
			
			Vector3 pos = type.GetRenderObject().transform.position;
			type.GetRenderObject().transform.position = new Vector3(pos.x, pos.y, pos.y);
		}
		
		public void Exit(CMonster type){
			
		}
		public void OnMessage(CMonster type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_MOVE_X ;
		}
		
		public static MonstXMoveState getInstance(){
			if(instance==null){instance = new MonstXMoveState();}
			return instance;
		}
	}
}

