using AppUtility ;
using System.Collections.Generic ;
using GameLogical.GameSkill;
using UnityEngine ;
namespace GameLogical.GameEnitity{
	public class PetData : DynamicCreatureData
	{
		public 		string	   severId	 ;
		public 		int        blood     ;
		public		int		   maxBlood	 ;
		public 		int        attack    ;
		public      AttackType attackType;
		
		public 		float	   attackArea		;
		public 		float	   eyeShotArea 		; 
		
		public      int        maxSpell ;
		public      int		   spell	;
		
		public 		List<int>  skillList;
		
		
		/** 暴击(数字1代表1%) */
		public int crit;
		/** 闪避(数字1代表1%) */
		public int duck;
		/** 冰属(1代表1%) */
		public int ice;
		/** 火属(1代表1%) */
		public int fire;
		/** 土属(1代表1%) */
		public int earth;
		/** 雷属(1代表1%) */
		public int thunder;
		/** 光属(1代表1%) */
		public int light;
		/** 风属(1代表1%) */
		public int wind;
		
		public PetData(){
			skillList = new List<int>();
		}
	}
	
	public class PetDto : BassStruct{
		/**  */
		public string id;
		/**  */
		public int betConfigId;
		/**
		 * 名称
		 */
		public string name;
		/**
		 * 职业
		 */
		public int professionId;
		/**
		 * 形象ID
		 */
		public int imageId;
		/** 等级 */
		public int lv;
		/** 品质 */
		public int quality;
		/** 星级 */
		public int starLv;
		/** 炼化次数 */
		public int refineryCount;
		/** 血量 */
		public int hp;
		/**  */
		public int curExp;
		/** 升级经验 */
		public int maxExp;
		/** 攻击力 */
		public int attack;
		/** 命中 */
		public int spell;
		/**
		 * 成长系数
		 */
		public int growFactor ;
		/** 移动速度 */
		public int speed;
		/**
		 * 每秒回血量
	 	*/
		public int addHpSeconds = 1;
		/** 暴击(数字1代表1%) */
		public int crit;
		/** 闪避(数字1代表1%) */
		public int duck;
		/** 冰属(1代表1%) */
		public int ice;
		/** 火属(1代表1%) */
		public int fire;
		/** 土属(1代表1%) */
		public int earth;
		/** 雷属(1代表1%) */
		public int thunder;
		/** 光属(1代表1%) */
		public int light;
		/** 风属(1代表1%) */
		public int wind;
		/** 技能集 */
		public List<PetSkillDto> skillDtoList;
		/** 技能数量上限 */
		public int skillLimitCount;
		/**
	 * 最大洗髓血量
	 */
		public  int maxHpTmp;
		/**
		 * 最大洗髓攻击
		 */
		public int maxAttackTmp;
		/**
		 * 最大洗髓法力
		 */
		public int maxSpellTmp;
	 	/**
	 * 宠物的装备
	 */
		public List<PetEquipmentDto> equipmentDtoList;
		/** 洗属性（血量） */
		public int washHp;
		/** 洗属性(攻击) */
		public int washAttack;
		/** 洗属性(法力) */
		public int washSpell;
		/** 进化后增加血量 */
		public int changeHp;
		/** 化进后增加攻击 */
		public int changeAttack;
		/** 进化后增加的法力 */
		//public int changeSpell;
		/** 下一等级进化后增加的闪避 */
		//public int changeCrit;
		/** 下一等级进化后增加的暴击 */
		//public int changeDuck;
		/** 下一等级装备解锁 0:不开启 1:开启 */
		//public int openEquipmentSeat = 0;
		/** 是否增加附加元素属性 (0:否 1:是) */
		//public int elementTmp;
		/** 是否开启新元素属性 (1:是 2:否) */
		//public int openElement;
		/** 是否开启新技能 (0:否 1:是) */
		public int openSkill;
			/**
		 * 进化需要的宠物配置Id
		 */
		public  string changeRequestPetId;
		/**
		 * 进化需要的宠物个数
		 */
		public int count;
		/**
		 * 最大进化次数
		 */
		public int maxRefineryCount = 9;
		
