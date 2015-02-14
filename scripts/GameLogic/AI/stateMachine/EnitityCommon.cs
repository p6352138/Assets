﻿using UnityEngine;
using System.Collections;

namespace GameLogic.AI{
	public class EnitityCommon  {
		//message
		public enum EnitityAction{
			ENITITY_ACTION_NULL ,
			ENITITY_ACTION_CREATE,
			ENITITY_ACTION_FIGHT,
			ENITITY_ACTION_ESCAPE,
			ENITITY_ACTION_CHANGE_PET,
			ENITITY_ACTION_CHANGE_PET_UP,
			//attack animation event action
			ENITITY_ACTION_FIGHT_SATRT, 
			ENITITY_ACTION_FIGHT_FINISH,
			ENITITY_ACTION_DEATH,
			ENITITY_ACTION_WEAK,
			ENITYTY_ACTION_SKILL,
			ENITYTY_ACTION_SKILL_START,
			ENITITY_ACTION_SKILL_FINISH,
			//
			ENITITY_ACTION_ENTER_COLLIDER,
			ENITITY_ACTION_EXIT_COLLIDER,
			//skill
			ENITITY_ACTION_SELECT_ENITITY,
			ENITITY_ACTION_SELECT_SKILL,
			ENITITY_ACTION_CANCEL_SELECT_SKILL,
			ENITITY_ACTION_LOCK_PET,
			ENITITY_ACTION_BE_DRAW,
			ENITITY_ACTION_START_ANGRY_SKILL,
		}
		
		//create enitity message
		public class EventMessageCreateEnitity : EventMessageBase{

			public EventMessageCreateEnitity()
			{

			}
		}

	}
}
