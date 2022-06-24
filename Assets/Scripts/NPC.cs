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

   [SerializeField]
   private NPCInteractionType _interactionType;

   [SerializeField] 
   private string[] _dialog;
   
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
         if (_interactionType == NPCInteractionType.FIGHT)
         {
            AppController.GameplayHudController.SetActiveFigthButton(state);
         }

         else if (_interactionType == NPCInteractionType.DIALOG)
         {
            AppController.GameplayHudController.SetActiveDialogButton(state, _dialog);
         }
      }
   }
}
