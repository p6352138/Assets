using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameEntity{
public class CTerrian : CCearcue {

		#region private Properties
		private Terrain terrain;
		private List<Bounds> m_BoundsList; 
		#endregion

		public CTerrian(int id,GameObject ob){
			base.id = id;
			base.go = ob;

			terrain = base.go.GetComponent<Terrain>();

			for (int i = 0;i<base.go.transform.FindChild("prop").childCount;i++){
				if(base.go.transform.FindChild("prop").GetChild(i).GetComponent<BoxCollider>() != null){
					m_BoundsList.Add(base.go.transform.FindChild("prop").GetChild(i).GetComponent<BoxCollider>().bounds);
				}
				else if(base.go.transform.FindChild("prop").GetChild(i).GetComponent<MeshCollider>() != null){
					m_BoundsList.Add(base.go.transform.FindChild("prop").GetChild(i).GetComponent<MeshCollider>().bounds);
				}
			}
		}

		#region public function
		public Vector3 GetTerrianPosion(){
			return base.go.transform.localPosition;
		}

		public List<Bounds> GetTerrianBounds(){
			return m_BoundsList;
		}
		#endregion
	}
}
