using UnityEngine;
using System.Collections;
using GameEvent ;
using GameLogical.GameSkill.Buff ;
using GameLogical.GameEnitity ;
using common ;
using Bass2D ;

namespace GameLogical.GameSkill{
	public abstract class CSkillBass
	{
		public		bool				isFreezed		;
		protected	SkillDataBass		m_data			;
		
		public 	void	CreateSkill(SkillDataBass data){
			m_data = data ;
			SkillMoudleData skillModuleData = (SkillMoudleData)fileMgr.GetInstance().GetData(m_data.moudleID,CsvType.CSV_TYPE_SKILL) ;
			SkillLvMoudleData skillLvMoudleData = (SkillLvMoudleData)fileMgr.GetInstance().GetData(1,CsvType.CSV_TYPE_SKILLLV) ;
			m_data.rate = skillLvMoudleData.skillLvList[skillModuleData.strength];
			BuffMoudleData buffMoudle ;
			for(int i = 0; i<skillModuleData.buffer.Count; ++i){
				buffMoudle = (BuffMoudleData)fileMgr.GetInstance().GetData(skillModuleData.buffer[i],CsvType.CSV_TYPE_BUFF) ;
				if(buffMoudle.effectName == "RangeEffect"){
					string[] dataList = buffMoudle.argument.Split('#');
					data.isRange = true ;
					data.range   = int.Parse(dataList[2]);
				}

			}

			
			EventMessageFreezeTimeOut messageData = new EventMessageFreezeTimeOut();
			messageData.skillID = m_data.id ;
			GameEvent.EventMgr.GetInstance().AddTimeEvent(skillModuleData.coolDown / 1000.0f,messageData);
			isFreezed = true ;
		}
		
		public abstract bool	canUse();
		
		public	abstract void 	OnMessage(EventMessageBase	message);
		
		public	abstract void	useSkill(object destObject);

		public  void 	PlayStartEffect(object destObject){
			//common.common.SlowScreen();
			SkillMoudleData skillModuleData = (SkillMoudleData)fileMgr.GetInstance().skillCsvData.dataDic[m_data.moudleID] ;
			GameObject effectOb = null ;
			GameObject sceneOb  = null ;
			effectOb = gameGlobal.g_rescoureMgr.GetGameObjectResource("43100-shifa") as GameObject;
			sceneOb  = GameObject.Instantiate(effectOb) as GameObject;
			CCreature creature = EnitityMgr.GetInstance().GetEnitity(m_data.carryID);
			
			
			Transform sceneCreature = sceneOb.transform.FindChild("creature") ;
			Transform destCreature = creature.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT) ;
			
