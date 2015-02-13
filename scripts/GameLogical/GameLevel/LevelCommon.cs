using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using AppUtility ;
using GameEvent  ;
using GameLogical.GameEnitity ;
namespace GameLogical.GameLevel{
	public enum LevelType{
		LEVEL_TYPE_NULL,
		LEVEL_TYPE_PVE_NORMAL_POINT,
		LEVEL_TYPE_PVE_CARBON_POINT,
		LEVEL_TYPE_PVE_TASK_NPC,
		LEVEL_TYPE_PVE_MONSTER_INVASION,
		LEVEL_TYPE_PVP,
	}

	public enum LevelMessageAction{
		LEVEL_MESSAGE_ACTION_CREATE_MONSTER,
		LEVEL_MESSAGE_ACTION_AUTO_SKILL,
		LEVEL_MESSAGE_ACTION_GUIDE_STOP,
	}
	
	public class MonsterPointCreateMessage : EventMessageBase{
		public  PointNpcDto		pointData ;
		public MonsterPointCreateMessage(){
			eventMessageModel = EventMessageModel.eEventMessageModel_Level ;
			eventMessageAction= (int)LevelMessageAction.LEVEL_MESSAGE_ACTION_CREATE_MONSTER;
		}
	}
	
	public class AutoSkillChangeMessage : EventMessageBase{
		public AutoSkillChangeMessage(){
			eventMessageModel = EventMessageModel.eEventMessageModel_Level ;
			eventMessageAction= (int)LevelMessageAction.LEVEL_MESSAGE_ACTION_AUTO_SKILL;
		}
	}

	public class GuideStopMessage : EventMessageBase{
		public GuideStopMessage(){
			eventMessageModel = EventMessageModel.eEventMessageModel_Level ;
			eventMessageAction= (int)LevelMessageAction.LEVEL_MESSAGE_ACTION_GUIDE_STOP;
		}
	}
	
	public class MapDto : BassStruct{
		public int ID;
		/**
		 * 图片ID
		 */
		public int imageId;
		/**
		 * 战斗背景ID
		 */
		public int fightID;
		/**
		 * 等级限制
		 */
		public int lvLimit;
		/**
		 * 前置Map
		 */
		public int beforeMapId;
		/**
	 * 名称
	 */
		public string name;
		/**
		 * 完成度
		 */
		public double completeRate;
		/**
		 * 是否开启(0：否 1:是)
		 */
		public int isLock = 0;
		/**
	 * 第一次进入开放的地图 0:否 1:是
	 */
		public int firstOpen = 0;

		public MapDto(Dictionary<string,object> dic){
			if(dic != null){
				this.parseData(dic);
			}
		}
	}
	
	public  class MapShow : BassStruct{
		public int mapId ;
	}
	
	
	public  class LevelPointDto : BassStruct{
		public int id;
		/**
		 * 图片ID
		 */
		//public int imageId;
		/**
		 * 关卡名称
		 */
		public string name;
		/**
		 * 星级评论
		 */
		public int starAssess;
		/**
		 * 是否已经打过
		 */
		public int isHaveAttack;
		/**
		 * 是否可以攻打 
		 * 0不可攻打 
		 * 1可以攻打
		 */
		public int isAttack;
		/**
		 * 关卡类型(1:显性 2:隐性 )
		 */
		public int type;
		/**
	 	* 解锁道具
	 	*/
		public int unlockItemId;
		public int lv;
		
		public LevelPointDto(Dictionary<string,object> dic){
			if(dic != null){
				this.parseData(dic);
			}
		}
	}
	
	public	class PoinEnter : BassStruct{
		public	int	pointId ;
	}
	
	
	public  class LevelDto : BassStruct{
		public int id;
		public string taskId = "";
		/**
		 * 怪物的集合
		 */
		public List<PointNpcDto> npcs;
		public int star = 0;
		/**
		 * 女人的数量
		 */
		public int girlCount;
		/**
		 * 换宠的次数
		 */
		public int changePetCount;
		public int imageId;
		/**
		 * 城堡的血量
		 * 
		 */
		public int cityHp;

		public LevelDto(Dictionary<string,object> dic){
			npcs = new List<PointNpcDto>();
			if(dic != null){
				this.parseData(dic);
			}
		}
	}
	
	public class PointNpcDto : BassStruct{
		public int time;

		public List<NpcDto>	npcList ;
		
		public PointNpcDto(Dictionary<string,object> dic){
			npcList = new List<NpcDto>();
			if(dic != null){
				this.parseData(dic);
			}
		}
	}
	
	
	public class NpcDto : BassStruct{
		public int npcId;
		public int number ;
		public int rewardId = -1;
		public int lastHp = -1;
		
		public NpcDto(Dictionary<string,object> dic){
			if(dic != null){
				this.parseData(dic);
			}
		}
	}
	
