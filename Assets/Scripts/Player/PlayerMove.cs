using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    [SerializeField] private float dashTime;
    [SerializeField] private float dashSpeed;
    public bool CanDash = true;

    private int jumpsLeft;
    public int MaxJumps = 1;
    
    [SerializeField] private Camera cam;
    [SerializeField] private Camera Wcam;

    [SerializeField] private new CapsuleCollider collider;
    

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        // ground check
        Grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.5f, whatIsGround);

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
        if(Input.GetKeyDown(jumpKey) && jumpsLeft > 0)
        {
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);

            jumpsLeft--;
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            collider.height = playerHeight / 2;
        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            collider.height = playerHeight;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftAlt) && CanDash) //  && transform.GetChild(0).GetComponent<grapplingToEnemy>().CanGrapple
        {
            CanDash = false;
            StartCoroutine(Dashing()); 
            StartCoroutine(WaitDashing()); 
        }
    }

    private void StateHandler()
    {
        // Mode - Crouching
        if (Input.GetKey(crouchKey))
        {
            rb.AddForce(transform.forward * crouchMoveSpeed);
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
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if (Grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else if (!Grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);



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
        float startTime = Time.time;
        Vector3 moveDir = new Vector3(horizontalInput, 0f, verticalInput);

        float prevHeight = GetComponent<HeadBobbing>().headBobHeight;
        GetComponent<HeadBobbing>().headBobHeight = 0f;
        float prevHeightWeapon = 0f;
        if (WeaponManager.weaponManager.curWeaponRanged)
        {
            prevHeightWeapon = WeaponManager.weaponManager.curWeaponRanged.transform.parent.GetComponent<HeadBobbing>().headBobHeight;
            WeaponManager.weaponManager.curWeaponRanged.transform.parent.GetComponent<HeadBobbing>().headBobHeight = 0f;
        }
        
        
        while (Time.time < startTime + dashTime)
        {
            cam.fieldOfView += 0.15f;
            Wcam.fieldOfView += 0.07f;
            rb.AddRelativeForce(moveDir.normalized * dashSpeed * Time.deltaTime, ForceMode.Impulse);
            if (rb.velocity.y > 5f)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            }

            yield return null;
        }

        startTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            cam.fieldOfView -= 0.15f;
            Wcam.fieldOfView -= 0.07f;
            yield return null;
        } 
        GetComponent<HeadBobbing>().headBobHeight = prevHeight;
        if (WeaponManager.weaponManager.curWeaponRanged)
            WeaponManager.weaponManager.curWeaponRanged.transform.parent.GetComponent<HeadBobbing>().headBobHeight = prevHeightWeapon;
        cam.fieldOfView = 70f;
        Wcam.fieldOfView = 70f;
    }

    IEnumerator WaitDashing()
    {
        yield return new WaitForSeconds(1f);

        CanDash = true;
    }
}
