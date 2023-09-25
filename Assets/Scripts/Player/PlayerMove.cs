using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    [Header("Crouching")]
    public float crouchMoveSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool Grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    public float horizontalInput;
    public float verticalInput;
    
    public Transform orientation;
    
    [SerializeField] private int dashTime;
    private int _dashTime;
    [SerializeField] private float dashSpeed;
    public bool CanDash = true;
    private bool isDashing;

    private int jumpsLeft;
    [HideInInspector] public bool CanJump;
    public int MaxJumps = 1;
    
    [SerializeField] private Camera cam;
    [SerializeField] private Camera Wcam;

    [SerializeField] private new CapsuleCollider collider;
    
    [SerializeField] private AudioSource footsteps;
    
    Vector3 moveDirection;

    Rigidbody rb;

    public bool isForcingDown;
    private bool isSliding;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        _dashTime = dashTime;

        cam.fieldOfView = PlayerPrefs.GetFloat("FOV");
        Wcam.fieldOfView = PlayerPrefs.GetFloat("WeaponFOV");
    }

    private void Update()
    {
        // ground check
        Grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        // handle drag
        if (Grounded)
        {
            rb.drag = groundDrag;
            if (!Input.GetKey(jumpKey))
            {
                jumpsLeft = MaxJumps;
            }
        }else rb.drag = 0;


        if (moveDirection.magnitude > 0 && !isSliding && Grounded && !isDashing) footsteps.enabled = true;
        else footsteps.enabled = false;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if(Input.GetKeyDown(jumpKey) && jumpsLeft > 0 && !isForcingDown && CanJump)
        {
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);

            jumpsLeft--;
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            collider.height = playerHeight / 2;
            isForcingDown = true;
            GetComponent<HeadBobbing>().enabled = false;
        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            if (!isForcingDown)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + playerHeight / 2 - 0.1f, transform.position.z);
                rb.velocity = Vector3.zero;
            }
            collider.height = playerHeight;
            GetComponent<HeadBobbing>().enabled = true;
            isSliding = false;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftAlt) && CanDash && moveDirection.magnitude > 0f && !isForcingDown && !isSliding)
        {
            CanDash = false;
            foreach (var v in WeaponManager.weaponManager.WeaponsRanged)
            {
                v.transform.parent.GetComponent<HeadBobbing>().headBobHeight = 0f;
            }
            StartCoroutine(Dashing()); 
            StartCoroutine(WaitDashing()); 
        }
    }

    private void StateHandler()
    {
        if (isForcingDown)
        {
            if (!Grounded)
            {
                GetComponent<HeadBobbing>().enabled = false;
                rb.AddForce(Vector3.down * 100f * Time.deltaTime, ForceMode.Impulse);
            }
            else
            {
                isForcingDown = false;
                RaycastHit rayhit;
                if (Physics.Raycast(transform.position, Vector3.down, out rayhit)) transform.position = rayhit.point;
            }
        }
        
        // Mode - Crouching
        else if (Input.GetKey(crouchKey))
        {
            isSliding = true;
            rb.AddForce(transform.forward * crouchMoveSpeed * Time.deltaTime);
        }

        // Mode - Sprinting
        else if(Grounded && Input.GetKey(sprintKey))
        {
            moveSpeed = sprintSpeed;
        }

        // Mode - Walking
        else if (Grounded)
        {
            moveSpeed = walkSpeed;
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (!isSliding)
        {
            if (OnSlope() && !exitingSlope)
            {
                rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

                if (rb.velocity.y > 0)
                    rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }

            // on ground
            else if (Grounded)
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            }
            else if (!Grounded)
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        // turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        exitingSlope = false;
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    IEnumerator Dashing()
    {
        Vector3 moveDir = new Vector3(horizontalInput, 0f, verticalInput);
        
        float prevHeight = GetComponent<HeadBobbing>().headBobHeight;
        GetComponent<HeadBobbing>().headBobHeight = 0f;

        isDashing = true;
        while (_dashTime > 0)
        {
            rb.AddRelativeForce(moveDir.normalized * dashSpeed * Time.deltaTime, ForceMode.Impulse);
            if (rb.velocity.y > 5f)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            }

            if (!Player.player.isPaused)
            {
                cam.fieldOfView += 0.15f;
                Wcam.fieldOfView += 0.07f;
                _dashTime--;
            }
            
            yield return null;
        }
        isDashing = false;
        _dashTime = dashTime;
        
        while (_dashTime > 0)
        {
            if (!Player.player.isPaused)
            {
                cam.fieldOfView -= 0.15f;
                Wcam.fieldOfView -= 0.07f;
                _dashTime--;
            }
            
            yield return null;
        }
        
        GetComponent<HeadBobbing>().headBobHeight = prevHeight;
        foreach (var v in WeaponManager.weaponManager.WeaponsRanged)
        {
            v.transform.parent.GetComponent<HeadBobbing>().headBobHeight = 0.1f;
        }
        cam.fieldOfView = PlayerPrefs.GetFloat("FOV");
        Wcam.fieldOfView = PlayerPrefs.GetFloat("WeaponFOV");
        _dashTime = dashTime;
    }

    IEnumerator WaitDashing()
    {
        yield return new WaitForSeconds(1f);

        CanDash = true;
    }
}
