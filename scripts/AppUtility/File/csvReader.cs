using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using AppUtility ;
using GameLogical;


namespace common{
	public enum CsvType 
	{
		CSV_TYPE_NULL,
		CSV_TYPE_PLAYER,
		CSV_TYPE_MONSTER,
		CSV_TYPE_RESOUCE,
		CSV_TYPE_DICTIONARY,
		CSV_TYPE_SKILL,
		CSV_TYPE_BUFF,
		CSV_TYPE_ITEM,
		CSV_TYPE_PET,
		CSV_TYPE_TALK,
		CSV_TYPE_SKILLLV,
		CSV_TYPE_TASK,
		CSV_TYPE_EXPUPGRADE,
	}
	public class csvReader{
		protected	string[]	lineArray		;
		protected	string[]	itemNameArray	;
		protected	string		stringData			;
		
		public		Dictionary<int,BassStruct>	dataDic		;
		
		public void Init(string data,CsvType type){
			data = data.Replace("\n","");
			lineArray = data.Split("\r"[0]) ;
			dataDic = new Dictionary<int, BassStruct>();
			itemNameArray = lineArray[0].Split ("," [0]) ;
	        for(int i = 1;i < lineArray.Length; i++)
	        {
	            Serialize( lineArray[i].Split ("," [0]),type);
	        }

			lineArray = null ;
			itemNameArray = null ;
			stringData = null ;
		}
		
