using UnityEngine ;
using System.Collections;
using System.Text;
using System.IO;
using System;
using AppUtility ;
using System.Collections.Generic ;

namespace common{
	public class ConfigData : BassStruct{
		public 		string		userServerIP = "" ;
		public		int			userServerPort 	  ;
		public		string		connectServerIP = "";
		public		int			connectServerPort;
		public		string		connectChatServerIP;
		public		int			connectChatServerPort;
		public		string		dataPath;
		public		string		monsterCsv;
		public		string      resouce ;
		public		string		dictionaryCsv;
		public		string		skillCsv;
		public		string		buffCsv ;
		public      string      playerCsv;
		public 		string		itemCsv ;
		public		int			version	;
		
		public		string		talkCsv	;
		public		string		petCsv	;
		public		string		skillLvCsv	;
		public		string		taskCsv;
		public		string		expUpCsv	;
		public ConfigData(Dictionary<string,object> dic){
			version = 1 ;
			this.parseData(dic);
		}
	}
	
	public class DictionaryData:BassStruct
	{
		public	int	id	;
		public	string	str	;
		
		public	DictionaryData(Dictionary<string,object> dic){
			this.parseData(dic);
		}
	}
	
	
	class fileMgr : Singleton<fileMgr>{
		public string str = "";
		public csvReader	monsterCsvData	;
		public csvReader	resouceCsvData	;
		public csvReader	dictionaryCsvData;
		public ConfigData 	config	;
		
		public csvReader	skillCsvData	;
		public csvReader	buffCsvData		;
		public csvReader	playerCsvData   ;
		public csvReader	itemCsvData   	;
		public csvReader	talkCsvData		;
		public csvReader	petCsvData		;
		public csvReader	skillLvCsvData	;
		public csvReader	taskCsvData		;
		public csvReader	expUpCsvData	;
		
		protected	int		loadingNum	;
		
		public void Init(){

			int progress = 0 ;
			loadingNum = 6 ;
			//gameGlobal.g_rescoureLoader.LoadRescoure(common.configPath,LoadConfig,ref progress);
		}
		
		public void LoadConfig(WWW assert){
			MonoBehaviour.print("load config ok");
			str = assert.text ;

			Dictionary<string,object> data = (Dictionary<string,object>)Json.Deserialize(str) ;
			config = new ConfigData(data);
			
			
			int progress = 0 ;
			gameGlobal.g_rescoureLoader.LoadRescoure(config.dictionaryCsv,LoadDictionaryCsv,ref progress);
			//gameGlobal.g_rescoureLoader.LoadRescoure(config.monsterCsv,LoadMonsterCsv,ref progress);
		}
		
		public void LoadMonsterCsv(WWW assert){
			MonoBehaviour.print("load LoadMonsterCsv ok");
			monsterCsvData = new csvReader();
			monsterCsvData.Init(assert.text,CsvType.CSV_TYPE_MONSTER);
			int progress = 0 ;
			gameGlobal.g_rescoureLoader.LoadRescoure(config.resouce,LoadResouceCsv,ref progress);
			loadingNum-- ;
			if(loadingNum == 0){
				gameGlobal.g_LoadingPage.EnterGameLoadingFinish();
			}
		}
		
		public void LoadResouceCsv(WWW assert){
			MonoBehaviour.print("load LoadResouceCsv ok");
			resouceCsvData = new csvReader();
			resouceCsvData.Init(assert.text,CsvType.CSV_TYPE_RESOUCE);
			int progress = 0 ;
			//gameGlobal.g_rescoureLoader.LoadRescoure(config.dictionaryCsv,LoadDictionaryCsv,ref progress);
			gameGlobal.g_rescoureLoader.LoadRescoure(config.skillCsv,LoadSkillCsv,ref progress);
			loadingNum-- ;
			if(loadingNum == 0){
				gameGlobal.g_LoadingPage.EnterGameLoadingFinish();
			}
		}
		
		public void LoadDictionaryCsv(WWW assert){
			MonoBehaviour.print("load LoadDictionaryCsv ok");
			dictionaryCsvData = new csvReader();
			dictionaryCsvData.Init(assert.text,CsvType.CSV_TYPE_DICTIONARY);
			int progress = 0 ;
			//gameGlobal.g_rescoureLoader.LoadRescoure(config.skillCsv,LoadSkillCsv,ref progress);
			//loadingNum-- ;
			//if(loadingNum == 0){
			//	gameGlobal.g_loadingUI.EnterGameLoadingFinish();
			//}
		}
		
