using UnityEngine;
using System.Collections;
using System ;
using AppUtility;
using NetWork.NetSession;
using NetWork.NetFrame;
using NetWork.NetModule;
using NetWork.NetDefine;
using System.Collections.Generic;
using System.IO;  
//using ICSharpCode.SharpZipLib;
//using ICSharpCode.SharpZipLib.GZip ;

namespace GameMessgeHandle
{
    public class MessgeHandle
    {
		static Dictionary<string,JsonHandleMsg> msgHandleDic = new Dictionary<string, JsonHandleMsg>();
		static int messageNum = 0 ;
        public static void HandleMsg2000(INetSession netSession, byte[] msgBuffer, Int32 bufferOffset, Int32 msgLength)
        {
			//gameGlobal.g_LoadingPage.hide();
			Debug.Log("bufferOffset :" + bufferOffset + "msgLength:" + msgLength);
			string data = System.Text.Encoding.UTF8.GetString ( msgBuffer ,bufferOffset, msgLength);
			//get the object from Json
	    	Dictionary<string,object> search = (Dictionary<string,object>) Json.Deserialize(data);
			//TestBody ts = new TestBody(search);
			messageNum++ ;
			//Debug.Log("message count :" + messageNum + "head:" + ts.head);
			//if(!msgHandleDic.ContainsKey(ts.head)){
			//	Debug.Log("head:" + ts.head + "had not register");
			//	return ;
			//}
			//msgHandleDic[ts.head](ts.errorCode,ts.body);
        }
		
		public static void HandleMsg2001(INetSession netSession, byte[] msgBuffer, Int32 bufferOffset, Int32 msgLength)
        {
			//gameGlobal.g_LoadingPage.hide();
			//Debug.Log("bufferOffset :" + bufferOffset + "msgLength:" + msgLength);
			byte[] dataBuffer = Decompress(msgBuffer,bufferOffset,msgLength);
			string data = System.Text.Encoding.UTF8.GetString ( dataBuffer );
			//get the object from Json
	    	Dictionary<string,object> search = (Dictionary<string,object>) Json.Deserialize(data);
			//TestBody ts = new TestBody(search);
			messageNum++ ;
			//Debug.Log("message count :" + messageNum + "head:" + ts.head);
			
			//if(!msgHandleDic.ContainsKey(ts.head)){
			//	Debug.Log("head:" + ts.head + "had not register");
			//	return ;
			//}
			//msgHandleDic[ts.head](ts.errorCode,ts.body);
        }
		
		
	    public static byte[] Decompress(byte[] zippedData,int index,int len)  
        {  
            MemoryStream ms = new MemoryStream(zippedData,index,len);  
            //GZipInputStream sm = new GZipInputStream(ms);
			
            MemoryStream outBuffer = new MemoryStream();  
            byte[] block = new byte[2048];  
            while (true)  
            {  
                //int bytesRead = sm.Read(block, 0, block.Length);  
                //if (bytesRead <= 0)  
                //    break;  
                //else  
                //    outBuffer.Write(block, 0, bytesRead);  
            }  
            //sm.Close();  
            return outBuffer.ToArray();  
        }
		
		public static bool RegisterMsgHanle(string head,JsonHandleMsg handle){
			if (handle == null)
            {
                Debug.Log("msgHandle is null");
                return false;
            }

            if (msgHandleDic.ContainsKey(head))
            {
                Debug.Log("reregister cmd" + head);
                return false;
            }

            msgHandleDic.Add(head, handle);

            return true;
		}
    }
}
