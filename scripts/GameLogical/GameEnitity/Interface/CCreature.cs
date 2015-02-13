using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using GameEvent;
using GameLogical.GameEnitity.AI ;
namespace GameLogical.GameEnitity{
	public interface CCreature {
		void Think();
		void Update(float deltaTime);
		void OnMessage(EventMessageBase message);
		EnitityType GetEnitityType();
		AIState  GetEnitityAiState();
		void Release();
		GameObject GetRenderObject();
		void Play(string name,WrapMode mode);
		int  GetId();
		void SetHp(int hp) ;
		EffectData GetEffectData();
		FightCreatureData GetFightCreatureData() ;
		bool AddBuff(int buffMoudleID,int buffID);
		int CheckBuff(int buffMoudleID) ;
		void DelBuff(int buffID);
		void SetCrit(bool isCrit) ;
		void SetColor(Color color);
		void LoadObjectCallBack(Object ob, MGResouce.LoadCreatureData tran);
	}
	
	public class CreatetureData{
		public int 		   id			;
		public EnitityType enitityType  ;
		public int		   moudleID		;
		public bool		   isMainRole	;
	}
	
	public class DynamicCreatureData : CreatetureData{
		public float      moveSpeed ;
		public float	  attackCD  = 1.2f ;
		public float      curAttackCD ;
	}
	
	public class FightCreatureData : DynamicCreatureData{
		public 		string	   severId	 ;
		public 		int        blood     ;
		public		int		   maxBlood	 ;
		public 		int        attack    ;
		public      AttackType attackType;
		
		public 		float	   attackArea		;
		public 		float	   eyeShotArea 	= 60; 
		
		public      int        maxSpell ;
		public      int		   spell	;

		public		int 		misPlace = 5	;

		public		int 		star	;
		
		public 		List<int>  skillList;
		public		int		   skillRate = 20 ;

		public		int		   seat ;
		public 		int		   camp ;
		public		int		   appHpSecond	;
		
		
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
		
		public FightCreatureData(){
			skillList = new List<int>();
		}
	}
}

