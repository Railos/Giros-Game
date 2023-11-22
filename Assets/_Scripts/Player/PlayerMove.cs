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
    public float crouchSpeed;

    public float groundDrag;
    
    [Header("Dashing")]
    [SerializeField] private int dashTime;
    private int _dashTime;
    [SerializeField] private float dashSpeed;
    private bool CanDash = true;
    private bool isDashing;

    [Header("Keybinds")]
    public KeyCode crouchKey = KeyCode.LeftControl;

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

    private float horizontalInput;
    private float verticalInput;
    
    [Header("Refs")]
    public Transform orientation;
    public Camera cam;
    [SerializeField] private Camera Wcam;
    public new CapsuleCollider collider;
    [SerializeField] private AudioSource footsteps;
    
    Vector3 moveDirection;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public HeadBobbing headBob;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        headBob = GetComponent<HeadBobbing>();

        _dashTime = dashTime;

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
        StateHandler();

        // handle drag
        if (Grounded)
        {
            if (!playerSliding.isSliding) rb.drag = groundDrag;
        }else rb.drag = 0;


        if (moveDirection.magnitude > 0 && Grounded && !isDashing && !playerSliding.isSliding) footsteps.enabled = true;
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

        // start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            collider.height = playerHeight / 2;
        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            collider.height = playerHeight;
            headBob.enabled = true;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftAlt) && CanDash && moveDirection.magnitude > 0f)
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

        // Mode - Crouching
        if (Input.GetKey(crouchKey))
        {
            collider.height = playerHeight / 2;
            moveSpeed = crouchSpeed;
        }

        // Mode - Walking
        else if (Grounded && !playerSliding.isSliding)
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

    IEnumerator Dashing()
    {
        Vector3 moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        float prevHeight = headBob.headBobHeight;
        headBob.headBobHeight = 0f;

        isDashing = true;
        while (_dashTime > 0)
        {
            rb.AddForce(moveDir * dashSpeed * Time.fixedDeltaTime, ForceMode.Impulse);
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

        foreach (var v in WeaponManager.weaponManager.WeaponsRanged)
        {
            v.transform.parent.GetComponent<HeadBobbing>().headBobHeight = 0.1f;
        }
        
        headBob.headBobHeight = prevHeight; 
        cam.fieldOfView = PlayerPrefs.GetFloat("FOV");
        Wcam.fieldOfView = PlayerPrefs.GetFloat("WeaponFOV");
        _dashTime = dashTime;
        moveSpeed = walkSpeed;
    }

    IEnumerator WaitDashing()
    {
        yield return new WaitForSeconds(1f);

        CanDash = true;
    }
}
