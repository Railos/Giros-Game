using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WeaponManager : MonoBehaviour
{
    public static WeaponManager weaponManager;
    public PlayerWeapon curWeapon;
    public List<PlayerWeapon> Weapons;

    private void Awake()
    {
        if (weaponManager != null)
        {
            Destroy(weaponManager);
        }
        else
        {
            weaponManager = this;
        }
    }
}
