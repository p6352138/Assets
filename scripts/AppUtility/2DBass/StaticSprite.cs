using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Bass2D{
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	public class StaticSprite : MonoBehaviour {
		/*
		// 1----2
		// |  / |
		// | /  |
		// 3----4
		protected Vector2[] _uv ;
		protected Vector3[] newVertices;
	    protected Vector2[] newUV;
	    protected int[] newTriangles;
		protected Mesh mesh;
		
		//public 
		//control uv
		//this can use multi-textrue in one
		public Vector2 _posPixed;  //uv point 1 pixed pos
		public Vector2 _size;      //uv pixed size
		public Vector2 _maxSize ;  //texture pixed size
		public bool     _isFullScreen; //full screen
		public bool     _isBottomPoint ;
		
		// Use this for initialization
		void Start () {
			mesh = new Mesh();
			GetComponent<MeshFilter>().mesh = mesh;
			
			_uv = new Vector2[4] ;
	        newVertices = new Vector3[4];
	        newUV = new Vector2[4] ;
	        newTriangles = new int[]{0,1,2,2,1,3};
			
			if(_isFullScreen)
			{
				newVertices[0] = new Vector3(0.0f,common.common.MAX_SCREEN_POINT.y,0.0f);
	        	newVertices[1] = new Vector3(common.common.MAX_SCREEN_POINT.x, common.common.MAX_SCREEN_POINT.y, 0.0f);
	        	newVertices[2] = new Vector3(0.0f, 0.0f, 0.0f);
	        	newVertices[3] = new Vector3(common.common.MAX_SCREEN_POINT.x, 0.0f, 0.0f);
			}
			else
			{
				if(_isBottomPoint == false){
					newVertices[0] = new Vector3(-0.5f,0.5f,0.0f);
	        		newVertices[1] = new Vector3(0.5f, 0.5f, 0.0f);
	        		newVertices[2] = new Vector3(-0.5f, -0.5f, 0.0f);
	        		newVertices[3] = new Vector3(0.5f, -0.5f, 0.0f);
				}
				else{
					newVertices[0] = new Vector3(0.0f,1.0f,0.0f);
	        		newVertices[1] = new Vector3(1.0f, 1.0f, 0.0f);
	        		newVertices[2] = new Vector3(0.0f, 0.0f, 0.0f);
	        		newVertices[3] = new Vector3(1.0f, 0.0f, 0.0f);
				}

			}
			
			//uv
			_uv[0].x = _posPixed.x / _maxSize.x ;
			_uv[0].y = (_maxSize.y - _posPixed.y) / _maxSize.y ;
			
			_uv[1].x = (_posPixed.x + _size.x) / _maxSize.x ;
			_uv[1].y = (_maxSize.y - _posPixed.y) / _maxSize.y ;
			
			_uv[2].x = _posPixed.x / _maxSize.x ;
			_uv[2].y = (_maxSize.y - (_posPixed.y + + _size.y)) / _maxSize.y ;
			
			_uv[3].x =  (_posPixed.x + _size.x) / _maxSize.x ;
			_uv[3].y = (_maxSize.y - (_posPixed.y + _size.y)) / _maxSize.y ;
			
			newUV = _uv;
			
	        mesh.vertices = newVertices;
			mesh.normals = new Vector3[]{ new Vector3(0.0f,0.0f,-1.0f),new Vector3(0.0f,0.0f,-1.0f),new Vector3(0.0f,0.0f,-1.0f),new Vector3(0.0f,0.0f,-1.0f)};
	        mesh.uv = newUV;
	        mesh.triangles = newTriangles;
	        mesh.name = gameObject.name;
		}
		
		// Update is called once per frame
		void Update () {
#if UNITY_EDITOR
			//vertices
			if(_isFullScreen)
			{
				newVertices[0] = new Vector3(0.0f,common.common.MAX_SCREEN_POINT.y,0.0f);
	        	newVertices[1] = new Vector3(common.common.MAX_SCREEN_POINT.x, common.common.MAX_SCREEN_POINT.y, 0.0f);
	        	newVertices[2] = new Vector3(0.0f, 0.0f, 0.0f);
	        	newVertices[3] = new Vector3(common.common.MAX_SCREEN_POINT.x, 0.0f, 0.0f);
			}
			else
			{
				if(_isBottomPoint == false){
					newVertices[0] = new Vector3(-0.5f,0.5f,0.0f);
	        		newVertices[1] = new Vector3(0.5f, 0.5f, 0.0f);
	        		newVertices[2] = new Vector3(-0.5f, -0.5f, 0.0f);
	        		newVertices[3] = new Vector3(0.5f, -0.5f, 0.0f);
				}
				else{
					newVertices[0] = new Vector3(0.0f,1.0f,0.0f);
	        		newVertices[1] = new Vector3(1.0f, 1.0f, 0.0f);
	        		newVertices[2] = new Vector3(0.0f, 0.0f, 0.0f);
	        		newVertices[3] = new Vector3(1.0f, 0.0f, 0.0f);
				}
			}
			mesh.vertices = newVertices;
			
			//uv
			_uv[0].x = _posPixed.x / _maxSize.x ;
			_uv[0].y = (_maxSize.y - _posPixed.y) / _maxSize.y ;
			
			_uv[1].x = (_posPixed.x + _size.x) / _maxSize.x ;
			_uv[1].y = (_maxSize.y - _posPixed.y) / _maxSize.y ;
			
			_uv[2].x = _posPixed.x / _maxSize.x ;
			_uv[2].y = (_maxSize.y - (_posPixed.y + + _size.y)) / _maxSize.y ;
			
			_uv[3].x =  (_posPixed.x + _size.x) / _maxSize.x ;
			_uv[3].y = (_maxSize.y - (_posPixed.y + _size.y)) / _maxSize.y ;

			newUV = _uv ;
			mesh.uv = newUV;
#endif
		}
		
		public void Refresh(){
			//vertices
			if(_isFullScreen)
			{
				newVertices[0] = new Vector3(0.0f,common.common.MAX_SCREEN_POINT.y,0.0f);
	        	newVertices[1] = new Vector3(common.common.MAX_SCREEN_POINT.x, common.common.MAX_SCREEN_POINT.y, 0.0f);
	        	newVertices[2] = new Vector3(0.0f, 0.0f, 0.0f);
	        	newVertices[3] = new Vector3(common.common.MAX_SCREEN_POINT.x, 0.0f, 0.0f);
			}
			else
			{
				if(_isBottomPoint == false){
					newVertices[0] = new Vector3(-0.5f,0.5f,0.0f);
	        		newVertices[1] = new Vector3(0.5f, 0.5f, 0.0f);
	        		newVertices[2] = new Vector3(-0.5f, -0.5f, 0.0f);
	        		newVertices[3] = new Vector3(0.5f, -0.5f, 0.0f);
				}
				else{
					newVertices[0] = new Vector3(0.0f,1.0f,0.0f);
	        		newVertices[1] = new Vector3(1.0f, 1.0f, 0.0f);
	        		newVertices[2] = new Vector3(0.0f, 0.0f, 0.0f);
	        		newVertices[3] = new Vector3(1.0f, 0.0f, 0.0f);
				}
			}
			mesh.vertices = newVertices;
			
			//uv
			_uv[0].x = _posPixed.x / _maxSize.x ;
			_uv[0].y = (_maxSize.y - _posPixed.y) / _maxSize.y ;
			
			_uv[1].x = (_posPixed.x + _size.x) / _maxSize.x ;
			_uv[1].y = (_maxSize.y - _posPixed.y) / _maxSize.y ;
			
			_uv[2].x = _posPixed.x / _maxSize.x ;
			_uv[2].y = (_maxSize.y - (_posPixed.y + + _size.y)) / _maxSize.y ;
			
			_uv[3].x =  (_posPixed.x + _size.x) / _maxSize.x ;
			_uv[3].y = (_maxSize.y - (_posPixed.y + _size.y)) / _maxSize.y ;

			newUV = _uv ;
			mesh.uv = newUV;
		}*/
	}
}

