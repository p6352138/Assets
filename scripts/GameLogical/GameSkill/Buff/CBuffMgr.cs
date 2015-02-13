using UnityEngine;
using System.Collections;
using AppUtility;
using GameEvent ;
using System.Collections.Generic;
using GameLogical.GameEnitity ;
using GameLogical.GameSkill.Effect;
using common ;
using Bass2D ;

namespace GameLogical.GameSkill.Buff{
	class CBuffMgr : Singleton<CBuffMgr>, IEvent {
		protected		Dictionary<int,BuffDataBass>	m_allBuff		;
		protected		int								index			;
	
		public void Init(){
			m_allBuff = new Dictionary<int, BuffDataBass>();
			EventMgr.GetInstance().AddLinsener(this);
		}
		
		public void Realse(){
			
		}
		
		public IBuffBase CreateBuff(BuffCreateBassData data){
			
			//CCreature scrCreature = GameEnitity.EnitityMgr.GetInstance().GetEnitity(data.srcCreatureID);
			switch(data.rangeType){
				//single
			case BuffRangeType.BUFF_RANGE_SINGLE:{
				SingleBuffCreateData single = (SingleBuffCreateData)data ;
				BuffMoudleData moudleData ;//= (BuffMoudleData)fileMgr.GetInstance().buffCsvData.dataDic[data.buffModuleID];
				CCreature destCreature = EnitityMgr.GetInstance().GetEnitity(single.destCreatureID);


				for(int i = 0; i<data.buffModuleID.Count; ++i){
					moudleData= (BuffMoudleData)fileMgr.GetInstance().buffCsvData.dataDic[data.buffModuleID[i]];

					if(destCreature == null)
					return null ;

					if(destCreature.GetEnitityAiState() == GameLogical.GameEnitity.AI.AIState.AI_STATE_DEATH){

						return null ;
					}

					//resistance
					if(destCreature.GetEffectData().resistanceList.Contains(moudleData.type)){
						continue ;
					}

					//rate
					int rate = Random.Range(0,100);
					if(single.buffRate.Count > i){
						if(rate >= single.buffRate[i])
							continue ;
					}

					//last buff
					if(moudleData.lastTime != -1){


						int repeatBuff = destCreature.CheckBuff(data.buffModuleID[i]) ;
						if(repeatBuff != -1)
						{
							this.DestroyBuff(repeatBuff);
						}

						index++ ;
						
						if(!destCreature.AddBuff(data.buffModuleID[i],index))
							return null ;

						SingleBuff buffData = new SingleBuff();
						buffData.rangeType= BuffRangeType.BUFF_RANGE_SINGLE ;
						buffData.lastTime = moudleData.lastTime ;
						if(moudleData.deltaTime == -1){
							buffData.deltaTime= moudleData.lastTime;
						}
						else{
							buffData.deltaTime= moudleData.deltaTime;
						}
						//buffData.deltaTime= moudleData.deltaTime;
						buffData.destCreatureID = single.destCreatureID ;
						buffData.effectName = moudleData.effectName ;
						buffData.moudleId = data.buffModuleID[i] ;
						buffData.id = index ;
						
						if(EffectMgr.GetInstance().ExcuteEffect(moudleData.effectName,single.srcCreatureID,single.destCreatureID,moudleData.argument)){
							m_allBuff.Add(buffData.id,buffData);
							PlayBuffEffect(single.destCreatureID,moudleData.effectID,buffData.id);
							LastBuffExcuteTimeEvent buffMessage = new LastBuffExcuteTimeEvent() ;
							buffMessage.id = buffData.id ;
							EventMgr.GetInstance().AddTimeEvent(buffData.deltaTime * 0.001f,buffMessage);
							
						}
					}
					else{
						EffectExcuteEventMessage effectExcute = new EffectExcuteEventMessage() ;
						effectExcute.effectName = moudleData.effectName ;
						effectExcute.scrId = single.srcCreatureID ;
						effectExcute.destId= single.destCreatureID;
						effectExcute.argument = moudleData.argument ;

						EventMgr.GetInstance().AddTimeEvent(0.5f,effectExcute) ;

						//EffectMgr.GetInstance().ExcuteEffect(moudleData.effectName,single.srcCreatureID,single.destCreatureID,moudleData.argument);
						PlayEffect(single.destCreatureID,moudleData.effectID);
					}
				}


			}
				break ;

			case BuffRangeType.BUFF_RANGE_CIRCLE:{
				RangeBuffCreateData rangeData = (RangeBuffCreateData)data ;
				for(int i = 0; i< rangeData.buffModuleID.Count; ++i){
					BuffMoudleData moudleData = (BuffMoudleData)fileMgr.GetInstance().GetData(data.buffModuleID[i],CsvType.CSV_TYPE_BUFF);
					if(rangeData != null){
						EffectMgr.GetInstance().ExcuteCricleRangeEffect(rangeData.srcCreatureID,rangeData.destCreatures,moudleData.argument);
						PlayerCricleRangeEffect(rangeData.destPos,moudleData.effectID);
					}
				}

			}
				break ;

			case BuffRangeType.BUFF_RANGE_ALL:{
				AllBuffCreatureData allBuffData = (AllBuffCreatureData)data ;
				for(int i = 0; i< allBuffData.buffModuleID.Count; ++i){
					BuffMoudleData moudleData = (BuffMoudleData)fileMgr.GetInstance().GetData(data.buffModuleID[i],CsvType.CSV_TYPE_BUFF);
					if(allBuffData != null){
						EffectMgr.GetInstance().ExcuteCricleRangeEffect(allBuffData.srcCreatureID,allBuffData.destCreatures,moudleData.argument);
						PlayerCricleRangeEffect(new Vector3(60.0f,40.0f,0.0f),moudleData.effectID);
					}
				}
			}
				break ;

				//passtivity
			case BuffRangeType.BUFF_RANGE_PASSTIVITY_FIGHT:{
				SingleBuffCreateData single = (SingleBuffCreateData)data ;
				CCreature destCreature = EnitityMgr.GetInstance().GetEnitity(single.destCreatureID);
				if(destCreature == null)
					return null ;

				for(int i = 0; i< single.buffModuleID.Count; ++i){
					BuffMoudleData moudleData = (BuffMoudleData)fileMgr.GetInstance().buffCsvData.dataDic[data.buffModuleID[i]];

					index++ ;
					if(!destCreature.AddBuff(data.buffModuleID[i],index))
						return null ;
					SingleBuff buffData = new SingleBuff();
					buffData.rangeType= BuffRangeType.BUFF_RANGE_PASSTIVITY_FIGHT ;
					buffData.destCreatureID = single.destCreatureID ;
					buffData.effectName = moudleData.effectName ;
					buffData.moudleId = data.buffModuleID[i] ;
					buffData.id = index ;
					
					m_allBuff.Add(buffData.id,buffData);
					EffectMgr.GetInstance().ExcuteEffect(moudleData.effectName,single.srcCreatureID,single.destCreatureID,moudleData.argument);
					PlayEffect(single.destCreatureID,moudleData.effectID);
				}

				
			}
				break ;
			case BuffRangeType.BUFF_RANGE_PASSTIVITY:{
				SingleBuffCreateData single = (SingleBuffCreateData)data ;
				CCreature destCreature = EnitityMgr.GetInstance().GetEnitity(single.destCreatureID);
				if(destCreature == null)
					return null ;

				for(int i = 0; i< single.buffModuleID.Count; ++i){
					BuffMoudleData moudleData = (BuffMoudleData)fileMgr.GetInstance().GetData(data.buffModuleID[i],CsvType.CSV_TYPE_BUFF);
					if(!destCreature.AddBuff(data.buffModuleID[i],index))
						return null ;
					index++ ;
					EffectMgr.GetInstance().ExcuteEffect(moudleData.effectName,single.srcCreatureID,single.destCreatureID,moudleData.argument);
					PlayEffect(single.destCreatureID,moudleData.effectID);
				}

				
			}
				break ;
			}
			return null ;
		}
		
