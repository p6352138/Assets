using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;

public class SocketTest : MonoBehaviour {

    public string IP;
    public int port;

    Socket socket;

	// Use this for initialization
	void Start () {
        IPEndPoint point = null;
        IPAddress address = IPAddress.Parse(IP);
        point = new IPEndPoint(address, port);

        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        catch(Exception e)
        {
            throw (e);
        }
        


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