		/**
		 * 消耗材料集
		 */
		public string consumeMaterial;
		/**
		 * 材料数量集
		 */
		public string materialCount;
		
		/**
		 * 下次技能解锁需要的宠物进化次数
		 */
		public int nextSkillRequest = 0;
		
		/**
		 * 下次装备解锁需要的宠物进化次数
		 */
		public int nextEquipmentOpenRequest = 0;

		/**
	 * 技能解锁需要的宠物进化次数
	 */
		public List<int> skillRequests;
		/**
	 * 装备解锁需要的宠物进化次数
	 */
		//public List<int> equipmentOpenRequests;
		
		public int nextSkillGold;
		public int nextEvolveGold;
		public int nextTrainGold;

		/**
		 * 进化需求的宠物等级
		 */
		public int requestLv;

		public int fightPower;

		public int seat;

		public PetDto(Dictionary<string,object> dic){
			if(dic != null){
				skillDtoList=new List<PetSkillDto>();
				equipmentDtoList=new List<PetEquipmentDto>();
				if(dic.ContainsKey("skillDtoList")){
					List<object> ItemDtoList= dic["skillDtoList"]as List<object>;//boxItems
					PetSkillDto petSkillDto;
					for(int i = 0; i < ItemDtoList.Count; ++i)
					{
						petSkillDto=new PetSkillDto((Dictionary<string,object>)ItemDtoList[i]);
						skillDtoList.Add(petSkillDto);
					}
				}

				if(dic.ContainsKey("equipmentDtoList")){
					List<object> objectList= dic["equipmentDtoList"]as List<object>;
					PetEquipmentDto petEquipmentDto;
					for(int i = 0; i < objectList.Count; ++i)
					{
						petEquipmentDto=new PetEquipmentDto((Dictionary<string,object>)objectList[i]);
						equipmentDtoList.Add(petEquipmentDto);
					}
				}
				if(dic.ContainsKey("skillRequests"))
				{
					List<object> skillOpenList = dic["skillRequests"] as List<object>;
					skillRequests = new List<int>();
					for(int i = 0; i < skillOpenList.Count; i++)
					{
						int openLv  = (int)skillOpenList[i];
						skillRequests.Add(openLv);
					}
				}
				/*if(dic.ContainsKey("equipmentOpenRequests"))
				{
					List<object> equipmentOpen = dic["equipmentOpenRequests"] as List<object>;
					equipmentOpenRequests = new List<int>();
					for(int i = 0; i < equipmentOpen.Count; i++)
					{
						
						int openLv  = (int)equipmentOpen[i];
						equipmentOpenRequests.Add(openLv);
					}
				}*/

				this.parseData(dic);

				PetMoudleData petMoudleData = common.fileMgr.GetInstance().GetData(this.betConfigId,common.CsvType.CSV_TYPE_PET) as PetMoudleData ;
				this.name = petMoudleData.name ;
				this.professionId = petMoudleData.profession ;
				this.imageId = petMoudleData.resourceID ;
				this.starLv  = petMoudleData.grade ;
			}
		}
	}

	public class CreaturePetData{
		public PetDto petDto ;
		public Vector3 pos	 ;
	}
	public class PetBaseInfoDto : BassStruct{
	
		public string id;
	
		public int imageId;
		/**
		 * 品质
		 */
		public int quality;
	
		public string name;
		/**
		 * 职业
		 * 11:刺客     12:战士       13:法师   14:德鲁伊
		 */
		public int professionId;
		/**
		 * 星级
		 */
		public int starLv;
		/**
		 * 等级
		 */
		public int lv;
		/**
		 * 是否出战 (0：否 1:是)
		 */
		public int isFight;
		/**
	 * 宠物配置Id
	 */
	    public int petBaseId;
		/**
	   * 攻击
	  */
	    public int attack;
		/** 当前进化次数 */
	    public int refineryCount;
		/**
		 * 最大进化次数
		 */
	    public int maxRefineryCount;
		
