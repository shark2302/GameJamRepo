using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class NPC : MonoBehaviour
{
   public enum NPCInteractionType
   {
      FIGHT,
      DIALOG
   }
   
   public enum SpeakerType
   {
      NPC, 
      HERO
   }

   [Serializable]
   public class DialogElement
   {
      public string Name;
      public Sprite Sprite;
      public SpeakerType SpeakerType;
      public string Text;
   }

   [Serializable]
   public class Dialog
   {
      public DialogElement[] DialogElements;
      public bool StartFightAfterDialog;
   }

   [Serializable]
   public class NPCDialogData
   {
      public Dialog StartDialog;
      public Dialog WinDialog;
      public Dialog LoseDialog;
      public GameObject[] Mobs;
   }

   [SerializeField]
   private NPCInteractionType _interactionType;

   [SerializeField] 
   private NPCDialogData _dialogData;
   
   
   private void OnTriggerEnter2D(Collider2D other)
   {
      CheckOtherTriggerAndSetButtonsStata(other, true);
   }
   
   private void OnTriggerExit2D(Collider2D other)
   { 
      CheckOtherTriggerAndSetButtonsStata(other, false);
   }
   

   private void CheckOtherTriggerAndSetButtonsStata(Collider2D other, bool state)
   {
      if (other.TryGetComponent<Movement>(out var hero))
      {
         if (_interactionType == NPCInteractionType.DIALOG)
         {
            AppController.GameplayHudController.SetActiveDialogButton(state);
            AppController.GameplayHudController.SetDialogData(_dialogData, this);
         }
      }
   }
}
