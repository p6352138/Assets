using UnityEngine;
using System.Collections;
using GameEvent ;
using GameLogical.GameSkill;
using GameLogical.GameSkill.Skill ;
using System.Collections.Generic ;
using common;


namespace GameLogical.GameEnitity.AI
{
	public class MonstYMoveState: CStateBase<CMonster>{
		protected static MonstYMoveState instance;
		private float destY = 25;

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
			float tempDisY = type.m_targetCreature.GetRenderObject().transform.position.y - type.GetRenderObject().transform.position.y ;

			List<CCreature> monsterList = EnitityMgr.GetInstance().GetMonsterList();
			Vector3[] yPos = GameLevel.GameLevelMgr.GetInstance().m_monsterBrithPointArr	;
			for(int j = 0; j < yPos.Length; j++)
			{
				if(Mathf.Abs(type.m_targetCreature.GetRenderObject().transform.position.y - yPos[j].y) <= 5)
				{
					destY = (int)yPos[j].y;
				}
			}
			if(Mathf.Abs((type.GetRenderObject().transform.position.y) - destY) < AICommon.AI_MONSTER_MISPLACE){
				type.m_stateMachine.ChangeState(MonsterAttackState.getInstance());
				return;
			}
			for(int i = 0; i<monsterList.Count; ++i){
				CMonster sameWayMonster = monsterList[i] as CMonster ;
				if(sameWayMonster.m_targetCreature != type.m_targetCreature)
					continue ;
				float disX = monsterList[i].GetRenderObject().transform.position.x - type.GetRenderObject().transform.position.x;
				if(Mathf.Abs(disX) < AICommon.AI_MONSTER_MISPLACE && Mathf.Abs(type.m_targetCreature.GetRenderObject().transform.position.y - destY) < AICommon.AI_MONSTER_MISPLACE)
				{
					if(type.GetRenderObject().transform.position.y < destY)
					{
						destY -= AICommon.AI_MONSTER_Y_WAY;
					}
					else
					{
						destY += AICommon.AI_MONSTER_Y_WAY;
					}
					break;
				}
			}
			if(Mathf.Abs((type.GetRenderObject().transform.position.y) - destY) < AICommon.AI_MONSTER_MISPLACE){
				type.m_stateMachine.ChangeState(MonsterAttackState.getInstance());
				return;
			}


			if(type.GetRenderObject().transform.position.y > destY)
			{
				type.GetRenderObject().transform.position += (new Vector3(0, -1, 0)) * time * type.monsterSpeed ;
			}
			else
			{
				type.GetRenderObject().transform.position += (new Vector3(0, 1, 0)) * time * type.monsterSpeed ;
			}
			Vector3 pos = type.GetRenderObject().transform.position;
			type.GetRenderObject().transform.position = new Vector3(pos.x, pos.y, pos.y);
		}
		
		public void Exit(CMonster type){
			
		}
		public void OnMessage(CMonster type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_MOVE_Y ;
		}
		
		public static MonstYMoveState getInstance(){
			if(instance==null){instance = new MonstYMoveState();}
			return instance;
		}
	}
}

