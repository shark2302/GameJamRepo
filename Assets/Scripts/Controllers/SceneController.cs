using System;
using UnityEngine;

namespace DefaultNamespace
{
	public class SceneController : MonoBehaviour
	{
		
		
		private void Awake()
		{
			AppController.SceneController = this;
		}
		
		
	}
}