		public void PlayEffect(int destID,int effectID){
			if(effectID == -1)
				return ;
			if(!fileMgr.GetInstance().resouceCsvData.dataDic.ContainsKey(effectID) ){
				common.debug.GetInstance().Error("load buff resource error: " + effectID);
				return ;
			}
			
			CCreature creature = EnitityMgr.GetInstance().GetEnitity(destID) ;
			if(creature==null)
				return ;
			
			ResourceMoudleData data = (ResourceMoudleData)fileMgr.GetInstance().GetData(effectID,CsvType.CSV_TYPE_RESOUCE) as ResourceMoudleData ;
			MGResouce.LoadEffectData loadData = new MGResouce.LoadEffectData();
			loadData.packName = data.packagePath ;
			loadData.name	  = data.path ;
			loadData.fun	  = this.FinshLoadEffectCallBack;
			loadData.effectID = effectID ;
			if(data.name == "F"){
				loadData.follow	  = true ;
			}
			else{
				loadData.follow	  = false ;
			}

			loadData.parent = creature.GetRenderObject().transform ;
			MGResouce.BundleMgr.Instance.LoadFightEffect(loadData);
		}

		public void FinshLoadEffectCallBack(Object ob, MGResouce.LoadEffectData data){
			if(ob != null){
				GameObject senceOb = MonoBehaviour.Instantiate(ob) as GameObject;
				Transform  sceneCreature = senceOb.transform.FindChild(gameGlobal.EFFECT_CREATURE) ;
				Transform  destCreature  = data.parent.FindChild(gameGlobal.CREATURE_POIN_ROOT) ;
				//attach point
				if(data.parent != null){

					Amin_2D_Ex aminEx = sceneCreature.GetComponent<Amin_2D_Ex>() ;
					if(aminEx != null){
						if(aminEx.attachPos != ""){
							if(data.follow){
								senceOb.transform.parent = destCreature.FindChild(aminEx.attachPos) ;
								senceOb.transform.localPosition = new Vector3(0.0f,0.0f,0.0f);
							}
							else{
								senceOb.transform.position = destCreature.FindChild(aminEx.attachPos).position;
							}
							
						}
						else{
							if(data.follow){
								senceOb.transform.parent = data.parent ;
								senceOb.transform.localPosition = new Vector3(0.0f,0.0f,0.0f);
							}
							else{
								senceOb.transform.position = data.parent.position ;
							}
						}
					}
					else{
						senceOb.transform.parent = destCreature ;
						senceOb.transform.localPosition = new Vector3(0.0f,0.0f,0.0f);
					}
				}
				else{
					if(data.follow){
						senceOb.transform.parent = data.parent ;
						senceOb.transform.localPosition = new Vector3(0.0f,0.0f,0.0f);
					}
					else{
						senceOb.transform.position = data.parent.position ;
					}
				}
				
				senceOb.name = data.effectID.ToString();
				if(sceneCreature != null){
					if(sceneCreature.animation.clip != null && sceneCreature.animation.clip.name != "effect"){

					}
					else{
						sceneCreature.animation.Play("effect");
					}
			}
		}
	}


