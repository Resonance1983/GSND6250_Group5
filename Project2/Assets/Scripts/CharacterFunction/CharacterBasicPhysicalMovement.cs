using Unity.VisualScripting;
using UnityEngine;

public class CharacterBasicPhysicalMovement : Tools.Singleton<CharacterBasicPhysicalMovement>
{
    // BasicSetting
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private Transform cameraForwardTransform;
    [Range(0,1)]
    public float frictionFactor = 0.5f;
    [Range(0,20)]
    public float gravityFactor = 9.8f;
    private bool isOnGround = true;
    

    // BasicSetting_Walk/Run
    public bool allowedRun = true;
    private bool isRunning = false;
    
    // AdvancedSetting_Walk/Run
    public bool isUsingLinearDrag = true;
    [Range(0,0.1f)]
    public float linearDragFactor = 0.05f;
    [Range(0,20)]
    public float acceleration_walk = 8;
    [Range(0,30)]
    public float acceleration_run = 12;
    [Range(0,50)]
    public float maxSpeed_walk = 8;
    [Range(0,100)]
    public float maxSpeed_run = 12;
    
    public void setIsCancelMaxSpeed(bool setBool) { isCancelMaxSpeed = setBool; }

    // Setting_Jump
    [SerializeField] private bool AllowedJump = true;
    [SerializeField] private bool AllowedJumpTwice = true;
    [Range(0,100)]
    public float acceleration_Jump = 5;
    private int JumpTimes = 0;
    
    // Interface external value for script
    private bool isCancelMaxSpeed = false;
    public bool isMoving = false;
    
    

    void FixedUpdate() {
        
        // Walk/Run Direction with camera
        var forward = cameraForwardTransform.forward;
        forward.y = 0;
        forward.Normalize();

        var right = cameraForwardTransform.right;
        right.y = 0;
        right.Normalize();

        forward *= Input.GetAxis("Vertical");
        right *= Input.GetAxis("Horizontal");

        var movVec = forward + right;
        if (movVec.magnitude > 1)
            movVec = movVec.normalized;
        
        
        // Walk/Run Physical Rule
        
        // MaxSpeed,Acceleration
        float playerSpeed = playerRb.velocity.magnitude;
        if (Mathf.Abs(playerSpeed) > (isRunning?maxSpeed_run:maxSpeed_walk) && !isCancelMaxSpeed)
            playerRb.velocity = playerRb.velocity.normalized * (isRunning?maxSpeed_run:maxSpeed_walk);
        if (movVec.magnitude > 0.2f)
        {
            playerRb.AddForce(movVec * playerRb.mass * (isRunning ? acceleration_run : acceleration_walk));
            isMoving = true;
        }else
            isMoving = false;
        
        // Friction
        if (isOnGround && movVec.magnitude < 0.3f)
            playerRb.AddForce(playerRb.velocity.normalized * -1 * playerRb.mass * gravityFactor * frictionFactor);
        
        
        // LinearDrag is only for atmospheric drag
        if (isUsingLinearDrag) {
            if (movVec.magnitude > 0.3f)
                playerRb.drag = linearDragFactor * playerRb.mass;
            else
                playerRb.drag = 0;
        }
        
    }

    private void Update() {
        // Switch Walk/Run
        if (allowedRun && Input.GetKey(KeyCode.LeftShift)) isRunning = !isRunning;
        
        // Jump
        if (Input.GetButtonDown("Jump")) {
            if (AllowedJump && (JumpTimes==0 || (JumpTimes==1 && AllowedJumpTwice))) {
                print("Jumping");
                playerRb.AddForce(new Vector3(0,1,0) * playerRb.mass * acceleration_Jump,ForceMode.Impulse);
                JumpTimes += 1;
                isOnGround = false;
            }
        }
        
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag.Equals("Ground")) {
            JumpTimes = 0;
            isOnGround = true;
        }
    }
    
    
}