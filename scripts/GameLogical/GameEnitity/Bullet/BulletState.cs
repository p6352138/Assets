using UnityEngine;
using System.Collections;
using GameLogical.GameEnitity.AI;
using GameEvent ;
using common ;
using GameLogical.GameSkill.Buff ;
using GameLogical.GameSkill ;
using Bass2D ;

namespace GameLogical.GameEnitity{
	public class BulletMove : CStateBase<CBullet>
	{
		protected static BulletMove instance;
		public void Release(){
			
		}
		public void Enter(CBullet type){
			CCreature destCreature = EnitityMgr.GetInstance().GetEnitity( type.m_data.destID );
			if(destCreature == null){
				type.m_stateMachine.ChangeState(new BulletEnd());
			}
			else{
				type.m_lastGoPos = destCreature.GetRenderObject().transform.position ;
			}
		}
		public void Execute(CBullet type, float time){
			CCreature destCreature = EnitityMgr.GetInstance().GetEnitity( type.m_data.destID );
			
			Vector3 disVec ;
			if(destCreature == null){
				disVec = type.m_lastGoPos - type.m_renderObject.transform.position;
			}
			else{
				if(type.m_data.destID != 1)
					disVec = destCreature.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_BODY).position - type.m_renderObject.transform.position;
				else 
					disVec = destCreature.GetRenderObject().transform.FindChild("Ponit/2").position - type.m_renderObject.transform.position;

				
				Vector3 newRotation = Vector3.zero;// = Quaternion.LookRotation(disVec).eulerAngles;
				
		
				newRotation.z = (Mathf.Atan(disVec.y / disVec.x) * 180) / (Mathf.PI);
				if(disVec.x < 0)
					newRotation.z += 180;
				
				type.m_renderObject.transform.localRotation = Quaternion.Euler(newRotation);
				type.m_lastGoPos = destCreature.GetRenderObject().transform.position ;
	//			type.m_renderObject.transform.LookAt(destCreature.GetRenderObject().transform, disVec.normalized);
			}

			disVec.z = 0.0f ;
			if(disVec.magnitude < 2.0f){
				type.m_stateMachine.ChangeState(new BulletEnd());
			}
			else{
				Vector3 pos = type.m_renderObject.transform.position ;
				pos += disVec.normalized * time * 70.0f;
				pos.z = pos.y ;

				type.m_renderObject.transform.position = pos ;
			}

		}
		public void Exit(CBullet type){
			
		}
		public void OnMessage(CBullet type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return AIState.AI_STATE_MOVE ;
		}
		
		public static BulletMove getInstace(){
			if(instance ==null){
				instance = new BulletMove();
			}
			
			return instance;
		}
	}
	
	public class BulletStart: CStateBase<CBullet>
	{
		protected static BulletStart instance;
		public void Release(){
		}
		
		public void Enter(CBullet type){
			type.Release();
		}
		public void Execute(CBullet type, float time){
			
		}
		public void Exit(CBullet type){
			
		}
		public void OnMessage(CBullet type, EventMessageBase data){
			type.m_stateMachine.ChangeState(new BulletMove());
		}
		public AIState  GetState(){
			return 0;//BulletState.STATE_BULLET_START ;
		}
		public static BulletStart getInstace(){
			if(instance ==null){
				instance = new BulletStart();
			}
			
			return instance;
		}
	}
	
	public class BulletEnd: CStateBase<CBullet>
	{
		protected static BulletEnd instance;
		public void Release(){
		}
		
		public void Enter(CBullet type){
			CCreature destCreature = EnitityMgr.GetInstance().GetEnitity( type.m_data.destID );
			if(destCreature != null){
				if(type.m_data.buffID.Count > 0){
					SingleBuffCreateData singleBuff = new SingleBuffCreateData() ;
					singleBuff.buffModuleID = type.m_data.buffID	;
					singleBuff.srcCreatureID = type.m_data.scrID ;
					singleBuff.destCreatureID= type.m_data.destID  ;
					singleBuff.rangeType = BuffRangeType.BUFF_RANGE_SINGLE ;
					CBuffMgr.GetInstance().CreateBuff(singleBuff);
					
				}
				else{
					EventMessageFight message = new EventMessageFight();
					message.scrCreatureId = type.m_data.scrID ;
					message.destCreatureId= type.m_data.destID ;
					EventMgr.GetInstance().OnEventMgr(message);
				}
				
				ResourceMoudleData resourceMoudleData = (ResourceMoudleData)fileMgr.GetInstance().resouceCsvData.dataDic[type.m_data.effectEndID] ;
				MGResouce.LoadEffectData loadData = new MGResouce.LoadEffectData() ;
				loadData.packName = resourceMoudleData.packagePath ;
				loadData.name 	  = resourceMoudleData.path ;

				loadData.pos = destCreature.GetRenderObject().transform.position ;
				loadData.parent = destCreature.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT) ;
				loadData.follow = type.m_data.follow ;
				//attach point

				
				/*if(creature != null){
					if(creature.animation != null){
						creature.animation.Play("effect");
					}
				}*/
				
				
				if(type.m_data.audioPath != null && type.m_data.audioPath != ""){
					//resourceMoudleData = (ResourceMoudleData)fileMgr.GetInstance().GetData(type.m_data.audioID,CsvType.CSV_TYPE_RESOUCE) ;
					loadData.aduioPath = type.m_data.audioPath ;
					//MuscClip.MusicClipMgr.GetInstance().MusicClips(type.m_data.audioPath);
				}

				loadData.fun = this.FinshLoadCallBack ;
				MGResouce.BundleMgr.Instance.LoadFightEffect(loadData);
				
			}

			EnitityMgr.GetInstance().DestroyEnitity(type) ;
		}

		public void FinshLoadCallBack(Object ob, MGResouce.LoadEffectData data){
			if(ob != null){

				GameObject sceneOb  = GameObject.Instantiate(ob) as GameObject;
				Transform creature  = sceneOb.transform.FindChild("creature") ;
				sceneOb.transform.position   = data.pos ;
				if(data.follow == true){
					if(data.parent != null && data.parent.gameObject != null){
						Amin_2D_Ex aminEx = creature.GetComponent<Amin_2D_Ex>() ;
						if(aminEx.attachPos != ""){
							sceneOb.transform.position = data.parent.FindChild(aminEx.attachPos).position ;
							sceneOb.transform.parent = data.parent.FindChild(aminEx.attachPos);
						}
						else{
							sceneOb.transform.parent = data.parent ;
						}
					}
					else{
						sceneOb.transform.parent = data.parent ;
					}
				}
				if(creature != null){
					if(creature.animation != null){
						creature.animation.Play("effect");
					}
				}

				if(data.aduioPath != null && data.aduioPath != ""){
					MuscClip.MusicClipMgr.GetInstance().MusicClips(data.aduioPath);
				}
			}



		}
		public void Execute(CBullet type, float time){
			
		}
		public void Exit(CBullet type){
			
		}
		public void OnMessage(CBullet type, EventMessageBase data){
			
		}
		public AIState  GetState(){
			return 0;//BulletState.STATE_BULLET_END ;
		}
		public static BulletEnd getInstace(){
			if(instance ==null){
				instance = new BulletEnd();
			}
			
			return instance;
		}
		
	}
	
}