		public void PlayerCricleRangeEffect(Vector3 destPos,int effectID){
			if(effectID == -1)
				return ;
			ResourceMoudleData data = (ResourceMoudleData)fileMgr.GetInstance().GetData(effectID,CsvType.CSV_TYPE_RESOUCE) as ResourceMoudleData ;

			MGResouce.LoadEffectData loadData = new MGResouce.LoadEffectData();
			loadData.packName = data.packagePath ;
			loadData.name     = data.path ;
			loadData.fun	  = this.FinshLoadCricleRangeEffectCallBack ;
			loadData.pos      = destPos ;
			//senceOb.transform.position = destPos ;
		}

		public void FinshLoadCricleRangeEffectCallBack(Object ob, MGResouce.LoadEffectData data){
			if(ob != null){
				GameObject scenceOb = MonoBehaviour.Instantiate(ob) as GameObject;
				scenceOb.transform.position = data.pos ;
			}
		}


		public void PlayBuffEffect(int destID,int effectID,int index){
			if(effectID == -1)
				return ;
			if(!fileMgr.GetInstance().resouceCsvData.dataDic.ContainsKey(effectID) ){
				common.debug.GetInstance().Error("load buff resource error: " + effectID);
				return ;
			}
			
			CCreature creature = EnitityMgr.GetInstance().GetEnitity(destID) ;
			if(creature==null)
				return ;
			
			ResourceMoudleData data = (ResourceMoudleData)fileMgr.GetInstance().GetData(effectID,CsvType.CSV_TYPE_RESOUCE) as ResourceMoudleData ;
			MGResouce.LoadEffectData loadData = new MGResouce.LoadEffectData() ;
			loadData.packName = data.packagePath ;
			loadData.name     = data.path		 ;
			loadData.fun	  = FinshLoadBuffEffectCallBack ;
			loadData.parent   = creature.GetRenderObject().transform ;
			loadData.effectID = index ;
			if(data.name == "F"){
				loadData.follow	  = true ;
			}
			else{
				loadData.follow	  = false ;
			}

			
			MGResouce.BundleMgr.Instance.LoadFightEffect(loadData);

		}