		void Serialize(string[] lineData,CsvType type){
			if(itemNameArray.Length != lineData.Length){
				debug.GetInstance().Warmming("csv data warmming:" + lineData);
				return  ;
			}
			
			stringData = "{" ;
			for(int i = 0; i < itemNameArray.Length ; ++ i){
				stringData += '"' + itemNameArray[i] + '"' ;
				stringData += ':' ;
				//is char
				try{
					float.Parse(lineData[i]);
					stringData += lineData[i] ;
				}
				catch{
					stringData += '"' ;
					stringData += lineData[i] ;
					stringData += '"' ;
				}
				
				//stringData += lineData[i] ;
				stringData += ',';
			}
			stringData += "}" ;
			
			Dictionary<string,object> typeData = (Dictionary<string,object>)Json.Deserialize(stringData);
			//TYPE type = new TYPE(typeData);
			switch(type){
			case CsvType.CSV_TYPE_PLAYER:{
				PlayerMoudleData data = new PlayerMoudleData(typeData);
				dataDic.Add(data.id,data);
			}
				break ;
				
				//monster
			case CsvType.CSV_TYPE_MONSTER:{
				MonsterMoudleData data = new MonsterMoudleData(typeData);
				string str = typeData["beSkillList"] as string;
				string[] reslut ;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.beSkillList.Add(id);
						}
						catch{
							
						}
					}
				}
				
				
				str = typeData["skillList"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.skillList.Add(id);
						}
						catch{
							
						}
					}
				}
				
				str = typeData["skillProbabilityList"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.skillProbabilityList.Add(id);
						}
						catch{
							
						}
					}
				}
				
				str = typeData["talkIDInMove"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.talkIDInMove.Add(id);
						}
						catch{
							
						}
					}
				}
				
				str = typeData["talkLvInMove"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.talkLvInMove.Add(id);
						}
						catch{
							
						}
					}
				}
				str = typeData["talkIDInSpeed"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.talkIDInSpeed.Add(id);
						}
						catch{
							
						}
					}
				}
				str = typeData["talkLvInSpeed"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.talkLvInSpeed.Add(id);
						}
						catch{
							
						}
					}
				}
				str = typeData["talkIDInAttack"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.talkIDInAttack.Add(id);
						}
						catch{
							
						}
					}
				}
				str = typeData["talkLvInAttack"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.talkLvInAttack.Add(id);
						}
						catch{
							
						}
					}
				}
				str = typeData["talkIDInRobWomen"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.talkIDInRobWomen.Add(id);
						}
						catch{
							
						}
					}
				}
				str = typeData["talkLvInRobWomen"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.talkLvInRobWomen.Add(id);
						}
						catch{
							
						}
					}
				}
				str = typeData["talkIDInRobDoor"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.talkIDInRobDoor.Add(id);
						}
						catch{
							
						}
					}
				}
				str = typeData["talkLvInRobDoor"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.talkLvInRobDoor.Add(id);
						}
						catch{
							
						}
					}
				}
				str = typeData["talkIDInDie"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.talkIDInDie.Add(id);
						}
						catch{
							
						}
					}
				}
				str = typeData["talkLvInDie"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.talkLvInDie.Add(id);
						}
						catch{
							
						}
					}
				}
				str = typeData["talkIDInDead"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.talkIDInDead.Add(id);
						}
						catch{
							
						}
					}
				}
				str = typeData["talkLvInDead"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.talkLvInDead.Add(id);
						}
						catch{
							
						}
					}
				}
				str = typeData["talkIDInSkill"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.talkIDInSkill.Add(id);
						}
						catch{
							
						}
					}
				}
				str = typeData["talkLvInSkill"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.talkLvInSkill.Add(id);
						}
						catch{
							
						}
					}
				}
				
				//attack area & eye shot area & attack lock count
				if(data.profession == 11){
					data.attackArea = 150;
					data.eyeShotArea= 600; 
					data.attackLockCount = 1 ;
				}
				else if(data.profession == 12){
					data.attackArea = 150;
					data.eyeShotArea= 450; 
					data.attackLockCount = 2 ;
				}
				else if(data.profession == 13){
					data.attackArea = 350;
					data.eyeShotArea= 550; 
					data.attackLockCount = 1 ;
				}
				else if(data.profession == 14){
					data.attackArea = 300;
					data.eyeShotArea= 550; 
					data.attackLockCount = 1 ;
				}
				
				if(dataDic.ContainsKey(data.ID))
				{
					debug.GetInstance().Error("monster moudle data have same id:" + data.ID);
				}
				else{
					dataDic.Add(data.ID,data);
				}
			}
				break ;
				
			case CsvType.CSV_TYPE_PET:
			{
				PetMoudleData data = new PetMoudleData(typeData);
				string str = typeData["heavy"] as string;
				string[] reslut ;
				/*if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.heavy.Add(id);
						}
						catch{
							
						}
					}
				}
				
				
				str = typeData["dodge"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.dodge.Add(id);
						}
						catch{
							
						}
					}
				}*/
				
				str = typeData["talkIDInSleep"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.talkIDInSleep.Add(id);
						}
						catch{
							
						}
					}
				}
				str = typeData["talkLvInSleep"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.talkLvInSleep.Add(id);
						}
						catch{
							
						}
					}
				}
				str = typeData["talkIDInSpeed"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.talkIDInSpeed.Add(id);
						}
						catch{
							
						}
					}
				}
				str = typeData["talkLvInSpeed"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.talkLvInSpeed.Add(id);
						}
						catch{
							
						}
					}
				}
				str = typeData["talkIDInAttack"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.talkIDInAttack.Add(id);
						}
						catch{
							
						}
					}
				}
				str = typeData["talkLvInAttack"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.talkLvInAttack.Add(id);
						}
						catch{
							
						}
					}
				}
				str = typeData["talkIDInDie"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.talkIDInDie.Add(id);
						}
						catch{
							
						}
					}
				}
				str = typeData["talkLvInDie"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.talkLvInDie.Add(id);
						}
						catch{
							
						}
					}
				}
				str = typeData["talkIDInDead"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.talkIDInDead.Add(id);
						}
						catch{
							
						}
					}
				}
				str = typeData["talkLvInDead"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.talkLvInDead.Add(id);
						}
						catch{
							
						}
					}
				}
				str = typeData["talkIDInSkill"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.talkIDInSkill.Add(id);
						}
						catch{
							
						}
					}
				}
				str = typeData["talkLvInSkill"] as string;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.talkLvInSkill.Add(id);
						}
						catch{
							
						}
					}
				}
				
				//attack area & eye shot area & attack lock count
				if(data.profession == 11){
					data.attackArea = 150;
					data.eyeShotArea= 600; 
					data.attackLockCount = 1 ;
				}
				else if(data.profession == 12){
					data.attackArea = 150;
					data.eyeShotArea= 450; 
					data.attackLockCount = 2 ;
				}
				else if(data.profession == 13){
					data.attackArea = 350;
					data.eyeShotArea= 550; 
					data.attackLockCount = 1 ;
				}
				else if(data.profession == 14){
					data.attackArea = 300;
					data.eyeShotArea= 550; 
					data.attackLockCount = 1 ;
				}
				
				if(dataDic.ContainsKey(data.ID))
				{
					debug.GetInstance().Error("pet moudle data have same id:" + data.ID);
				}
				else{
					dataDic.Add(data.ID,data);
				}
			}
				break;
			case CsvType.CSV_TYPE_TALK:{
				TalkMoudleDate data = new TalkMoudleDate(typeData);
				if(dataDic.ContainsKey(data.ID))
				{
					debug.GetInstance().Error("talk moudle data have same id:" + data.ID);
				}
				else{
					dataDic.Add(data.ID,data);
				}
			}
				break;
			case CsvType.CSV_TYPE_SKILLLV:{
				SkillLvMoudleData data = new SkillLvMoudleData(typeData);
				
				string str = typeData["skillLvList"] as string;
				string[] reslut ;
				if(str != null){
					reslut = str.Split('#');
					for(int i = 0; i<reslut.Length; ++i){
						try{
							int id = int.Parse(reslut[i]);
							data.skillLvList.Add(id);
						}
						catch{
							
						}
					}
				}
				
				if(dataDic.ContainsKey(data.ID))
				{
					debug.GetInstance().Error("skill level moudle data have same id:" + data.ID);
				}
				else{
					dataDic.Add(data.ID,data);
				}
			}
				break ;
				//resouce
			case CsvType.CSV_TYPE_RESOUCE:{
				ResourceMoudleData data = new ResourceMoudleData(typeData);
				if(dataDic.ContainsKey(data.ID))
				{
					debug.GetInstance().Error("resource moudle data have same id:" + data.ID);
				}
				else{
					dataDic.Add(data.ID,data);
				}
			}
				break ;
				
				//dictionary
			case CsvType.CSV_TYPE_DICTIONARY:{
				DictionaryData data = new DictionaryData(typeData);
				if(dataDic.ContainsKey(data.id))
				{
					debug.GetInstance().Error("dictionary moudle data have same id:" + data.id);
				}
				else{
					dataDic.Add(data.id,data);
				}
			}
				break ;
				
				//skill
			case CsvType.CSV_TYPE_SKILL:{
				SkillMoudleData data = new SkillMoudleData(typeData);
				if(data.costType == 1){
					string str = (string)typeData["cost"] ;
					string[] reslut = str.Split('#') ;
					data.greenStone = int.Parse(reslut[0]) ;
					data.blueStone  = int.Parse(reslut[1]) ;
					data.redStone   = int.Parse(reslut[2]) ;
				}
				else{
					string str = (string)typeData["cost"] ;
					string[] reslut = str.Split('#') ;
					data.magic = int.Parse(reslut[0]) ;
				}
				
				string effectIdStr = (string)typeData["effectID"] ;
				string[] effectIDList = effectIdStr.Split('#');
				for(int i = 0; i<effectIDList.Length; ++i){
					if(effectIDList[i] != ""){
						data.effectID.Add(int.Parse(effectIDList[i]));
					}
				}
				
				string buffIDStr = (string)typeData["buffer"] ;
				string[] buffIDList= buffIDStr.Split('#');
				for(int i = 0; i<buffIDList.Length; ++i){
					if(buffIDList[i] != ""){
						data.buffer.Add(int.Parse(buffIDList[i]));
					}
				}
				
				string buffRateStr = (string)typeData["buffRate"] ;
				string[] buffRateList= buffRateStr.Split('#');
				for(int i = 0; i<buffRateList.Length; ++i){
					if(buffRateList[i] != ""){
						data.buffRate.Add(int.Parse(buffRateList[i]));
					}
				}
				
				if(dataDic.ContainsKey(data.id))
				{
					debug.GetInstance().Error("skill moudle data have same id:" + data.id);
				}
				else{
					dataDic.Add(data.id,data);
				}
			}
				break ;
				
				//buff
			case CsvType.CSV_TYPE_BUFF:{
				BuffMoudleData data = new BuffMoudleData(typeData);
				if(dataDic.ContainsKey(data.id))
				{
					debug.GetInstance().Error("buff moudle data have same id:" + data.id);
				}
				else{
					dataDic.Add(data.id,data);
				}
			}
				break ;
				
				//item
			case CsvType.CSV_TYPE_ITEM:{
				ItemMoudleData data = new ItemMoudleData(typeData);
				if(dataDic.ContainsKey(data.ID))
				{
					debug.GetInstance().Error("item moudle data have same id:" + data.ID);
				}
				else{
					dataDic.Add(data.ID,data);
				}
			}
				break ;
				
			case CsvType.CSV_TYPE_TASK:{
				TaskMoudleData data = new TaskMoudleData(typeData);
				
				string buffRateStr = (string)typeData["taskInfoId"] ;
				string[] buffRateList= buffRateStr.Split('#');
				for(int i = 0; i<buffRateList.Length; ++i){
					if(buffRateList[i] != ""){
						data.taskInfoId = int.Parse(buffRateList[i]);
					}
				}
				
				buffRateStr = (string)typeData["getTaskId"] ;
				buffRateList= buffRateStr.Split('#');
				for(int i = 0; i<buffRateList.Length; ++i){
					if(buffRateList[i] != ""){
						data.getTaskId = int.Parse(buffRateList[i]);
					}
				}
				
				buffRateStr = (string)typeData["setTaskTalkId"] ;
				buffRateList= buffRateStr.Split('#');
				for(int i = 0; i<buffRateList.Length; ++i){
					if(buffRateList[i] != ""){
						data.setTaskTalkId.Add(int.Parse(buffRateList[i]));
					}
				}
				
				buffRateStr = (string)typeData["attackNpcTalkId"] ;
				buffRateList= buffRateStr.Split('#');
				for(int i = 0; i<buffRateList.Length; ++i){
					if(buffRateList[i] != ""){
						data.attackNpcTalkId.Add(int.Parse(buffRateList[i]));
					}
				}
				
				buffRateStr = (string)typeData["unfilTaskTalkId"] ;
				buffRateList= buffRateStr.Split('#');
				for(int i = 0; i<buffRateList.Length; ++i){
					if(buffRateList[i] != ""){
						data.unfilTaskTalkId.Add(int.Parse(buffRateList[i]));
					}
				}
				
				buffRateStr = (string)typeData["filTaskTalkId"] ;
				buffRateList= buffRateStr.Split('#');
				for(int i = 0; i<buffRateList.Length; ++i){
					if(buffRateList[i] != ""){
						data.filTaskTalkId.Add(int.Parse(buffRateList[i]));
					}
				}
				/*
				buffRateStr = (string)typeData["npcResId"] ;
				buffRateList= buffRateStr.Split('#');
				data.npcResId = "";
				 */
				
				if(dataDic.ContainsKey(data.id))
				{
					debug.GetInstance().Error("task moudle data have same id:" + data.id);
				}
				else{
					dataDic.Add(data.id,data);
				}
				
				}
				break;
				case CsvType.CSV_TYPE_EXPUPGRADE:{
				ExpUpGradeMoudleData data = new ExpUpGradeMoudleData(typeData);
				dataDic.Add(data.ID,data);
				}
				break ;
			}
			
		}
		
	}
}

