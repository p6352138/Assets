using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
//using GameLogical;
using common;
//using GameLogical.GameEnitity;
using GameMessgeHandle;

public class CommonMessageHandle
{
	static public void CommonMsg(string errorCode,object data){
	//	if(errorCode == "-1")
		{
			Dictionary<string,object> dataListJson = (Dictionary<string,object>)data ;
			MonoBehaviour.print(dataListJson["msg"]);
			DictionaryData dic = (DictionaryData)fileMgr.GetInstance().GetData(73,CsvType.CSV_TYPE_DICTIONARY);

			int i = 0;
			if(object.ReferenceEquals(dataListJson["msg"].GetType(),i.GetType())){
				Debug.Log(dataListJson["msg"].GetType().ToString());
			}

			try{
				int id;
				int.TryParse(dataListJson["msg"].ToString(),out id);

					if(id != 0){
					}
					else if(id == -204){
					}
					else if(id == -1603){
						/*
						if(GameDataCenter.GetInstance().emailReward == 0){
							gameGlobal.g_tipOneButtom.ShowById(1042);
						}else if(GameDataCenter.GetInstance().emailReward == 1){
							gameGlobal.g_tipOneButtom.ShowById(id);
						}*/
					}
					else if(id == -1900){
						//GameDataCenter.GetInstance().isRecruitPet = true;
					}
					else if(id == -1901){
						//GameDataCenter.GetInstance().isRecruitPet = true;
					}
					else if(id == -1902){
						//GameDataCenter.GetInstance().isRecruitPet = true;
					}
					else if(id == -2007){
						//GameDataCenter.GetInstance().isRecruitPet = true;
					}

					if(id == -2102){
						//GameDataCenter.GetInstance().IsShowDiagbox = 1;
					}

					if(id == -2302){
						//GameDataCenter.GetInstance().task.m_mopup.Show(4);
					}
					else if(id == -2303){
						//GameDataCenter.GetInstance().task.m_mopup.Show(2);
					}
					else if(id == -2701){
						//gameGlobal.g_fuBenSelect.ResetCarBonTips();
					}
					else if(id == -2704){
						//gameGlobal.g_fuBenSelect.BuyOpenCount();
					}
					else{
						id = 1000 - id ;
						//gameGlobal.g_tipOneButtom.ShowById(id);
					}
				//}

			}
			catch(UnityException e){
				//common.debug.GetInstance().Error(e.ToString());
			}

			//gameGlobal.g_tipOneButtom.Show(dataListJson["msg"].ToString());
		}
	}

	public static void SendPetEmail(string errorCode,object data){
		if(!errorCode.Equals("-1")){
			/*
			Dictionary<string,object> dataListJson = (Dictionary<string,object>)data ;
			if(dataListJson.ContainsKey("isPetEmail")){
				if(dataListJson["isPetEmail"] != null){

				}
			}*/
			//gameGlobal.g_tipOneButtom.ShowById(1040);
		}
	}
	
	public static void SendItemEmail(string errorCode,object data){
		if(!errorCode.Equals("-1")){
			/*
			Dictionary<string,object> dataListJson = (Dictionary<string,object>)data ;
			if(dataListJson.ContainsKey("isItemEmail")){
				if(dataListJson["isItemEmail"] != null){
					
				}
			}*/
			//gameGlobal.g_tipOneButtom.ShowById(1041);
		}
	}
}