		public void FinshLoadBuffEffectCallBack(Object ob, MGResouce.LoadEffectData data){
			if(ob != null){
				GameObject senceOb = MonoBehaviour.Instantiate(ob) as GameObject ;

				//attach point
				if(data.parent != null){
					Transform sceneCreature = senceOb.transform.FindChild(gameGlobal.EFFECT_CREATURE) ;
					Transform destCreature = data.parent.FindChild(gameGlobal.CREATURE_POIN_ROOT) ;
					if(destCreature != null){
						Amin_2D_Ex aminEx = sceneCreature.GetComponent<Amin_2D_Ex>() ;
						if(aminEx.attachPos != ""){
							if(data.follow){
								senceOb.transform.parent = destCreature.FindChild(aminEx.attachPos) ;
								senceOb.transform.localPosition = new Vector3(0.0f,0.0f,0.0f);
							}
							else{
								senceOb.transform.position = destCreature.FindChild(aminEx.attachPos).position;
							}
						}
						else{
							if(data.follow){
								senceOb.transform.parent = data.parent ;
								senceOb.transform.localPosition = new Vector3(0.0f,0.0f,0.0f);
							}
							else{
								senceOb.transform.position = data.parent.position ;
							}
						}
					}
					else{
						if(data.follow){
							senceOb.transform.parent = data.parent ;
							senceOb.transform.localPosition = new Vector3(0.0f,0.0f,0.0f);
						}
						else{
							senceOb.transform.position = data.parent.position ;
						}
					}
					
					senceOb.name = data.effectID.ToString();

					Transform tran = senceOb.transform.FindChild("creature");
					if(tran != null){
						tran.animation.wrapMode = WrapMode.Loop ;
						tran.animation.Play("effect");
					}

					if(m_allBuff.ContainsKey(data.effectID)){
						m_allBuff[data.effectID].effectOb = senceOb ;
					}

				}

			}
		}

		public void DestroyEffect(int destID,int effectID){
			CCreature creature = EnitityMgr.GetInstance().GetEnitity(destID) ;
			if(creature != null){
				Transform trans = creature.GetRenderObject().transform.FindChild(effectID.ToString()) ;
				if(trans != null){
					MonoBehaviour.Destroy( trans.gameObject);
				}	
			}
		}
		
		/// <summary>
		/// Raises the message event.
		/// </summary>
		/// <param name='message'>
		/// Message.
		/// </param>
		public void OnMessage(EventMessageBase message){
			if(message.eventMessageModel == EventMessageModel.eEventMessageModel_Buff){
				switch((BuffMessageAction)message.eventMessageAction){
					//last buff
				case BuffMessageAction.BUFF_MESSAGE_EXCUTE:{
					LastBuffExcuteTimeEvent buffMessage = (LastBuffExcuteTimeEvent)message ;
					if(m_allBuff.ContainsKey(buffMessage.id)){
						BuffDataBass data = m_allBuff[buffMessage.id] ;
						data.lastTime -= data.deltaTime ;
						if(data.lastTime > 0){
							
							BuffDataBass buffBata = GetBuffData(data.id);
							if(buffBata.rangeType == BuffRangeType.BUFF_RANGE_SINGLE){
								SingleBuff singleBuffData = buffBata as SingleBuff ;
								CCreature destCreature = EnitityMgr.GetInstance().GetEnitity(singleBuffData.destCreatureID);
								if(destCreature == null){
									DestroyBuff(data.id);
								}
								else{
									ExcuteBuff(data.id);
									EventMgr.GetInstance().AddTimeEvent(data.deltaTime * 0.001f,buffMessage);
								}
								//PlayEffect(singleBuffData.destCreatureID,singleBuffData.moudleId);
							}
							else{
								ExcuteBuff(data.id);
								EventMgr.GetInstance().AddTimeEvent(data.deltaTime * 0.001f,buffMessage);
							}
						}else{
							DestroyBuff(data.id);
						}
					}
				}
					break ;
					
					//attack
				case BuffMessageAction.BUFF_MESSAGE_ATTACK:{
					EventMessageAttack attackMessage = (EventMessageAttack)message ;
					foreach(BuffDataBass buff in m_allBuff.Values){
						SingleBuff singleBuff = buff as SingleBuff ;
						if(singleBuff != null){
							BuffMoudleData moudleData = (BuffMoudleData)fileMgr.GetInstance().GetData(singleBuff.moudleId,CsvType.CSV_TYPE_BUFF);
							if(singleBuff.destCreatureID == attackMessage.scrID ){
								attackMessage.effectFun = moudleData.effectName ;
								attackMessage.param = moudleData.argument ;
								EffectMgr.GetInstance().OnMessage(attackMessage);
							}
							else if(singleBuff.destCreatureID == attackMessage.destID){
								EventMessageBeAttack beAttackMessage = new EventMessageBeAttack();
								beAttackMessage.destID = attackMessage.destID ;
								beAttackMessage.scrID  = attackMessage.scrID  ;
								beAttackMessage.hurt   = attackMessage.hurt   ;
								beAttackMessage.effectFun = moudleData.effectName ;
								beAttackMessage.param = moudleData.argument ;
								EffectMgr.GetInstance().OnMessage(beAttackMessage);
							}
						}
					}
				}
					break ;

				case BuffMessageAction.BUFF_MESSAGE_EFFECT_EXCUTE:{
					EffectExcuteEventMessage effectExcute = (EffectExcuteEventMessage)message ;
					
					//EventMgr.GetInstance().AddTimeEvent(0.5f,effectExcute) ;
					if(EnitityMgr.GetInstance().GetEnitity(effectExcute.scrId) == null || EnitityMgr.GetInstance().GetEnitity(effectExcute.destId) == null)
						return ;

					EffectMgr.GetInstance().ExcuteEffect(effectExcute.effectName,effectExcute.scrId,effectExcute.destId,effectExcute.argument);
				}
					break ;
				}
			}
			
			
		}
		