		public void LoadSkillCsv(WWW assert){
			MonoBehaviour.print("load LoadSkillCsv ok");
			skillCsvData = new csvReader();
			skillCsvData.Init(assert.text,CsvType.CSV_TYPE_SKILL);
			int progress = 0 ;
			gameGlobal.g_rescoureLoader.LoadRescoure(config.playerCsv,LoadPlayerCsv,ref progress);
			loadingNum-- ;
			if(loadingNum == 0){
				gameGlobal.g_LoadingPage.EnterGameLoadingFinish();
			}
		}
		
		public void LoadPlayerCsv(WWW assert){
			MonoBehaviour.print("load LoadPlayerCsv ok");
			playerCsvData	= new csvReader();
			playerCsvData.Init(assert.text,CsvType.CSV_TYPE_PLAYER);
			loadingNum-- ;
			int progress = 0 ;
			gameGlobal.g_rescoureLoader.LoadRescoure(config.itemCsv,LoadItemCsv,ref progress);
		}
		
		public void LoadItemCsv(WWW assert){
			MonoBehaviour.print("load LoadItemCsv ok");
			itemCsvData	= new csvReader();
			itemCsvData.Init(assert.text,CsvType.CSV_TYPE_ITEM);
			loadingNum-- ;
			int progress = 0 ;
			gameGlobal.g_rescoureLoader.LoadRescoure(config.buffCsv,LoadBuffCsv,ref progress);
		}
		
		public void LoadBuffCsv(WWW assert){
			MonoBehaviour.print("load LoadBuffCsv ok");
			buffCsvData	= new csvReader();
			buffCsvData.Init(assert.text,CsvType.CSV_TYPE_BUFF);
			loadingNum-- ;
			//if(loadingNum == 0){
				//gameGlobal.g_loadingUI.EnterGameLoadingFinish();
			//}
			
			int progress = 0;
			gameGlobal.g_rescoureLoader.LoadRescoure(config.petCsv,LoadPetCsv,ref progress);
		}
		
		public void LoadPetCsv(WWW assert){
			MonoBehaviour.print("load LoadPetCsv ok");
			petCsvData	= new csvReader();
			petCsvData.Init(assert.text, CsvType.CSV_TYPE_PET);
			loadingNum--;
			int progress = 0;
			gameGlobal.g_rescoureLoader.LoadRescoure(config.talkCsv, LoadTalkCsv, ref progress);
		}
		
		public void LoadTalkCsv(WWW assert){
			MonoBehaviour.print("load LoadPTalkCsv ok");
			talkCsvData		= new csvReader();
			talkCsvData.Init(assert.text, CsvType.CSV_TYPE_TALK);
			loadingNum--;
			
			int progress = 0;
			gameGlobal.g_rescoureLoader.LoadRescoure(config.skillLvCsv, LoadSkillLvCsv, ref progress);
		
		}
		
		public void LoadSkillLvCsv(WWW assert){
			MonoBehaviour.print("load LoadPSkillLvCsv ok");
			skillLvCsvData		= new csvReader();
			skillLvCsvData.Init(assert.text, CsvType.CSV_TYPE_SKILLLV);
			loadingNum--;
			
			//gameGlobal.g_loadingUI.EnterGameLoadingFinish();
			int progress = 0;
			gameGlobal.g_rescoureLoader.LoadRescoure(config.expUpCsv, LoadExpLvCsv, ref progress);
		
		}
		
		public void LoadExpLvCsv(WWW assert){
			MonoBehaviour.print("load LoadPSkillLvCsv ok");
			expUpCsvData		= new csvReader();
			expUpCsvData.Init(assert.text, CsvType.CSV_TYPE_EXPUPGRADE);
			loadingNum--;
			
			//gameGlobal.g_loadingUI.EnterGameLoadingFinish();
			int progress = 0;
			gameGlobal.g_rescoureLoader.LoadRescoure(config.taskCsv, LoadTaskCsv, ref progress);
		
		}
		
		public void LoadTaskCsv(WWW assert){
			MonoBehaviour.print("load LoadTaskCsv ok");
			taskCsvData		= new csvReader();
			taskCsvData.Init(assert.text, CsvType.CSV_TYPE_TASK);
			loadingNum--;
			
			gameGlobal.g_LoadingPage.EnterGameLoadingFinish();
			//int progress = 0;
			//gameGlobal.g_rescoureLoader.LoadRescoure(config.talkCsv, LoadTalkCsv, ref progress);
		
		}
		
		
		
		void CreateFile(string path,string name,byte[] info)
		{
			try{
				FileStream fs = new FileStream(path + "//" + name,FileMode.OpenOrCreate) ;
				fs.Write(info,0,info.Length);
				fs.Flush();
				fs.Close();
			}
			catch(IOException e){
				Debug.Log("An IO exception has been thrown" + e.ToString());
			}
		}
		
