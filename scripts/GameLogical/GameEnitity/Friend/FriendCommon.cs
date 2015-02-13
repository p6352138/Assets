using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using AppUtility;

namespace GameLogical.GameEnitity{
	
	public class FriendTempDto : BassStruct{
		/* 好友ID */
		public string m_FriendId;
		/* 性别,0-男，1-女 */
		public int m_sex;
		/* 好友昵称 */
		public string m_name;
		/*好友当前头像*/
		public int m_igureurl;
		/* 职业 */
		public int m_professionId;	
		/*等级*/ 
		public int m_lv;
		
		public FriendTempDto(Dictionary<string,object> dic){
			if(dic != null)
				parseData(dic);
		}
	}
	
	public class RecommendFriendDto : BassStruct
	{

		public string friendId;
	
		public string friendName;
	
		public int friendImage;
	
		public int friendSex;
	
		public int friendLv;
	
		/**
		 * 竞技场胜率
		 */
		public int winRate;
		/**
		 * 排名
		 */
		public int rank;
		
		/**
	 * 申请时间
	 */
		public string applyDate;
		
		public RecommendFriendDto(Dictionary<string,object>dictionary)
		{
			this.parseData(dictionary);
		}
	}
	
	public class FriendDto :BassStruct
	{

		public string id;
	
		public string friendId;
	
		public string friendName;
	
		public int friendImage;
	
		public int friendSex;
	
		public int friendLv;
	
		public string createDate;
		/**
		 * 竞技场胜率
		 */
		public int winRate;
		/**
		 * 排名
		 */
		public int rank;
		/**
		 * 分享的经验
		 */
		public int shareExp;
		/**
		 * 今天是否登录 0：没有登录 1:登录
		 */
		public int todayLogin;
		/**
		 * 是否已经获取好友分享的经验 0：否 1:是
		 */
		public int isGet;
		
		/**
	 * 公会名称
	 */
		public string guildName;
		/**
	 * 最近登陆时间
	 */
		public string loginDate;
		/**
	 * 金币
	 */
		public int gold;
		
		

		
		public FriendDto(Dictionary<string,object>dictionary)
		{
			this.parseData(dictionary);
		}
	}
}
