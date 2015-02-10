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

			//int progress = 0 ;
			loadingNum = 6 ;
			//gameGlobal.g_rescoureLoader.LoadRescoure(common.configPath,LoadConfig,ref progress);
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
			BassStruct reuslt = new BassStruct ();
			return reuslt;
		}
	}
}

