using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using GameEvent ;


namespace GameLogical.GameEnitity.AI
{
	/**
	 * move to the city gate state
	 * **/
	public class MonsterMisplaceState: CStateBase<CMonster>{
		protected static MonsterMisplaceState instance;
		public void Release(){
			
		}
		public void Enter(CMonster type){
			type.Play("walk",WrapMode.Loop);
		}

		public void Execute(CMonster type, float time){
			List<CCreature> monsterList = EnitityMgr.GetInstance().GetMonsterList();
			if(monsterList.Count > 0){
				float tempDis ;

				bool displace =  false;
				//find the nestest target on eye shot
				for(int i = 0; i<monsterList.Count; ++i){
					//not come back
					tempDis = Vector3.Distance(monsterList[i].GetRenderObject().transform.position,type.GetRenderObject().transform.position) ;
					if(tempDis<type.m_data.misPlace && (monsterList[i].GetEnitityAiState() == AIState.AI_STATE_ACTTACK)){
		//				displace = true;
		//				type.GetRenderObject().transform.position += (new Vector3(0, 1, 0)) * time * type.monsterSpeed ;
		//				Vector3 typePos = type.GetRenderObject().transform.position ;
		//				typePos.z = typePos.y/80.0f;
		//				break;

						displace = true;
						if(monsterList[i].GetRenderObject().transform.position.y < 70 && monsterList[i].GetRenderObject().transform.position.y > 0)
						{
							if(monsterList[i].GetRenderObject().transform.position.y < type.GetRenderObject().transform.position.y)
								type.GetRenderObject().transform.position += (new Vector3(-1, 1, 0)).normalized * time * type.monsterSpeed ;
							else
								type.GetRenderObject().transform.position += (new Vector3(-1, -1, 0)).normalized * time * type.monsterSpeed ;
						}
						
						if(monsterList[i].GetRenderObject().transform.position.y > 70)
							type.GetRenderObject().transform.position += (new Vector3(-1, -1, 0)).normalized * time * type.monsterSpeed ;
						else if (monsterList[i].GetRenderObject().transform.position.y < 0)
							type.GetRenderObject().transform.position += (new Vector3(-1, 1, 0)).normalized * time * type.monsterSpeed ;
						
						Vector3 typePos = type.GetRenderObject().transform.position ;
						typePos.z = typePos.y/80.0f;
						break;
					}

					if(tempDis<type.m_data.misPlace && (monsterList[i].GetEnitityAiState() == AIState.AI_STATE_ACTTACK_CITY ||
					     monsterList[i].GetEnitityAiState() == AIState.AI_STATE_ESCAPE)){

						
						displace = true;
						if(monsterList[i].GetRenderObject().transform.position.y < 70 && monsterList[i].GetRenderObject().transform.position.y > 0)
						{
							if(monsterList[i].GetRenderObject().transform.position.y < type.GetRenderObject().transform.position.y)
								type.GetRenderObject().transform.position += (new Vector3(0, 1, 0)).normalized * time * type.monsterSpeed ;
							else
								type.GetRenderObject().transform.position += (new Vector3(0, -1, 0)).normalized * time * type.monsterSpeed ;
						}
						
						if(monsterList[i].GetRenderObject().transform.position.y > 70)
							type.GetRenderObject().transform.position += (new Vector3(0, -1, 0)).normalized * time * type.monsterSpeed ;
						else if (monsterList[i].GetRenderObject().transform.position.y < 0)
							type.GetRenderObject().transform.position += (new Vector3(0, 1, 0)).normalized * time * type.monsterSpeed ;
						
						Vector3 typePos = type.GetRenderObject().transform.position ;
						typePos.z = typePos.y/80.0f;
						type.GetRenderObject().transform.position = typePos ;
						break;
					}
				}

				if(!displace)
				{
					type.m_stateMachine.ChangeState(MonsterMoveState.getInstance());
				}
				//find one
				if(type.m_targetCreature==null){
					type.m_stateMachine.ChangeState(MonsterMoveState.getInstance());
					return;
				}
			}
			else
				type.m_stateMachine.ChangeState(MonsterMoveState.getInstance());
		}
		public void Exit(CMonster type){
			//type.Play("stand",WrapMode.Loop);
		}
		public void OnMessage(CMonster type, EventMessageBase data){
		}
			
		public AIState  GetState(){
			return AIState.AI_STATE_MISPLACE ;
		}
		public static MonsterMisplaceState getInstance(){
			if(instance ==null){
				instance = new MonsterMisplaceState();
			}
			return instance;
		}
	}
}

