using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using AppUtility;
using common;
using System;
using GameLogical.Building;

namespace GameLogical.GameEnitity{
	
	public class PlayerTempDto : BassStruct{
		/** 角色ID */
		public string playerId;
		/** 玩家性别,0-男，1-女 */
		public int sex;
		/** 玩家昵称 */
		public string name;
		/**
		 * 玩家当前的图片地址
		 */
		public int figureurl;
		/** 职业 */
		public int professionId;
	
		/**
		 * 等级
		 */
		public int lv;
		/**
	 * 创建时间
	 */
		public string createDate;

		
		public PlayerTempDto(Dictionary<string,object> dic){
			if(dic != null)
				parseData(dic);
		}
	}
	
	public class PlayerDto : BassStruct
	{
		/** 角色ID */
		public string playerId;
		/** 玩家性别,0-男，1-女 */
		public int sex;
		/** 玩家昵称 */
		public string name;
		/**
		 * 玩家当前的图片地址
		 */
		public int figureurl;
		/** 玩家等级 */
		public int lv;
		/** 货币 */
		public int money;
		/** 职业 */
		//public int professionId;
		/** 黄能量 */
		//public int yellowEnergy;
		/** 蓝能量 */
		//public int blueEnery;
		/** 红能量 */
		//public int redEnergy;
		/** 攻击力 */
		//public int attack;
		/** 暴击 */
		//public int crit;
		/** 冰属 */
		//public int ice;
		/** 土属 */
		//public int earth;
		/** 火属 */
		//public int fire;
		/** 光属性 */
		//public int light;
		/** 雷属性 */
		//public int thunder;
		/** 风属性 */
		//public int wind;
		/** 宠物携带上限 */
		//public int petLimitCount;
		/** 当前战斗所在地图 */
		//public int mapId;
		/** 当前经验 */
		public int exp;
		/** 最大经验 */
		public int maxExp;
		/** 游戏币 */
		public int gold;
		/** 技能点 */
		public int skillPoint;
		/**
	 * 分享好友经验的次数
	 */
		public int shareExpCount=0;
		//public string  skipPveTime;
		 /**
		 * 新手强制引导步骤
		 */
		public int firstStep;
		/**
	 * 不是强制引导，已经完成了哪些步骤
	 */
		public string step;
		/**
		 * 怒气技能ID
		 */
		//public string angerSkill;
		/**
	 * 成就点
	 */
		//public int successPoint = 0;
		/**
	 * 当前对应的成就称号ID
	 */
		//public int successId;
		/**
	 * 所有开放的称号ID
	 */
		public List<int> allSuccessIds;
		public bool  isfriend;
		/**
	 * 已经完成的成就数量
	 */
		public int successCount = 0;
		/** 今天的刷宠次数 */
		public int refreshPetCount;
		public int allRefreshCount;
		/** 下次刷宠时间 */
		public string nextFreshPetDate;
		/**
	 	* 跳过战斗的次数
	 	*/
		public int skipPveCount;
		/**
	 * 扫荡次数加一的时间
	 */
		public string nextSkipTime;
		///<summary>晶钻刷宠消耗</summary>	 
		public List<int> refreshPetMoney;

		/**
		 * vip等级
		 */
		public int vipLv = 0;

		/**
	 * 竞技场，购买1个挑战次数花费的晶钻 
	 */
		public int arenaPvpCount; 
		/**
	 * 消耗元宝清理的竞技场CD时间(分钟)，即多长时间消耗一个晶钻
	 */
		public int clearCdTime;
		/**
	 * 消耗元宝清理的怪物入侵CD时间(分钟),即多长时间消耗一个晶钻
	 */
		public int clearCdEventTime;
		/// <summary>
		/// 购买扫荡次数
		/// </summary>
		public int buySkipPrice;
		/// <summary>
		/// 关卡购买挑战次数
		/// </summary>
		public int buyFightCountMoney;



		public PlayerDto(Dictionary<string,object> ob){
			refreshPetMoney = new List<int>();
			/*allSuccessIds = new List<int>();

			if(ob.ContainsKey("allSuccessIds")){
				if(ob["allSuccessIds"] != null){
					List<object> data = ob["allSuccessIds"] as List<object>;
					foreach(object obj in data){
						int temp = (int)obj;
						allSuccessIds.Add(temp);
					}
				}
			}*/

			if(ob.ContainsKey("refreshPetMoney")){
				if(ob["refreshPetMoney"] != null){
					string temp = ob["refreshPetMoney"] as String;
					string[] split = temp.Split(',');
					refreshPetMoney.Add(int.Parse(split[0]));
					refreshPetMoney.Add(int.Parse(split[1]));
				}
			}

			this.parseData(ob);
		}
	}
	
