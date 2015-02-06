using UnityEngine;
using System.Collections;

namespace GameEntity{
	public class CCearcue  {

		private int ID;
		private GameObject Go;

		public int id {
			set {ID = value;}
			get {return ID;}
		}

		public GameObject go{
			set {Go = value;}
			get {return Go;}
		}

		public CCearcue(){
		}

		public CCearcue(int id,GameObject ob){
			this.id = id;
			this.go = ob;
		}
	}
}
