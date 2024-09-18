using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEditor.Search;
using UnityEngine;

public class CharacterMovement : Singleton<CharacterMovement>
{
    //Setting_Basic
    public Rigidbody PlayerRb;
    public Transform CameraForwardTransform;
    
    // Setting_Walk/Run
    [SerializeField] private Boolean AllowedRun = true;
    [Range(0,80)]
    public float WalkForce = 30;
    [Range(30,120)]
    public float RunForce = 60;
    private Boolean isRunning = false;

    // Setting_Jump
    [SerializeField] private Boolean AllowedJump = true;
    [SerializeField] private Boolean AllowedJumpInAir = false;
    [Range(500,1000)]
    public float JumpForce = 600;
    private int JumpTimes = 0;

    void FixedUpdate()
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

        PlayerRb.AddForce(movVec * (isRunning?WalkForce:RunForce));

    }

    private void Update()
    {
        // Switch Walk/Run
        if (AllowedRun && Input.GetKey(KeyCode.LeftShift)) isRunning = !isRunning;


        // Jump
        if (Input.GetButtonDown("Jump")) {
            if (AllowedJump && (JumpTimes==0 || (JumpTimes==1 && AllowedJumpInAir))) {
                print("Jumping");
                PlayerRb.AddForce(JumpForce*new Vector3(0,1,0));
                JumpTimes += 1;
            }
        }
        
        
    }

    private void OnCollisionEnter(Collision other)
    {
        //Reset JumpTimes
        if (other.gameObject.tag.Equals("Ground")) {
            JumpTimes = 0;
        }
    }
    
}