using UnityEngine;
using System.Collections;

public class Vertex  {
	
	
	public int v;
	
	//transform
	private int x;
	private int y;
	private Vector3 vec = new Vector3();

	public int g;


	public int f;
	
	//false 
	public bool valid = true;
	
	public int parent;

	public Vertex(int _v,int _x,int _y){
		v = _v;
		x = _x;
		y = _x;

		vec.x = _x;
		vec.z = _y;
		vec.y = 20;

		f = 0;
		g = 0;
		parent = -1;

	}

	public int getX(){
		return x;
	}

	public int getY(){
		return y;
	}
	public Vector3 getVec(){
		return vec;
	}
	
	
}