		void ExcuteBuff(int id){
			BuffDataBass data = m_allBuff[id] ;
			BuffMoudleData moduleData = (BuffMoudleData)fileMgr.GetInstance().buffCsvData.dataDic[data.moudleId] ;
			switch(data.rangeType){
				//single buff
			case BuffRangeType.BUFF_RANGE_SINGLE:{
				SingleBuff singleData = (SingleBuff)m_allBuff[id] ;
				//PlayEffect(singleData.destCreatureID,moduleData.effectID);
				EffectMgr.GetInstance().ExcuteEffect(data.effectName,singleData.srcCreatureID,singleData.destCreatureID,moduleData.argument);
			}
				break;
				
				//circle buff
			case BuffRangeType.BUFF_RANGE_CIRCLE:{
				
			}
				break ;
			}
		}
		
		public BuffDataBass GetBuffData(int id){
			if(m_allBuff.ContainsKey(id)){
				return m_allBuff[id] ;
			}
			/*for(int i = 0; i < m_allBuff.Count; ++i){
				if(m_allBuff[i].id == id){
					return m_allBuff[i] ;
				}
			}*/
			return null ;
		}
		
		public void DestroyBuff(int id){
			if(!m_allBuff.ContainsKey(id))
				return  ;
			BuffDataBass data = m_allBuff[id] ;
			BuffMoudleData moduleData = (BuffMoudleData)fileMgr.GetInstance().buffCsvData.dataDic[data.moudleId] ;
			switch(data.rangeType){
				//single buff
			case BuffRangeType.BUFF_RANGE_SINGLE:{
				SingleBuff singleData = (SingleBuff)m_allBuff[id] ;
				DestroyEffect(singleData.destCreatureID,moduleData.effectID);
				CCreature destCreature = EnitityMgr.GetInstance().GetEnitity(singleData.destCreatureID);
				
				EffectMgr.GetInstance().ExcuteEffect(moduleData.effectName + "End",singleData.srcCreatureID,singleData.destCreatureID,moduleData.argument);
				
				if(destCreature != null)
					destCreature.DelBuff(data.id);

				if(singleData.effectOb != null){
					MonoBehaviour.Destroy(singleData.effectOb);
					singleData.effectOb = null ;
				}
					
				m_allBuff.Remove(data.id);
			}
				break;
				
				//circle buff
			case BuffRangeType.BUFF_RANGE_CIRCLE:{
				
			}
				break ;
			}
		}
		
		public void Clear(){
			m_allBuff.Clear();
		}
	}
}

