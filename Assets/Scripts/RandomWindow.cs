using System;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;


namespace DefaultNamespace
{
	public class RandomWindow : MonoBehaviour
	{
		public Action<int> RandomNumberGenerated;

		public Text NumberText;

		public Button AcceptButton;

		public Button DropButton;

		public GameObject MissText;

		private int _lastDrop;

		private Random _random = new Random();

		private int _damageFrom;

		private int _damageTo;

		private bool _dropOnce;

		public void SetData(int damageFrom, int damageTo)
		{
			AcceptButton.interactable = false;
			DropButton.interactable = true;
			_damageFrom = damageFrom;
			_damageTo = damageTo;
			_lastDrop = 0;
			NumberText.text = string.Empty;
			MissText.SetActive(false);
		}

		public void OnDropButton()
		{
			int drop = _random.Next(_damageFrom, _damageTo + 1);
			NumberText.text = drop.ToString();
			if (_lastDrop != 0 && drop < _lastDrop)
			{
				RandomNumberGenerated.Invoke(0);
				MissText.SetActive(true);
				DropButton.interactable = false;
				AcceptButton.interactable = false;
			}
			else if (_lastDrop != 0 && drop == _lastDrop)
			{
				RandomNumberGenerated.Invoke(drop);
				DropButton.interactable = false;
				AcceptButton.interactable = false;
			}
			else
			{
				_lastDrop = drop;
				AcceptButton.interactable = true;
			}
		}

		public void OnAcceptButtonClick()
		{
			RandomNumberGenerated.Invoke(_lastDrop);
			DropButton.interactable = false;
			AcceptButton.interactable = false;
		}

	}
}