using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class A_star : MonoBehaviour {

	// Use this for initialization

	public Dictionary<int,Vertex> map = new Dictionary<int ,Vertex> ();

	public int start = 20;
	public int end  = 4;

	public PHeap heap = new PHeap();

	public List<int> results = new List<int> ();
	public int size ;


	public List<Vector3> lines = new List< Vector3>();

	void Start () {
		Debug.Log ("start");


		LineRenderer mLine = (LineRenderer)this.gameObject.GetComponent (typeof(LineRenderer));


		int i = 0;

		//add vertecs
	
		for(int x = -40;x <= 40;x += 20,i++){

			map.Add (i, new Vertex(i,x,40));

		}
		for(int x = -40;x <= 40;x += 20,i++){
			map.Add (i, new Vertex(i,x,20));
			
		}

		for(int x = -40;x <= 40;x += 20,i++){
			map.Add (i, new Vertex(i,x,0));
			
		}


		for(int x = -40;x <= 40;x += 20,i++){
			map.Add (i, new Vertex(i,x,-20));
			
		}


		for(int x = -40;x <= 40;x += 20,i++){
			map.Add (i, new Vertex(i,x,-40));
			
		}




		map [16].valid = false;
		map [6].valid = false;
		map [3] .valid = false;
		map [8].valid = false;
		map [13].valid = false;
		map [24].valid = false;


		heap.add (map [start]);


		//solve
		int t;
		size = 0;

		while ((t = heap.top () ) >= 0) {


			if(t == end ){
	
				break;
			}

			map[t].valid = false;

			int x = t % 5;
			int y = t / 5;

			int temp_x = x -1;
			int temp_y = y;


			if(temp_x >= 0 && temp_x < 5) {
				if(map[temp_x  + 5 *temp_y].valid){
					map[temp_x + 5* temp_y].parent = t;
					map[temp_x + 5*temp_y].g = map[t].g + 1;
					map[temp_x + 5 *temp_y].f = map[temp_x + 5*temp_y].g + mah (end,temp_x + 5 *temp_y);

					heap.add (map[temp_x + 5 *temp_y]);


				}

			}

			temp_x = x + 1; 
			temp_y = y;



			if(temp_x >= 0 && temp_x < 5) {
				if(map[temp_x  + 5 *temp_y].valid){
					map[temp_x + 5* temp_y].parent = t;
					map[temp_x + 5*temp_y].g = map[t].g + 1;
					map[temp_x + 5 *temp_y].f = map[temp_x + 5*temp_y].g + mah (end,temp_x + 5 *temp_y);
					heap.add (map[temp_x + 5 *temp_y]);
					
				}
				
			}

			temp_x = x;
			temp_y = y - 1;




			if(temp_y >= 0 && temp_y < 5) {
				if(map[temp_x  + 5 *temp_y].valid){
					map[temp_x + 5* temp_y].parent = t;
					map[temp_x + 5*temp_y].g = map[t].g + 1;
					map[temp_x + 5 *temp_y].f = map[temp_x + 5*temp_y].g + mah (end,temp_x + 5 *temp_y);
					
					heap.add (map[temp_x + 5 *temp_y]);
				}
				
			}

			temp_x = x;
			temp_y = y + 1;



			if(temp_y >= 0 && temp_y < 5) {
				if(map[temp_x  + 5 *temp_y].valid){
					map[temp_x + 5* temp_y].parent = t;
					map[temp_x + 5*temp_y].g = map[t].g + 1;
					map[temp_x + 5 *temp_y].f = map[temp_x + 5*temp_y].g + mah (end,temp_x + 5 *temp_y);
					heap.add (map[temp_x + 5 *temp_y]);
					
				}
				
			}

		//end if
			
		}

		i = end;
		while (map[i].parent >= 0) {
			results.Add ( i);
			size++;
			i = map[i].parent;

		}

		results.Add (start);


		mLine.SetVertexCount (size);

		for(int j = 0;j < size;++j){
			

			Vector3 v = map[results[j]].getVec();

			lines.Add(v);

			mLine.SetPosition(j,v);

			Debug.Log (v.x + " " + v.y + " " + v.z);
		
		}

		Debug.Log ("done");


	}
	
	// Update is called once per frame
	void Update () {
	
	}

	int mah(int x,int y){
		return (int)Mathf.Abs (map [x].getX() - map [y].getX()) + Mathf.Abs (map [x].getY() - map [y].getY());
	}
	
}


