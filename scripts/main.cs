using UnityEngine;
using System.Collections;
using System ;
using System.IO; 
using AppUtility;
using NetWork.NetSession;
using NetWork.NetFrame;
using NetWork.NetModule;
using NetWork.NetDefine;
using System.Net ;
using System.Net.Security;
using System.Collections.Generic;
using System.Text;
//using GameEvent ;
//using GameLogical.GameEnitity;
//using GameMessgeHandle;
using GameMessgeHandle ;
//using GameLogical;
using common ;
//using GameLogical.Guide;




public class main : MonoBehaviour {
    static private INetSession netSession ;
	static private INetSession chatSession;

	//冷却时间
	//private PackageCoolDown packageCoolDown=PackageCoolDown.packageCoolDown;
    public static string stringToEditSeverIP    = "192.168.0.195";//115.236.59.168//192.168.0.123
	public static string chatSeverIP = "192.168.0.195" ;
	public static int Gameport = 15629;
	public static int chatPort = 15628;
	private float	deltaTime ;

	public static long  heart_beat 	   	= 0 ;
	private long  scoket_delta	   		= 0 ;

	static  bool shutDownNet = false;
	
	static public string connectAgain = "请重新连接网络";
	static public string connectLost = "网络没有连接";
	Rect rect=new Rect(680,Screen.height-50,1000,1000);
	GUIStyle style=new GUIStyle();
	void OnDestroy() {
		if(netSession != null){
			NetFrameMgr.GetInstance().CloseNetSession(netSession.GetSessionID());
		}
		if(chatSession != null){
			NetFrameMgr.GetInstance().CloseNetSession(chatSession.GetSessionID());
		}
	}
    public static void AppNetSesssionConnected(INetSession netSession, EVENTSESSIONPLACE eventPlace, bool connectSuccess)
    {
        if (connectSuccess)
        {
            Console.WriteLine("success");
        }
        else
		{
			Console.WriteLine("failed");
		}
    }

	//显示版本号
	void OnGUI()
	{

		/*if(GUI.Button(new Rect(0.0f,0.0f,50.0f,50.0f),"new fight")){

			NewPlayerGuide.curGuide=99;
			//Time.timeScale = 1.0f ;
	//		Resources.UnloadUnusedAssets();
			ReChangeAccount();
		}
		style.fontSize=30;
	    GUI.color=Color.red;
		if( GameDataCenter.GetInstance().userLoginData!=null)
		GUI.Label(rect, "版本号："+GameDataCenter.GetInstance().userLoginData.serverVersion,style);*/

	}
	
    public static void AppNetSessionClosed(INetSession netSession, EVENTSESSIONPLACE eventPlace)
    {
		
		//if(msgStruct.dwPlayerName == 1)
		//{
			
		//}
    }
//


	
	void Awake()
	{
        netSession = null;
		Application.targetFrameRate = 30 ;
		Application.runInBackground = true ;
	}
	
	void Start () {
		//UICommon.Init();
		//GameDataCenter.GetInstance().Init();
	}
	
