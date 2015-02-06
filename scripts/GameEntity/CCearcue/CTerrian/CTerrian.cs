using UnityEngine;
using System.Collections;

namespace GameEntity{
public class CTerrian : CCearcue {

		#region private Properties
		private Terrain terrain;
		#endregion

		public CTerrian(int id,GameObject ob){
			base.id = id;
			base.go = ob;

			terrain = base.go.GetComponent<Terrain>();
		}

		#region public function
		public Vector3 GetTerrianPosion(){
			return base.go.transform.localPosition;
		}
		#endregion
	}
}