	public class PlayerSkillListDto
	{
		public List<PlayerSkillDto> skillDtoList;
		public PlayerSkillListDto(object dictionary)
		{
		   skillDtoList=new List<PlayerSkillDto>();
		   List<object> dataList= dictionary as List<object>;
		   int count=dataList.Count;
			PlayerSkillDto playerSkillDto;
			for(int index=0;index<count;index++)
			{
				playerSkillDto=new PlayerSkillDto((Dictionary<string,object>)dataList[index]);
				skillDtoList.Add(playerSkillDto);
			}
		}
	}
	
	public class PlayerSkillDto:BassStruct
	{
	
		public string id;
		/**
		 * 技能配置Id
		 */
		public int skillConfigId;
		/**
		 * 是否开放
		 */
		public int isOpen;
		/** 是否出战 0:否 1:是 */
		public  int isFight;
		/**
		 * 升级需要消耗的技能书
		 */	
		public int requestItemId;
		/**
		 * 是否可以升级 0:否 1:是
		 */
		public int isUpgradelv ;

		public int seat;
	
		public PlayerSkillDto(Dictionary<string,object> dictionary)
		{
		 this.parseData(dictionary);
		}
		
	}
	
	
	public class EmailDto:BassStruct 
	{

		public string id;
		/**
		 * 邮件标题
		 */
		public string title;
		/** 邮件内容 */
		public string content;
		/** 附件物品 */
		public List<AffixInfo> affixList;
		/** 是否已读 0:否 1:是 */
		public int isRead;
		/** 创建时间 */
		public string createDate;
		public EmailDto(Dictionary<string,object> dic)
		{
			if(dic != null){
				affixList =new List<AffixInfo>();

				List<object> affixJasonList = dic["affixList"] as List<object>;
				for(int i = 0 ; i<affixJasonList.Count;++i)
				{
					AffixInfo temp =new AffixInfo (affixJasonList[i] as Dictionary<string,object>);
					affixList.Add(temp);
				}
				
				this.parseData(dic);
			}
			
			
		}
	}
	
	public class AffixInfo :BassStruct
	{
		/**
		 * 附件类型 1:金币 2:宠物 3:道具 4:晶钻 5:玩家经验
		 */
		public int type;
		/**
		 * 配置ID
		 */
		public int configId;
		/**
		 * 数目
		 */
		public int value=0;
		public AffixInfo(Dictionary<string,object> dic)
		{
			this.parseData(dic);
		}
	}

	/**
 * 
 * 
 * 活动信息
 * 
 * @author lichengjun
 * 
 */
	public class NoticeDto :BassStruct{
		/** 开始日期 */
		public string startDate;
		/** 结束日期 */
		public string endDate;
		/** 每天开始时间 */
		public int startTime;
		/** 每天结束时间 */
		public int endTime;
		/** 等级限制 */
		public int lvLimit;
		/** 活动名称 */
		public string name;
		/** 描述 */
		public string des;
		/**
		 * 类型名称
		 */
		public List<string> typeNames ;
		/** 活动图片Id */
		public int imageId;


		public	NoticeDto(Dictionary<string,object>ob)
		{

			if(ob.ContainsKey("typeNames"))
			{
				typeNames = new List<string>();

				List<object> typenames = ob["typeNames"] as List<object>;
				for(int i =0;i<typenames.Count;i++)
				{
					typeNames.Add(typenames[i] as string);
				}
			}
			this.parseData(ob);

			//this.startDate = TimeChange(this.startDate);
			//this.endDate = TimeChange(this.endDate);
		}

	}



	public class PlayerData : CreatetureData
	{
		//public		int		gateBlood	;
		//public		int		girlCount	;
	}
	
		/**
	 * 
	 * 随机事件信息
	 * 
	 * @author lcj
	 * 
	 */
	public class EventDto:BassStruct
	{
		/*事件id*/
		public string id;		
		
		public string playerId;
	
		public string name;
	
		public int playerLv;
		/*玩家头像*/
		public int image;
	
		/**
		 * 结束时间
		 */
		public string endTime;
		/**
		 * 1：掳妹分队 2：掳妹联盟
		 */
		public int type;
		/**
		 * 最大血量
		 */
		public int maxHp;
		/**
		 * 剩余血量
		 */
		public int lastHp;
		/**
		 * 已经攻打次数
		 */
		public int attackCount;
		/**
	 * 0:可以攻打 1:胜利 2:失败
	 */
		public int result = 0;
		/**
	 * BOSS配置ID
	 */
		public int bossId;

			
		public EventDto(Dictionary<string,object> ob)
		{
			this.parseData(ob);
		}
	}


