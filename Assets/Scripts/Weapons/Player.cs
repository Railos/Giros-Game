using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PickableWeapon"))
        {
            Destroy(other.gameObject);
            other.GetComponent<PickUpWeapon>().onPickedUpWeapon.Raise();
        }
    }
}