	public static void HttpTect(string account, string password){
				//gameGlobal.g_LoadingPage.show();
				string URL = string.Empty;
				/*
				if (Application.platform == RuntimePlatform.IPhonePlayer) {

						if (fileMgr.GetInstance ().config.version != string.Empty) {
								URL = "http://" + fileMgr.GetInstance ().config.userServerIP + ":" + fileMgr.GetInstance ().config.userServerPort.ToString () +
										"/LoginServer/webservice/game/ppLogin?username=" + account +
										"&password=" + account + "&version=" + fileMgr.GetInstance ().config.version;
						} else {
								URL = "http://" + fileMgr.GetInstance ().config.userServerIP + ":" + fileMgr.GetInstance ().config.userServerPort.ToString () +
										"/LoginServer/webservice/game/ppLogin?username=" + account + "&password=" + password;
						}

				} else {
						if (fileMgr.GetInstance ().config.version != string.Empty) {
								URL = "http://" + fileMgr.GetInstance ().config.userServerIP + ":" + fileMgr.GetInstance ().config.userServerPort.ToString () +
										"/LoginServer/webservice/game/loginVersion?username=" + account +
										"&password=" + password + "&version=" + fileMgr.GetInstance ().config.version;
						} else {
								URL = "http://" + fileMgr.GetInstance ().config.userServerIP + ":" + fileMgr.GetInstance ().config.userServerPort.ToString () +
										"/LoginServer/webservice/game/login?username=" + account + "&password=" + password;
						}
				}*/

				//GameDataCenter.GetInstance().userName = account;
				//GameDataCenter.GetInstance().password = password;
		
				Debug.Log (URL);
				string str = CreateGetHttpResponse (URL, 10000, null);
				//HttpWebResponse response = CreateGetHttpResponse(URL,10000,null);
				//Stream stream = response.GetResponseStream();

				//StreamReader reader = new StreamReader(stream);

				/*if(response!=null)
		{
			response.Close();
		}
		if(reader!=null)
		{
			reader.Close();
		}
		System.GC.Collect();*/

				//string str = reader.ReadToEnd();
				//TipOneButtom.Callback loseNetAction = ()=>{
				//	gameGlobal.g_LoadingPage.show();
				//	HttpTect(account,password);
				//};
				//if(str == "  <html></html>")
				//{
				//	gameGlobal.g_LoadingPage.hide();
				//	gameGlobal.g_tipOneButtom.Show("", gameGlobal.GetStr(317), loseNetAction);
				//	return;
				//}

				Dictionary<string,object> dic = (Dictionary<string,object>)Json.Deserialize (str);

				if (dic.ContainsKey ("flag")) {
						int flag = (int)(dic ["flag"] as object);
						PlayerPrefs.SetString ("username", account);
						PlayerPrefs.GetString ("password", password);
						//gameGlobal.g_LoadingPage.hide();
						//gameGlobal.g_LoginUI.show();
				}
				/*MGResouce.LoadUIData uiData = new MGResouce.LoadUIData();
		uiData.name = "ServerListPanel";
		uiData.LoadUICallBack = gameGlobal.ShowServerList;
		uiData.packName = "ServerListPanel";
		MGResouce.BundleMgr.Instance.LoadUI(uiData);*/
				//gameGlobal.g_LoginUI.hide();
		
	}

	 public static string CreateGetHttpResponse(string url,int timeout, string userAgent)  
	 {  
		HttpWebRequest request =null;
		HttpWebResponse response =null;
		StreamReader reader =null;
		Stream stream = null;
		string str =null;
        if (string.IsNullOrEmpty(url))  
        {  
            throw new ArgumentNullException("url");  
        } 
		try
		{
			request = WebRequest.Create(url) as HttpWebRequest;  
	        request.Method = "GET";   
	        request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";;  
	        if (!string.IsNullOrEmpty(userAgent))  
	        {  
	            request.UserAgent = userAgent;  
	        }  
			if(timeout != 0){
				request.Timeout = timeout ;
			}
			response = request.GetResponse() as HttpWebResponse;
			//HttpWebResponse response = CreateGetHttpResponse(url,10000,null);
			stream = response.GetResponseStream();
			reader = new StreamReader(stream);
			str = reader.ReadToEnd();
		}
		catch
		{

		}
		finally
		{
			
			if(response!=null)
			{
				response.Close();
			}
			if(stream != null)
			{
				stream.Close();
			}
			if(reader!=null)
			{
				reader.Close();
			}
			if(request!=null)
			{
				request.Abort();
			}
			System.GC.Collect();
		}

		return   str;
	  }  
	static public bool InitNetWork()
	{
		if(fileMgr.GetInstance().config.connectServerIP == "")
			return false ;
	    NetModuleInit netModuleInit = new NetModuleInit();
        netModuleInit.bufferReserves = 10;
        netModuleInit.sendBufSize = 8192;
        netModuleInit.sendBufExtend = 6000;
        netModuleInit.rcvBufSize = 8192;            
        netModuleInit.rcvBufExtend = 6000;          
        netModuleInit.sessionInitCount = 4;      
        netModuleInit.sessionExtendCount = 2;    
        netModuleInit.msgBufferCounts = 4;       
        netModuleInit.msgBufferSize = 8192;      
        netModuleInit.netSessionClosedCallbackFunc = AppNetSessionClosed;         
        netModuleInit.netSessionConnectedCallbackFunc = AppNetSesssionConnected;  

       
        NetFrameMgr.GetInstance().Init(ref netModuleInit);

        GameMessgeHandle.MessageRegister.RegisterMessage();
		
     	string ip = fileMgr.GetInstance().config.userServerIP;
        bool connectRet = false;
		netSession = NetFrameMgr.GetInstance().Connect(stringToEditSeverIP, Gameport, true, ref connectRet);

		if(connectRet == true ){
			chatSession= NetFrameMgr.GetInstance().Connect(chatSeverIP, chatPort, true, ref connectRet);
		}

		return true;
	}
	
