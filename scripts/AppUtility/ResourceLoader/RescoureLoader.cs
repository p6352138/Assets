using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using common ;

public class RescoureLoader : MonoBehaviour
{
	public struct sAddData 
	{
		public 		WWW			rescoure	;
		public 		CallBack	callBack	;
		public		string		path		;
	}
	protected		Dictionary<string,sAddData>		m_handleDic ;
	//protected		sAddData[]						m_handleDic ;
	protected		WWW								m_tempData  ;
	protected		float							m_deltaTime	;
	protected		float							m_curTime	;
	
	public 	delegate void	CallBack(WWW assert);
	void	Start(){
	}
	
	public void Init(){
		m_handleDic = new Dictionary<string, sAddData>();
		m_deltaTime = 0.2f ;
		//int progress = 0 ;
		//LoadRescoure(common.common.configPath,fileMgr.GetInstance().LoadConfig,ref progress);
	}
	
	public	void	LoadRescoure(string	path,CallBack fun,ref int progress){
		if(!m_handleDic.ContainsKey(path)){
			sAddData data = new sAddData();
			data.callBack = fun		 ;
			data.path	  = path	 ;
			m_handleDic.Add(path,data);
			StartCoroutine(load(path,fun));
		}
	}
	
	IEnumerator	load(string path,CallBack fun){
		//WWW rescoure ;
		if(path.Contains(".assetBundle")){
			if(fileMgr.GetInstance().config != null){
				m_tempData = WWW.LoadFromCacheOrDownload(path ,fileMgr.GetInstance().config.version);
			}else{
				m_tempData = WWW.LoadFromCacheOrDownload(path ,1);
			}
		}
		else{
			m_tempData = new WWW(path);
		}

		yield	return m_tempData.progress ;
		/*if(m_tempData.isDone == true){
			fun(m_tempData);
		}*/
	}
	
	void Update(){
		m_curTime += Time.deltaTime ;
		if(m_curTime < m_deltaTime)
			return ;

		m_curTime = 0.0f ;
		List<string> key = new List<string>(m_handleDic.Keys) ;
		for(int i = m_handleDic.Count - 1; i>=0; --i){
			sAddData temp = m_handleDic[key[i]] ;
			if(temp.rescoure != null && temp.rescoure.isDone == true){
				temp.callBack(temp.rescoure);
				temp.rescoure.Dispose();
				m_handleDic.Remove(temp.path);
			}
			else{
				//StartCoroutine(load (temp.path,temp.callBack));
				sAddData data = m_handleDic[key[i]];
				data.rescoure = m_tempData ;
				m_handleDic[key[i]] = data ;
			}
		}
		
		/*foreach(sAddData temp in m_handleDic.Values){
			if(temp.rescoure != null && temp.rescoure.isDone == true){
				temp.callBack(temp.rescoure);
				temp.rescoure.Dispose();
				//m_handleDic.Remove(temp);
				m_handleDic.Remove(temp.path);
			}
			else{
				StartCoroutine(load (temp.path,temp.callBack));
			}
		}*/
	}
}

