using UnityEngine;
using System.Collections;
using GameEvent;
using AppUtility;
using System.Collections.Generic;
using GameLogical.GameEnitity ;

namespace GameLogical.GameSkill.Effect{
	public delegate bool CEffectFun(EffectBassData data);
	class EffectMgr : Singleton<EffectMgr>, IEvent
	{
		protected	Dictionary<string,CEffectFun>	m_effectFunMap	;
		protected	CEffect							m_effect		;
		protected	EffectBassData					m_effectData	;
		public void Init(){
			m_effect = new CEffect();
			m_effectData = new EffectBassData();
			m_effectFunMap = new Dictionary<string, CEffectFun>();
			m_effectFunMap.Add("EffectHp",m_effect.EffectHp);
			m_effectFunMap.Add("EffectMaxHpPercent",m_effect.EffectMaxHpPercent);
			m_effectFunMap.Add("EffectAttackPrecentHp",m_effect.EffectAttackPrecentHp);
			
			m_effectFunMap.Add("EffectHpMax",m_effect.EffectHpMax);
			m_effectFunMap.Add("EffectHpMaxEnd",m_effect.EffectHpMaxEnd);

			m_effectFunMap.Add("EffectMyHp",m_effect.EffectMyHp);

			m_effectFunMap.Add("EffectSpeed",m_effect.EffectSpeed);
			m_effectFunMap.Add("EffectSpeedEnd",m_effect.EffectSpeedEnd);

			m_effectFunMap.Add("EffectSpeedPercent",m_effect.EffectSpeedPercent);
			m_effectFunMap.Add("EffectSpeedPercentEnd",m_effect.EffectSpeedPercentEnd);

			
			m_effectFunMap.Add("EffectMp",m_effect.EffectMp);
			m_effectFunMap.Add("EffectMpPercent",m_effect.EffectMpPercent);
			m_effectFunMap.Add("EffectMpMax",m_effect.EffectMpMax);
			m_effectFunMap.Add("EffectMpMaxEnd",m_effect.EffectMpMaxEnd);

			m_effectFunMap.Add("EffectAttack",m_effect.EffectAttack);
			m_effectFunMap.Add("EffectAttackEnd",m_effect.EffectAttackEnd);

			m_effectFunMap.Add("EffectAttackPrecent",m_effect.EffectAttackPrecent);
			m_effectFunMap.Add("EffectAttackPrecentEnd",m_effect.EffectAttackPrecentEnd);

			
			m_effectFunMap.Add("EffectCrit",m_effect.EffectCrit);
			m_effectFunMap.Add("EffectCritEnd",m_effect.EffectCritEnd);

			m_effectFunMap.Add("EffectCritPercent",m_effect.EffectCritPercent);
			m_effectFunMap.Add("EffectCritPercentEnd",m_effect.EffectCritPercentEnd);
			
			m_effectFunMap.Add("EffectDuck",m_effect.EffectDuck);
			m_effectFunMap.Add("EffectDuckEnd",m_effect.EffectDuckEnd);

			m_effectFunMap.Add("EffectDuckPrecent",m_effect.EffectDuckPrecent);
			m_effectFunMap.Add("EffectDuckPrecentEnd",m_effect.EffectDuckPrecentEnd);

			
			m_effectFunMap.Add("EffectSuckBlood",m_effect.EffectSuckBlood);
			m_effectFunMap.Add("EffectSuckBloodPrecent",m_effect.EffectSuckBloodPrecent);
			
			m_effectFunMap.Add("EffectReboundAttack",m_effect.EffectReboundAttack);
			m_effectFunMap.Add("EffectReboundAttackPrecent",m_effect.EffectReboundAttackPrecent);
			
			m_effectFunMap.Add("EffectSuddenDeath",m_effect.EffectSuddenDeath);
			
			m_effectFunMap.Add("EffectWindElement",m_effect.EffectWindElement);
			m_effectFunMap.Add("EffectFireElement",m_effect.EffectFireElement);
			m_effectFunMap.Add("EffectDarkElement",m_effect.EffectDarkElement);
			m_effectFunMap.Add("EffectIceElement",m_effect.EffectIceElement);
			m_effectFunMap.Add("EffectLightElement",m_effect.EffectLightElement);
			m_effectFunMap.Add("EffectEarthElement",m_effect.EffectEarthElement);
			
			
			m_effectFunMap.Add("EffectHurt",m_effect.EffectHurt);
			
			m_effectFunMap.Add("EffectWaveChange",m_effect.EffectWaveChange);
			m_effectFunMap.Add("EffectWaveChangeEnd",m_effect.EffectWaveChangeEnd);

			m_effectFunMap.Add("EffectAttackChange",m_effect.EffectAttackChange);
			m_effectFunMap.Add("EffectAttackChangeEnd",m_effect.EffectAttackChangeEnd);

			m_effectFunMap.Add("EffectHurtChange",m_effect.EffectHurtChange);
			m_effectFunMap.Add("EffectHurtChangeEnd",m_effect.EffectHurtChangeEnd);

			m_effectFunMap.Add("EffectIceAttackChange",m_effect.EffectIceAttackChange);
			m_effectFunMap.Add("EffectIceAttackChangeEnd",m_effect.EffectIceAttackChangeEnd);

			m_effectFunMap.Add("EffectIceHurtChange",m_effect.EffectIceHurtChange);
			m_effectFunMap.Add("EffectIceHurtChangeEnd",m_effect.EffectIceHurtChangeEnd);

			m_effectFunMap.Add("EffectFireAttackChange",m_effect.EffectFireAttackChange);
			m_effectFunMap.Add("EffectFireAttackChangeEnd",m_effect.EffectFireAttackChangeEnd);

			m_effectFunMap.Add("EffectFireHurtChange",m_effect.EffectFireHurtChange);
			m_effectFunMap.Add("EffectFireHurtChangeEnd",m_effect.EffectFireHurtChangeEnd);

			m_effectFunMap.Add("EffectEarthAttackChange",m_effect.EffectEarthAttackChange);
			m_effectFunMap.Add("EffectEarthAttackChangeEnd",m_effect.EffectEarthAttackChangeEnd);

			m_effectFunMap.Add("EffectEarthHurtChange",m_effect.EffectEarthHurtChange);
			m_effectFunMap.Add("EffectEarthHurtChangeEnd",m_effect.EffectEarthHurtChangeEnd);

			m_effectFunMap.Add("EffectThunderAttackChange",m_effect.EffectThunderAttackChange);
			m_effectFunMap.Add("EffectThunderAttackChangeEnd",m_effect.EffectThunderAttackChangeEnd);

			m_effectFunMap.Add("EffectThunderHurtChange",m_effect.EffectThunderHurtChange);
			m_effectFunMap.Add("EffectThunderHurtChangeEnd",m_effect.EffectThunderHurtChangeEnd);

			m_effectFunMap.Add("EffectLightAttackChange",m_effect.EffectLightAttackChange);
			m_effectFunMap.Add("EffectLightAttackChangeEnd",m_effect.EffectLightAttackChangeEnd);

			m_effectFunMap.Add("EffectLightHurtChange",m_effect.EffectLightHurtChange);
			m_effectFunMap.Add("EffectLightHurtChangeEnd",m_effect.EffectLightHurtChangeEnd);

			m_effectFunMap.Add("EffectWindAttackChange",m_effect.EffectWindAttackChange);
			m_effectFunMap.Add("EffectWindAttackChangeEnd",m_effect.EffectWindAttackChangeEnd);

			m_effectFunMap.Add("EffectWindHurtChange",m_effect.EffectWindHurtChange);
			m_effectFunMap.Add("EffectWindHurtChangeEnd",m_effect.EffectWindHurtChangeEnd);

			m_effectFunMap.Add("EffectAllElementAttackChange",m_effect.EffectAllElementAttackChange);
			m_effectFunMap.Add("EffectAllElementAttackChangeEnd",m_effect.EffectAllElementAttackChangeEnd);

			m_effectFunMap.Add("EffectAllElementHurtChange",m_effect.EffectAllElementHurtChange);
			m_effectFunMap.Add("EffectAllElementHurtChangeEnd",m_effect.EffectAllElementHurtChangeEnd);
			
			
			m_effectFunMap.Add("EffectMustBingo",m_effect.EffectMustBingo);
			m_effectFunMap.Add("EffectMustBingoEnd",m_effect.EffectMustBingoEnd);

			m_effectFunMap.Add("EffectInvincibility",m_effect.EffectInvincibility);
			m_effectFunMap.Add("EffectInvincibilityEnd",m_effect.EffectInvincibilityEnd);

			m_effectFunMap.Add("EffectOneHurt",m_effect.EffectOneHurt);
			m_effectFunMap.Add("EffectOneHurtEnd",m_effect.EffectOneHurtEnd);

			m_effectFunMap.Add("EffectCantHp",m_effect.EffectCantHp);
			m_effectFunMap.Add("EffectCantHpEnd",m_effect.EffectCantHpEnd);

			m_effectFunMap.Add("EffectResistance",m_effect.EffectResistance);
			m_effectFunMap.Add("EffectResistanceEnd",m_effect.EffectResistanceEnd);

			m_effectFunMap.Add("EffectRemoveDebuff",m_effect.EffectRemoveDebuff);
			m_effectFunMap.Add("EffectRelive",m_effect.EffectRelive);
			
			m_effectFunMap.Add("EffectCantSkill",m_effect.EffectCantSkill);
			m_effectFunMap.Add("EffectCantSkillEnd",m_effect.EffectCantSkillEnd);

			m_effectFunMap.Add("EffectStoneState",m_effect.EffectStoneState);
			m_effectFunMap.Add("EffectStoneStateEnd",m_effect.EffectStoneStateEnd);

			m_effectFunMap.Add("MustSkill",m_effect.MustSkill);
			m_effectFunMap.Add("MustSkillEnd",m_effect.MustSkillEnd);

			m_effectFunMap.Add("EffectReboundFarAttack",m_effect.EffectReboundFarAttack);
			m_effectFunMap.Add("EffectReboundFarAttackPrecent",m_effect.EffectReboundFarAttackPrecent);

			m_effectFunMap.Add("EffectSunAttackChange",m_effect.EffectSunAttackChange);
			m_effectFunMap.Add("EffectSunAttackChangeEnd",m_effect.EffectSunAttackChangeEnd);
			m_effectFunMap.Add("EffectSunHurtChange",m_effect.EffectSunHurtChange);
			m_effectFunMap.Add("EffectSunHurtChangeEnd",m_effect.EffectSunHurtChangeEnd);

			m_effectFunMap.Add("EffectMoonAttackChange",m_effect.EffectMoonAttackChange);
			m_effectFunMap.Add("EffectMoonAttackChangeEnd",m_effect.EffectMoonAttackChangeEnd);
			m_effectFunMap.Add("EffectMoonHurtChange",m_effect.EffectMoonHurtChange);
			m_effectFunMap.Add("EffectMoonHurtChangeEnd",m_effect.EffectMoonHurtChangeEnd);

			m_effectFunMap.Add("EffectStarAttackChange",m_effect.EffectStarAttackChange);
			m_effectFunMap.Add("EffectStarAttackChangeEnd",m_effect.EffectStarAttackChangeEnd);
			m_effectFunMap.Add("EffectStarHurtChange",m_effect.EffectStarHurtChange);
			m_effectFunMap.Add("EffectStarHurtChangeEnd",m_effect.EffectStarHurtChangeEnd);

			m_effectFunMap.Add("EffectCritAttackChange",m_effect.EffectCritAttackChange);
			m_effectFunMap.Add("EffectCritAttackChangeEnd",m_effect.EffectCritAttackChangeEnd);
			m_effectFunMap.Add("EffectCritHurtChange",m_effect.EffectCritHurtChange);
			m_effectFunMap.Add("EffectCritHurtChangeEnd",m_effect.EffectCritHurtChangeEnd);

			m_effectFunMap.Add("EffectSkillAttackChange",m_effect.EffectSkillAttackChange);
			m_effectFunMap.Add("EffectSkillAttackChangeEnd",m_effect.EffectSkillAttackChangeEnd);
			m_effectFunMap.Add("EffectSkillHurtChange",m_effect.EffectSkillHurtChange);
			m_effectFunMap.Add("EffectSkillHurtChangeEnd",m_effect.EffectSkillHurtChangeEnd);

			m_effectFunMap.Add("EffectChangeHpPercent",m_effect.EffectChangeHpPercent);
			m_effectFunMap.Add("EffectChangeHpPercentEnd",m_effect.EffectChangeHpPercentEnd);

			m_effectFunMap.Add("EffectChangeAddHpPercent",m_effect.EffectChangeHpPercent);
			m_effectFunMap.Add("EffectChangeAddHpPercentEnd",m_effect.EffectChangeHpPercentEnd);


			m_effectFunMap.Add("EffectRealHurt",m_effect.EffectRealHurt);
			m_effectFunMap.Add("EffectRealHurtEnd",m_effect.EffectRealHurtEnd);


			m_effectFunMap.Add("RangeEffect",m_effect.RangeEffect);
			m_effectFunMap.Add("CricleRangeEffect",m_effect.CircleRangeEffect);

			m_effectFunMap.Add("EffectCutCdTime",m_effect.EffectCutCdTime);
			m_effectFunMap.Add("EffectCutCdTimeEnd",m_effect.EffectCutCdTimeEnd);

		}
		
