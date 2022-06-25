using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
	public class GameplayHUDController : MonoBehaviour
	{

		[Serializable]
		public class DialogView
		{
			public GameObject View;
			public Text Text;
			public Image Image;
			public Text Name;
		}
		
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

		[SerializeField] 
		private DialogView _npcView;

		[SerializeField] 
		private DialogView _heroView;

		private Queue<NPC.DialogElement> _dialogQueue;

		private void Awake()
		{
			AppController.GameplayHudController = this;
		}
		
		private void Start()
		{
			_figthButton.gameObject.SetActive(false);
			_dialogButton.gameObject.SetActive(false);
			_dialogPanel.SetActive(false);
			_npcView.View.SetActive(false);
			_heroView.View.SetActive(false);
		}

		public void SetActiveFigthButton(bool state)
		{
			_figthButton.gameObject.SetActive(state);
		}

		public void SetActiveDialogButton(bool state, NPC.DialogElement[] dialog)
		{
			_dialogButton.gameObject.SetActive(state);
			_dialogQueue = new Queue<NPC.DialogElement>(dialog);
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
				SetDialogView(_dialogQueue.Dequeue());
				DialogStartedEvent?.Invoke();
				_dialogButton.gameObject.SetActive(false);
			}
		}

		public void OnNextDialogButtonClick()
		{
			if (_dialogQueue.Count > 0)
			{
				var element = _dialogQueue.Dequeue();
				SetDialogView(element);
			}
			else
			{
				_dialogPanel.SetActive(false);
				DialogEndedEvent?.Invoke();
				_dialogQueue = null;
			}
		}

		private void SetDialogView(NPC.DialogElement dialog)
		{
			if (dialog.SpeakerType == NPC.SpeakerType.NPC)
			{
				_npcView.View.SetActive(true);
				_heroView.View.SetActive(false);
				_npcView.Image.sprite = dialog.Sprite;
				_npcView.Name.text = dialog.Name;
				_npcView.Text.text = dialog.Text;
			}
			else if (dialog.SpeakerType == NPC.SpeakerType.HERO)
			{
				_npcView.View.SetActive(false);
				_heroView.View.SetActive(true);
				_heroView.Image.sprite = dialog.Sprite;
				_heroView.Name.text = dialog.Name;
				_heroView.Text.text = dialog.Text;
			}
		} 
	}
}