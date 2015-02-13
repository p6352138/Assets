using UnityEngine ;
using GameEvent ;
namespace GameLogical.GameSkill.Buff{
	public interface IBuffBase
	{
		void			Init()			;
		void			Release()		;
		GameObject		GetObject()		;
		void			OnMessage(EventMessageBase message)		;
	}
}


