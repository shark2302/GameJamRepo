using System;
using UnityEngine;

namespace DefaultNamespace
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class DialogTriggerZone : MonoBehaviour
	{
		public NPC.Dialog Dialog;

		public bool TriggerOncePerSession;

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.TryGetComponent<Movement>(out var m) && (TriggerOncePerSession && !AppController.GameplayHudController.CheckDialogShowedOnce(Dialog) || !TriggerOncePerSession))
			{
				AppController.GameplayHudController.SetCurrentDialog(Dialog);
				AppController.GameplayHudController.ShowDialog(Dialog.DialogElements);
			}
		}
	}
}