	void Update () {
		/*
			if(cdTime >= 120000)
			{
				SendNoReturnNetMessage(PlayerMessageRegister.HEART_BEAT, null);
				heart_beat = GameDataCenter.GetInstance().userLoginData.time;
			}

			if(scoketCdTime > 5000){
				if(GameDataCenter.GetInstance().SocketReset == 1 && !shutDownNet)
				{
					TipOneButtom.Callback ReloginCallBack = ()=>{
							Relogin();
					};
					//if()
					NewPlayerGuide.curGuide = NewPlayerGuide.m_GuideEnd;
					GuideCheck2Message msg = new GuideCheck2Message();
					msg.guideStep = NewPlayerGuide.curGuide;
					NewPlayerGuide.GetInstance().OnMessage(msg);
					
					//NewPlayerGuide.CheckGuide2((NewPlayerGuide.GuideType)3204);
					gameGlobal.g_tipOneButtom.Show("", "请重新连接网络", ReloginCallBack);					
				}
			}*/
		/*
		if(heart_beat < 120)
			heart_beat += Time.deltaTime;
		else{
			heart_beat = 0;
			SendNoReturnNetMessage(PlayerMessageRegister.HEART_BEAT, null);
		}*/
		//print(Time.time);
			/*
        if(netSession!=null)
        {
            NetFrameMgr.GetInstance().Update();
        }
		if(netSession != null){
			
		}
		
		if(gameGlobal.g_gameState == GameState.GAME_STATE_MAIN_GAME){
			deltaTime += Time.deltaTime ;
			UICommon.Update();
			EventMgr.GetInstance().UpdateEvent();
			//GameLogical.CSmoothMgr.GetInstance().Update();
			//CLineSmoothMgr.GetInstance().Update(Time.deltaTime);
			EnitityMgr.GetInstance().Update(Time.deltaTime);
			GameLogical.GameLevel.GameLevelMgr.GetInstance().Update(Time.deltaTime);
		}
		*/
	}

	public static void SendNetMessage(string head, Dictionary<string,object> dataList)
	{
		print("send message:" + head) ;
		if(netSession == null)
		{
			//gameGlobal.g_tipOneButtom.Show(gameGlobal.GetStr(1024));
			return;
		}
		netSession.SendMessage(head,dataList);
		//gameGlobal.g_LoadingPage.show();
		if(GameDataCenter.GetInstance().userLoginData != null){
			heart_beat = GameDataCenter.GetInstance().userLoginData.time ;
		}

	}

	public static void SendNoReturnNetMessage(string head, Dictionary<string,object> dataList)
	{
		print("send message:" + head) ;
		netSession.SendMessage(head,dataList);
		heart_beat = GameDataCenter.GetInstance().userLoginData.time ;
	}
	
	public static void SendChatMessage(string head, Dictionary<string,object> dataList){
		if(chatSession != null){
			print("send chat message :" + head) ;
			chatSession.SendMessage(head,dataList);
			//gameGlobal.g_LoadingPage.show();
		}
	}

