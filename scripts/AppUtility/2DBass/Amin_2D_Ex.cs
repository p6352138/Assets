using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AppUtility ;


#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Bass2D{
	/*
	public class AnimFrameData : BassStruct{
		public int x ;
		public int y ;
		public int w ;
		public int h ;
		
		public AnimFrameData(){
		}
		
		public AnimFrameData(Dictionary<string,object> dic){
			this.parseData(dic);			
		}
	}
	
	public class AnimData : BassStruct{
		public 		AnimFrameData    	frame     ;
		public      string				eventFun  ;
		public		string				audio	  ;
		public		string				beEffect  ;
		//public 		bool				rotated   ;
		public AnimData(Dictionary<string,object> dic){
			frame = new AnimFrameData();
			this.parseData(dic);			
		}
	}
	
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(Animation))]
	public class Amin_2D_Ex : MonoBehaviour
	{
		// 1----2
		// |  / |
		// | /  |
		// 3----4
		protected Vector2 _uv1 ;
		protected Vector2 _uv2 ;
		protected Vector2 _uv3 ;
		protected Vector2 _uv4 ;
		
		protected Vector3[] newVertices;
	    protected Vector2[] newUV;
	    protected int[] newTriangles;
		protected Mesh mesh;
		
		//public 
		//control uv
		//this can use multi-textrue in one
		public Vector2 posPixed;  //uv point 1 pixed pos
		public Vector2 size;      //uv pixed size
		public Vector2 maxSize ;  //texture pixed size
		public bool    isFullScreen; //full screen
		public bool    isBottomPoint ;
		public float   delta ;
		
		public TextAsset  m_animFile ;

		// init
		void Awake(){
			Init();
		}
		// Use this for initialization
		void Start (){
			
		}
		
		void Init(){
			mesh = new Mesh();
			GetComponent<MeshFilter>().mesh = mesh;

	        newVertices = new Vector3[4];
	        newUV = new Vector2[4] ;
	        newTriangles = new int[]{0,1,2,2,1,3};
			
			if(isFullScreen)
			{
				newVertices[0] = new Vector3(0.0f,common.common.MAX_SCREEN_POINT.y,0.0f);
	        	newVertices[1] = new Vector3(common.common.MAX_SCREEN_POINT.x, common.common.MAX_SCREEN_POINT.y, 0.0f);
	        	newVertices[2] = new Vector3(0.0f, 0.0f, 0.0f);
	        	newVertices[3] = new Vector3(common.common.MAX_SCREEN_POINT.x, 0.0f, 0.0f);
			}
			else
			{
				if(isBottomPoint == false){
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
			
			_uv1.x = posPixed.x / maxSize.x ;
			_uv1.y = (maxSize.y - posPixed.y) / maxSize.y ;
			
			_uv2.x = (posPixed.x + size.x) / maxSize.x ;
			_uv2.y = (maxSize.y - posPixed.y) / maxSize.y ;
			
			_uv3.x = posPixed.x / maxSize.x ;
			_uv3.y = (maxSize.y - (posPixed.y + size.y)) / maxSize.y ;
			
			_uv4.x =  (posPixed.x + size.x) / maxSize.x ;
			_uv4.y = (maxSize.y - (posPixed.y + size.y)) / maxSize.y ;

			newUV[0] = _uv1;
	        newUV[1] = _uv2;
	        newUV[2] = _uv3;
	        newUV[3] = _uv4;
			
	        mesh.vertices = newVertices;
			mesh.normals = new Vector3[]{ new Vector3(0.0f,0.0f,-1.0f),new Vector3(0.0f,0.0f,-1.0f),new Vector3(0.0f,0.0f,-1.0f),new Vector3(0.0f,0.0f,-1.0f)};
	        mesh.uv = newUV;
	        mesh.triangles = newTriangles;
	        mesh.name = gameObject.name + "mesh";
			
			CreateAnim();
		}
		// Update is called once per frame
		void Update ()
		{
#if UNITY_EDITOR

			
			if(isFullScreen)
			{
				newVertices[0] = new Vector3(0.0f,common.common.MAX_SCREEN_POINT.y,0.0f);
	        	newVertices[1] = new Vector3(common.common.MAX_SCREEN_POINT.x, common.common.MAX_SCREEN_POINT.y, 0.0f);
	        	newVertices[2] = new Vector3(0.0f, 0.0f, 0.0f);
	        	newVertices[3] = new Vector3(common.common.MAX_SCREEN_POINT.x, 0.0f, 0.0f);
			}
			else
			{
				if(isBottomPoint == false){
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
#endif
			
			_uv1.x = posPixed.x / maxSize.x ;
			_uv1.y = (maxSize.y - posPixed.y) / maxSize.y ;
			
			_uv2.x = (posPixed.x + size.x) / maxSize.x ;
			_uv2.y = (maxSize.y - posPixed.y) / maxSize.y ;
			
			_uv3.x = posPixed.x / maxSize.x ;
			_uv3.y = (maxSize.y - (posPixed.y + + size.y)) / maxSize.y ;
			
			_uv4.x =  (posPixed.x + size.x) / maxSize.x ;
			_uv4.y = (maxSize.y - (posPixed.y + size.y)) / maxSize.y ;
			
			newUV[0] = _uv1;
	        newUV[1] = _uv2;
	        newUV[2] = _uv3;
	        newUV[3] = _uv4;
			
			mesh.uv = newUV;
			
		}
		
		void CreateAnim(){
			if(m_animFile == null)
				return ;
			Dictionary<string,object> result =( Dictionary<string,object>)Json.Deserialize(m_animFile.text);
			Dictionary<string,object> animDataMap = (Dictionary<string,object>)result["frames"] ;
			//each frame
			Dictionary<string,ArrayList> animMap = new Dictionary<string, ArrayList>();
			
			//	analysis animation data
			foreach(KeyValuePair<string,object> temp in animDataMap){
				Dictionary<string,object> dic = (Dictionary<string,object>)temp.Value;
				AnimData data = new AnimData(dic) ;
				
				Dictionary<string,object> frame = (Dictionary<string,object>)dic["frame"] ;
				AnimFrameData frameData = new AnimFrameData(frame);
				
				data.frame = frameData ;
				
				string name = temp.Key ;
				string[] animName = name.Split('/');
				if(animName[0] != null){
					if(animMap.ContainsKey(animName[0])){
						animMap[animName[0]].Add(data);
					}
					else{
						ArrayList dataArr = new ArrayList();
						dataArr.Add(data);
						animMap.Add(animName[0],dataArr);
					}
				}
			}
			
			// create animation from data
			delta = 0.084f ;
			foreach(KeyValuePair<string,ArrayList> temp in animMap){
				AnimationCurve scaleX = new AnimationCurve();
				AnimationCurve scaleY = new AnimationCurve();
				AnimationCurve sizeX = new AnimationCurve();  
				AnimationCurve sizeY = new AnimationCurve(); 
				AnimationCurve posX = new AnimationCurve();  
				AnimationCurve posY = new AnimationCurve(); 
				
				
				Keyframe[] scaleXkeys = new Keyframe[temp.Value.Count + 1] ;
				Keyframe[] scaleYkeys = new Keyframe[temp.Value.Count + 1] ;
				
				Keyframe[] posXkeys = new Keyframe[temp.Value.Count + 1] ;
				Keyframe[] posYkeys = new Keyframe[temp.Value.Count + 1] ;	
				
				ArrayList animEventList = new ArrayList();
				//AnimationEvent[] animEvent = new AnimationEvent[];
				
				for(int i = 0; i < temp.Value.Count; ++i){
					AnimData data =  temp.Value[i] as AnimData ;
					scaleXkeys[i].tangentMode = 31 ;
					scaleXkeys[i].inTangent = float.MaxValue *2f ;
					scaleXkeys[i].outTangent= float.MaxValue *2f ;
					scaleXkeys[i].time = delta * i;
					scaleXkeys[i].value= data.frame.w;
					scaleX.AddKey(scaleXkeys[i]);
					sizeX.AddKey(scaleXkeys[i]);
					
					scaleYkeys[i].tangentMode = 31 ;
					scaleYkeys[i].inTangent = float.MaxValue *2f ;
					scaleYkeys[i].outTangent= float.MaxValue *2f ;
					scaleYkeys[i].time = delta * i;
					scaleYkeys[i].value= data.frame.h;	
					scaleY.AddKey(scaleYkeys[i]);
					sizeY.AddKey(scaleYkeys[i]);
					
					posXkeys[i].tangentMode = 31 ;
					posXkeys[i].inTangent = float.MaxValue *2f ;
					posXkeys[i].outTangent= float.MaxValue *2f ;
					posXkeys[i].time = delta * i;
					posXkeys[i].value= data.frame.x;
					posX.AddKey(posXkeys[i]);
					
					posYkeys[i].tangentMode = 31 ;
					posYkeys[i].inTangent = float.MaxValue *2f ;
					posYkeys[i].outTangent= float.MaxValue *2f ;
					posYkeys[i].time = delta * i;
					posYkeys[i].value= data.frame.y;
					posY.AddKey(posYkeys[i]);
					
					//last frame
					if(i == temp.Value.Count - 1){
						scaleXkeys[i + 1].tangentMode = 31 ;
						scaleXkeys[i + 1].inTangent = float.MaxValue *2f ;
						scaleXkeys[i + 1].outTangent= float.MaxValue *2f ;
						scaleXkeys[i + 1].time = delta * (i + 1);
						scaleXkeys[i + 1].value= scaleXkeys[0].value;
						scaleX.AddKey(scaleXkeys[i + 1]);
						sizeX.AddKey(scaleXkeys[i + 1]);
						
						scaleYkeys[i + 1].tangentMode = 31 ;
						scaleYkeys[i + 1].inTangent = float.MaxValue *2f ;
						scaleYkeys[i + 1].outTangent= float.MaxValue *2f ;
						scaleYkeys[i + 1].time = delta * (i + 1);
						scaleYkeys[i + 1].value= scaleYkeys[0].value;	
						scaleY.AddKey(scaleYkeys[i + 1]);
						sizeY.AddKey(scaleYkeys[i + 1]);
						
						posXkeys[i + 1].tangentMode = 31 ;
						posXkeys[i + 1].inTangent = float.MaxValue *2f ;
						posXkeys[i + 1].outTangent= float.MaxValue *2f ;
						posXkeys[i + 1].time = delta * (i + 1);
						posXkeys[i + 1].value= posXkeys[0].value;
						posX.AddKey(posXkeys[i + 1]);
						
						posYkeys[i + 1].tangentMode = 31 ;
						posYkeys[i + 1].inTangent = float.MaxValue *2f ;
						posYkeys[i + 1].outTangent= float.MaxValue *2f ;
						posYkeys[i + 1].time = delta * (i + 1);
						posYkeys[i + 1].value= posYkeys[0].value;
						posY.AddKey(posYkeys[i + 1]);
					}
					
					//add event
					if(data.eventFun != null){
						AnimationEvent animEvent = new AnimationEvent();
						animEvent.time = delta * i;
						animEvent.functionName = data.eventFun ;
						animEvent.stringParameter = data.audio + "#" + data.beEffect ;
						animEventList.Add(animEvent);
					}
				}
				
				AnimationClip clip = new AnimationClip();   
				//set curve
				clip.SetCurve("", typeof(Transform), "localScale.x", scaleX);
				clip.SetCurve("", typeof(Transform), "localScale.y", scaleY);
				clip.SetCurve("", typeof(Amin_2D_Ex), "size.x", sizeX);
				clip.SetCurve("", typeof(Amin_2D_Ex), "size.y", sizeY);
				clip.SetCurve("", typeof(Amin_2D_Ex), "posPixed.x", posX);
				clip.SetCurve("", typeof(Amin_2D_Ex), "posPixed.y", posY);
				
				//set event
				foreach(AnimationEvent tempEvent in animEventList){
					clip.AddEvent(tempEvent);
				}
				animation.AddClip(clip, temp.Key); 
				
			}
			//animation.wrapMode = WrapMode.Loop ;
			//animation.playAutomatically = true ;
			//animation.Play("attack");
		}
		
		//
		Vector3[] GetUV(float x, float y){
			Vector3[] uv = new Vector3[4] ;
			uv[0].x = x / maxSize.x ;
			uv[0].y = (maxSize.y - y) / maxSize.y ;
			
			uv[1].x = (x + size.x) / maxSize.x ;
			uv[1].y = (maxSize.y - y) / maxSize.y ;
			
			uv[2].x = posPixed.x / maxSize.x ;
			uv[2].y = (maxSize.y - (y + + size.y)) / maxSize.y ;
			
			uv[3].x =  (x + size.x) / maxSize.x ;
			uv[3].y = (maxSize.y - (y + size.y)) / maxSize.y ;
			return uv ;
		}
	}*/
}