		//no update
		public int curExp;
		public int fightPower;

		public int seat;

		/**
		 * 是否可以在升级技能
		 */
		public bool isUpgradeSkill = false;
		/**
	 * 宠物技能
	 */
		public List<int> skillIdList = new List<int>();
		public int	camp	;
		/** 成长系数 */
		public int growFactor;

		public PetBaseInfoDto(Dictionary<string,object> dic){
			skillIdList = new List<int>();

			if(dic != null){
				if(dic.ContainsKey("skillIdList")){
					if(dic["skillIdList"] != null){
						List<object> oblist = dic["skillIdList"] as List<object>;
						foreach(object ob in oblist){
							skillIdList.Add((int)ob);
						}
					}
				}

				this.parseData(dic);
				PetMoudleData petMoudleData = common.fileMgr.GetInstance().GetData(this.petBaseId,common.CsvType.CSV_TYPE_PET) as PetMoudleData ;
				this.name = petMoudleData.name ;
				this.professionId = petMoudleData.profession ;
				this.imageId = petMoudleData.resourceID ;
				this.starLv  = petMoudleData.grade ;
				this.camp    = petMoudleData.camp ;
			}
		}
	}
	
	
	public class PetEatDto : BassStruct{
		public string petId ;
		public List<string>  eatPetIds = new List<string>();
	}

	public class PetSoldDto : BassStruct{
		public List<string>  eatPetIds = new List<string>();
	}
	
	public class PetWashDto:BassStruct
	{
		public string petId;
	}
	
	public class PetEatDtoRev : BassStruct{
		public PetDto petDto ;
		public List<string>  eatPetIds = new List<string>();
		public PetEatDtoRev(Dictionary<string,object> dic){
			if(dic != null){
				this.parseData(dic);
			}
		}
	}
	
	public class PetPropertyUpDto{
		public int curAddSpellTmp ;
		public int curAddHpTmp ;
		public int curAddAttackTmp ;
	}
	
	public class PetShowChangeResultDto : BassStruct{
		/**
		 * 当前宠物名称
		 */
		public string curPetName;
		/**
		 * 进化后的宠物名称
		 */
		public string changePetName;
	
		/**
		 * 攻击力
		 */
		public int addAttack;
	
		/**
		 * 气血
		 */
		public int addHp;
	
		/**
		 * 法力
		 */
		public int addSpell;
	
		/**
		 * 是否开放技能位(0:否 1:是)
		 */

		public int addSkillSeat;
	
		/**
		 * 是否开放装备位置(-1:不开启 0:武器 1:衣服 2:子鞋 3:项链 4:戒指)
		 */

		public int addEquipmentSeat;
		/**
		 * 是否开启新的元素属性(0:否 1:是)
		 */
		public int addNewProperty;
		/**
		 * 是否开启锻魂
		 */
		public int openForge;
		
		public PetShowChangeResultDto(Dictionary<string,object> dic){
			if(dic != null){
				this.parseData(dic);
			}
		}
	}
	
	public class PetEquipmentDto :BassStruct
	{
		/**  */
		public string id;
		/** 装备的位置 */
		public int seat;
		/** 道具配置ID */
		public int itemconfigId;
		/**
		 * 图片
		 */
		public int imageId;
		/**
		 * 类型 0:宝箱 1：英雄装扮 2：宠物装备 3：材料 4：消耗品 5：技能书 6：任务物品 7.宝石类
		 */
		public int type;
		/**
		 * 详细子类别
		 * <li>1001-2000为英雄装扮，2001-3000为宠物装备，3001-4000为材料，4001-5000为消耗品，7001-8000为宝石类</li>
		 * <li>1001：翅膀 1002：头饰 1003：面具</li>
		 * <li>2001：武器 2002：头盔 2003：衣服 2004：首饰 </li>
		 * <li>3001：宠物进化材料 3002：宠物装备升阶材料 3003：开启关卡材料 </li>
		 * <li>4001：药水 4002：宝箱4003：复活道具</li>
		 * <li>7001：攻击宝石 7002：气血宝石 7003：法力宝石 7004：暴击宝石 7005：闪避宝石 7006：水属宝石7007：火属宝石
		 * 7008：地属宝石 7009：暗属宝石 7010：光属宝石 7011：风属宝石</li>
		 * <li>-1表示没有子类型）</li>
		 */
		public int detailType;
		/** 道具名称 */
		public string itemName;
	
