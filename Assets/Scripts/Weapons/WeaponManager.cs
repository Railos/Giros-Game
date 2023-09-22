using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WeaponManager : MonoBehaviour
{
    public static WeaponManager weaponManager;
    public PlayerWeaponRanged curWeaponRanged;
    public List<PlayerWeaponRanged> WeaponsRanged;

    public FistPunch fistPunch;

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
