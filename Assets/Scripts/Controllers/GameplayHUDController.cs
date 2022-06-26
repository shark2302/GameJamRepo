using System;
using System.Collections.Generic;
using DefaultNamespace.Utils;
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

		public Image ProgressBar;

		public Sprite[] BarImages;
		
		[SerializeField] 
		private Button _figthButton;

		[SerializeField] 
		private Button _dialogButton;

		[SerializeField] 
		private GameObject _dialogPanel;
		
		[SerializeField] 
		private DialogView _npcView;

		[SerializeField] 
		private DialogView _heroView;

		private HashSet<NPC.Dialog> _endedDialogs;
		
		private Queue<NPC.DialogElement> _dialogQueue;
		private NPC.NPCDialogData _currentDialogData;
		private NPC.Dialog _currentDialog;

		private void Awake()
		{
			AppController.GameplayHudController = this;
			_endedDialogs = new HashSet<NPC.Dialog>();
		}
		
		private void Start()
		{
			_figthButton.gameObject.SetActive(false);
			_dialogButton.gameObject.SetActive(false);
			_dialogPanel.SetActive(false);
			_npcView.View.SetActive(false);
			_heroView.View.SetActive(false);
			UpdateProgressBar();
		}

		public void SetActiveFigthButton(bool state)
		{
			_figthButton.gameObject.SetActive(state);
		}

		public void SetActiveDialogButton(bool state)
		{
			_dialogButton.gameObject.SetActive(state);
		}

		public void SetDialogData(NPC.NPCDialogData dialogData)
		{
			_currentDialogData = dialogData;
			
		}

		public void OnShowDialogClick()
		{
			ShowDialog(_currentDialogData.StartDialog.DialogElements);
			_currentDialog = _currentDialogData.StartDialog;
		}

		public void SetCurrentDialog(NPC.Dialog dialog)
		{
			_currentDialog = dialog;
		}

		public void ShowDialog(NPC.DialogElement[] dialog)
		{
			_dialogQueue = new Queue<NPC.DialogElement>(dialog);
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
				_endedDialogs.Add(_currentDialog);
				if (_currentDialog != null && _currentDialog.StartFightAfterDialog)
				{
					AppController.SceneController.SwitchToFightScene(_currentDialogData.Mobs);
					AppController.FightController.FightEndedEvent += OnFightEndedEvent;
				}
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
		
		private void OnFightEndedEvent(bool win)
		{
			if (win)
			{
				ShowDialog(_currentDialogData.WinDialog.DialogElements);
				_currentDialog = _currentDialogData.WinDialog;
			}
			else
			{
				ShowDialog(_currentDialogData.LoseDialog.DialogElements);
				_currentDialog = _currentDialogData.WinDialog;
			}

			_currentDialog = null;
			AppController.FightController.FightEndedEvent -= OnFightEndedEvent;
		}

		public bool CheckDialogShowedOnce(NPC.Dialog dialog)
		{
			return _endedDialogs.Contains(dialog);
		}

		public void HideProgressBar()
		{
			ProgressBar.gameObject.SetActive(false);
		}

		public void ShowProgressBar()
		{
			ProgressBar.gameObject.SetActive(true);
			UpdateProgressBar();
		}

		private void UpdateProgressBar()
		{
			var wins = CachedParams.GetWinCount();
			if (wins == 0)
			{
				ProgressBar.sprite = BarImages[0];
			}
			else if (wins == 1)
			{
				ProgressBar.sprite = BarImages[4];
			}
			else if (wins == 2)
			{
				ProgressBar.sprite = BarImages[0];
			}
			else if (wins == 3)
				ProgressBar.sprite = BarImages[1];
			else if (wins == 4)
				ProgressBar.sprite = BarImages[2];
			else if (wins == 5)
				ProgressBar.sprite = BarImages[3];
			else if(wins == 6)
				ProgressBar.sprite = BarImages[4];
			else if (wins >= 7)
			{
				ProgressBar.sprite = BarImages[5];
			}
			

		}
	}
}