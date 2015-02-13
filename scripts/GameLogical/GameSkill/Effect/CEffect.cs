using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using GameLogical.GameEnitity ;
using GameLogical.GameSkill.Buff ;
using common ;
using GameLogical.GameEnitity.AI ;

namespace GameLogical.GameSkill.Effect{
	
	public class CEffect
	{
		//hp
		public bool	EffectHp(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			List<int> buffList = GetCreatrueBuffList(creatrue);
			if(creatrue != null){
				string[] dataList = ob.data.Split('#');
				int hp = int.Parse(dataList[1]);
				int percent = int.Parse(dataList[0]) ;
				
				if(hp > 0 || percent >0){
					if(buffList!= null && buffList.Count != 0){
						for(int i = 0; i<buffList.Count; ++i){
							BuffDataBass buffData = CBuffMgr.GetInstance().GetBuffData(buffList[i]);
							if(buffData != null){
								BuffMoudleData moudleData = fileMgr.GetInstance().GetData(buffData.moudleId,CsvType.CSV_TYPE_BUFF) as BuffMoudleData;
								if(moudleData.type == 601)
									return false ;
							}
						}
					}
				}
				else{
					if(buffList!= null && buffList.Count != 0){
						for(int i = 0; i<buffList.Count; ++i){
							BuffDataBass buffData = CBuffMgr.GetInstance().GetBuffData(buffList[i]);
							if(buffData != null){
								BuffMoudleData moudleData = fileMgr.GetInstance().GetData(buffData.moudleId,CsvType.CSV_TYPE_BUFF) as BuffMoudleData;
								if(moudleData.type == 611)
									return false ;
							}
						}
					}
				}
				
				//1 percent
				//2 num
				//EffectData effectData = creatrue.GetEffectData();
				//effectData.hpPercent += percent ;
				hp += (int)(creatrue.GetFightCreatureData().blood * percent * 0.01f) ;
				if(creatrue.GetEffectData().changeHpPercent != 0){
					hp = (int)(hp * creatrue.GetEffectData().changeHpPercent * 0.01f) ;
				}
				creatrue.SetHp(hp);
				return true ;
			}
			return false ;
		}

		//depend attack
		public bool EffectAttackPrecentHp(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			List<int> buffList = GetCreatrueBuffList(creatrue);
			if(creatrue != null){
				string[] dataList = ob.data.Split('#');
				int hp = int.Parse(dataList[1]);
				int percent = int.Parse(dataList[0]) ;
				
				if(hp > 0 || percent >0){
					if(buffList!= null && buffList.Count != 0){
						for(int i = 0; i<buffList.Count; ++i){
							BuffDataBass buffData = CBuffMgr.GetInstance().GetBuffData(buffList[i]);
							if(buffData != null){
								BuffMoudleData moudleData = fileMgr.GetInstance().GetData(buffData.moudleId,CsvType.CSV_TYPE_BUFF) as BuffMoudleData;
								if(moudleData.type == 601)
									return false ;
							}
						}
					}
				}
				else{
					if(buffList!= null && buffList.Count != 0){
						for(int i = 0; i<buffList.Count; ++i){
							BuffDataBass buffData = CBuffMgr.GetInstance().GetBuffData(buffList[i]);
							if(buffData != null){
								BuffMoudleData moudleData = fileMgr.GetInstance().GetData(buffData.moudleId,CsvType.CSV_TYPE_BUFF) as BuffMoudleData;
								if(moudleData.type == 611)
									return false ;
							}
						}
					}
				}
				
				//1 percent
				//2 num
				int percentHp = (int)(creatrue.GetFightCreatureData().attack + creatrue.GetEffectData().attack + creatrue.GetFightCreatureData().attack * creatrue.GetEffectData().attackPrecent * 0.01f) ;
				percentHp = (int)(percentHp * percent * 0.01f) ;
				if(creatrue.GetEffectData().changeHpPercent != 0){
					hp = (int)(hp * creatrue.GetEffectData().changeHpPercent * 0.01f) ;
				}
				creatrue.SetHp(hp + percentHp);
				return true ;
			}
			return false ;
		}

		//depand max hp
		public bool EffectMaxHpPercent(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				string[] dataList = ob.data.Split('#');
				int hp = creatrue.GetFightCreatureData().maxBlood + creatrue.GetEffectData().hpMax ;
				hp = (int)(hp * int.Parse(dataList[0]) * 0.01f) ;
				hp += int.Parse(dataList[1]) ;
				creatrue.SetHp(hp);
				//creatrue.GetEffectData().changeHpPercent += int.Parse(dataList[0]);
				return true ;
			}
			return false ;
		}

