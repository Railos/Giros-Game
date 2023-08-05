using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player player;

    private void Awake()
    {
        if (player != null)
        {
            Destroy(player);
        }
        else
        {
            player = this;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PickableWeapon"))
        {
            Destroy(other.gameObject);
            other.GetComponent<PickUpWeapon>().onPickedUpWeapon.Raise();
        }

        if (other.gameObject.GetComponent<Ammo>())
        {
            other.GetComponent<Ammo>().CanPickUp();
        }
    }
}
