using UnityEngine;
using System.Collections;
using System.Collections.Generic ;

namespace GameLogical.GameEnitity{
	public class BulletData : DynamicCreatureData
	{
		public int destID ;
		public int scrID  ;
		public int effectID;
		public int effectEndID ;
		public List<int> buffID ;
		public string audioPath;
		public bool follow ;
		public Vector3 pos ;
		
		public BulletData(){
			destID = -1 ;
			scrID  = -1 ;
		 	effectID=-1 ;
			effectEndID = -1;
		 	buffID = new List<int>() ;
			follow = true ;
		}
	}
}

