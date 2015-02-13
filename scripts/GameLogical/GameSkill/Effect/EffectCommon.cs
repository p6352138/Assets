using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using GameLogical.GameEnitity ;
namespace GameLogical.GameSkill.Effect{
	public class EffectBassData
	{
		public		int		scrID	;
		public		int		destID	;
		public		string	data	;
	}
	
	public class EffectCircleRangeData : EffectBassData{
		public		List<CCreature> 	destCreatures	;
		public	EffectCircleRangeData(){
			destCreatures = new List<CCreature>();
		}
	}

	/*public	class EffectHpData:EffectBassData
	{
		public		int	hp		;
	}
	
	public	class EffectHpPercentData:EffectBassData
	{
		public		float	hpPercent	;
	}
	
	public	class EffectSpeedData:EffectBassData
	{
		public		int		speed		;
	}
	
	public	class EffectSpeedPercentData:EffectBassData{
		public		float	speedPercent;
	}*/
}


