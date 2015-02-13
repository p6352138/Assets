using UnityEngine;
using System.Collections;
using GameEvent ;

namespace GameLogical.GameEnitity.AI{
	public class EnemyPetDeathState : CStateBase<CEnemyPet>
	{
		protected static EnemyPetDeathState instance;
		public void Release(){
			
		}
		public void Enter(CEnemyPet type){
			GameObject ob = gameGlobal.g_rescoureMgr.GetGameObjectResource("death") ;//gameGlobal.g_rescoureMgr.GetGameObjectResource("object/effect/death") as GameObject;
			GameObject sceneOb = MonoBehaviour.Instantiate(ob) as GameObject;
			sceneOb.transform.position = type.GetRenderObject().transform.position ;
			sceneOb.transform.FindChild("creature").animation.wrapMode = WrapMode.Once ;
			sceneOb.transform.FindChild("creature").animation.Play("effect");


		}
		public void Execute(CEnemyPet type, float time){
			
		}
		public void Exit(CEnemyPet type){
			
		}
		public void OnMessage(CEnemyPet type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_DEATH ;
		}
		public static EnemyPetDeathState getInstance(){
			if(instance ==null){
				instance = new EnemyPetDeathState();
			}
			
			return instance;
		}
	}
}


