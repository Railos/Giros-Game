using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PlayerWeaponRanged : MonoBehaviour
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
    public GameEvent onPickedUpWeapon;
    public GameEvent textRefresh;
    
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public AudioClip pickedUpSound;

    public void SwitchToThis(int index)
    {
        if (WeaponManager.weaponManager.curWeaponRanged.isReloading)
        {
            WeaponManager.weaponManager.curWeaponRanged.interruptReload = true;
            WeaponManager.weaponManager.curWeaponRanged.isReloading = false;
        }
        transform.GetChild(0).gameObject.SetActive(true);
        textRefresh.Raise(this);
        WeaponManager.weaponManager.curWeaponRanged.transform.GetChild(0).gameObject.SetActive(false);
        WeaponManager.weaponManager.curWeaponRanged.isChosen = false;
        WeaponManager.weaponManager.curWeaponRanged = WeaponManager.weaponManager.WeaponsRanged[index];
        isChosen = true;
    }

}
