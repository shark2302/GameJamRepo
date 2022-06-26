using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;


namespace DefaultNamespace
{
	public class RandomWindow : MonoBehaviour
	{
		public Action<int> RandomNumberGenerated;

		public Image DiceImage;
		
		public Text NumberText;

		public Button AcceptButton;

		public Button DropButton;

		public GameObject MissText;

		private int _lastDrop;

		private Random _random = new Random();

		private int _damageFrom;

		private int _damageTo;

		private bool _dropOnce;

		private Sprite[] _dropAnimations;

		public void SetData(int damageFrom, int damageTo, Sprite[] dropAnimationsSprite)
		{
			AcceptButton.interactable = false;
			DropButton.interactable = true;
			_damageFrom = damageFrom;
			_damageTo = damageTo;
			_lastDrop = 0;
			NumberText.text = string.Empty;
			DiceImage.gameObject.SetActive(false);
			_dropAnimations = dropAnimationsSprite;
			MissText.SetActive(false);
		}

		public void OnDropButton()
		{
			DiceImage.gameObject.SetActive(true);
			StartCoroutine(DropAnimation());
		}

		public void ShowResult()
		{
			NumberText.gameObject.SetActive(true);
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

		private IEnumerator DropAnimation()
		{
			NumberText.gameObject.SetActive(false);
			for (int i = 0; i < 12; i++)
			{
				DiceImage.sprite = _dropAnimations[i % _dropAnimations.Length];
				yield return new WaitForSeconds(0.05f);
			}

			DiceImage.sprite = _dropAnimations[0];
			ShowResult();
		}

		public void OnAcceptButtonClick()
		{
			RandomNumberGenerated.Invoke(_lastDrop);
			DropButton.interactable = false;
			AcceptButton.interactable = false;
		}

	}
}