		/** 强化等级 */
		public int lv;
		/** 是否开放 0:否1:是 */
		public int status;
		/** 品质 */
		public int quality;
		/** 孔数 */
		public int hole;
		/** 装备增加血量 */
		public int hp;
		/** 装备增加攻击力 */
		public int attack;
		/** 装备增加法力 */
		public int spell;
		/** 强化影响血量 */
		public int strongHp;
		/** 强化影响攻击 */
		public int strongAttack;
		/** 强化影响法力 */
		public int strongSpell;
			/**
		 * 下级强化影响血量
		 */
		public int nextStrongHp;
		/**
		 * 下级强化影响攻击
		 */
		public int nextStrongAttack;
		/**
		 * 下级强化影响法力
		 */
		public int nextStrongSpell;
	
		/** 当前升阶影响的血量 */
		public int rankHp;
		/** 当前升阶影响的攻击 */
		public int rankAttack;
		/** 当前升阶影响的法力 */
		public int rankSpell;
	
		/** 下阶品质 */
		public int nextQuality;
		/** 孔数 */
		public int nextHole;
		/** 下级升阶影响的血量 */
		public int nextRankHp;
		/** 下级升阶影响的攻击 */
		public int nextRankAttack;
		/** 下级升阶影响的法力 */
		public int nextRankSpell;
	    /**
		 * 强化消耗游戏币
		 */
		public int strongUseGlod;
		/*升阶所需金币*/
		public int rainingDegreeGold = 100;
		/**
		 * 镶嵌的宝石
		 */
		public List<MosaicItemDto> mosaicItemList ;
		
		/**
		 * 装备升阶消耗道具ID
		 */
		public int upgradeQualityItemId = 31002;
		
		/**
		 * 装备升阶消耗道具个数
		 */
		public int upgradeQualityItemCount = 1;
		/**
	 * 升阶影响的当前强化攻击力
	 */
		public int curStrongAttack = 0;
		/**
	 * 升阶影响的当前强化血量
	 */
		public int curStrongHp = 0;
		/**
	 * 装备强化到下一级需要的道具数量
	 */

		public int nextStrongItemCount = 0;
		/**
	 * 装备强化到下一级需要的道具ID
	 */

		public int nextStrongItemId;

		/**
		 * 装备强化到下一级需要强化等级
		 */
		public int requestStrongLv;

		/**
		 * 下一阶的装备名称
		 */
		public string newItemName;
			
		public PetEquipmentDto(Dictionary<string,object> dic){
			if(dic != null){
				mosaicItemList = new List<MosaicItemDto>();
				if(dic.ContainsKey("mosaicItemList")){
					List<object> mosaicItemListJson = dic["mosaicItemList"] as List<object> ;
					for(int i = 0; i<mosaicItemListJson.Count; ++i){
						MosaicItemDto mosaicItem = new MosaicItemDto((Dictionary<string,object>)mosaicItemListJson[i]);
						mosaicItemList.Add(mosaicItem);
					}
				}
				
				this.parseData(dic);
			}
		}
		
	}
	public class LightDto :BassStruct
	{
		/** 类型 1:宠物 2:宠物技能 */
		public int type;
		/** 宠物或技能配置ID */
		public int configId;
				
		public LightDto(Dictionary<string,object> dictionary)
		{
				this.parseData(dictionary);
		}
	}
	
}


