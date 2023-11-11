using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour, IPickable
{
    public PlayerWeaponRanged weapon;

    public void PickUp()
    {
        weapon.isChosen = true;
        weapon.isPickedUp = true;
        if (WeaponManager.weaponManager.curWeaponRanged != null)
        {
            WeaponManager.weaponManager.curWeaponRanged.transform.GetChild(0).gameObject.SetActive(false);
            WeaponManager.weaponManager.curWeaponRanged.isChosen = false;
        }
        WeaponManager.weaponManager.curWeaponRanged = weapon;
        weapon.transform.GetChild(0).gameObject.SetActive(true);
        WeaponManager.weaponManager.WeaponsRanged.Add(weapon);
        weapon.onPickedUpWeapon.Raise(this, weapon.pickedUpSound);
        
        Destroy(this.gameObject);
    }
}
