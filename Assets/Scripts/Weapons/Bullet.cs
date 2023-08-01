using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public float bulletSpeed;

    public void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
    }
}
