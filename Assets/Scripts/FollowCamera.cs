using System;
using UnityEngine;

namespace DefaultNamespace
{
	public class FollowCamera : MonoBehaviour
	{
		[SerializeField] 
		private GameObject FollowedObject;

		[SerializeField] 
		private GameObject Fight;

		private bool _notFollowPlayer;

		private void Awake()
		{
			AppController.Camera = this;
		}

		private void Update()
		{
			if (!_notFollowPlayer)
			{
				var position = FollowedObject.transform.position;
				transform.position = new Vector3(position.x, position.y, transform.position.z);
			}
		}

		public void MoveToFight()
		{
			_notFollowPlayer = true;
			transform.position = new Vector3(Fight.transform.position.x, Fight.transform.position.y, transform.position.z);
		}

		public void MoveToHero()
		{
			_notFollowPlayer = false;
		}

		public void ChangeFollowObject(GameObject follow)
		{
			FollowedObject = follow;
		}
	}
}