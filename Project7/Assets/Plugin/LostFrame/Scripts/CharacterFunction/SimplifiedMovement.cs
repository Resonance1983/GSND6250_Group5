using System;
using Unity.VisualScripting;
using UnityEngine;

namespace LostFrame
{
    [RequireComponent(typeof(Rigidbody))]
    public class SimplifiedMovement : Singleton<SimplifiedMovement>
    {
        public float speed = 5;
        public Transform CameraForwardTransform;
        
        private CharacterController characterController;
        private Vector3 moveDirection;

        void Start()
        {
            characterController = GetComponent<CharacterController>();
        }
        
        void Update()
        {
            
            float horizontal = Input.GetAxis("Horizontal"); //A D 左右
            float vertical = Input.GetAxis("Vertical"); //W S 上 下
            Vector3 forward = CameraForwardTransform.forward; 
            Vector3 right = CameraForwardTransform.right; 
            forward.y = 0f; right.y = 0f; 
            forward.Normalize(); 
            right.Normalize();
            
            moveDirection = forward * vertical + right * horizontal;
            
            // Apply gravity
            moveDirection.y = -9.8f * Time.deltaTime; 
            
            /*if (moveDirection.magnitude > 0.1f)
                transform.forward = moveDirection; // 使角色面向移动方向*/
            
            characterController.Move(moveDirection * speed * Time.deltaTime);
            
        }
    }
}