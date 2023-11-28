using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float walkSpeed;

    public float groundDrag;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool Grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    public bool exitingSlope;

    [Header("Mechanics")] 
    public PlayerSprinting playerSprinting;
    public PlayerSliding playerSliding;
    public PlayerJumping playerJumping;
    public PlayerDashing playerDashing;
    public PlayerCrouching playerCrouching;

    private float horizontalInput;
    private float verticalInput;
    
    [Header("Refs")]
    public Transform orientation;
    public Camera cam;
    public Camera Wcam;
    public new CapsuleCollider collider;
    [SerializeField] private AudioSource footsteps;
    
    public Vector3 moveDirection;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public HeadBobbing headBob;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        headBob = GetComponent<HeadBobbing>();

        collider.height = playerHeight;
        
        cam.fieldOfView = PlayerPrefs.GetFloat("FOV");
        Wcam.fieldOfView = PlayerPrefs.GetFloat("WeaponFOV");
    }

    private void Update()
    {
        // ground check
        Grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();

        // handle drag
        if (Grounded)
        {
            if (!playerSliding.isSliding) rb.drag = groundDrag;
        }else rb.drag = 0;


        if (moveDirection.magnitude > 0 && Grounded && !playerDashing.isDashing && !playerSliding.isSliding) footsteps.enabled = true;
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
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!Grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * playerJumping.airMultiplier, ForceMode.Force);

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

    
}