	public static void ReChangeAccount(){
		
		main.SendNoReturnNetMessage(PlayerMessageRegister.CLOSE_GAME, null);
		if(netSession != null){
			NetFrameMgr.GetInstance().CloseNetSession(netSession.GetSessionID());
			netSession = null ;
		}
		if(chatSession != null){
			NetFrameMgr.GetInstance().CloseNetSession(chatSession.GetSessionID());
			chatSession = null ;
		}
		NetFrameMgr.GetInstance().Shutdown();
		GameObject parentUI =GameObject.Find("UI");
		//gameGlobal.g_curPage.hide();
		for(int i = parentUI.transform.GetChildCount() - 1; i > 0; i--)
		{
			Destroy(parentUI.transform.GetChild(i).gameObject);
		}
		parentUI =GameObject.Find("UI Root (2D)/Camera/Anchor/Panel");
		for(int i = parentUI.transform.GetChildCount() - 1; i > 0; i--)
		{
			Destroy(parentUI.transform.GetChild(i).gameObject);
		}

		shutDownNet = true;
		fileMgr.GetInstance().monsterCsvData.dataDic.Clear();
		fileMgr.GetInstance().resouceCsvData.dataDic.Clear();
		//fileMgr.GetInstance().guideCsvData.dataDic.Clear();
		//fileMgr.GetInstance().NameCsvData.dataDic.Clear();
		//fileMgr.GetInstance().vipCsvData.dataDic.Clear();
		fileMgr.GetInstance().skillCsvData.dataDic.Clear();
		fileMgr.GetInstance().buffCsvData.dataDic.Clear();
		fileMgr.GetInstance().playerCsvData.dataDic.Clear();
		fileMgr.GetInstance().itemCsvData.dataDic.Clear();
		fileMgr.GetInstance().talkCsvData.dataDic.Clear();
		//fileMgr.GetInstance().minLevelData.dataDic.Clear();
		//fileMgr.GetInstance().searchingData.dataDic.Clear();
		//fileMgr.GetInstance().achieveData.dataDic.Clear();
		//fileMgr.GetInstance().titleData.dataDic.Clear();
		fileMgr.GetInstance().petCsvData.dataDic.Clear();
		fileMgr.GetInstance().skillLvCsvData.dataDic.Clear();
		fileMgr.GetInstance().taskCsvData.dataDic.Clear();
		fileMgr.GetInstance().expUpCsvData.dataDic.Clear();
		//fileMgr.GetInstance().levelCsvData.dataDic.Clear();

		//gameGlobal.g_loginUIPanel.show();
		//gameGlobal.g_loginUIPanel.isBeginGame = true;
	}
	
	public void Relogin(){
		if(netSession != null){
			NetFrameMgr.GetInstance().CloseNetSession(netSession.GetSessionID());
			netSession = null ;
		}
		if(chatSession != null){
			NetFrameMgr.GetInstance().CloseNetSession(chatSession.GetSessionID());
			chatSession = null ;
		}
		NetFrameMgr.GetInstance().Shutdown();
		Reconnect();
		//Invoke("Reconnect",10.0f);
		//bool connectRet = false;
		//netSession = NetFrameMgr.GetInstance().Connect(stringToEditSeverIP, Gameport, true, ref connectRet);
		//print("reconnect session id :" + netSession.GetSessionID()) ;
	}

	void Reconnect(){
		bool connectRet = InitNetWork();
		print("reconnect :" + connectRet);
		if(connectRet == false){
			Invoke("Reconnect",2.0f);
		}
		else if(connectRet == true){
			//GameMessgeHandle.MessageRegister.RegisterMessage();
			Dictionary<string,object> dic =  new Dictionary<string, object>();
			dic.Add("userId",GameDataCenter.GetInstance().userLoginData.userDto.userId);
			dic.Add("playerId",GameDataCenter.GetInstance().playerData.playerId);
			SendNetMessage(LoginMessageRegister.COMMON_RESET_LOGIN,dic);
			
			GameDataCenter.GetInstance().SocketReset = 2;
		}
	}
}