	public class PetFightPackDto : BassStruct{
		public string id;

		//public List<PetDto> petDtoList;
		/** 出战的宠物 */
		public List<PetDto> petDtoList;
		public List<PetDto> sparePetList;
		//public List<int> playerSkillIds;
		
		/** 组包类型 0:PVE 1:PVP */
		public int	 fightFlag;

		/** 出战的玩家技能 */
		//public List<FightSkillSeatInfo> playerSkillIds ;

		public PetFightPackDto(Dictionary<string,object> dic){
			petDtoList = new List<PetDto>();
			sparePetList = new List<PetDto>();
			//playerSkillIds = new List<FightSkillSeatInfo>();
			if(dic != null){
				if(dic.ContainsKey("petDtoList")){
					List<object> petIdsJsonList = dic["petDtoList"] as List<object>;
					for(int i = 0; i<petIdsJsonList.Count; ++i){
						PetDto petDto = new PetDto((Dictionary<string,object>)petIdsJsonList[i]);
						petDtoList.Add(petDto);
					}
				}

				if(dic.ContainsKey("sparePetList")){
					List<object> petIdsJsonList = dic["sparePetList"] as List<object>;
					for(int i = 0; i<petIdsJsonList.Count; ++i){
						PetDto petDto = new PetDto((Dictionary<string,object>)petIdsJsonList[i]);
						sparePetList.Add(petDto);
					}
				}
			/*	if(dic.ContainsKey("playerSkillIds")){
					List<object> playerSkillIdsJsonList = dic["playerSkillIds"] as List<object>;
					for(int i = 0; i<playerSkillIdsJsonList.Count; ++i){
						FightSkillSeatInfo skillseat = new FightSkillSeatInfo((Dictionary<string,object>)playerSkillIdsJsonList[i]);

						foreach(PlayerSkillDto data in GameDataCenter.GetInstance().playerSkillDtoList){
							if(data.skillConfigId == skillseat.id){
								skillseat.sId = data.id;
								break;
							}
						}

						playerSkillIds.Add(skillseat);
					}
				}*/
				this.parseData(dic);
			}
		}
	}
	//1019-3~fuzai
	//pvp fight player info
	public class FightPlayerDto : BassStruct{
		public string playerId ;
		/**
		 * 图像
		 */
		public int imageId;
	
		/**
		 * 等级
		 */
		public int lv;
	
		/**
		 * 名称
		 */
		public string name;
		/** 职业 */
		public int professionId;
		/** 玩家性别,0-男，1-女 */
		public int sex;
		/** 攻击力 */
		public int attack;
		/** 暴击 */
		public int crit;
		/** 冰属 */
		public int ice = 0;
		/** 土属 */
		public int earth = 0;
		/** 火属 */
		public int fire = 0;
		/** 光属性 */
		public int light = 0;
		/** 暗属性 */
		public int thunder = 0;
		/** 风属性 */
		public int wind = 0;
		/**
		 * 怒气技能ID,BUFFERID
		 */
		public string angerSkill;

		/** 黄能量 */
		public int yellowEnergy;
		/** 蓝能量 */
		public int blueEnery;
		/** 红能量 */
		public int redEnergy;

		public int mpStone	;

		public int m_engry  ;
		public int engry{
			get{
				return m_engry ;
			}
			set{
				m_engry = value ;
				gameGlobal.g_PvPFightSceneUI.SetOtherEnergy(m_engry);
			}
		}

		
		public FightPlayerDto(Dictionary<string,object> dic){
			if(dic != null){
				this.parseData(dic);
			}
		}
	}

	
	
	public class EndGameDto : BassStruct{
		/**
		 * 掉落ID
		 */
		public List<int> rewardList = new List<int>();
		/**
		 * 打败的NPC
		 */
		public int loseNpc;
		/**
		 * 是否赢(1:赢 2:输)
		 */
		public int isWin;	
		/**
		 * 当前关卡的ID
		 */
		public int pointId;
		/**
		 * 抢到的女人数量
		 */
		public int robWoman;
		/**
		 * 城门剩余血量
		 */
		public int cityLastHp;
		public List<int> npcList ;
		public string taskId = "";
		/**
		 * 三消获得N点能量
		 */
		public int energy = 0;
		/**
		 * 消除宝石的数量
		 */
		public int gemCount = 0;

	}
	
	
	public class PVEResultInfoDto : BassStruct{

		public PlayerDto playerDto ;

		public List<PetDto> petDtoList = new List<PetDto>();

		public List<DropDto> dropDtoList = new List<DropDto>();

		public List<int> unLockPointList;

		/**
		 * 三消获得N点能量
		 */
		public int energy = 0;
		/**
		 * 消除宝石的数量
		 */
		public int gemCount = 0;
	}

