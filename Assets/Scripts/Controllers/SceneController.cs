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

		public void SwitchToFightScene(GameObject[] enemyMobs, NPC npc, Sprite back)
		{
			OpenWorld.SetActive(false);
			Fight.SetActive(true);
			AppController.FightController.SetData(enemyMobs, npc, back);
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