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

}
