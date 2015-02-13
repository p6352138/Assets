using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppUtility;
using GameLogical.GameLevel;

namespace GameLogical.GameEnitity{
	public class AchieveComm {

	}

	public class PlayerAchieveDto : BassStruct {
		/**  */
		public string id;
		/** 配置ID */
		public int configId;
		/** 当前完成的数量 */
		public int curCount = 0;
		/** 需要完成数量 */
		public int requestCount = 0;
		/**
	 * 是否已完成 0:未完成 1:已完成
	 */
		public int isSuccess = 0;
		/** 是否领奖1:已领取 0:未领取 */
		public int status = 0;
		/// <summary> 0:不指引	1:指引	/// </summary>
		public int isGetIn;
		public DropDto dropDto;


		public PlayerAchieveDto(Dictionary<string,object> dic){

			if(dic.ContainsKey("dropDto")){
				if(dic["dropDto"] != null){
					Dictionary<string,object> dropDic = (Dictionary<string,object>)dic["dropDto"] ;
					dropDto = new DropDto(dropDic);
				}
			}

			if(dic != null){
				parseData(dic);
			}
		}
	}
}
