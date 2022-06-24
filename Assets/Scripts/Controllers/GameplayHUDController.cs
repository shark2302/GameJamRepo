using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
	public class GameplayHUDController : MonoBehaviour
	{
		public Action DialogStartedEvent;

		public Action DialogEndedEvent;
		
		[SerializeField] 
		private Button _figthButton;

		[SerializeField] 
		private Button _dialogButton;

		[SerializeField] 
		private GameObject _dialogPanel;

		[SerializeField] 
		private Text _dialogText;

		private Queue<string> _dialogQueue;

		private void Awake()
		{
			AppController.GameplayHudController = this;
		}
		
		private void Start()
		{
			_figthButton.gameObject.SetActive(false);
			_dialogButton.gameObject.SetActive(false);
			_dialogPanel.SetActive(false);
		}

		public void SetActiveFigthButton(bool state)
		{
			_figthButton.gameObject.SetActive(state);
		}

		public void SetActiveDialogButton(bool state, string[] dialog)
		{
			_dialogButton.gameObject.SetActive(state);
			_dialogQueue = new Queue<string>(dialog);
		}

		public void OnShowDialogClick()
		{
			ShowDialog();
		}

		private void ShowDialog()
		{
			if (_dialogQueue.Count > 0)
			{
				_dialogPanel.SetActive(true);
				_dialogText.text = _dialogQueue.Dequeue();
				DialogStartedEvent?.Invoke();
				_dialogButton.gameObject.SetActive(false);
			}
		}

		public void OnNextDialogButtonClick()
		{
			if(_dialogQueue.Count > 0)
				_dialogText.text = _dialogQueue.Dequeue();
			else
			{
				_dialogPanel.SetActive(false);
				DialogEndedEvent?.Invoke();
				_dialogQueue = null;
			}
		}
	}
}