		public void OnMessage(EventMessageBase message){
			if(message.eventMessageModel == EventMessageModel.eEventMessageModel_Buff){
				if((BuffMessageAction)message.eventMessageAction == BuffMessageAction.BUFF_MESSAGE_ATTACK){
					EventMessageAttack attackMessage = (EventMessageAttack)message ;
					switch(attackMessage.effectFun){
					case "EffectSuckBlood":{
						ExcuteEffect(attackMessage.effectFun,attackMessage.destID,attackMessage.scrID,attackMessage.param);
					}
						break ;
					case "EffectSuckBloodPrecent":{
						string param = attackMessage.hurt.ToString() + "#" + attackMessage.param ;
						ExcuteEffect(attackMessage.effectFun,attackMessage.destID,attackMessage.scrID,param);
					}
						break ;
					}
				}
				else if((BuffMessageAction)message.eventMessageAction == BuffMessageAction.BUFF_MESSAGE_BE_ATTACK){
					EventMessageBeAttack beAttackMessage = (EventMessageBeAttack)message ;
					switch(beAttackMessage.effectFun){
					case "EffectReboundAttack":{
						ExcuteEffect(beAttackMessage.effectFun,beAttackMessage.scrID,beAttackMessage.destID,beAttackMessage.param);
					}
						break ;
					case "EffectReboundAttackPrecent":{
						string param = beAttackMessage.hurt.ToString() + "#" + beAttackMessage.param ;
						ExcuteEffect(beAttackMessage.effectFun,beAttackMessage.scrID,beAttackMessage.destID,param);
					}
						break ;

					case "EffectReboundFarAttack":{
						ExcuteEffect(beAttackMessage.effectFun,beAttackMessage.scrID,beAttackMessage.destID,beAttackMessage.param);
					}
						break ;

					case "EffectReboundFarAttackPrecent":{
						string param = beAttackMessage.hurt.ToString() + "#" + beAttackMessage.param ;
						ExcuteEffect(beAttackMessage.effectFun,beAttackMessage.scrID,beAttackMessage.destID,param);
					}
						break ;
					}
				}
			}
		}
		
		public bool	ExcuteEffect(string effectName, int scrID,int destID ,string	data){
			if(m_effectFunMap.ContainsKey(effectName)){
				m_effectData.data = data ;
				m_effectData.scrID = scrID;
				m_effectData.destID=destID;
				return m_effectFunMap[effectName](m_effectData);
			}
			else{
				if(!effectName.Contains("End")){
					common.debug.GetInstance().Error("excute error effect function:" + effectName);
				}
				return false ;
			}
		}

		public bool ExcuteCricleRangeEffect(int scrID,List<CCreature> destCreature,string data){
			EffectCircleRangeData rangeData = new EffectCircleRangeData();
			rangeData.destCreatures = destCreature ;
			rangeData.data = data ;
			rangeData.scrID= scrID;
			return m_effectFunMap["CricleRangeEffect"](rangeData) ;
		}
	}
}


