using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprinting : MonoBehaviour
{

    public bool isSprinting;
    [SerializeField] private KeyCode sprintKey;
    public float sprintSpeed;
    private PlayerMove PM;
    
    private void Start()
    {
        PM = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        if (!PM.Grounded) return;
        if (Input.GetKey(sprintKey))
        {
            PM.moveSpeed = sprintSpeed;
            isSprinting = true;
        }
        else
        {
            PM.moveSpeed = PM.walkSpeed;
            isSprinting = false;
        }
    }
}
