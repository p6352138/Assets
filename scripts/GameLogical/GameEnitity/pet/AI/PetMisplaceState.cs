using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using GameEvent ;


namespace GameLogical.GameEnitity.AI
{
	/**
	 * move to the city gate state
	 * **/
	public class PetMisplaceState: CStateBase<CPet>{
		protected static PetMisplaceState instance;
		public void Release(){
			
		}
		public void Enter(CPet type){
			type.Play("walk",WrapMode.Loop);
		}

		public void Execute(CPet type, float time){
			List<CCreature> petList = EnitityMgr.GetInstance().GetPetList();
			if(petList.Count > 0){
				float tempDis ;

				bool displace =  false;
				//find the nestest target on eye shot
				for(int i = 0; i<petList.Count; ++i){
					//not come back
					tempDis = Vector3.Distance(petList[i].GetRenderObject().transform.position,type.GetRenderObject().transform.position) ;
					if(tempDis<type.m_data.misPlace && petList[i].GetId() != type.GetId()){
						displace = true;
						if(petList[i].GetRenderObject().transform.position.y < 70 && petList[i].GetRenderObject().transform.position.y > 0)
						{
							if(petList[i].GetRenderObject().transform.position.y < type.GetRenderObject().transform.position.y)
								type.GetRenderObject().transform.position += (new Vector3(1, 1, 0)).normalized * time * type.speed ;
							else
								type.GetRenderObject().transform.position += (new Vector3(1, -1, 0)).normalized * time * type.speed ;
						}

						if(petList[i].GetRenderObject().transform.position.y > 70)
							type.GetRenderObject().transform.position += (new Vector3(1, -1, 0)).normalized * time * type.speed ;
						else if (petList[i].GetRenderObject().transform.position.y < 0)
							type.GetRenderObject().transform.position += (new Vector3(1, 1, 0)).normalized * time * type.speed ;

						Vector3 typePos = type.GetRenderObject().transform.position ;
						typePos.z = typePos.y ;
						type.GetRenderObject().transform.position = typePos ;
						break;
					}
				}

				if(!displace)
				{
					type.m_stateMachine.ChangeState(PetAttackState.getInstance());
				}
				//find one
				if(type.m_targetCreature==null){
					type.m_stateMachine.ChangeState(PetMoveState.getInstance());
					return;
				}
			}
			else
				type.m_stateMachine.ChangeState(PetMisplaceState.getInstance());
		}
		public void Exit(CPet type){
			//type.Play("stand",WrapMode.Loop);
		}
		public void OnMessage(CPet type, EventMessageBase data){
		}
			
		public AIState  GetState(){
			return AIState.AI_STATE_MISPLACE ;
		}
		public static PetMisplaceState getInstance(){
			if(instance ==null){
				instance = new PetMisplaceState();
			}
			return instance;
		}
	}
}