		//change hp
		public bool EffectChangeHpPercent(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeHpPercent += int.Parse(dataList[0]);
				return true ;
			}
			return false ;
		}

		public bool EffectChangeHpPercentEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeHpPercent -= int.Parse(dataList[0]);
				return true ;
			}
			return false ;
		}

		//change add hp
		public bool EffectChangeAddHpPercent(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeAddHpPercent += int.Parse(dataList[0]);
				return true ;
			}
			return false ;
		}
		
		public bool EffectChangeAddHpPercentEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeAddHpPercent -= int.Parse(dataList[0]);
				return true ;
			}
			return false ;
		}

		//cut cd time
		public bool EffectCutCdTime(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().cutCoolDownTime += int.Parse(dataList[0]);
				return true ;
			}
			return false ;
		}

		public bool EffectCutCdTimeEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().cutCoolDownTime -= int.Parse(dataList[0]);
				return true ;
			}
			return false ;
		}

		//my hp
		public bool	EffectMyHp(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.scrID) ;
			List<int> buffList = GetCreatrueBuffList(creatrue);
			if(creatrue != null){
				string[] dataList = ob.data.Split('#');
				int hp = int.Parse(dataList[1]);
				int percent = int.Parse(dataList[0]) ;
				
				if(hp < 0 || percent <0){
					if(buffList!= null && buffList.Count != 0){
						for(int i = 0; i<buffList.Count; ++i){
							BuffDataBass buffData = CBuffMgr.GetInstance().GetBuffData(buffList[i]);
							if(buffData != null){
								BuffMoudleData moudleData = fileMgr.GetInstance().GetData(buffData.moudleId,CsvType.CSV_TYPE_BUFF) as BuffMoudleData;
								if(moudleData.type == 611)
									return false ;
							}
						}
					}
				}
				
				//1 percent
				//2 num
				EffectData effectData = creatrue.GetEffectData();
				effectData.mpPercent += percent ;
				creatrue.SetHp(hp);
				return true ;
			}
			return false ;
		}
		
		/*public bool	EffectHpPercent(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				EffectData effectData = creatrue.GetEffectData();
				string[] dataList = ob.data.Split('#');
				effectData.hpPercent += int.Parse(dataList[0]) ;
			}
		}*/
		
		//hp max
		public bool EffectHpMax(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				EffectData effectData = creatrue.GetEffectData();
				string[] dataList = ob.data.Split('#');
				effectData.hpMax += int.Parse(dataList[0]) ;
				creatrue.SetHp(int.Parse(dataList[0]));
				return true ;
			}
			return false ;
		}
		
		public bool EffectHpMaxEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				EffectData effectData = creatrue.GetEffectData();
				string[] dataList = ob.data.Split('#');
				effectData.hpMax -= int.Parse(dataList[0]) ;
				return true ;
			}
			return false ;
		}
		
		public bool EffectHpMaxPercent(EffectBassData ob){
			return false ;
		}
		
		public bool EffectHpMaxPercentEnd(EffectBassData ob){
			return false ;
		}
		
		
		//speed
		public bool	EffectSpeed(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			
			if(creatrue != null){
				string[] dataList = ob.data.Split('#');
				int speed = int.Parse(dataList[0]) ;
				if(speed < 0){
					List<int> buffList = GetCreatrueBuffList(creatrue) ;
					if(buffList!= null && buffList.Count != 0){
						for(int i = 0; i<buffList.Count; ++i){
							BuffDataBass buffData = CBuffMgr.GetInstance().GetBuffData(buffList[i]);
							if(buffData != null){
								BuffMoudleData moudleData = fileMgr.GetInstance().GetData(buffData.moudleId,CsvType.CSV_TYPE_BUFF) as BuffMoudleData;
								if(moudleData.type == 611)
									return false ;
							}

						}
					}
				}
				
				
				EffectData effectData = creatrue.GetEffectData();
				effectData.speed += speed ;
				return true ;
			}
			return false ;
		}
		
		public bool	EffectSpeedEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				EffectData effectData = creatrue.GetEffectData();
				string[] dataList = ob.data.Split('#');
				effectData.speed -= int.Parse(dataList[0]) ;
			}
			return true ;
		}

		//speed
		public bool	EffectRealHurt(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			
			if(creatrue != null){
				string[] dataList = ob.data.Split('#');
				
				EffectData effectData = creatrue.GetEffectData();
				effectData.realHurt = true ;
				return true ;
			}
			return false ;
		}
		
		public bool	EffectRealHurtEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				EffectData effectData = creatrue.GetEffectData();
				string[] dataList = ob.data.Split('#');
				effectData.realHurt = false ;
			}
			return true ;
		}

		
		public bool	EffectSpeedPercent(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				EffectData effectData = creatrue.GetEffectData();
				string[] dataList = ob.data.Split('#');
				
				int speed = int.Parse(dataList[0]) ;
				if(speed < 0){
					List<int> buffList = GetCreatrueBuffList(creatrue) ;
					if(buffList!= null && buffList.Count != 0){
						for(int i = 0; i<buffList.Count; ++i){
							BuffDataBass buffData = CBuffMgr.GetInstance().GetBuffData(buffList[i]);
							if(buffData != null){
								BuffMoudleData moudleData = fileMgr.GetInstance().GetData(buffData.moudleId,CsvType.CSV_TYPE_BUFF) as BuffMoudleData;
								if(moudleData.type == 611)
									return false ;
							}

						}
					}
				}
				
				effectData.speedPercent += speed ;
				return true ;
			}
			return false ;
		}
		
		public bool	EffectSpeedPercentEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				EffectData effectData = creatrue.GetEffectData();
				string[] dataList = ob.data.Split('#');
				effectData.speedPercent -= int.Parse(dataList[0]) ;
			}
			return true ;
		}
		
		//mp
		public bool EffectMp(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				EffectData effectData = creatrue.GetEffectData();
				string[] dataList = ob.data.Split('#');
				
				int mp = int.Parse(dataList[0]) ;
				if(mp < 0){
					List<int> buffList = GetCreatrueBuffList(creatrue) ;
					if(buffList!= null && buffList.Count != 0){
						for(int i = 0; i<buffList.Count; ++i){
							BuffDataBass buffData = CBuffMgr.GetInstance().GetBuffData(buffList[i]);
							if(buffData != null){
								BuffMoudleData moudleData = fileMgr.GetInstance().GetData(buffData.moudleId,CsvType.CSV_TYPE_BUFF) as BuffMoudleData;
								if(moudleData.type == 611)
									return false ;
							}

						}
					}
				}
				
				effectData.mp += mp ;
				return true ;
			}
			return false ;
		}
		
		public bool EffectMpPercent(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				EffectData effectData = creatrue.GetEffectData();
				string[] dataList = ob.data.Split('#');
				
				int mp = int.Parse(dataList[0]) ;
				if(mp < 0){
					List<int> buffList = GetCreatrueBuffList(creatrue) ;
					if(buffList!= null && buffList.Count != 0){
						for(int i = 0; i<buffList.Count; ++i){
							BuffDataBass buffData = CBuffMgr.GetInstance().GetBuffData(buffList[i]);
							if(buffData != null){
								BuffMoudleData moudleData = fileMgr.GetInstance().GetData(buffData.moudleId,CsvType.CSV_TYPE_BUFF) as BuffMoudleData;
								if(moudleData.type == 611)
									return false ;
							}

						}
					}
				}
				
				effectData.mpPercent += mp ;
				return true ;
			}
			return false ;
		}
		

		
		public bool EffectMpMax(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				EffectData effectData = creatrue.GetEffectData();
				string[] dataList = ob.data.Split('#');
				
				int mp = int.Parse(dataList[0]) ;
				if(mp < 0){
					List<int> buffList = GetCreatrueBuffList(creatrue) ;
					if(buffList!= null && buffList.Count != 0){
						for(int i = 0; i<buffList.Count; ++i){
							BuffDataBass buffData = CBuffMgr.GetInstance().GetBuffData(buffList[i]);
							if(buffData != null){
								BuffMoudleData moudleData = fileMgr.GetInstance().GetData(buffData.moudleId,CsvType.CSV_TYPE_BUFF) as BuffMoudleData;
								if(moudleData.type == 611)
									return false ;
							}

						}
					}
				}
				
				effectData.mp += mp ;
				effectData.mpMax += mp ;
				return true ;
			}
			return false ;
		}
		
		public bool EffectMpMaxEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				EffectData effectData = creatrue.GetEffectData();
				string[] dataList = ob.data.Split('#');
				effectData.mpMax -= int.Parse(dataList[0]) ;
			}
			return true ;
		}
		
		//attack
		public bool EffectAttack(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				EffectData effectData = creatrue.GetEffectData();
				string[] dataList = ob.data.Split('#');
				
				int attack = int.Parse(dataList[0]) ;
				if(attack < 0){
					List<int> buffList = GetCreatrueBuffList(creatrue) ;
					if(buffList!= null && buffList.Count != 0){
						for(int i = 0; i<buffList.Count; ++i){
							BuffDataBass buffData = CBuffMgr.GetInstance().GetBuffData(buffList[i]);
							if(buffData != null){
								BuffMoudleData moudleData = fileMgr.GetInstance().GetData(buffData.moudleId,CsvType.CSV_TYPE_BUFF) as BuffMoudleData;
								if(moudleData.type == 611)
									return false ;
							}

						}
					}
				}
				
				effectData.attack += attack ;
				return true ;
			}
			return false ;
		}
		
		public bool EffectAttackEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				EffectData effectData = creatrue.GetEffectData();
				string[] dataList = ob.data.Split('#');
				effectData.attack -= int.Parse(dataList[0]) ;
			}
			return true ;
		}
		
		public bool EffectAttackPrecent(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				EffectData effectData = creatrue.GetEffectData();
				string[] dataList = ob.data.Split('#');
				
				int attackPercent = int.Parse(dataList[0]) ;
				//int attackPercent = int.Parse(dataList[1]);
				if(attackPercent < 0){
					List<int> buffList = GetCreatrueBuffList(creatrue) ;
					if(buffList!= null && buffList.Count != 0){
						for(int i = 0; i<buffList.Count; ++i){
							BuffDataBass buffData = CBuffMgr.GetInstance().GetBuffData(buffList[i]);
							if(buffData != null){
								BuffMoudleData moudleData = fileMgr.GetInstance().GetData(buffData.moudleId,CsvType.CSV_TYPE_BUFF) as BuffMoudleData;
								if(moudleData.type == 611)
									return false ;
							}
							
						}
					}
				}
				
				//effectData.attack += attack ;
				effectData.attackPrecent += attackPercent ;
				return true ;
			}
			return false ;
		}
		
		public bool EffectAttackPrecentEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				EffectData effectData = creatrue.GetEffectData();
				string[] dataList = ob.data.Split('#');
				//effectData.attack -= int.Parse(dataList[0]) ;
				effectData.attackPrecent -=  int.Parse(dataList[0]) ;
			}
			return true ;
		}

		//crit
		public bool EffectCrit(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				EffectData effectData = creatrue.GetEffectData();
				string[] dataList = ob.data.Split('#');
				int crit = int.Parse(dataList[0]) ;
				if(crit < 0){
					List<int> buffList = GetCreatrueBuffList(creatrue) ;
					if(buffList!= null && buffList.Count != 0){
						for(int i = 0; i<buffList.Count; ++i){
							BuffDataBass buffData = CBuffMgr.GetInstance().GetBuffData(buffList[i]);
							if(buffData != null){
								BuffMoudleData moudleData = fileMgr.GetInstance().GetData(buffData.moudleId,CsvType.CSV_TYPE_BUFF) as BuffMoudleData;
								if(moudleData.type == 611)
									return false ;
							}

						}
					}
				}
				
				effectData.crit += crit ;
				return true ;
			}
			return false ;
		}
		
		public bool EffectCritEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				EffectData effectData = creatrue.GetEffectData();
				string[] dataList = ob.data.Split('#');
				effectData.crit -= int.Parse(dataList[0]) ;
			}
			return true ;
		}
		
		public bool EffectCritPercent(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				
				EffectData effectData = creatrue.GetEffectData();
				string[] dataList = ob.data.Split('#');
				int crit = int.Parse(dataList[0]) ;
				if(crit < 0){
					List<int> buffList = GetCreatrueBuffList(creatrue) ;
					if(buffList!= null && buffList.Count != 0){
						for(int i = 0; i<buffList.Count; ++i){
							BuffDataBass buffData = CBuffMgr.GetInstance().GetBuffData(buffList[i]);
							if(buffData != null){
								BuffMoudleData moudleData = fileMgr.GetInstance().GetData(buffData.moudleId,CsvType.CSV_TYPE_BUFF) as BuffMoudleData;
								if(moudleData.type == 611)
									return false ;
							}

						}
					}
				}
				effectData.critPrecent += crit ;
				return true ;
			}
			return false ;
		}
		
		public bool EffectCritPercentEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				EffectData effectData = creatrue.GetEffectData();
				string[] dataList = ob.data.Split('#');
				effectData.critPrecent -= int.Parse(dataList[0]) ;
			}
			return true ;
		}
		
		//duck
		public bool EffectDuck(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				EffectData effectData = creatrue.GetEffectData();
				string[] dataList = ob.data.Split('#');
				int duck = int.Parse(dataList[0]) ;
				if(duck < 0){
					List<int> buffList = GetCreatrueBuffList(creatrue) ;
					if(buffList!= null && buffList.Count != 0){
						for(int i = 0; i<buffList.Count; ++i){
							BuffDataBass buffData = CBuffMgr.GetInstance().GetBuffData(buffList[i]);
							if(buffData != null){
								BuffMoudleData moudleData = fileMgr.GetInstance().GetData(buffData.moudleId,CsvType.CSV_TYPE_BUFF) as BuffMoudleData;
								if(moudleData.type == 611)
									return false ;
							}

						}
					}
				}
				effectData.duck += duck ;
				return true ;
			}
			return false ;
		}
		
		public bool EffectDuckEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				EffectData effectData = creatrue.GetEffectData();
				string[] dataList = ob.data.Split('#');
				effectData.duck -= int.Parse(dataList[0]) ;
			}
			return true ;
		}
		
		public bool EffectDuckPrecent(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				EffectData effectData = creatrue.GetEffectData();
				string[] dataList = ob.data.Split('#');
				int duck = int.Parse(dataList[0]) ;
				if(duck < 0){
					List<int> buffList = GetCreatrueBuffList(creatrue) ;
					if(buffList!= null && buffList.Count != 0){
						for(int i = 0; i<buffList.Count; ++i){
							BuffDataBass buffData = CBuffMgr.GetInstance().GetBuffData(buffList[i]);
							if(buffData != null){
								BuffMoudleData moudleData = fileMgr.GetInstance().GetData(buffData.moudleId,CsvType.CSV_TYPE_BUFF) as BuffMoudleData;
								if(moudleData.type == 611)
									return false ;
							}

						}
					}
				}
				effectData.duckPrecent += duck ;
				return true ;
			}
			return false ;
		}
		
		public bool EffectDuckPrecentEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				EffectData effectData = creatrue.GetEffectData();
				string[] dataList = ob.data.Split('#');
				effectData.duckPrecent -= int.Parse(dataList[0]) ;
			}
			return true ;
		}
		
		//suck blood
		public bool EffectSuckBlood(EffectBassData ob){
			CCreature creatureDest = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			CCreature creatrueScr = EnitityMgr.GetInstance().GetEnitity(ob.scrID) ;
			if(creatureDest != null && creatrueScr != null){
				List<int> buffList = null ;
				if(buffList!= null && buffList.Count != 0){
					for(int i = 0; i<buffList.Count; ++i){
						BuffDataBass buffData = CBuffMgr.GetInstance().GetBuffData(buffList[i]);
						if(buffData != null){
							BuffMoudleData moudleData = fileMgr.GetInstance().GetData(buffData.moudleId,CsvType.CSV_TYPE_BUFF) as BuffMoudleData;
							if(moudleData.type == 611)
								return false ;
						}
						
					}
				}
				
				float bassAttack = 0 ;
				int  random = Random.Range(0,100);
				if(random < creatureDest.GetEffectData().oneHurt)
				{
					bassAttack = 1 ;
					creatureDest.SetHp((int)-bassAttack);
					return true ;
				}

				//1.hurt
				string[] dataList = ob.data.Split('#');
				bassAttack += int.Parse(dataList[0]);
				creatureDest.SetHp((int)-bassAttack);
				creatrueScr.SetHp((int)bassAttack);
			}
			return true ;
		}

		public bool EffectSuckBloodPrecent(EffectBassData ob){
			CCreature creatrueScr = EnitityMgr.GetInstance().GetEnitity(ob.scrID) ;
			CCreature creatureDest = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatureDest != null){
				List<int> buffList = null ;
				if(buffList!= null && buffList.Count != 0){
					for(int i = 0; i<buffList.Count; ++i){
						BuffDataBass buffData = CBuffMgr.GetInstance().GetBuffData(buffList[i]);
						if(buffData != null){
							BuffMoudleData moudleData = fileMgr.GetInstance().GetData(buffData.moudleId,CsvType.CSV_TYPE_BUFF) as BuffMoudleData;
							if(moudleData.type == 611)
								return false ;
						}
						
					}
				}
				
				float bassAttack = 0 ;
				int  random = Random.Range(0,100);
				if(random < creatureDest.GetEffectData().oneHurt)
				{
					bassAttack = 1 ;
					creatureDest.SetHp((int)-bassAttack);
					return true ;
				}
				
				//0:percent
				//1:num
				//2:suck blood precent
				string[] dataList = ob.data.Split('#');
				
				bassAttack = (creatrueScr.GetFightCreatureData().attack + creatrueScr.GetEffectData().attack) * (1.0f + creatrueScr.GetEffectData().attackPrecent * 0.01f);
				
				bassAttack =  (int)(bassAttack * int.Parse(dataList[0]) / 100.0f) ;
				bassAttack += int.Parse(dataList[1]) ;
				
				//element
				/*if(dataList[2] != null && dataList[2] != ""){
					if(dataList[2] == "3"){
						bassAttack += (int)((int.Parse(dataList[1]) + bassAttack) * (creatrueScr.GetEffectData().earth - creatureDest.GetEffectData().earth)/1000.0f + Random.Range(0,5));
					}
					else if(dataList[2] == "1"){
						bassAttack += (int)((int.Parse(dataList[1]) + bassAttack) * (creatrueScr.GetEffectData().ice - creatureDest.GetEffectData().ice)/1000.0f + Random.Range(0,5));
					}
					else if(dataList[2] == "2"){
						bassAttack += (int)((int.Parse(dataList[1]) + bassAttack) * (creatrueScr.GetEffectData().fire - creatureDest.GetEffectData().fire)/1000.0f + Random.Range(0,5));
					}
					else if(dataList[2] == "6"){
						bassAttack += (int)((int.Parse(dataList[1]) + bassAttack) * (creatrueScr.GetEffectData().wind - creatureDest.GetEffectData().wind)/1000.0f + Random.Range(0,5));
					}
					else if(dataList[2] == "5"){
						bassAttack += (int)((int.Parse(dataList[1]) + bassAttack) * (creatrueScr.GetEffectData().light - creatureDest.GetEffectData().light)/1000.0f + Random.Range(0,5));
					}
					else if(dataList[2] == "4"){
						bassAttack += (int)((int.Parse(dataList[1]) + bassAttack) * (creatrueScr.GetEffectData().thunder - creatureDest.GetEffectData().thunder)/1000.0f + Random.Range(0,5));
					}
				}*/
				
				
				
				//wave hurt
				if(creatureDest.GetEffectData().waveHurtPercentMin != 0){
					random = Random.Range(creatureDest.GetEffectData().waveHurtPercentMin,creatureDest.GetEffectData().waveHurtPercentMax);
					bassAttack = (int)(bassAttack * (random/100.0f)) ;
				}
				
				//change hurt
				bassAttack += creatrueScr.GetEffectData().changeAttack + creatureDest.GetEffectData().changeHurt ;
				if(bassAttack < 0)
					bassAttack = 0 ;
				float changePercent = (float)(creatrueScr.GetEffectData().changeAttackPercent + creatureDest.GetEffectData().changeHurtPercent)/ 100.0f ;
				if(changePercent != 0){
					bassAttack += (int)(bassAttack * changePercent)  ;
					if(bassAttack < 0)
						bassAttack = 0 ;
				}
				
				//change skill hurt
				bassAttack += creatrueScr.GetEffectData().changeSkillAttack - creatureDest.GetEffectData().changeSkillHurt ;
				float changeSkillPercent = (float)(creatrueScr.GetEffectData().changeSkillHurtPercent - creatureDest.GetEffectData().changeSkillAttackPercent)/ 100.0f ;
				if(changeSkillPercent != 0){
					bassAttack = (int)(bassAttack * changeSkillPercent)  ;
					if(bassAttack < 0)
						bassAttack = 0 ;
				}
				
				//change element
				/*if(dataList[2] != null && dataList[2] != ""){
					if(dataList[2] == "3"){
						bassAttack += creatrueScr.GetEffectData().changeEarthHurt - creatureDest.GetEffectData().changeEarthHurt ;
						changePercent = (float)((creatrueScr.GetEffectData().changeEarthHurtPercent - creatureDest.GetEffectData().changeEarthHurtPercent)/ 100.0f) ;
						if(changePercent != 0){
							bassAttack = (int)(bassAttack * changePercent)  ;
						}
					}
					else if(dataList[2] == "1"){
						bassAttack += creatrueScr.GetEffectData().changeIceHurt - creatureDest.GetEffectData().changeIceAttack ;
						changePercent = (float)((creatrueScr.GetEffectData().changeIceHurtPercent - creatureDest.GetEffectData().changeIceAttackPercent)/ 100.0f) ;
						if(changePercent != 0){
							bassAttack = (int)(bassAttack * changePercent)  ;
						}
					}
					else if(dataList[2] == "2"){
						bassAttack += creatrueScr.GetEffectData().changeFireHurt - creatureDest.GetEffectData().changeFireAttack ;
						changePercent = (float)((creatrueScr.GetEffectData().changeFireHurtPercent - creatureDest.GetEffectData().changeFireAttackPercent)/ 100.0f) ;
						if(changePercent != 0){
							bassAttack = (int)(bassAttack * changePercent)  ;
						}
					}
					else if(dataList[2] == "6"){
						bassAttack += creatrueScr.GetEffectData().changeWindHurt - creatureDest.GetEffectData().changeWindAttack ;
						changePercent = (float)((creatrueScr.GetEffectData().changeWindHurtPercent - creatureDest.GetEffectData().changeWindAttackPercent)/ 100.0f) ;
						if(changePercent != 0){
							bassAttack = (int)(bassAttack * changePercent)  ;
						}
					}
					else if(dataList[2] == "5"){
						bassAttack += creatrueScr.GetEffectData().changeLightHurt - creatureDest.GetEffectData().changeLightAttack ;
						changePercent = (float)((creatrueScr.GetEffectData().changeLightHurtPercent - creatureDest.GetEffectData().changeLightAttackPercent)/ 100.0f) ;
						if(changePercent != 0){
							bassAttack = (int)(bassAttack * changePercent)  ;
						}
					}
					else if(dataList[2] == "4"){
						bassAttack += creatrueScr.GetEffectData().changeThunderHurt - creatureDest.GetEffectData().changeThunderAttack ;
						changePercent = (float)((creatrueScr.GetEffectData().changeThunderHurtPercent - creatureDest.GetEffectData().changeThunderAttackPercent)/ 100.0f) ;
						if(changePercent != 0){
							bassAttack = (int)(bassAttack * changePercent)  ;
						}
					}
					
					if(bassAttack < 0)
						bassAttack = 0 ;
				}*/
				
				
				
				
				//camp
				//ri
				int scrCamp = creatrueScr.GetFightCreatureData().camp ;
				int destCamp= creatureDest.GetFightCreatureData().camp;
				if(destCamp == 1 ){
					if(scrCamp == 1){
						bassAttack *= 1.0f + (creatrueScr.GetEffectData().changeSunAttackPercent + creatureDest.GetEffectData().changeSunHurtPercent) * 0.01f ;
						bassAttack += creatrueScr.GetEffectData().changeSunAttack + creatureDest.GetEffectData().changeSunAttack ;
					}
					else if(scrCamp == 2){
						bassAttack *= FightCommon.BE_REFRAIN_COEFFICIENT + (creatrueScr.GetEffectData().changeSunAttackPercent + creatureDest.GetEffectData().changeMoonHurtPercent) * 0.01f ;
						bassAttack += creatrueScr.GetEffectData().changeSunAttack + creatureDest.GetEffectData().changeMoonHurt ;
					}
					else if(scrCamp == 3){
						bassAttack *= FightCommon.REFRAIN_COEFFICIENT + (creatrueScr.GetEffectData().changeSunAttackPercent + creatureDest.GetEffectData().changeSunHurtPercent) * 0.01f ;
						bassAttack += creatrueScr.GetEffectData().changeSunAttack + creatureDest.GetEffectData().changeSunHurt ;
					}
				}
				//yue
				else if(destCamp == 2){
					if(scrCamp == 1 ){
						bassAttack *= FightCommon.REFRAIN_COEFFICIENT + (creatrueScr.GetEffectData().changeMoonAttackPercent + creatureDest.GetEffectData().changeSunHurtPercent) * 0.01f ;
						bassAttack += creatrueScr.GetEffectData().changeMoonAttack + creatureDest.GetEffectData().changeSunAttack ;
					}
					else if(scrCamp == 2){
						bassAttack *= 1.0f + (creatrueScr.GetEffectData().changeMoonAttackPercent + creatureDest.GetEffectData().changeMoonHurtPercent) * 0.01f ;
						bassAttack += creatrueScr.GetEffectData().changeMoonAttack + creatureDest.GetEffectData().changeMoonHurt ;
					}
					else if(scrCamp == 3){
						bassAttack *= FightCommon.BE_REFRAIN_COEFFICIENT + (creatrueScr.GetEffectData().changeMoonAttackPercent + creatureDest.GetEffectData().changeSunHurtPercent) * 0.01f ;
						bassAttack += creatrueScr.GetEffectData().changeMoonAttack + creatureDest.GetEffectData().changeSunHurt ;
					}
				}
				//xin
				else if(destCamp == 3){
					if(scrCamp == 1){
						bassAttack *= FightCommon.BE_REFRAIN_COEFFICIENT + (creatrueScr.GetEffectData().changeStarAttackPercent + creatureDest.GetEffectData().changeSunHurtPercent) * 0.01f ;
						bassAttack += creatrueScr.GetEffectData().changeStarAttack + creatureDest.GetEffectData().changeSunAttack ;
					}
					else if(scrCamp == 2){
						bassAttack *= FightCommon.REFRAIN_COEFFICIENT + (creatrueScr.GetEffectData().changeStarAttackPercent + creatureDest.GetEffectData().changeMoonHurtPercent) * 0.01f ;
						bassAttack += creatrueScr.GetEffectData().changeStarAttack + creatureDest.GetEffectData().changeMoonHurt ;
					}
					else if(scrCamp == 3){
						bassAttack *= 1.0f + (creatrueScr.GetEffectData().changeStarAttackPercent + creatureDest.GetEffectData().changeSunHurtPercent) * 0.01f ;
						bassAttack += creatrueScr.GetEffectData().changeStarAttack + creatureDest.GetEffectData().changeSunHurt ;
					}
				}
				
				//double hurt
				random = Random.Range(0,1000);
				int crit = creatrueScr.GetFightCreatureData().crit + creatrueScr.GetEffectData().crit ;
				crit += (int)(creatrueScr.GetFightCreatureData().crit + creatrueScr.GetEffectData().critPrecent / 1000.0f );
				if(random <=  crit){
					bassAttack *= 2 ;
					bassAttack *= 1.0f + creatrueScr.GetEffectData().critHurtPercent * 0.01f ;
					bassAttack += creatrueScr.GetEffectData().critHurt ;
					creatureDest.SetCrit(true);
				}
				
				//element
				/*if(dataList[2] != null && dataList[2] != ""){
					if(dataList[2] == "3"){
						bassAttack += (int)((int.Parse(dataList[1]) + bassAttack) * (creatrueScr.GetEffectData().earth - creatureDest.GetEffectData().earth)/1000.0f + Random.Range(0,5));
					}
					else if(dataList[2] == "1"){
						bassAttack += (int)((int.Parse(dataList[1]) + bassAttack) * (creatrueScr.GetEffectData().ice - creatureDest.GetEffectData().ice)/1000.0f + Random.Range(0,5));
					}
					else if(dataList[2] == "2"){
						bassAttack += (int)((int.Parse(dataList[1]) + bassAttack) * (creatrueScr.GetEffectData().fire - creatureDest.GetEffectData().fire)/1000.0f + Random.Range(0,5));
					}
					else if(dataList[2] == "6"){
						bassAttack += (int)((int.Parse(dataList[1]) + bassAttack) * (creatrueScr.GetEffectData().wind - creatureDest.GetEffectData().wind)/1000.0f + Random.Range(0,5));
					}
					else if(dataList[2] == "5"){
						bassAttack += (int)((int.Parse(dataList[1]) + bassAttack) * (creatrueScr.GetEffectData().light - creatureDest.GetEffectData().light)/1000.0f + Random.Range(0,5));
					}
					else if(dataList[2] == "4"){
						bassAttack += (int)((int.Parse(dataList[1]) + bassAttack) * (creatrueScr.GetEffectData().thunder - creatureDest.GetEffectData().thunder)/1000.0f + Random.Range(0,5));
					}
				}*/
				
				
				
				//wave hurt
				/*if(creatureDest.GetEffectData().waveHurtPercentMin != 0){
					random = Random.Range(creatureDest.GetEffectData().waveHurtPercentMin,creatureDest.GetEffectData().waveHurtPercentMax);
					bassAttack = (int)(bassAttack * (random/100.0f)) ;
				}
				
				//change hurt
				bassAttack += creatrueScr.GetEffectData().changeAttack - creatureDest.GetEffectData().changeHurt ;
				float changePercent = (float)(creatrueScr.GetEffectData().changeHurtPercent - creatureDest.GetEffectData().changeAttackPercent)/ 100.0f ;
				if(changePercent != 0){
					bassAttack = (int)(bassAttack * changePercent)  ;
					if(bassAttack < 0)
						bassAttack = 0 ;
				}
				
				//change element
				if(dataList[2] != null && dataList[2] != ""){
					if(dataList[2] == "3"){
						bassAttack += creatrueScr.GetEffectData().changeEarthHurt - creatureDest.GetEffectData().changeEarthHurt ;
						changePercent = (float)((creatrueScr.GetEffectData().changeEarthHurtPercent - creatureDest.GetEffectData().changeEarthHurtPercent)/ 100.0f) ;
						if(changePercent != 0){
							bassAttack = (int)(bassAttack * changePercent)  ;
						}
					}
					else if(dataList[2] == "1"){
						bassAttack += creatrueScr.GetEffectData().changeIceHurt - creatureDest.GetEffectData().changeIceAttack ;
						changePercent = (float)((creatrueScr.GetEffectData().changeIceHurtPercent - creatureDest.GetEffectData().changeIceAttackPercent)/ 100.0f) ;
						if(changePercent != 0){
							bassAttack = (int)(bassAttack * changePercent)  ;
						}
					}
					else if(dataList[2] == "2"){
						bassAttack += creatrueScr.GetEffectData().changeFireHurt - creatureDest.GetEffectData().changeFireAttack ;
						changePercent = (float)((creatrueScr.GetEffectData().changeFireHurtPercent - creatureDest.GetEffectData().changeFireAttackPercent)/ 100.0f) ;
						if(changePercent != 0){
							bassAttack = (int)(bassAttack * changePercent)  ;
						}
					}
					else if(dataList[2] == "6"){
						bassAttack += creatrueScr.GetEffectData().changeWindHurt - creatureDest.GetEffectData().changeWindAttack ;
						changePercent = (float)((creatrueScr.GetEffectData().changeWindHurtPercent - creatureDest.GetEffectData().changeWindAttackPercent)/ 100.0f) ;
						if(changePercent != 0){
							bassAttack = (int)(bassAttack * changePercent)  ;
						}
					}
					else if(dataList[2] == "5"){
						bassAttack += creatrueScr.GetEffectData().changeLightHurt - creatureDest.GetEffectData().changeLightAttack ;
						changePercent = (float)((creatrueScr.GetEffectData().changeLightHurtPercent - creatureDest.GetEffectData().changeLightAttackPercent)/ 100.0f) ;
						if(changePercent != 0){
							bassAttack = (int)(bassAttack * changePercent)  ;
						}
					}
					else if(dataList[2] == "4"){
						bassAttack += creatrueScr.GetEffectData().changeThunderHurt - creatureDest.GetEffectData().changeThunderAttack ;
						changePercent = (float)((creatrueScr.GetEffectData().changeThunderHurtPercent - creatureDest.GetEffectData().changeThunderAttackPercent)/ 100.0f) ;
						if(changePercent != 0){
							bassAttack = (int)(bassAttack * changePercent)  ;
						}
					}
					
					if(bassAttack < 0)
						bassAttack = 0 ;
				}
				
				
				
				//double hurt
				random = Random.Range(0,1000);
				int crit = creatrueScr.GetFightCreatureData().crit + creatrueScr.GetEffectData().crit ;
				crit += (int)(creatrueScr.GetFightCreatureData().crit + creatrueScr.GetEffectData().critPrecent / 1000.0f );
				if(random <=  crit){
					bassAttack *= 2 ;
					creatureDest.SetCrit(true);
				}*/

				//bassAttack += int.Parse(dataList[1]) ;
				//bassAttack += bassAttack * int.Parse(dataList[0]) * 0.0f ;
				creatureDest.SetHp((int)-bassAttack);
				float blood  = bassAttack * int.Parse(dataList[2]) * 0.01f ;
				creatrueScr.SetHp((int)blood);
				return true ;
			}
			return false ;
		}
		
		
		//rebound attack
		public bool EffectReboundAttack(EffectBassData ob){
			CCreature destCreature = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.scrID) ;
			if(destCreature.GetEnitityType() == creatrue.GetEnitityType())
				return true ;
			if(creatrue != null){
				if(creatrue.GetFightCreatureData().attackArea < 20 && creatrue.GetEffectData().resistanceList.Contains(209) == false){
					string[] dataList = ob.data.Split('#');
					creatrue.SetHp(-int.Parse(dataList[0]));
				}

			}
			return true ;
		}
		
		public bool EffectReboundAttackPrecent(EffectBassData ob){
			CCreature destCreature = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.scrID) ;
			if(destCreature.GetEnitityType() == creatrue.GetEnitityType())
				return true ;
			if(creatrue != null){
				//0:attack
				//1:precent
				if(creatrue.GetFightCreatureData().attackArea < 20 && creatrue.GetEffectData().resistanceList.Contains(209) == false){
					string[] dataList = ob.data.Split('#');
					int attack = int.Parse(dataList[2]) ;
					attack = (int)(attack + attack * creatrue.GetEffectData().attackPrecent * 0.01f) ;
					int blood  = (int)(attack  * int.Parse(dataList[0]) * 0.01f) ;
					creatrue.SetHp(-blood);
				}

			}
			return true ;
		}

		//rebound attack far
		public bool EffectReboundFarAttack(EffectBassData ob){
			CCreature destCreature = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.scrID) ;
			if(destCreature.GetEnitityType() == creatrue.GetEnitityType())
				return true ;
			if(creatrue != null){
				if(creatrue.GetFightCreatureData().attackArea > 20 && creatrue.GetEffectData().resistanceList.Contains(209) == false){
					string[] dataList = ob.data.Split('#');
					creatrue.SetHp(-int.Parse(dataList[0]));
				}
			}
			return true ;
		}
		
		public bool EffectReboundFarAttackPrecent(EffectBassData ob){
			CCreature destCreature = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.scrID) ;
			if(destCreature.GetEnitityType() == creatrue.GetEnitityType())
				return true ;
			if(creatrue != null){
				//0:attack
				//1:precent
				if(creatrue.GetFightCreatureData().attackArea > 20 && creatrue.GetEffectData().resistanceList.Contains(209) == false){
					string[] dataList = ob.data.Split('#');
					int attack = int.Parse(dataList[2]) ;
					attack = (int)(attack + attack * creatrue.GetEffectData().attackPrecent * 0.01f) ;
					int blood  = (int)(attack  * int.Parse(dataList[0]) * 0.01f) ;
					creatrue.SetHp(-blood);
				}

			}
			return true ;
		}
		
		//sudden death
		public bool EffectSuddenDeath(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				if(creatrue.GetEnitityType() == EnitityType.ENITITY_TYPE_MONSTER){
					CMonster monster = (CMonster)creatrue ;
					MonsterMoudleData moudleData = fileMgr.GetInstance().GetData(monster.m_data.moudleID,CsvType.CSV_TYPE_MONSTER) as MonsterMoudleData ;
					if(moudleData.strength >= 3)
						return false;
				}

				creatrue.SetHp(-99999999);
			}
			return true ;
		}
		
		
		//wind
		public bool EffectWindElement(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().wind += int.Parse(dataList[0]);
			}
			return true ;
		}
		
		//fire
		public bool EffectFireElement(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().fire += int.Parse(dataList[0]);
			}
			return true ;
		}
		
		//dark
		public bool EffectDarkElement(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().thunder += int.Parse(dataList[0]);
			}
			return true ;
		}
		
		//ice
		public bool EffectIceElement(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().ice += int.Parse(dataList[0]);
			}
			return true ;
		}
		
		//light
		public bool EffectLightElement(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().light += int.Parse(dataList[0]);
			}
			return true ;
		}
		
		//earth
		public bool EffectEarthElement(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().earth += int.Parse(dataList[0]);
			}
			return true ;
		}
		
		//hurt
		public bool EffectHurt(EffectBassData ob){
			CCreature creatrueScr = EnitityMgr.GetInstance().GetEnitity(ob.scrID) ;
			CCreature creatureDest = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatureDest != null && creatrueScr != null){
				if(creatureDest.GetEnitityAiState() == AIState.AI_STATE_DEATH || creatrueScr.GetEnitityAiState() == AIState.AI_STATE_DEATH)
					return false;
				List<int> buffList = null ;
				if(buffList!= null && buffList.Count != 0){
					for(int i = 0; i<buffList.Count; ++i){
						BuffDataBass buffData = CBuffMgr.GetInstance().GetBuffData(buffList[i]);
						if(buffData != null){
							BuffMoudleData moudleData = fileMgr.GetInstance().GetData(buffData.moudleId,CsvType.CSV_TYPE_BUFF) as BuffMoudleData;
							if(moudleData.type == 611)
								return false ;
						}

					}
				}
				
				float bassAttack = 0 ;
				int  random = Random.Range(0,100);
				if(random < creatureDest.GetEffectData().oneHurt)
				{
					bassAttack = 1.0f ;
					creatureDest.SetHp(-(int)bassAttack);
					return true ;
				}

				//can not hurt
				if(creatureDest.GetEffectData().invincibility == true){
					return true;
				}

				//0:percent
				//1:num
				//2:element
				string[] dataList = ob.data.Split('#');
				
				bassAttack = (creatrueScr.GetFightCreatureData().attack + creatrueScr.GetEffectData().attack) * (1.0f + creatrueScr.GetEffectData().attackPrecent * 0.01f);
				
				bassAttack =  (int)(bassAttack * int.Parse(dataList[0]) / 100.0f) ;
				bassAttack += int.Parse(dataList[1]) ;
				
				//element
				/*if(dataList[2] != null && dataList[2] != ""){
					if(dataList[2] == "3"){
						bassAttack += (int)((int.Parse(dataList[1]) + bassAttack) * (creatrueScr.GetEffectData().earth - creatureDest.GetEffectData().earth)/1000.0f + Random.Range(0,5));
					}
					else if(dataList[2] == "1"){
						bassAttack += (int)((int.Parse(dataList[1]) + bassAttack) * (creatrueScr.GetEffectData().ice - creatureDest.GetEffectData().ice)/1000.0f + Random.Range(0,5));
					}
					else if(dataList[2] == "2"){
						bassAttack += (int)((int.Parse(dataList[1]) + bassAttack) * (creatrueScr.GetEffectData().fire - creatureDest.GetEffectData().fire)/1000.0f + Random.Range(0,5));
					}
					else if(dataList[2] == "6"){
						bassAttack += (int)((int.Parse(dataList[1]) + bassAttack) * (creatrueScr.GetEffectData().wind - creatureDest.GetEffectData().wind)/1000.0f + Random.Range(0,5));
					}
					else if(dataList[2] == "5"){
						bassAttack += (int)((int.Parse(dataList[1]) + bassAttack) * (creatrueScr.GetEffectData().light - creatureDest.GetEffectData().light)/1000.0f + Random.Range(0,5));
					}
					else if(dataList[2] == "4"){
						bassAttack += (int)((int.Parse(dataList[1]) + bassAttack) * (creatrueScr.GetEffectData().thunder - creatureDest.GetEffectData().thunder)/1000.0f + Random.Range(0,5));
					}
				}*/

				
				
				//wave hurt
				if(creatureDest.GetEffectData().waveHurtPercentMin != 0){
					random = Random.Range(creatureDest.GetEffectData().waveHurtPercentMin,creatureDest.GetEffectData().waveHurtPercentMax);
					bassAttack = (int)(bassAttack * (random/100.0f)) ;
				}
				
				//change hurt
				bassAttack += creatrueScr.GetEffectData().changeAttack + creatureDest.GetEffectData().changeHurt ;
				if(bassAttack < 0)
					bassAttack = 0 ;
				float changePercent = (float)(creatrueScr.GetEffectData().changeAttackPercent + creatureDest.GetEffectData().changeHurtPercent)/ 100.0f ;
				if(changePercent != 0){
					bassAttack = (int)(bassAttack * changePercent)  ;
					if(bassAttack < 0)
						bassAttack = 0 ;
				}

				//change skill hurt
				bassAttack += creatrueScr.GetEffectData().changeSkillAttack - creatureDest.GetEffectData().changeSkillHurt ;
				float changeSkillPercent = (float)(creatrueScr.GetEffectData().changeSkillHurtPercent - creatureDest.GetEffectData().changeSkillAttackPercent)/ 100.0f ;
				if(changeSkillPercent != 0){
					bassAttack = (int)(bassAttack * changeSkillPercent)  ;
					if(bassAttack < 0)
						bassAttack = 0 ;
				}
				
				//change element
				/*if(dataList[2] != null && dataList[2] != ""){
					if(dataList[2] == "3"){
						bassAttack += creatrueScr.GetEffectData().changeEarthHurt - creatureDest.GetEffectData().changeEarthHurt ;
						changePercent = (float)((creatrueScr.GetEffectData().changeEarthHurtPercent - creatureDest.GetEffectData().changeEarthHurtPercent)/ 100.0f) ;
						if(changePercent != 0){
							bassAttack = (int)(bassAttack * changePercent)  ;
						}
					}
					else if(dataList[2] == "1"){
						bassAttack += creatrueScr.GetEffectData().changeIceHurt - creatureDest.GetEffectData().changeIceAttack ;
						changePercent = (float)((creatrueScr.GetEffectData().changeIceHurtPercent - creatureDest.GetEffectData().changeIceAttackPercent)/ 100.0f) ;
						if(changePercent != 0){
							bassAttack = (int)(bassAttack * changePercent)  ;
						}
					}
					else if(dataList[2] == "2"){
						bassAttack += creatrueScr.GetEffectData().changeFireHurt - creatureDest.GetEffectData().changeFireAttack ;
						changePercent = (float)((creatrueScr.GetEffectData().changeFireHurtPercent - creatureDest.GetEffectData().changeFireAttackPercent)/ 100.0f) ;
						if(changePercent != 0){
							bassAttack = (int)(bassAttack * changePercent)  ;
						}
					}
					else if(dataList[2] == "6"){
						bassAttack += creatrueScr.GetEffectData().changeWindHurt - creatureDest.GetEffectData().changeWindAttack ;
						changePercent = (float)((creatrueScr.GetEffectData().changeWindHurtPercent - creatureDest.GetEffectData().changeWindAttackPercent)/ 100.0f) ;
						if(changePercent != 0){
							bassAttack = (int)(bassAttack * changePercent)  ;
						}
					}
					else if(dataList[2] == "5"){
						bassAttack += creatrueScr.GetEffectData().changeLightHurt - creatureDest.GetEffectData().changeLightAttack ;
						changePercent = (float)((creatrueScr.GetEffectData().changeLightHurtPercent - creatureDest.GetEffectData().changeLightAttackPercent)/ 100.0f) ;
						if(changePercent != 0){
							bassAttack = (int)(bassAttack * changePercent)  ;
						}
					}
					else if(dataList[2] == "4"){
						bassAttack += creatrueScr.GetEffectData().changeThunderHurt - creatureDest.GetEffectData().changeThunderAttack ;
						changePercent = (float)((creatrueScr.GetEffectData().changeThunderHurtPercent - creatureDest.GetEffectData().changeThunderAttackPercent)/ 100.0f) ;
						if(changePercent != 0){
							bassAttack = (int)(bassAttack * changePercent)  ;
						}
					}
					
					if(bassAttack < 0)
						bassAttack = 0 ;
				}*/



				
				//camp
				//ri
				int scrCamp = creatrueScr.GetFightCreatureData().camp ;
				int destCamp= creatureDest.GetFightCreatureData().camp;
				if(destCamp == 1 ){
					if(scrCamp == 1){
						bassAttack *= 1.0f + (creatrueScr.GetEffectData().changeSunAttackPercent + creatureDest.GetEffectData().changeSunHurtPercent) * 0.01f ;
						bassAttack += creatrueScr.GetEffectData().changeSunAttack + creatureDest.GetEffectData().changeSunAttack ;
					}
					else if(scrCamp == 2){
						bassAttack *= FightCommon.BE_REFRAIN_COEFFICIENT + (creatrueScr.GetEffectData().changeSunAttackPercent + creatureDest.GetEffectData().changeMoonHurtPercent) * 0.01f ;
						bassAttack += creatrueScr.GetEffectData().changeSunAttack + creatureDest.GetEffectData().changeMoonHurt ;
					}
					else if(scrCamp == 3){
						bassAttack *= FightCommon.REFRAIN_COEFFICIENT + (creatrueScr.GetEffectData().changeSunAttackPercent + creatureDest.GetEffectData().changeSunHurtPercent) * 0.01f ;
						bassAttack += creatrueScr.GetEffectData().changeSunAttack + creatureDest.GetEffectData().changeSunHurt ;
					}
				}
				//yue
				else if(destCamp == 2){
					if(scrCamp == 1 ){
						bassAttack *= FightCommon.REFRAIN_COEFFICIENT + (creatrueScr.GetEffectData().changeMoonAttackPercent + creatureDest.GetEffectData().changeSunHurtPercent) * 0.01f ;
						bassAttack += creatrueScr.GetEffectData().changeMoonAttack + creatureDest.GetEffectData().changeSunAttack ;
					}
					else if(scrCamp == 2){
						bassAttack *= 1.0f + (creatrueScr.GetEffectData().changeMoonAttackPercent + creatureDest.GetEffectData().changeMoonHurtPercent) * 0.01f ;
						bassAttack += creatrueScr.GetEffectData().changeMoonAttack + creatureDest.GetEffectData().changeMoonHurt ;
					}
					else if(scrCamp == 3){
						bassAttack *= FightCommon.BE_REFRAIN_COEFFICIENT + (creatrueScr.GetEffectData().changeMoonAttackPercent + creatureDest.GetEffectData().changeSunHurtPercent) * 0.01f ;
						bassAttack += creatrueScr.GetEffectData().changeMoonAttack + creatureDest.GetEffectData().changeSunHurt ;
					}
				}
				//xin
				else if(destCamp == 3){
					if(scrCamp == 1){
						bassAttack *= FightCommon.BE_REFRAIN_COEFFICIENT + (creatrueScr.GetEffectData().changeStarAttackPercent + creatureDest.GetEffectData().changeSunHurtPercent) * 0.01f ;
						bassAttack += creatrueScr.GetEffectData().changeStarAttack + creatureDest.GetEffectData().changeSunAttack ;
					}
					else if(scrCamp == 2){
						bassAttack *= FightCommon.REFRAIN_COEFFICIENT + (creatrueScr.GetEffectData().changeStarAttackPercent + creatureDest.GetEffectData().changeMoonHurtPercent) * 0.01f ;
						bassAttack += creatrueScr.GetEffectData().changeStarAttack + creatureDest.GetEffectData().changeMoonHurt ;
					}
					else if(scrCamp == 3){
						bassAttack *= 1.0f + (creatrueScr.GetEffectData().changeStarAttackPercent + creatureDest.GetEffectData().changeSunHurtPercent) * 0.01f ;
						bassAttack += creatrueScr.GetEffectData().changeStarAttack + creatureDest.GetEffectData().changeSunHurt ;
					}
				}

				//double hurt
				random = Random.Range(0,1000);
				int crit = creatrueScr.GetFightCreatureData().crit + creatrueScr.GetEffectData().crit ;
				crit += (int)(creatrueScr.GetFightCreatureData().crit + creatrueScr.GetEffectData().critPrecent / 1000.0f );
				if(random <=  crit){
					bassAttack *= 2 ;
					bassAttack *= 1.0f + creatrueScr.GetEffectData().critHurtPercent * 0.01f ;
					bassAttack += creatrueScr.GetEffectData().critHurt ;
					creatureDest.SetCrit(true);
				}
				
				creatureDest.SetHp(-(int)bassAttack);
				if(creatrueScr.GetEnitityType() == EnitityType.ENITITY_TYPE_PET){
					if(creatureDest.GetFightCreatureData().blood == 0){
						if(GameLevel.GameLevelMgr.GetInstance().m_levelType != GameLogical.GameLevel.LevelType.LEVEL_TYPE_PVP){
							MonsterMoudleData monsterData = fileMgr.GetInstance().GetData(creatureDest.GetFightCreatureData().moudleID,CsvType.CSV_TYPE_MONSTER) as MonsterMoudleData ;
							if(monsterData.strength == 1){
								gameGlobal.g_fightSceneUI.CutCoolDownTime(0.5f,creatrueScr.GetFightCreatureData().seat);
							}
							else if(monsterData.strength == 2){
								gameGlobal.g_fightSceneUI.CutCoolDownTime(1f,creatrueScr.GetFightCreatureData().seat);
							}
							else if(monsterData.strength == 3){
								gameGlobal.g_fightSceneUI.CutCoolDownTime(2f,creatrueScr.GetFightCreatureData().seat);
							}
						}
					}
				}

				return true ;
			}
			return false ;
		}
		
		//wave
		public bool EffectWaveChange(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:Min
				//1:Max
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().waveHurtPercentMin += int.Parse(dataList[0]);
				creatrue.GetEffectData().waveHurtPercentMax += int.Parse(dataList[1]);
			}
			return true ;
		}
		
		public bool EffectWaveChangeEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:Min
				//1:Max
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().waveHurtPercentMin -= int.Parse(dataList[0]);
				creatrue.GetEffectData().waveHurtPercentMax -= int.Parse(dataList[1]);
			}
			return true ;
		}

		//change attack
		public bool EffectAttackChange(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeAttackPercent += int.Parse(dataList[0]);
				creatrue.GetEffectData().changeAttack += int.Parse(dataList[1]);
			}
			return true ;
		}
		
		public bool EffectAttackChangeEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeAttackPercent -= int.Parse(dataList[0]);
				creatrue.GetEffectData().changeAttack -= int.Parse(dataList[1]);
			}
			return true ;
		}
		
		
		//change hurt
		public bool EffectHurtChange(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeHurtPercent += int.Parse(dataList[0]);
				creatrue.GetEffectData().changeHurt += int.Parse(dataList[1]);
			}
			return true ;
		}
		
		public bool EffectHurtChangeEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeHurtPercent -= int.Parse(dataList[0]);
				creatrue.GetEffectData().changeHurt -= int.Parse(dataList[1]);
			}
			return true ;
		}

		//change sun attack
		public bool EffectSunAttackChange(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeSunAttackPercent += int.Parse(dataList[0]);
				creatrue.GetEffectData().changeSunAttack += int.Parse(dataList[1]);
			}
			return true ;
		}
		
		public bool EffectSunAttackChangeEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeSunAttackPercent -= int.Parse(dataList[0]);
				creatrue.GetEffectData().changeSunAttack -= int.Parse(dataList[1]);
			}
			return true ;
		}
		
		//change sun hurt
		public bool EffectSunHurtChange(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeSunHurtPercent += int.Parse(dataList[0]);
				creatrue.GetEffectData().changeSunHurt += int.Parse(dataList[1]);
			}
			return true ;
		}
		
		public bool EffectSunHurtChangeEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeIceHurtPercent -= int.Parse(dataList[0]);
				creatrue.GetEffectData().changeIceHurt -= int.Parse(dataList[1]);
			}
			return true ;
		}

		//change moon attack
		public bool EffectMoonAttackChange(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeSunAttackPercent += int.Parse(dataList[0]);
				creatrue.GetEffectData().changeSunAttack += int.Parse(dataList[1]);
			}
			return true ;
		}
		
		public bool EffectMoonAttackChangeEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeSunAttackPercent -= int.Parse(dataList[0]);
				creatrue.GetEffectData().changeSunAttack -= int.Parse(dataList[1]);
			}
			return true ;
		}
		
		//change moon hurt
		public bool EffectMoonHurtChange(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeMoonHurtPercent += int.Parse(dataList[0]);
				creatrue.GetEffectData().changeMoonHurt += int.Parse(dataList[1]);
			}
			return true ;
		}
		
		public bool EffectMoonHurtChangeEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeMoonHurtPercent -= int.Parse(dataList[0]);
				creatrue.GetEffectData().changeMoonHurt -= int.Parse(dataList[1]);
			}
			return true ;
		}

		//change star attack
		public bool EffectStarAttackChange(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeStarAttackPercent += int.Parse(dataList[0]);
				creatrue.GetEffectData().changeStarAttack += int.Parse(dataList[1]);
			}
			return true ;
		}
		
		public bool EffectStarAttackChangeEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeStarAttackPercent -= int.Parse(dataList[0]);
				creatrue.GetEffectData().changeStarAttack -= int.Parse(dataList[1]);
			}
			return true ;
		}
		
		//change star hurt
		public bool EffectStarHurtChange(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeStarHurtPercent += int.Parse(dataList[0]);
				creatrue.GetEffectData().changeStarHurt += int.Parse(dataList[1]);
			}
			return true ;
		}
		
		public bool EffectStarHurtChangeEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeStarHurtPercent -= int.Parse(dataList[0]);
				creatrue.GetEffectData().changeStarHurt -= int.Parse(dataList[1]);
			}
			return true ;
		}



		//change crit attack
		public bool EffectCritAttackChange(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().critPrecent += int.Parse(dataList[0]);
				creatrue.GetEffectData().crit += int.Parse(dataList[1]);
			}
			return true ;
		}
		
		public bool EffectCritAttackChangeEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().critPrecent -= int.Parse(dataList[0]);
				creatrue.GetEffectData().crit -= int.Parse(dataList[1]);
			}
			return true ;
		}
		
		//change crit hurt
		public bool EffectCritHurtChange(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().critHurtPercent += int.Parse(dataList[0]);
				creatrue.GetEffectData().critHurt += int.Parse(dataList[1]);
			}
			return true ;
		}
		
		public bool EffectCritHurtChangeEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().critHurtPercent -= int.Parse(dataList[0]);
				creatrue.GetEffectData().critHurt -= int.Parse(dataList[1]);
			}
			return true ;
		}


		//change skill attack
		public bool EffectSkillAttackChange(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeSkillAttackPercent += int.Parse(dataList[0]);
				creatrue.GetEffectData().changeSkillAttack += int.Parse(dataList[1]);
			}
			return true ;
		}
		
		public bool EffectSkillAttackChangeEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeSkillAttackPercent -= int.Parse(dataList[0]);
				creatrue.GetEffectData().changeSkillAttack -= int.Parse(dataList[1]);
			}
			return true ;
		}
		
		//change skill hurt
		public bool EffectSkillHurtChange(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeSkillHurtPercent += int.Parse(dataList[0]);
				creatrue.GetEffectData().changeSkillHurt += int.Parse(dataList[1]);
			}
			return true ;
		}
		
		public bool EffectSkillHurtChangeEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeSkillHurtPercent -= int.Parse(dataList[0]);
				creatrue.GetEffectData().changeSkillHurt -= int.Parse(dataList[1]);
			}
			return true ;
		}


		//change ice Attack
		public bool EffectIceAttackChange(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeIceAttackPercent += int.Parse(dataList[0]);
				creatrue.GetEffectData().changeIceAttack += int.Parse(dataList[1]);
			}
			return true ;
		}
		
		public bool EffectIceAttackChangeEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeIceAttackPercent -= int.Parse(dataList[0]);
				creatrue.GetEffectData().changeIceAttack -= int.Parse(dataList[1]);
			}
			return true ;
		}


		
		//change ice hurt
		public bool EffectIceHurtChange(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeIceHurtPercent += int.Parse(dataList[0]);
				creatrue.GetEffectData().changeIceHurt += int.Parse(dataList[1]);
			}
			return true ;
		}
		
		public bool EffectIceHurtChangeEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeIceHurtPercent -= int.Parse(dataList[0]);
				creatrue.GetEffectData().changeIceHurt -= int.Parse(dataList[1]);
			}
			return true ;
		}
		

		//change fire attack
		public bool EffectFireAttackChange(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeFireAttackPercent += int.Parse(dataList[0]);
				creatrue.GetEffectData().changeFireAttack += int.Parse(dataList[1]);
			}
			return true ;
		}
		
		public bool EffectFireAttackChangeEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeFireAttackPercent -= int.Parse(dataList[0]);
				creatrue.GetEffectData().changeFireAttack -= int.Parse(dataList[1]);
			}
			return true ;
		}
		
		
		
		//change frie hurt
		public bool EffectFireHurtChange(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeFireHurtPercent += int.Parse(dataList[0]);
				creatrue.GetEffectData().changeFireHurt += int.Parse(dataList[1]);
			}
			return true ;
		}
		
		public bool EffectFireHurtChangeEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeFireHurtPercent -= int.Parse(dataList[0]);
				creatrue.GetEffectData().changeFireHurt -= int.Parse(dataList[1]);
			}
			return true ;
		}
		
		
		
		//change earth attack
		public bool EffectEarthAttackChange(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeEarthAttackPercent += int.Parse(dataList[0]);
				creatrue.GetEffectData().changeEarthAttack += int.Parse(dataList[1]);
			}
			return true ;
		}
		
		public bool EffectEarthAttackChangeEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeEarthAttackPercent -= int.Parse(dataList[0]);
				creatrue.GetEffectData().changeEarthAttack -= int.Parse(dataList[1]);
			}
			return true ;
		}
		
		
		
		//change earth hurt
		public bool EffectEarthHurtChange(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeEarthHurtPercent += int.Parse(dataList[0]);
				creatrue.GetEffectData().changeEarthHurt += int.Parse(dataList[1]);
			}
			return true ;
		}
		
		public bool EffectEarthHurtChangeEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeEarthHurtPercent -= int.Parse(dataList[0]);
				creatrue.GetEffectData().changeEarthHurt -= int.Parse(dataList[1]);
			}
			return true ;
		}
		
		
		
		//change thunder attack
		public bool EffectThunderAttackChange(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeThunderAttackPercent += int.Parse(dataList[0]);
				creatrue.GetEffectData().changeThunderAttack += int.Parse(dataList[1]);
			}
			return true ;
		}
		
		public bool EffectThunderAttackChangeEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeThunderAttackPercent -= int.Parse(dataList[0]);
				creatrue.GetEffectData().changeThunderAttack -= int.Parse(dataList[1]);
			}
			return true ;
		}
		
		
		
		//change thunder hurt
		public bool EffectThunderHurtChange(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeThunderHurtPercent += int.Parse(dataList[0]);
				creatrue.GetEffectData().changeThunderHurt += int.Parse(dataList[1]);
			}
			return true ;
		}
		
		public bool EffectThunderHurtChangeEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeThunderHurtPercent -= int.Parse(dataList[0]);
				creatrue.GetEffectData().changeThunderHurt -= int.Parse(dataList[1]);
			}
			return true ;
		}

		
		
		//change light attack
		public bool EffectLightAttackChange(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeLightAttackPercent += int.Parse(dataList[0]);
				creatrue.GetEffectData().changeLightAttack += int.Parse(dataList[1]);
			}
			return true ;
		}
		
		public bool EffectLightAttackChangeEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeLightAttackPercent -= int.Parse(dataList[0]);
				creatrue.GetEffectData().changeLightAttack -= int.Parse(dataList[1]);
			}
			return true ;
		}
		
		
		
		//change light hurt
		public bool EffectLightHurtChange(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeLightHurtPercent += int.Parse(dataList[0]);
				creatrue.GetEffectData().changeLightHurt += int.Parse(dataList[1]);
			}
			return true ;
		}
		
		public bool EffectLightHurtChangeEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeLightHurtPercent -= int.Parse(dataList[0]);
				creatrue.GetEffectData().changeLightHurt -= int.Parse(dataList[1]);
			}
			return true ;
		}

		
		
		//change wind attack
		public bool EffectWindAttackChange(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeWindAttackPercent += int.Parse(dataList[0]);
				creatrue.GetEffectData().changeWindAttack += int.Parse(dataList[1]);
			}
			return true ;
		}
		
		public bool EffectWindAttackChangeEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeWindAttackPercent -= int.Parse(dataList[0]);
				creatrue.GetEffectData().changeWindAttack -= int.Parse(dataList[1]);
			}
			return true ;
		}
		
		
		
		//change wind hurt
		public bool EffectWindHurtChange(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeWindHurtPercent += int.Parse(dataList[0]);
				creatrue.GetEffectData().changeWindHurt += int.Parse(dataList[1]);
			}
			return true ;
		}
		
		public bool EffectWindHurtChangeEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().changeWindHurtPercent -= int.Parse(dataList[0]);
				creatrue.GetEffectData().changeWindHurt -= int.Parse(dataList[1]);
			}
			return true ;
		}
		
		
		//change all element attack
		public bool EffectAllElementAttackChange(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				int percent = int.Parse(dataList[0]) ;
				int hurt = int.Parse(dataList[1]) ;
				creatrue.GetEffectData().changeIceAttack += hurt ;
				creatrue.GetEffectData().changeIceAttackPercent += percent;
				
				creatrue.GetEffectData().changeFireAttackPercent += percent;
				creatrue.GetEffectData().changeFireAttack += hurt;
				
				creatrue.GetEffectData().changeLightAttackPercent += percent;
				creatrue.GetEffectData().changeLightAttack += hurt;
				
				creatrue.GetEffectData().changeEarthAttackPercent += percent;
				creatrue.GetEffectData().changeEarthAttack += hurt;
				
				creatrue.GetEffectData().changeThunderAttackPercent += percent;
				creatrue.GetEffectData().changeThunderAttack += hurt;
				
				creatrue.GetEffectData().changeWindAttackPercent += percent;
				creatrue.GetEffectData().changeWindAttack += hurt;
				
			}
			return true ;
		}
		
		public bool EffectAllElementAttackChangeEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				int percent = int.Parse(dataList[0]) ;
				int hurt = int.Parse(dataList[1]) ;
				creatrue.GetEffectData().changeIceAttack -= hurt ;
				creatrue.GetEffectData().changeIceAttackPercent -= percent;
				
				creatrue.GetEffectData().changeFireAttackPercent -= percent;
				creatrue.GetEffectData().changeFireAttack -= hurt;
				
				creatrue.GetEffectData().changeLightAttackPercent -= percent;
				creatrue.GetEffectData().changeLightAttack -= hurt;
				
				creatrue.GetEffectData().changeEarthAttackPercent -= percent;
				creatrue.GetEffectData().changeEarthAttack -= hurt;
				
				creatrue.GetEffectData().changeThunderAttackPercent -= percent;
				creatrue.GetEffectData().changeThunderAttack -= hurt;
				
				creatrue.GetEffectData().changeWindAttackPercent -= percent;
				creatrue.GetEffectData().changeWindAttack -= hurt;
				
			}
			return true ;
		}
		
		
		//change all element hurt
		public bool EffectAllElementHurtChange(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				int percent = int.Parse(dataList[0]) ;
				int hurt = int.Parse(dataList[1]) ;
				creatrue.GetEffectData().changeIceHurt += hurt ;
				creatrue.GetEffectData().changeIceHurtPercent += percent;
				
				creatrue.GetEffectData().changeFireHurtPercent += percent;
				creatrue.GetEffectData().changeFireHurt += hurt;
				
				creatrue.GetEffectData().changeLightHurtPercent += percent;
				creatrue.GetEffectData().changeLightHurt += hurt;
				
				creatrue.GetEffectData().changeEarthHurtPercent += percent;
				creatrue.GetEffectData().changeEarthHurt += hurt;
				
				creatrue.GetEffectData().changeThunderHurtPercent += percent;
				creatrue.GetEffectData().changeThunderHurt += hurt;
				
				creatrue.GetEffectData().changeWindHurtPercent += percent;
				creatrue.GetEffectData().changeWindHurt += hurt;
				
			}
			return true ;
		}
		
		public bool EffectAllElementHurtChangeEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//0:percent
				//1:num
				string[] dataList = ob.data.Split('#');
				int percent = int.Parse(dataList[0]) ;
				int hurt = int.Parse(dataList[1]) ;
				creatrue.GetEffectData().changeIceHurt -= hurt ;
				creatrue.GetEffectData().changeIceHurtPercent -= percent;
				
				creatrue.GetEffectData().changeFireHurtPercent -= percent;
				creatrue.GetEffectData().changeFireHurt -= hurt;
				
				creatrue.GetEffectData().changeLightHurtPercent -= percent;
				creatrue.GetEffectData().changeLightHurt -= hurt;
				
				creatrue.GetEffectData().changeEarthHurtPercent -= percent;
				creatrue.GetEffectData().changeEarthHurt -= hurt;
				
				creatrue.GetEffectData().changeThunderHurtPercent -= percent;
				creatrue.GetEffectData().changeThunderHurt -= hurt;
				
				creatrue.GetEffectData().changeWindHurtPercent -= percent;
				creatrue.GetEffectData().changeWindHurt -= hurt;
				
			}
			return true ;
		}
		
		//must bingo
		public bool EffectMustBingo(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				creatrue.GetEffectData().mustBingo = true ;
			}
			return true ;
		}
		
		public bool EffectMustBingoEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				creatrue.GetEffectData().mustBingo = false ;
			}
			return true ;
		}
		
		//invincibility
		public bool EffectInvincibility(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				creatrue.GetEffectData().invincibility = true ;
			}
			return true ;
		}
		
		public bool EffectInvincibilityEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				creatrue.GetEffectData().invincibility = false ;
			}
			return true ;
		}
		
		//one hurt
		public bool EffectOneHurt(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().oneHurt += int.Parse(dataList[0]) ;
			}
			return true ;
		}
		
		public bool EffectOneHurtEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().oneHurt -= int.Parse(dataList[0]) ;
			}
			return true ;
		}
		
		//can not add hp
		public bool EffectCantHp(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				creatrue.GetEffectData().cantHp = true ;
			}
			return true ;
		}
		
		public bool EffectCantHpEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				creatrue.GetEffectData().cantHp = false ;
			}
			return true ;
		}
		
		//resistance
		public bool EffectResistance(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				List<int> buffList = GetCreatrueBuffList(creatrue) ;
				string[] dataList = ob.data.Split('#');
				for(int i = 0; i<dataList.Length; ++i){
					if(!string.IsNullOrEmpty( dataList[i] )){
						int buffID = int.Parse(dataList[i]) ;
						creatrue.GetEffectData().resistanceList.Add(buffID);
						if(buffList.Contains(buffID)){
							creatrue.DelBuff(buffID);
						}
					}
				}
			}
			return true ;
		}
		
		public bool EffectResistanceEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				List<int> buffList = GetCreatrueBuffList(creatrue) ;
				string[] dataList = ob.data.Split('#');
				for(int i = 0; i<dataList.Length; ++i){
					int buffID = int.Parse(dataList[i]) ;
					if(creatrue.GetEffectData().resistanceList.Contains(buffID)){
						creatrue.GetEffectData().resistanceList.Remove(buffID);
					}
					
				}
			}
			return true ;
		}
		
		//remove debuff
		public bool EffectRemoveDebuff(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				List<int> buffList = GetCreatrueBuffList(creatrue) ;
				string[] dataList = ob.data.Split('#');
				int result = 0 ;
				for(int i = 0; i<dataList.Length; ++i){
					try{
						int.TryParse(dataList[i],out result);
						if(buffList.Contains(result)){
							creatrue.DelBuff(result);
						}
					}
					catch{

					}
				}
			}
			return true ;
		}
		
		//relive
		public bool EffectRelive(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				//1. percent
				//2. num
				//3. rate
				string[] dataList = ob.data.Split('#');
				creatrue.GetEffectData().relive += int.Parse(dataList[1]) ;
				creatrue.GetEffectData().relive += (int)(creatrue.GetFightCreatureData().maxBlood * int.Parse(dataList[0]) * 0.01f) ;
				creatrue.GetEffectData().reLiveRate = int.Parse(dataList[2]) ;
			}
			return true ;
		}
		
		//can not use skill
		public bool EffectCantSkill(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				creatrue.GetEffectData().cantSkill = true ;
			}
			return true ;
		}
		
		public bool EffectCantSkillEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				creatrue.GetEffectData().cantSkill = false ;
			}
			return true ;
		}
		
		//stone state
		public bool EffectStoneState(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				if(creatrue.GetEnitityType() == EnitityType.ENITITY_TYPE_PET){
					CPet pet = (CPet)creatrue ;
					if(pet.GetEnitityAiState() != AIState.AI_STATE_STONE)
						pet.m_stateMachine.ChangeState(PetStoneState.getInstance()) ;
				}
				else if(creatrue.GetEnitityType() == EnitityType.ENITITY_TYPE_MONSTER){
					CMonster monster = (CMonster)creatrue ;
					if(monster.GetEnitityAiState() != AIState.AI_STATE_STONE)
						monster.m_stateMachine.ChangeState(MonsterStoneState.getInstance()) ;
				}
				else if(creatrue.GetEnitityType() == EnitityType.ENITITY_TYPE_ENEMY_PET){
					CEnemyPet enemyPet = (CEnemyPet)creatrue ;
					if(enemyPet.GetEnitityAiState() != AIState.AI_STATE_STONE)
						enemyPet.m_stateMachine.ChangeState(EnemyPetStoneState.getInstance()) ;
				}
				
			}
			return true ;
		}
		
		public bool EffectStoneStateEnd(EffectBassData ob){
			CCreature creatrue = EnitityMgr.GetInstance().GetEnitity(ob.destID) ;
			if(creatrue != null){
				if(creatrue.GetEnitityType() == EnitityType.ENITITY_TYPE_PET){
					CPet pet = (CPet)creatrue ;
					pet.m_stateMachine.ChangeState(pet.m_stateMachine.GetPreviosState()) ;
				}
				else if(creatrue.GetEnitityType() == EnitityType.ENITITY_TYPE_MONSTER){
					CMonster monster = (CMonster)creatrue ;
					monster.m_stateMachine.ChangeState(monster.m_stateMachine.GetPreviosState()) ;
				}
				else if(creatrue.GetEnitityType() == EnitityType.ENITITY_TYPE_ENEMY_PET){
					CEnemyPet enemyPet = (CEnemyPet)creatrue ;
					enemyPet.m_stateMachine.ChangeState(enemyPet.m_stateMachine.GetPreviosState()) ;
				}
			}
			return true ;
		}

		public bool MustSkill(EffectBassData ob){
			CCreature creature = EnitityMgr.GetInstance().GetEnitity(ob.destID);
			if(creature !=null){
				creature.GetEffectData().skillRate = 100; 
				return true ;
			}
			return false ;
		}

		public bool MustSkillEnd(EffectBassData ob){
			CCreature creature = EnitityMgr.GetInstance().GetEnitity(ob.destID);
			if(creature !=null){
				creature.GetEffectData().skillRate = 0; 
				return true ;
			}
			return false ;
		}

		//
		public bool BloodBoomEffect(EffectBassData ob){
			CCreature creature = EnitityMgr.GetInstance().GetEnitity(ob.destID);
			if(creature != null){
				creature.GetEffectData().bloodBoom = true ;
				return true ;
			}
			return false ;
		}

		//range
		public bool RangeEffect(EffectBassData ob){
			CCreature creature = EnitityMgr.GetInstance().GetEnitity(ob.destID);
			if(creature != null){
				string[] dataList = ob.data.Split('#');
				//0:dest buff id
				//1:range buff id
				//2:range
				//3:buff rate
				//4:max buff num
				
				SingleBuffCreateData singleBuff = new SingleBuffCreateData() ;
				singleBuff.buffModuleID.Add(int.Parse(dataList[0])) 	;
				singleBuff.srcCreatureID = ob.scrID ;
				singleBuff.destCreatureID= ob.destID  ;
				singleBuff.rangeType = BuffRangeType.BUFF_RANGE_SINGLE ;
				CBuffMgr.GetInstance().CreateBuff(singleBuff);
				
				List<CCreature> creatureList = null;
				if(creature.GetEnitityType() == EnitityType.ENITITY_TYPE_MONSTER){
					creatureList = EnitityMgr.GetInstance().FindAreaCreatureList(creature,EnitityType.ENITITY_TYPE_MONSTER,(float)int.Parse(dataList[2]));
				}
				else if(creature.GetEnitityType() == EnitityType.ENITITY_TYPE_PET){
					creatureList = EnitityMgr.GetInstance().FindAreaCreatureList(creature,EnitityType.ENITITY_TYPE_PET,(float)int.Parse(dataList[2]));
				}
				
				int maxNum = 0 ;
				if(dataList.Length > 4){
					maxNum = int.Parse(dataList[4]) ;
				}
				int curNum = 0;
				
				if(creatureList != null){
					for(int i = 0 ; i < creatureList.Count; ++i){
						int random = Random.Range(0,100);
						if(random < int.Parse(dataList[3])){
							if(maxNum == 0 || maxNum>curNum){
								SingleBuffCreateData rangeBuff = new SingleBuffCreateData() ;
								rangeBuff.buffModuleID.Add(int.Parse(dataList[1])) ;
								rangeBuff.srcCreatureID = ob.scrID ;
								rangeBuff.destCreatureID= creatureList[i].GetId()  ;
								rangeBuff.rangeType = BuffRangeType.BUFF_RANGE_SINGLE ;
								CBuffMgr.GetInstance().CreateBuff(rangeBuff);
								++curNum ;
							}
							else{
								break ;
							}
						}
					}
				}
			}
			return true ;
		}



		//
		public bool CircleRangeEffect(EffectBassData ob){
			EffectCircleRangeData rangeData = (EffectCircleRangeData)ob ;
			if(rangeData != null){
				//0:dest buff id
				//1:range buff id
				//2:range
				//3:buff rate
				//4:max buff num
				string[] dataList = ob.data.Split('#');
				
				int maxNum = 0 ;
				if(dataList.Length > 4){
					maxNum = int.Parse(dataList[4]) ;
				}

				int curNum = 0;
				int random = 0 ;
				for(int i = 0 ; i < rangeData.destCreatures.Count; ++i){
					if(rangeData.destCreatures[i] != null && rangeData.destCreatures[i].GetRenderObject() != null){
						random = Random.Range(0,100);
						if(random < int.Parse(dataList[3])){
							if(maxNum == 0 || maxNum>curNum){
								SingleBuffCreateData rangeBuff = new SingleBuffCreateData() ;
								rangeBuff.buffModuleID.Add(int.Parse(dataList[1])) 	;
								rangeBuff.srcCreatureID = ob.scrID ;
								rangeBuff.destCreatureID= rangeData.destCreatures[i].GetId()  ;
								rangeBuff.rangeType = BuffRangeType.BUFF_RANGE_SINGLE ;
								CBuffMgr.GetInstance().CreateBuff(rangeBuff);
								curNum++ ;
							}
						}

					}
				}
			}

			return false ;
		}

		List<int> GetCreatrueBuffList(CCreature creature){
			if(creature.GetEnitityType() == EnitityType.ENITITY_TYPE_PET || creature.GetEnitityType() == EnitityType.ENITYTY_TYPE_BACK_UP_PET){
				CPet pet = (CPet)creature ;
				return pet.m_buffList ;
			}
			else if(creature.GetEnitityType() == EnitityType.ENITITY_TYPE_MONSTER){
				CMonster monster = (CMonster)creature ;
				return monster.m_buffList ;
			}
			else if(creature.GetEnitityType() == EnitityType.ENITITY_TYPE_ENEMY_PET){
				CEnemyPet enemyPet = (CEnemyPet)creature ;
				return enemyPet.m_buffList ;
			}
			
			return null ;
		}

	}
}


