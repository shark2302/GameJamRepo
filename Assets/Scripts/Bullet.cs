using System;
using UnityEngine;

namespace DefaultNamespace
{
	public class Bullet : MonoBehaviour
	{
		public float Speed;
		
		private Fighter _target;

		public void SetTarget(Fighter target)
		{
			_target = target;
		}

		private void Update()
		{
			gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position,
				_target.gameObject.transform.position, Speed * Time.deltaTime);

			if (gameObject.transform.position == _target.gameObject.transform.position)
			{
				_target.PlayDamagedAnimation();
				Destroy(gameObject);
			}
				
				
		}
	}
}