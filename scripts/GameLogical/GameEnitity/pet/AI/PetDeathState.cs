using UnityEngine;
using System.Collections;
using GameEvent;
using System.Collections.Generic;

namespace GameLogical.GameEnitity.AI
{
	public class PetDeathState: CStateBase<CPet>{
		protected static PetDeathState instance;

		public void Release(){
			
		}
		
		public void Enter(CPet type){
			type.GetRenderObject().transform.FindChild(gameGlobal.CREATURE).gameObject.SetActive(false);
			
			GameObject ob = gameGlobal.g_rescoureMgr.GetGameObjectResource("death") ;//gameGlobal.g_rescoureMgr.GetGameObjectResource("object/effect/death") as GameObject;
			GameObject sceneOb = MonoBehaviour.Instantiate(ob) as GameObject;
			sceneOb.transform.position = type.GetRenderObject().transform.position ;
			sceneOb.transform.FindChild("creature").animation.wrapMode = WrapMode.Once ;
			sceneOb.transform.FindChild("creature").animation.Play("effect");
		
			ob = gameGlobal.g_rescoureMgr.GetGameObjectResource("weak") ;//gameGlobal.g_rescoureMgr.GetGameObjectResource("object/pet/weak") as GameObject;
			sceneOb = MonoBehaviour.Instantiate(ob) as GameObject;
			Vector3 scale = sceneOb.transform.localScale ;
			sceneOb.transform.parent = type.GetRenderObject().transform ;
			sceneOb.transform.localScale = scale ;
			sceneOb.transform.localPosition = Vector3.zero ;
			
			//talk
			/*PetMoudleData  petMoudleData = (PetMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID, common.CsvType.CSV_TYPE_PET);
			type.TalkLv(petMoudleData.talkLvInDead, petMoudleData.talkIDInDead);*/

			type.m_bloodBar.SetActive(false);
			type.m_data.attackCD = 0.0f ;

			type.ClearAttack();

			if(type.GetFightCreatureData().isMainRole == true){
				type.GetFightCreatureData().isMainRole = false ;
				EnitityMgr.GetInstance().m_staticScene-- ;
				Time.timeScale = 1.0f ;
				common.common.blackScreen(false);
			}
		}
		
		public void Execute(CPet type, float time){
			type.m_data.attackCD += time ;
			if(type.m_data.attackCD > 1.0f){
				//PetMoudleData  petMoudleData = (PetMoudleData)common.fileMgr.GetInstance().GetData(type.m_data.moudleID, common.CsvType.CSV_TYPE_PET);
				//type.TalkLv(petMoudleData.talkLvInDead, petMoudleData.talkIDInDead);
				type.m_data.attackCD = 0.0f ;
			}
		}
		public void Exit(CPet type){
			
		}
		public void OnMessage(CPet type, EventMessageBase data){
	
		}
		public AIState  GetState(){
			return AIState.AI_STATE_DEATH ;
		}
		
		public static PetDeathState getInstance(){
			if(instance==null){instance = new PetDeathState();}
			return instance;
		}
	}
}

