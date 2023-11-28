using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouching : MonoBehaviour
{
    [SerializeField] private KeyCode crouchKey;
    public float crouchSpeed;
    
    private PlayerMove PM;

    private void Start()
    {
        PM = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(crouchKey))
        {
            PM.collider.height = PM.playerHeight / 2;
            PM.playerSprinting.enabled = false;
        }
        
        if (Input.GetKey(crouchKey))
        {
            PM.collider.height = PM.playerHeight / 2;
            PM.moveSpeed = crouchSpeed;
        }
        
        if (Input.GetKeyUp(crouchKey))
        {
            PM.collider.height = PM.playerHeight;
            PM.headBob.enabled = true;
            PM.playerSprinting.enabled = true;
        }
    }
}
