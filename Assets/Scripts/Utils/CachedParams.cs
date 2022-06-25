using UnityEngine;

namespace DefaultNamespace.Utils
{
	public class CachedParams
	{
		public static void AddWin()
		{
			PlayerPrefs.SetInt("Wins", GetWinCount() + 1);
		}

		public static int GetWinCount()
		{
			return PlayerPrefs.GetInt("Wins");
		}
	}
}