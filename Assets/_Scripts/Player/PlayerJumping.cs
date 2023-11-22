using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumping : MonoBehaviour
{
    
    public KeyCode jumpKey;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private int jumpsLeft;
    public int maxJumps = 1;

    private PlayerMove PM;
    
    private void Start()
    {
        PM = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        if (PM.Grounded)
        {
            if (!Input.GetKey(jumpKey))
            {
                jumpsLeft = maxJumps;
            }
        }
        
        if(Input.GetKeyDown(jumpKey) && jumpsLeft > 0)
        {
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);

            jumpsLeft--;
        }
    }
    
    private void Jump()
    {
        PM.exitingSlope = true;

        // reset y velocity
        PM.rb.velocity = new Vector3(PM.rb.velocity.x, 0f, PM.rb.velocity.z);

        PM.rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    
    private void ResetJump()
    {
        PM.exitingSlope = false;
    }
}
