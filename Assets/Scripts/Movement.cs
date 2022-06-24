using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour
{
   [SerializeField] 
   private float _moveSpeed;

   private Vector3 _target;

   private bool _movementAllowed;

   private void OnEnable()
   {
      _movementAllowed = true;

      if (AppController.GameplayHudController != null)
      {
         AppController.GameplayHudController.DialogStartedEvent += OnDialogStartedEvent;

         AppController.GameplayHudController.DialogEndedEvent += OnDialogEndedEvent;
      }
   
   }

   private void OnDisable()
   {
      if (AppController.GameplayHudController != null)
      {
         AppController.GameplayHudController.DialogStartedEvent -= OnDialogStartedEvent;

         AppController.GameplayHudController.DialogEndedEvent -= OnDialogEndedEvent;
      }
     
   }

   private void Update()
   {
      if (EventSystem.current.IsPointerOverGameObject() || !_movementAllowed) 
         return;
      
      if (Input.GetMouseButtonDown(0))
      {
         _target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
      }

      if (_target != Vector3.zero && _target != gameObject.transform.position)
      {
         gameObject.transform.position =
            Vector2.MoveTowards(gameObject.transform.position, _target, Time.deltaTime * _moveSpeed);
      }
   }

   private void OnDialogStartedEvent()
   {
      _movementAllowed = false;
   }

   private void OnDialogEndedEvent()
   {
      _movementAllowed = true;
   }
}
