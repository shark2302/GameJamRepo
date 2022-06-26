using System;
using UnityEngine;

namespace DefaultNamespace
{
	public class SceneController : MonoBehaviour
	{
		public GameObject OpenWorld;
		public GameObject Fight;
		private void Awake()
		{
			AppController.SceneController = this;
		}

		public void SwitchToFightScene(GameObject[] enemyMobs, NPC npc)
		{
			OpenWorld.SetActive(false);
			Fight.SetActive(true);
			AppController.FightController.SetData(enemyMobs, npc);
			AppController.Camera.MoveToFight();
			AppController.Music.SwitchToFightMusic();
			AppController.GameplayHudController.HideProgressBar();
		}

		public void SwitchToOpenWorldScene()
		{
			OpenWorld.SetActive(true);
			Fight.SetActive(false);
			AppController.Camera.MoveToHero();
			AppController.Music.SwitchToWorldMusic();
			AppController.GameplayHudController.ShowProgressBar();
		}
	}
}