			//attach point
			if(sceneCreature != null && destCreature != null){
				Amin_2D_Ex aminEx = sceneCreature.GetComponent<Amin_2D_Ex>() ;
				if(aminEx.attachPos != ""){
					sceneOb.transform.position = destCreature.FindChild(aminEx.attachPos).position ;
					sceneOb.transform.parent = destCreature.FindChild(aminEx.attachPos);
				}
				else{
					sceneOb.transform.position = creature.GetRenderObject().transform.position ;
					sceneOb.transform.parent = creature.GetRenderObject().transform ;
				}
			}
			else{
				sceneOb.transform.position = creature.GetRenderObject().transform.position ;
				sceneOb.transform.rotation = creature.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT).rotation ;
				sceneOb.transform.parent = creature.GetRenderObject().transform ;
			}
			sceneOb.transform.FindChild("creature").animation.Play("effect");
		}

		protected void PlayEffect(object destObject){
			SkillMoudleData skillModuleData = (SkillMoudleData)fileMgr.GetInstance().skillCsvData.dataDic[m_data.moudleID] ;
			GameObject sceneOb  = null ;
			ResourceMoudleData resourceMoudleData = null ;
			if(skillModuleData.effectID[0] != -1){
				resourceMoudleData = (ResourceMoudleData)fileMgr.GetInstance().GetData(skillModuleData.effectID[0],CsvType.CSV_TYPE_RESOUCE) ;
				//effectOb = gameGlobal.g_rescoureMgr.GetGameObjectResource(resourceMoudleData.path) as GameObject;
				//sceneOb  = GameObject.Instantiate(effectOb) as GameObject;
				MGResouce.LoadEffectData loadData = new MGResouce.LoadEffectData();
				loadData.packName = resourceMoudleData.packagePath ;
				loadData.name	  = resourceMoudleData.path		   ;
				loadData.fun	  = this.FinshLoadCallBack 		   ;
				CCreature creature = EnitityMgr.GetInstance().GetEnitity(m_data.carryID);


				//Transform sceneCreature = sceneOb.transform.FindChild(gameGlobal.CREATURE) ;
				Transform destCreature = creature.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT) ;
				loadData.parent = destCreature ;
				//attach point
				/*if(sceneCreature != null && destCreature != null){
					Amin_2D_Ex aminEx = sceneCreature.GetComponent<Amin_2D_Ex>() ;
					if(aminEx.attachPos != ""){
						//sceneOb.transform.position = destCreature.FindChild(aminEx.attachPos).position ;
						//sceneOb.transform.parent = destCreature.FindChild(aminEx.attachPos);
						loadData.follow = true ;
						//loadData.pos 	= destCreature.FindChild(aminEx.attachPos).position ;
						loadData.parent = destCreature.FindChild(aminEx.attachPos);
					}
					else{
						//sceneOb.transform.position = creature.GetRenderObject().transform.position ;
						//sceneOb.transform.parent = creature.GetRenderObject().transform ;
						loadData.parent = creature.GetRenderObject().transform;
					}
				}
				else{
					//sceneOb.transform.position = creature.GetRenderObject().transform.position ;
					//sceneOb.transform.parent = creature.GetRenderObject().transform ;
					loadData.parent = creature.GetRenderObject().transform;
				}*/
				MGResouce.BundleMgr.Instance.LoadFightEffect(loadData);
			}
			
			
			if(skillModuleData.effectID[2] != -1 && skillModuleData.isBullet == 2){
				resourceMoudleData = (ResourceMoudleData)fileMgr.GetInstance().GetData(skillModuleData.effectID[2],CsvType.CSV_TYPE_RESOUCE);
			
				MGResouce.LoadEffectData loadData = new MGResouce.LoadEffectData();
				loadData.packName = resourceMoudleData.packagePath ;
				loadData.name	  = resourceMoudleData.path		   ;
				loadData.fun	  = this.FinshLoadCallBack 		   ;

				//effectOb = gameGlobal.g_rescoureMgr.GetGameObjectResource(resourceMoudleData.path) as GameObject;
				//sceneOb  = GameObject.Instantiate(effectOb) as GameObject;
				CCreature creature = EnitityMgr.GetInstance().GetEnitity((int)destObject);
				if(creature != null){
					loadData.parent = creature.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT) ;
					//Transform tran = sceneOb.transform.FindChild("creature") ;

					//Transform sceneCreature = sceneOb.transform.FindChild(gameGlobal.CREATURE) ;
					/*Transform destCreature = creature.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_POIN_ROOT) ;
					if(sceneCreature == null)
					{
						sceneCreature = sceneOb.transform.FindChild("creature") ;
					}

					//attach point
					if(sceneCreature != null && destCreature != null){
						Amin_2D_Ex aminEx = sceneCreature.GetComponent<Amin_2D_Ex>() ;
						if(aminEx.attachPos != ""){
							//sceneOb.transform.position = destCreature.FindChild(aminEx.attachPos).position ;
							loadData.parent = destCreature.FindChild(aminEx.attachPos);
						}
						else{
							//sceneOb.transform.position = creature.GetRenderObject().transform.position ;
							loadData.parent = creature.GetRenderObject().transform;
						}
					}
					else{
						//sceneOb.transform.position = creature.GetRenderObject().transform.position ;
						//sceneOb.transform.rotation = creature.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT).rotation ;
						loadData.parent   = creature.GetRenderObject().transform ;
						loadData.rotation = creature.GetRenderObject().transform.FindChild(gameGlobal.CREATURE_ROOT).rotation ;
					}*/
					MGResouce.BundleMgr.Instance.LoadFightEffect(loadData);
					/*if(sceneCreature != null){
						sceneOb.transform.FindChild("creature").animation.Play("effect");
					}*/
				}
			}
		}

		public void FinshLoadCallBack(Object ob, MGResouce.LoadEffectData data){
			if(ob != null){
				
				GameObject sceneOb  = GameObject.Instantiate(ob) as GameObject;
				Transform creature  = sceneOb.transform.FindChild("creature") ;
				//sceneOb.transform.position   = data.pos ;

				if(data.parent != null && data.parent.gameObject != null){
					if(creature != null){
						Amin_2D_Ex aminEx = creature.GetComponent<Amin_2D_Ex>() ;
						if(aminEx != null){
							if(aminEx.attachPos != ""){
								sceneOb.transform.parent = data.parent.FindChild(aminEx.attachPos);
								sceneOb.transform.localPosition = Vector3.zero ;
							}
							else{
								sceneOb.transform.position = data.parent.position ;
								//sceneOb.transform.localPosition = Vector3.zero ;
							}
						}
						else{
							sceneOb.transform.position = data.parent.position ;
						}
					}
					else{
						sceneOb.transform.position = data.parent.position ;
						sceneOb.transform.parent = data.parent ;
					}
				}
				else{
					sceneOb.transform.position = data.parent.position ;
					//sceneOb.transform.localPosition = Vector3.zero ;
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

		public SkillDataBass GetSkillData(){
			return m_data ;
		}


		/*protected PetDto GetPetDto(){
			if()
		}*/
	}

}

