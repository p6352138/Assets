using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameLogic.Navigation;

namespace GameEntity{
public class CTerrian : CCearcue {

		#region private Properties
		private Terrain terrain;
		private List<Bounds> m_BoundsList; 
		#endregion

		public CTerrian(int id,GameObject ob){
			m_BoundsList = new List<Bounds>();

			base.id = id;
			base.go = ob;

			terrain = base.go.GetComponent<Terrain>();
			BoxCollider box;
			MeshCollider mesh;

			for (int i = 0;i<base.go.transform.FindChild("prop").childCount;i++){
				box = base.go.transform.FindChild("prop").GetChild(i).GetComponent<BoxCollider>();

				if(box == null){
					mesh = base.go.transform.FindChild("prop").GetChild(i).GetComponent<MeshCollider>();
					if(mesh != null){
						m_BoundsList.Add(mesh.bounds);
					}
				}
				else{
					m_BoundsList.Add(box.bounds);
				}
			}

			NavigationMgr.GetInstance().InitPathData(m_BoundsList);
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
