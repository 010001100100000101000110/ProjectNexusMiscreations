using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(CharacterController))]


public class MovementController : MonoBehaviour
{
    [SerializeField] SOPlayerSettings playerSettings;    
    Animator animator;
    CharacterController characterController;
    HealthAndPower healthAndPower;
    Footsteps footsteps;
    

    [SerializeField] Transform lFootTransform;
    [SerializeField] Transform rFootTransform;    
    [SerializeField] Transform headCheck;
    [SerializeField] Transform groundCheck;

    float movementTime = 0.4f;
    [HideInInspector] public float movementSpeed;
    float gravity = 9.8f;

    public bool canJump;
    [HideInInspector] public bool canMove;
    [HideInInspector] public bool canDash;

    bool isInLookAroundMode;

    float dashCooldown;
    float dashStart;

    Vector3 origCenter;
    float origHeight;

    [SerializeField] LayerMask layerMask;

    [SerializeField] int playerLayerNumber;
    float radius;

    //jaakon

    private Vector2 inputs;
    private Vector3 movementDirection;
    [HideInInspector] public Vector3 velocity;
    private Camera mainCam;    
    private Quaternion targetRotation;
    Quaternion previousRotation;
    Vector3 angularVelocity;

    void Start()
    {        
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        healthAndPower = GetComponent<HealthAndPower>();
        footsteps = GetComponentInChildren<Footsteps>();

        origCenter = characterController.center;
        origHeight = characterController.height;

        canMove = true;

        dashCooldown = playerSettings.DashCooldown;
        dashStart = Time.time + dashCooldown;

        radius = characterController.radius;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //jaakon
        mainCam = Camera.main;
        previousRotation = transform.rotation;
    }

    void Update()
    {        
        Gravity();        
        CameraController();
        GroundCheck();
        if (canMove && isInLookAroundMode == false) CharacterMovement();
        if (canMove == false) footsteps.isFootstepping = false;
        Death();
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walking")) canMove = true;
    }

    private void CameraController()
    {
        if (Input.GetMouseButton(2) && (animator.GetCurrentAnimatorStateInfo(0).IsName("Walking") | (animator.GetCurrentAnimatorStateInfo(0).IsName("Crouch"))))
        {
            isInLookAroundMode = true;
        }
        else isInLookAroundMode = false;

        if (isInLookAroundMode)
        {
            LookAroundCamera();
            animator.SetBool("IsWalking", true);
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", 0);
            animator.SetFloat("Rotation", 0);            
        }
        else AttackStateCamera();        
    }

    private void AttackStateCamera()
    {
        float yAngle = Mathf.Atan2(mainCam.transform.forward.x, mainCam.transform.forward.z) * Mathf.Rad2Deg;
        targetRotation = Quaternion.Euler(0, yAngle, 0);
    }

    private void LookAroundCamera()
    {
        if (inputs.x != 0 || inputs.y != 0)
        {
            float yAngle = Mathf.Atan2(mainCam.transform.forward.x, mainCam.transform.forward.z) * Mathf.Rad2Deg;
            targetRotation = Quaternion.Euler(0, yAngle, 0);
        }
    }
    private void Gravity()
    {
        // Painovoima
        velocity.y -= gravity * (Time.deltaTime * 3);

        // Painovoimaa varten, sama kuin fps controllerissa
        if (characterController.isGrounded) velocity.y = -7;
        if (velocity.y < -7) velocity.y = -7;       
    }

    private void CharacterMovement()
    {
        Jump();
        //jaakon
        inputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        movementDirection = new Vector3(mainCam.transform.forward.x, 0, mainCam.transform.forward.z) * inputs.y + new Vector3(mainCam.transform.right.x, 0, mainCam.transform.right.z) * inputs.x;
        characterController.Move(movementDirection * Time.deltaTime * movementSpeed + (velocity * Time.deltaTime));

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, playerSettings.PlayerTurningSpeed * Time.deltaTime);
        
        //mun
        animator.SetFloat("MoveX", inputs.x, movementTime, Time.deltaTime);
        animator.SetFloat("MoveY", inputs.y, movementTime, Time.deltaTime);


        if (GroundCheck() == true && animator.GetCurrentAnimatorStateInfo(0).IsName("Walking")) movementSpeed = playerSettings.PlayerSpeed;

