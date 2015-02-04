using UnityEngine;
using System.Collections;

using System.Collections.Generic;

//fck unity
public class PHeap {

	//fck this list,int what world a list has no method to return its size!!!!!!!!!!!
	public List<Vertex> l = new List<Vertex> ();
	public int size = 0;

	public PHeap() {

	}

	public void add (Vertex a) {
		l.Add (a);
		size++;
	}

	public int top() {

		int result = -1;
		int p = -1;

		for (int i = 0; i < size; i++) {
			if(!l[i].valid) 
				continue;
			if(result < 0 || result < l[i].f ) {
				p = l[i].v;
				result = l[i].f;
			}

		}
		return p;
	}
	
}