	public class DropDto : BassStruct{
		public int playerExp;
		public int petExp;
		public int gold;
		public int money;

		public List<DropGoodsDto> dropGoodsList;
		
		public DropDto(Dictionary<string,object> dic){
			if(dic != null){
				dropGoodsList = new List<DropGoodsDto>();
				if(dic.ContainsKey("dropGoodsList"))
				{
					List<object> temp = (List<object>)dic["dropGoodsList"];
					for(int i=0;i<temp.Count;i++)
					{
						DropGoodsDto temp1 = new DropGoodsDto((Dictionary<string,object>)temp[i]);
						dropGoodsList.Add(temp1);
					}
				}
				this.parseData(dic);
			}
		}
	}

	public class DropGoodsDto : BassStruct{

		public int id;
		/**
		 * 类型
		 */
		public int type;
		/**
		 * 名称
		 */
		public string name;
		/**
		 * 图片ID
		 */
		public int imageId;
		
		public DropGoodsDto(Dictionary<string,object> dic){
			if(dic != null){
				this.parseData(dic);
			}
		}
	}
	
	public class EventResultDto : BassStruct{

		/**
		 * 掉落ID
		 */
		public List<int> rewardList = new List<int>();
		/**
		 * 副本分享用户Id
		 */
		public string playerId;
		/**
		 * 副本ID
		 */
		public string eventId;
		/**
		 * 进去副本时候BOSS的血量
		 */
		public int hp;
		/**
		 * 当前剩余的BOSS血量
		 */
		public int lastHp;
		public List<int> npcList ;
		
		public EventResultDto(Dictionary<string,object> dic){
			if(dic != null){
				this.parseData(dic);
			}
		}
	}

	public class PvPFightResult : BassStruct{
		public int win ;
		public int exp ;
		public int gold;
		public int num ;
		public int curRank;
		
		public PvPFightResult(Dictionary<string,object> dic){
			if(dic != null){
				this.parseData(dic);
			}
		}
	}

	public class RoadPosition{
		public static Vector3[] loadList = {new Vector3(0.0f,0.0f, -0.6f),new Vector3(0.0f,10.0f, -0.4f),new Vector3(0.0f,0.0f, -0.2f),new Vector3(0.0f,20.0f, 0.0f),
											new Vector3(0.0f,30.0f, 0.2f),new Vector3(0.0f,40.0f, 0.4f),new Vector3(0.0f,50.0f, 0.6f),new Vector3(0.0f,60.0f, 0.8f)};
	}

	public class PointRewardDto : BassStruct {
		/**
	 * 金币
	 */
		public int gold;
		/**
	 * 经验
	 */
		public int exp;
		/**
	 * 可能奖励的宠物列表
	 */
		public List<int> petList;
		/**
	 * 可能奖励的道具列表
	 */
		public List<int> itemList;
		/**
	 * 隐性关卡解锁类型(1:玩家等级 2:总星数 3:Ø指定关卡达到3星)
	 */
		public int hideUnlockType;
		/**
	 * 隐性关卡解锁条件
	 */
		public int hideUnlockCondition;
		/**
	 * 前置关卡ID
	 */
		public string beforePoints;
		/**
	 * 解锁需求道具ID
	 */
		public int requestItemId;
		public List<int> beforePointsList;
		/**
	 * 解锁需要的任务ID
	 */
		public int requestTaskId;
		/**
	 	* 可以攻打次数
	 	*/
		public int attackCount;
		/**
	 	* 隐性关卡解锁需要的总星数
	 	*/
		public int hidePointRequestStar;
		/**
		 * 阵营
		 */
		public int camp;
		/**
	 * 最大攻打次数
	 */
		public int maxAttackCount;

		public PointRewardDto(Dictionary<string,object> dic){

			petList = new List<int>();
			itemList = new List<int>();
			beforePointsList = new List<int>();

			if(dic.ContainsKey("petList")){
				if(dic["petList"] != null){
					List<object> data = dic["petList"] as List<object>;

					foreach(object o in data){
						int temp = (int)o;
						petList.Add(temp);
					}
				}
			}

			if(dic.ContainsKey("itemList")){
				if(dic["itemList"] != null){
					List<object> data = dic["itemList"] as List<object>;
					
					foreach(object o in data){
						int temp = (int)o;
						itemList.Add(temp);
					}
				}
			}

			if(dic.ContainsKey("beforePoints")){
				if(dic["beforePoints"] != null){
					beforePoints = dic["beforePoints"] as string;
					
					string []cell = beforePoints.Split('#');
					for(int i=0;i<cell.Length;i++){
						if(!cell[i].Equals("")){
							beforePointsList.Add(int.Parse(cell[i]));
						}
					}
				}
			}
			if(dic != null){
				this.parseData(dic);
			}
		}
	}

}

