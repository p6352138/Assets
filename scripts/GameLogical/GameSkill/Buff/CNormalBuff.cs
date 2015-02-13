using UnityEngine;
using System.Collections;
using GameLogical.GameEnitity.AI ;
using GameEvent ;

namespace GameLogical.GameSkill.Buff{
	public class CNormalBuff : IBuffBase
	{
		protected		BuffDataBass					m_buffData ;
		protected		StateMachine<CNormalBuff>		m_StateMachine ;
		protected		GameObject						m_effectObject ;
		
		///////////////////////////////////interface////////////////////////////////
		
		/// <summary>
		/// Init this instance.
		/// </summary>
		public void Init(){
			m_StateMachine = new StateMachine<CNormalBuff>(this);
		}
		
		/// <summary>
		/// Release this instance.
		/// </summary>
		public void Release(){
			
		}
		
		/// <summary>
		/// Gets the effect object.
		/// </summary>
		/// <returns>
		/// The object.
		/// </returns>
		public GameObject GetObject(){
			return m_effectObject ;
		}
		
		/// <summary>
		/// handle the event message .
		/// </summary>
		/// <param name='message'>
		/// Message.
		/// </param>
		public void	OnMessage(EventMessageBase message){
			
		}
		
		///////////////////////////////////get set data////////////////////////////////
		public BuffDataBass buffData{
			get{
				return m_buffData ;
			}
			set{
				m_buffData = value;
			}
		}
	}

}

