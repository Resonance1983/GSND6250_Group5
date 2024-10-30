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
        
        void Update()
        {
            float horizontal = Input.GetAxis("Horizontal"); //A D 左右
            float vertical = Input.GetAxis("Vertical"); //W S 上 下
            GetComponent<Rigidbody>().velocity = CameraForwardTransform.forward * vertical * speed + CameraForwardTransform.right * horizontal * speed;
            
        }
    }
}