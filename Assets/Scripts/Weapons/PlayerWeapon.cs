using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerWeapon : MonoBehaviour
{
    
    public bool isChosen;
    public bool isPickedUp;

    public void PickUp()
    {
        isChosen = true;
        isPickedUp = true;
        if (WeaponManager.weaponManager.curWeapon != null)
        {
            WeaponManager.weaponManager.curWeapon.transform.GetChild(0).gameObject.SetActive(false);
            WeaponManager.weaponManager.curWeapon.isChosen = false;
        }
        WeaponManager.weaponManager.curWeapon = this;
        transform.GetChild(0).gameObject.SetActive(true);
        WeaponManager.weaponManager.Weapons.Add(this);
    }

    public void SwitchToThis(int index)
    {
        WeaponManager.weaponManager.curWeapon.transform.GetChild(0).gameObject.SetActive(false);
        WeaponManager.weaponManager.curWeapon.isChosen = false;
        WeaponManager.weaponManager.curWeapon = WeaponManager.weaponManager.Weapons[index];
        isChosen = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