		void LoadFile(string path,string name)
		{
			byte[] bytes = new byte[2046];
			FileStream fs ;
			try{
				fs = File.Open(path+"//"+ name,FileMode.Open);
				fs.Read(bytes,0,(int)fs.Length);
				//br = new BinaryReader(fs);
			}catch(Exception e)
			{
				Debug.Log("An IO exception has been thrown" + e.ToString());
				//return ;
			}
			
			//string data = System.Text.Encoding.UTF8.GetString ( bytes ,0, (int)fs.Length);
			//Dictionary<string,object> search = (Dictionary<string,object>) Json.Deserialize(data);
			//TestStruct ts = new TestStruct(search);
			//return null ;
		
		}
		
		void DeleteFile(string path,string name)
		{
			File.Delete(path+"//"+ name);
		}
		
		public BassStruct GetData(int id, CsvType type){
			switch(type){
			case CsvType.CSV_TYPE_BUFF:{
				if(buffCsvData.dataDic.ContainsKey(id)){
					return buffCsvData.dataDic[id] ;
				}
				else{
					debug.GetInstance().Error("Get buff csv data error:" + id);
					return null ;
				}
			}
				
			case CsvType.CSV_TYPE_DICTIONARY:{
				if(dictionaryCsvData.dataDic.ContainsKey(id)){
					return dictionaryCsvData.dataDic[id] ;
				}
				else{
					debug.GetInstance().Error("Get dictionary csv data error:" + id);
					return null ;
				}
			}

			case CsvType.CSV_TYPE_MONSTER:{
				if(monsterCsvData.dataDic.ContainsKey(id)){
					return monsterCsvData.dataDic[id] ;
				}
				else{
					debug.GetInstance().Error("Get monster csv data error:" + id);
					return null ;
				}
			}
				
			case CsvType.CSV_TYPE_PLAYER:{
				if(playerCsvData.dataDic.ContainsKey(id)){
					return playerCsvData.dataDic[id] ;
				}
				else{
					debug.GetInstance().Error("Get player csv data error:" + id);
				}
				return null ;
			}
				
			case CsvType.CSV_TYPE_RESOUCE:{
				if(resouceCsvData.dataDic.ContainsKey(id)){
					return resouceCsvData.dataDic[id] ;
				}
				else{
					debug.GetInstance().Error("Get resouce csv data error:" + id);
					return null ;
				}
			}
				
			case CsvType.CSV_TYPE_SKILL:{
				if(skillCsvData.dataDic.ContainsKey(id)){
					return skillCsvData.dataDic[id] ;
				}
				else{
					debug.GetInstance().Error("Get skill csv data error:" + id);
					return null ;
				}
			}
				
			case CsvType.CSV_TYPE_ITEM:{
				if(itemCsvData.dataDic.ContainsKey(id)){
					return itemCsvData.dataDic[id] ;
				}
				else{
					debug.GetInstance().Error("Get item csv data error:" + id);
					return null ;
				}
			}
			case CsvType.CSV_TYPE_PET:{
				if(petCsvData.dataDic.ContainsKey(id)){
					return petCsvData.dataDic[id] ;
				}
				else{
					debug.GetInstance().Error("Get pet csv data error:" + id);
					return null ;
				}
			}
			case CsvType.CSV_TYPE_TALK:{
				if(talkCsvData.dataDic.ContainsKey(id)){
					return talkCsvData.dataDic[id] ;
				}
				else{
					debug.GetInstance().Error("Get pet csv data error:" + id);
					return null ;
				}
			}
				
			case CsvType.CSV_TYPE_SKILLLV:{
				if(skillLvCsvData.dataDic.ContainsKey(id)){
					return skillLvCsvData.dataDic[id] ;
				}
				else{
					debug.GetInstance().Error("Get skilllv csv data error:" + id);
					return null ;
				}
			}
				
			case CsvType.CSV_TYPE_TASK:{
				if(taskCsvData.dataDic.ContainsKey(id)){
					return taskCsvData.dataDic[id] ;
				}
				else{
					debug.GetInstance().Error("Get skilllv csv data error:" + id);
					return null ;
				}
			}
				
			case CsvType.CSV_TYPE_EXPUPGRADE:{
				
				if(expUpCsvData.dataDic.ContainsKey(id)){
					
					return expUpCsvData.dataDic[id];
					//return taskCsvData.dataDic[id] ;
				}
				else{
					debug.GetInstance().Error("Get skilllv csv data error:" + id);
					return null ;
				}
			}
			
			default:{
				return null ;
			}

			}
		}
	}
}

