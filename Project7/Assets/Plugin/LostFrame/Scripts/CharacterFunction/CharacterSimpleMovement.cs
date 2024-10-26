using System;
using UnityEngine;

namespace LostFrame
{
    public class CharacterSimpleMovement : Singleton<CharacterSimpleMovement>
    {
        //Setting_Basic
        public Rigidbody PlayerRb;
        public Transform CameraForwardTransform;

        // Setting_Walk/Run
        [SerializeField] private bool AllowedRun = true;
        public float WalkForce = 30;
        [ConditionalHide("AllowedRun", true)] public float RunForce = 60;
        private bool isRunning = false;

        // Setting_Jump
        [SerializeField] private bool AllowedJump = true;

        [SerializeField] [ConditionalHide("AllowedJump", true)]
        private bool AllowedJumpInAir = false;

        [ConditionalHide("AllowedJump", true)] public float JumpForce = 60;
        private int JumpTimes = 0;

        private void FixedUpdate()
        {
            // Walk
            var forward = CameraForwardTransform.forward;
            forward.y = 0;
            forward.Normalize();

            var right = CameraForwardTransform.right;
            right.y = 0;
            right.Normalize();

            forward *= Input.GetAxis("Vertical");
            right *= Input.GetAxis("Horizontal");

            var movVec = forward + right;
            if (movVec.magnitude > 1)
                movVec = movVec.normalized;

            PlayerRb.AddForce(movVec * (isRunning ? WalkForce : RunForce));
        }

        private void Update()
        {
            // Switch Walk/Run
            if (AllowedRun && Input.GetKey(KeyCode.LeftShift)) isRunning = !isRunning;


            // Jump
            if (Input.GetButtonDown("Jump"))
                if (AllowedJump && (JumpTimes == 0 || (JumpTimes == 1 && AllowedJumpInAir)))
                {
                    print("Jumping");
                    PlayerRb.AddForce(JumpForce * new Vector3(0, 1, 0), ForceMode.Impulse);
                    JumpTimes += 1;
                }
        }

        private void OnCollisionEnter(Collision other)
        {
            //Reset JumpTimes
            if (other.gameObject.tag.Equals("Ground")) JumpTimes = 0;
        }
    }
}