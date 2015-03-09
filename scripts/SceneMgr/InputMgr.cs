using UnityEngine;
using System.Collections;
using GameEntity;
using GameLogic.AI;

public class InputMgr : MonoBehaviour {

	EventMessageBase message; 

    public int x;
    public int y;
    public int width;
    public int height;


	void Start () {
		message = new EventMessageBase ();
		message.eventMessageModel = EventMessageModel.eEventMessageModel_PLAY_MOVE_STATE;
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR || UNITY_STANDALONE_WIN 
		if(Input.GetKey(KeyCode.W))
		{
			MoveTop();
		}
		else if(Input.GetKey(KeyCode.S))
		{
			MoveBottom();
		}
		else if(Input.GetKey(KeyCode.A))
		{
			MoveLeft();
		}
		else if(Input.GetKey(KeyCode.D))
		{
			MoveRight();
		}
		else if(Input.GetKey(KeyCode.J))
		{
			Attack();
		}
		else if(Input.GetKey(KeyCode.K))
		{
			Skill();
		}
		#elif UNITY_ANDROID 
		
		#endif
	}

    void OnGUI() 
    {
        #if UNITY_ANDROID 
        if (GUI.Button(new Rect(65, 320, 50, 50), "W"))
        {
            MoveTop();
        }
        else if (GUI.Button(new Rect(65, 400, 50, 50), "S"))
        {
            MoveBottom();
        }
        else if (GUI.Button(new Rect(6, 360, 50, 50), "A"))
        {
            MoveLeft();
        }
        else if (GUI.Button(new Rect(121, 360, 50, 50), "D"))
        {
            MoveRight();
        }
        else if (GUI.Button(new Rect(600, 360, 50, 50), "J"))
        {
            Attack();
        }
        #endif
    }

	void MoveTop()
	{
		message.eventMessageModel = EventMessageModel.eEventMessageModel_PLAY_MOVE_STATE;
		message.eventMessageAction = (int)EnitityCommon.EnitityAction.ENITITY_ACTION_MOVETOP;
		CCearcueMgr.GetInstance ().player.OnMessage (message);
	}

	void MoveBottom()
	{
		message.eventMessageModel = EventMessageModel.eEventMessageModel_PLAY_MOVE_STATE;
		message.eventMessageAction = (int)EnitityCommon.EnitityAction.ENITITY_ACTION_MOVEBOTTOM;
		CCearcueMgr.GetInstance ().player.OnMessage (message);
	}

	void MoveLeft()
	{
		message.eventMessageModel = EventMessageModel.eEventMessageModel_PLAY_MOVE_STATE;
		message.eventMessageAction = (int)EnitityCommon.EnitityAction.ENITITY_ACTION_MOVELEFT;
		CCearcueMgr.GetInstance ().player.OnMessage (message);
	}

	void MoveRight()
	{
		message.eventMessageModel = EventMessageModel.eEventMessageModel_PLAY_MOVE_STATE;
		message.eventMessageAction = (int)EnitityCommon.EnitityAction.ENITITY_ACTION_MOVERIGHT;
		CCearcueMgr.GetInstance ().player.OnMessage (message);
	}

	void Skill()
	{
		Debug.Log("~~~~~~~~~~Skill~~~~~~~~~~~~~");
	}

	void Attack()
	{
		message.eventMessageModel = EventMessageModel.eEventMessageModel_PLAY_ATTACK_STATE;
		message.eventMessageAction = (int)EnitityCommon.EnitityAction.ENITITY_ACTION_FIGHT;
		CCearcueMgr.GetInstance ().player.OnMessage (message);
	}
}
