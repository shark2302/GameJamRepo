using System;
using DefaultNamespace.Utils;
using UnityEngine;

namespace DefaultNamespace
{
	public class FogZone : MonoBehaviour
	{
		public int NeedWinToOpen;

		private void OnEnable()
		{
			if (CachedParams.GetWinCount() >= NeedWinToOpen)
			{
				Destroy(gameObject);
			}
			else if(AppController.FightController != null)
			{
				AppController.FightController.FightEndedEvent += OnFightEndedEvent; 
			}
		}

		private void OnDisable()
		{
			if(AppController.FightController != null)
			{
				AppController.FightController.FightEndedEvent -= OnFightEndedEvent; 
			}
		}

		private void OnFightEndedEvent(bool win)
		{
			if (CachedParams.GetWinCount() >= NeedWinToOpen)
			{
				Destroy(gameObject);
			}
		}
	}
}