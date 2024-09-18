using System;
using Tools;
using UnityEngine;

public class CharacterMovement : Singleton<CharacterMovement>
{
    //Setting_Basic
    [SerializeField] private Rigidbody PlayerRb;
    [SerializeField] private Transform CameraForwardTransform;
    
    // BasicSetting_Walk/Run
    public Boolean AllowedRun = true;
    [Range(0,20)]
    public float WalkForce = 3;
    [Range(0,30)]
    public float RunForce = 5;
    private Boolean isRunning = false;
    
    //AdvancedSetting_Walk/Run
    [Tooltip("If you have used Physic Material for Ground, please don't use it")]
    public bool isUsingLinearDrag = true;
    [Range(0,3)]
    public float LinearDrag = 0.5f;
    [Range(0,10)]
    public float Acceleration_Walk = 5;
    [Range(0,20)]
    public float Acceleration_Run = 8f;
    [Range(0,20)]
    public float MaxSpeed_Walk = 8;
    [Range(0,30)]
    public float MaxSpeed_Run = 12;

    // Setting_Jump
    [SerializeField] private Boolean AllowedJump = false;
    [SerializeField] private Boolean AllowedJumpInAir = false;
    [Range(500,1000)]
    public float JumpForce = 600;
    private int JumpTimes = 0;

    void FixedUpdate() {
        
        // Walk/Run Direction with camera
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
        
        
        //Walk/Run Limitation Physical Rule
        float playerSpeed = PlayerRb.velocity.magnitude;
        
        // MaxSpeed
        if (Mathf.Abs(playerSpeed) > MaxSpeed_Walk)
            PlayerRb.velocity = PlayerRb.velocity.normalized * (isRunning?MaxSpeed_Walk:MaxSpeed_Run);
        // Acceleration
        PlayerRb.AddForce(movVec * (isRunning?WalkForce:RunForce) * (isRunning?Acceleration_Walk:Acceleration_Run));
        //LinearDrag(Friction)
        if (isUsingLinearDrag) {
            if (movVec.magnitude < 0.3f)
                PlayerRb.drag = LinearDrag;
            else
                PlayerRb.drag = 0;
        }
        
    }

    private void Update() {
        // Switch Walk/Run
        if (AllowedRun && Input.GetKey(KeyCode.LeftShift)) isRunning = !isRunning;
        
        // Jump
        if (Input.GetButtonDown("Jump")) {
            if (AllowedJump && (JumpTimes==0 || (JumpTimes==1 && AllowedJumpInAir))) {
                print("Jumping");
                PlayerRb.AddForce(JumpForce*new Vector3(0,1,0),ForceMode.Impulse);
                JumpTimes += 1;
            }
        }
        
    }

    private void OnCollisionEnter(Collision other) {
        //Reset JumpTimes
        if (other.gameObject.tag.Equals("Ground")) {
            JumpTimes = 0;
        }
    }
    
}