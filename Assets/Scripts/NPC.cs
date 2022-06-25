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

   [SerializeField]
   private NPCInteractionType _interactionType;

   [SerializeField] 
   private DialogElement[] _dialog;

   [SerializeField] 
   private bool _startFightAfterDialog;

   [SerializeField] 
   private GameObject[] _mobs;

   private void OnEnable()
   {
      if (_startFightAfterDialog && AppController.GameplayHudController != null)
      {
         AppController.GameplayHudController.DialogEndedEvent += DialogEndedEvent;
      }
   }

   private void OnDisable()
   {
      if (_startFightAfterDialog && AppController.GameplayHudController != null)
      {
         AppController.GameplayHudController.DialogEndedEvent -= DialogEndedEvent;
      }
   }


   private void OnTriggerEnter2D(Collider2D other)
   {
      CheckOtherTriggerAndSetButtonsStata(other, true);
   }
   
   private void OnTriggerExit2D(Collider2D other)
   { 
      CheckOtherTriggerAndSetButtonsStata(other, false);
   }
   
   
   private void DialogEndedEvent()
   {
      AppController.SceneController.SwitchToFightScene(_mobs);
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