	public class LotteryInfoDto :BassStruct{
		/**
	 * 一次抽奖消耗的奖券
	 */
		public int lottery;
		/**
	 * 十次抽奖消耗的奖券
	 */
		public int tenLottery;
		/**
	 * 一次晶钻抽奖消耗的晶钻
	 */
		public int moneyLottey;
		/**
	 * 10次晶钻抽奖消耗的晶钻
	 */
		public int tenMoneyLottery;
		/**
	 * 晶钻抽奖券
	 */
		public int lotteryItem=1;
		public LotteryInfoDto(Dictionary<string,object> ob)
		{
			this.parseData(ob);
		}
	}

	public class ShopDto:BassStruct
	{
		public string id;
		/**  */
		public int itemConfigId;
		/**  */
		public string itemName;

		/// <summary>/** 商品类型 0:道具 1:金币 2:晶钻*/// </summary>
		public int type;
		/** 晶钻 */
		public int money;
		/** 金币 */
		public int gold;
		/** 功勋点 */
		public int point;
		/** 折扣(-1:不打折) */
		public int discount;
		/** 是否出售(0:不出售 1:限制时间出售 2:不限制时间出售) */
		public int isSell;
		/** 开始销售时间 */
		public string startTime;
		/** 销售结束时间 */
		public string endTime;
		/** 限制出售数量(-1:不限制) */
		public int limitCount;
		/** 元宝对应的金币数量 */
		public int count;
		
		public int shopType;
		
		public ShopDto(Dictionary<string,object> dic){
			if(dic != null){
				this.parseData(dic);
			}
		}
	}

	public class myRankDto:BassStruct{
		/** 玩家ID */
		public string id;
		/** 排名 (-1 500名以后)*/
		public int rank; 
			/** 玩家名次 */
		public string name;
		/** 公会名称 */
		public string society;
		/** 经验值 */
		public int exp;
		/** 等级 */
		public int lv;
		/** 宠物配置id */
		public int betConfigId;
		/** 金币 */
		public int gold;
		/** 宠物战斗力 */
		public int fightPower;
		/** 宠物品质 */
		public int quality;
		/** 宠物名次 */
		public string petName;

		public myRankDto(Dictionary<string,object> dic){
			if(dic != null){
				this.parseData(dic);
			}
		}
	}

	public class RankDtoInfo:BassStruct{
		/** 玩家ID */
		public string id;
		/** 排名 */
		public int rank;
		/** 玩家名次 */
		public string name;
		/** 公会名称 */
		public string society;
		/** 经验值 */
		public int exp;
		/** 等级 */
		public int lv;
		/** 宠物配置id */
		public int betConfigId;
		/** 金币 */
		public int gold;
		/** 宠物战斗力 */
		public int fightPower = 0;
		/** 宠物品质 */
		public int quality;
		/** 宠物名次 */
		public string petName;

		public RankDtoInfo(Dictionary<string,object> dic){
			if(dic != null){
				this.parseData(dic);
			}
		}
	}

	public class LotteryRewardDto :BassStruct
	{
		/**
	 * 奖励类型1、游戏币；2、晶钻；3、道具；4、宠物
	 */
		public int type;
		/**
	 * 奖励宠物
	 */
		public PetBaseDto petBaseDto;
		/**
	 * 奖励的道具
	 */
		public int itemId;
		/**
	 * 奖励的游戏币
	 */
		public int gold = 0;
		/**
	 * 奖励的元宝
	 */
		public int money = 0;

		public LotteryRewardDto(Dictionary<string,object> dic){
			if(dic != null){
				if(dic.ContainsKey("petBaseDto"))
				{
					object petBase = dic["petBaseDto"] as object;
					petBaseDto = new PetBaseDto(petBase as Dictionary<string,object>);
				}
				this.parseData(dic);
			}
		}	
	}

	public class VipDto :BassStruct{
		
		public int lv;
		/**
	 * 总的充值晶钻
	 */
		public int money;
		/**
	 * 等级奖励（0:未领取 1:已领取）
	 */
		public int lvReward = 0;
		/**
	 * 每天奖励（0:未领取 1:已领取）
	 */
		public int dayReward = 0;

		public VipDto(Dictionary<string,object> dic){
			if(dic != null){
				this.parseData(dic);
			}
		}
	}



	public enum RankTpye{
		LEVEL = 1,
		PETFIGHT = 2,
		UNION = 3,
		MONEY = 4,
		ACHIEVE = 5,
	}

	public class MyExploreDto : BassStruct {
		/**
	 * 探索点的ID
	 */
		public int id;
		/** 0:未领取，1:已领取 3:CD时间 */
		public int state;
		/**
	 * 收取资源的CD时间
	 */
		public string cd;

		public MyExploreDto(Dictionary<string,object> dic){
			if(dic != null){
				this.parseData(dic);
			}
		}
	}
}


