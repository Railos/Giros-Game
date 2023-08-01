using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeaponBullet : MonoBehaviour
{
    [Header("Stats")]
    public bool canShoot;
    public bool canHold;
    public bool isShooting;

    public int bulletsInTime;
    private int _bulletsLeft;
    
    public float waitBeforeNextShot;
    public float damage;
    public float raycastRange;
    private float _spread;

    public Bullet bullet;
    public Transform attackPoint;

    [Header("Refs")] 
    public Camera cam;

    public GunRecoil recoil;

    private RaycastHit _rayHit;

    private void Start()
    {
        canShoot = true;
    }
    
    private void Update()
    {
        Inputing();
        Checkings();
    }

    private void Inputing()
    {
        if (!canShoot) return;
        if (canHold)
        {
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }
    }
    
    private void Checkings()
    {
        if (!canShoot) return;
        if (!isShooting) return;
        Shoot();
    }
    
    private void Shoot()
    {
        canShoot = false;
        
        _bulletsLeft--;
        Invoke(nameof(ResetShot), waitBeforeNextShot);
        
        // && bulletsLeftInMagazine > 0
        if (_bulletsLeft > 0)
        {
            Shoot();
        }
    }

    private void ResetShot()
    {
        canShoot = true;
    }
}
