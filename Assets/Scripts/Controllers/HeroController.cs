using System;
using DefaultNamespace.Utils;
using UnityEngine;

namespace DefaultNamespace
{
	public class HeroController : MonoBehaviour
	{
		public GameObject SpawnPoint;
		public GameObject Parent;
		public GameObject Level1Prefab;
		public GameObject Level2Prefab;

		private GameObject _currentHero;
		private void Awake()
		{
			AppController.Hero = this;
		}

		private void Start()
		{
			
			if (CachedParams.GetWinCount() >= 2 && Level2Prefab != null)
			{
				_currentHero= Instantiate(Level2Prefab, SpawnPoint.transform.position, Quaternion.identity, Parent.transform);
				
			}
			else
			{ _currentHero = Instantiate(Level1Prefab, SpawnPoint.transform.position, Quaternion.identity, Parent.transform);
			}
			AppController.Camera.ChangeFollowObject(_currentHero);
		}

		public void ChangeHeroLevel()
		{
			if (_currentHero != null)
			{
				var go = _currentHero;
				Destroy(_currentHero);
				_currentHero = Instantiate(Level2Prefab, go.transform.position, Quaternion.identity, Parent.transform);
				AppController.Camera.ChangeFollowObject(_currentHero);
			}
		}
	}
}