using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSliding : MonoBehaviour
{

    [SerializeField] private KeyCode slideKey;
    [SerializeField] private float maxSlideSpeed;
    [SerializeField] private float boostDuration;
    [SerializeField] private float slideCooldown;
    private bool isBoosting;
    public bool isSliding;
    private bool canSlide = true;
    
    private PlayerMove PM;

    private void Start()
    {
        PM = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        Sliding();
    }
    
    private void Sliding()
    {
        if (Input.GetKey(slideKey) && PM.playerSprinting.isSprinting && canSlide && PM.Grounded)
        {
            if (!isSliding)
            {
                PM.playerJumping.enabled = false;
                canSlide = false;
                isSliding = true;
                PM.collider.height = 1f;
                PM.playerSprinting.enabled = false;
                isBoosting = true;
                PM.headBob.headBobFrequency = 1000f;
                StartCoroutine(nameof(DecreaseSpeedAfterBoost));
            }
        }

        if (isBoosting)
        {
            PM.moveSpeed = Mathf.Lerp(PM.moveSpeed, maxSlideSpeed, Time.deltaTime * 5f);
            PM.cam.fieldOfView += 0.1f;
        }

        if (Input.GetKeyUp(slideKey))
        {
            if (isSliding)
            {
                StopSliding();
            }
        }
    }
    
    private void ResetSlide()
    {
        canSlide = true;
    }
    
    private IEnumerator DecreaseFOVSlide()
    {
        while (PM.cam.fieldOfView > PlayerPrefs.GetFloat("FOV"))
        {
            PM.cam.fieldOfView -= 0.25f;
            yield return new WaitForSeconds(0.01f);
        }

        PM.cam.fieldOfView = PlayerPrefs.GetFloat("FOV");
    }

    private void StopSliding()
    {
        isSliding = false;
        Invoke(nameof(ResetSlide), slideCooldown);
        StopCoroutine(nameof(DecreaseSpeedAfterBoost));
        PM.collider.height = 2f;
        PM.playerSprinting.enabled = true;
        isBoosting = false;
        PM.headBob.headBobFrequency = 1.8f;
        PM.playerJumping.enabled = true;
    }
    
    IEnumerator DecreaseSpeedAfterBoost()
    {
        yield return new WaitForSeconds(boostDuration);
        isBoosting = false;

        StartCoroutine(nameof(DecreaseFOVSlide));
        
        while (PM.moveSpeed > 0f)
        {
            PM.moveSpeed -= 1.5f;
            yield return new WaitForSeconds(0.1f);
        }
        
        StopSliding();
    }
}
