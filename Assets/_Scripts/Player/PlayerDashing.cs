using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashing : MonoBehaviour
{
 
    public KeyCode dashKey = KeyCode.LeftAlt;
    public int dashTime;
    private int _dashTime;
    public float dashSpeed;
    [SerializeField] private float dashCooldown;
    private bool CanDash = true;
    public bool isDashing;

    private PlayerMove PM;

    private void Start()
    {
        PM = GetComponent<PlayerMove>();
        
        _dashTime = dashTime;

        StartCoroutine(nameof(Dashing));
    }
    
    private IEnumerator Dashing()
    {
        while (true)
        {
            if (Input.GetKeyDown(dashKey) && CanDash && PM.moveDirection.magnitude > 0f)
            {
                CanDash = false;
                foreach (var v in WeaponManager.weaponManager.WeaponsRanged)
                {
                    v.transform.parent.GetComponent<HeadBobbing>().headBobHeight = 0f;
                }
                Invoke(nameof(WaitDashing), dashCooldown);
                
                Vector3 moveDir = PM.moveDirection;
                float prevHeight = PM.headBob.headBobHeight;
                PM.headBob.headBobHeight = 0f;

                isDashing = true;
                while (_dashTime > 0)
                {
                    PM.rb.AddForce(moveDir * dashSpeed * Time.deltaTime, ForceMode.Impulse);
                    if (PM.rb.velocity.y > 5f)
                    {
                        PM.rb.velocity = new Vector3(PM.rb.velocity.x, 0f, PM.rb.velocity.z);
                    }

                    if (!Player.player.isPaused)
                    {
                        PM.cam.fieldOfView += 0.15f;
                        PM.Wcam.fieldOfView += 0.07f;
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
                        PM.cam.fieldOfView -= 0.15f;
                        PM.Wcam.fieldOfView -= 0.07f;
                        _dashTime--;
                    }

                    yield return null;

                }

                foreach (var v in WeaponManager.weaponManager.WeaponsRanged)
                {
                    v.transform.parent.GetComponent<HeadBobbing>().headBobHeight = 0.1f;
                }
        
                PM.headBob.headBobHeight = prevHeight; 
                PM.cam.fieldOfView = PlayerPrefs.GetFloat("FOV");
                PM.Wcam.fieldOfView = PlayerPrefs.GetFloat("WeaponFOV");
                _dashTime = dashTime;
                PM.moveSpeed = PM.walkSpeed;
            }


            yield return null;
        }
    }

    private void WaitDashing()
    {
        CanDash = true;
    }
    
    
}