        Rotation();        
        Dash();        
        Crouch();
    }

    private void Rotation()
    {
        //jaakon
        Quaternion deltaRotation = transform.rotation * Quaternion.Inverse(previousRotation);
        previousRotation = transform.rotation;
        deltaRotation.ToAngleAxis(out var angle, out var axis);
        angle *= Mathf.Deg2Rad;
        angularVelocity = (1.0f / Time.deltaTime) * angle * axis;        
        float yVelocity = Mathf.Clamp(angularVelocity.y, -1, 1);
        if (float.IsNaN(angularVelocity.y))
        {
            angularVelocity.y = 0;
        }
        animator.SetFloat("Rotation", yVelocity, 0.2f, Time.deltaTime);
    }
   

    private bool GroundCheck()
    {
        Collider[] hitGround = Physics.OverlapSphere(groundCheck.position, radius, layerMask);
        if (hitGround.Length > 0) return true;
        else return false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheck.position, radius);
    }
    private void Jump()
    {        
        if ((animator.GetCurrentAnimatorStateInfo(0).IsName("Walking") | animator.GetCurrentAnimatorStateInfo(0).IsName("JumpLand")) & (GroundCheck() == true | characterController.isGrounded))
        {
            canJump = true;
        }
        else canJump = false;

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            velocity.y = Mathf.Sqrt(playerSettings.JumpForce * -2f * -gravity);
            animator.SetTrigger("Jump");
            animator.ResetTrigger("Landing");
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("InAir"))
        {            
            animator.ResetTrigger("Jump");
        }
        if (GroundCheck() == true && animator.GetCurrentAnimatorStateInfo(0).IsName("InAir"))
        {
            animator.SetBool("Falling", false);
            animator.SetTrigger("Landing");
            movementSpeed = playerSettings.PlayerSpeed;
        }
        else
        {
            Falling();
        }

        if (Physics.Raycast(transform.position, Vector3.down, 0.2f, LayerMask.GetMask("Ground")))
        {
            animator.SetTrigger("Landing");
        }


    }
    private void Falling()
    {
        if (GroundCheck() == false && velocity.y < -5)
        {
            animator.SetBool("Falling", true);
            animator.ResetTrigger("Landing");
            movementSpeed = playerSettings.PlayerSpeedInAir;
        }
    }

    private void Dash()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walking") && healthAndPower.Power > 0) canDash = true;
        else canDash = false;              

        if (Input.GetKeyDown(KeyCode.LeftShift) && (movementDirection.x !=0 || movementDirection.z !=0) && canDash &&  characterController.isGrounded && healthAndPower.hasPower && dashStart <= Time.time)       
        {
            dashStart = Time.time + dashCooldown;
            AddForce(movementDirection * playerSettings.DashForce);
            StartCoroutine(ResetForce());
            animator.SetBool("IsDashing", true);
            healthAndPower.LosePower(playerSettings.DashPowerConsumption);            
        }
        else 
        {
            animator.SetBool("IsDashing", false);
        }
                
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dash"))
        {
            animator.ResetTrigger("Jump");
            velocity.y = -7;
        }
    }

   
    private void Crouch()
    {
        int playerLayer = playerLayerNumber;
        int layerMask = ~(1 << playerLayer);
        Collider[] hitObjects = Physics.OverlapSphere(headCheck.position, 0.4f, layerMask);
        
        if (Input.GetKey(KeyCode.LeftControl))
        {
            movementSpeed = playerSettings.PlayerSpeedInCrouch;
            animator.SetBool("IsCrouching", true);           
            characterController.center = new Vector3(0, -0.32f, 0);
            characterController.height = 0.1f;        
        }
        else        
        {
            if (hitObjects.Length == 0)
            {
                animator.SetBool("IsCrouching", false);
                characterController.center = origCenter;
                characterController.height = origHeight;                
            }      
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Crouch")) canDash = false;
        Rotation();
    }
 
    void OnDrawGizmosSelected()
    {       
        Gizmos.color = Color.yellow;        
        Gizmos.DrawWireSphere(headCheck.position, 0.4f);
    }   
        
    public void AddForce(Vector3 force)
    {
        velocity += force;
    }

    private IEnumerator ResetForce()
    {
        yield return new WaitForSeconds(playerSettings.DashDurationInSeconds);
        velocity.x = 0;
        velocity.z = 0;        
    }

 

    private void Death()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("DEATH")) canMove = false;
    }
}
