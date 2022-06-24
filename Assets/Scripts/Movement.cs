using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
   [SerializeField] 
   private float _moveSpeed;

   private Vector3 _target;

   private void Update()
   {
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
}
