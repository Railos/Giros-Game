using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PlayerWeapon : MonoBehaviour
{
    
    public bool isChosen;
    public bool isPickedUp;
    
    public int bulletsInTime;
    public int bulletsMagazineTotal;
    public int clipCapacity;
    public int bulletsClip;
    
    public bool isReloading;
    [HideInInspector] public bool interruptReload = false;
    
    public GameEvent onWeaponShot;
    public GameEvent onWeaponReload;
    public GameEvent textRefresh;

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
        if (WeaponManager.weaponManager.curWeapon.isReloading)
        {
            WeaponManager.weaponManager.curWeapon.interruptReload = true;
            WeaponManager.weaponManager.curWeapon.isReloading = false;
        }
        transform.GetChild(0).gameObject.SetActive(true);
        textRefresh.Raise(this);
        WeaponManager.weaponManager.curWeapon.transform.GetChild(0).gameObject.SetActive(false);
        WeaponManager.weaponManager.curWeapon.isChosen = false;
        WeaponManager.weaponManager.curWeapon = WeaponManager.weaponManager.Weapons[index];
        isChosen = true;
    }

}
