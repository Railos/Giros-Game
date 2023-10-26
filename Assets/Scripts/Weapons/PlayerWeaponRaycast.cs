using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerWeaponRaycast : PlayerWeaponRanged
{
    [Header("Stats")]
    public bool canShoot;
    public bool canHold;
    public bool isShooting;
    
    private int _bulletsLeft;

    public float reloadTime;
    public float waitBeforeNextShot;
    public float waitBeforeNextBullet;
    public float damage;
    public float raycastRange;
    private float _spread;

    public float minSpread;
    public float maxSpread;

    public KeyCode reloadKey;
    

    [Header("Camera Shake")]
    public float magnitude;
    public float roughness;
    public float fadeInTime;
    public float fadeOutTime;

    [Header("Refs")] 
    public Transform attackPoint;
    
    public Camera cam;

    public GunRecoil recoil;

    public GunRecoil camRecoil;

    private RaycastHit _rayHit;

    public GameObject muzzleFlash;



    private void Start()
    {
        canShoot = true;
    }
    
    private void Update()
    {
        CheckReload();
        CheckShooting();
        Checkings();
        _bulletsLeft = bulletsInTime;
    }

    private void CheckReload()
    {
        if (!isChosen) return;
        if (!Input.GetKeyDown(reloadKey)) return;
        if (isReloading) return;
        if (bulletsClip < clipCapacity)
        {
            interruptReload = false;
            isReloading = true;
            onWeaponReload.Raise(this);
            Invoke(nameof(Reload), reloadTime);
        }
    }

    private void CheckShooting()
    {
        if (!canShoot) return;
        isShooting = canHold ? Input.GetKey(KeyCode.Mouse0) : Input.GetKeyDown(KeyCode.Mouse0);
    }
    
    private void Checkings() 
    {
        if (!isPickedUp) return;
        if (!isChosen) return;
        if (isReloading) return;
        if (!canShoot) return;
        if (!isShooting) return;
        if (bulletsClip <= 0) return;
        Shoot();
    }

    private void Shoot()
    {
        canShoot = false;
        recoil.Fire();
        camRecoil.rotationSpeed = recoil.rotationSpeed;
        camRecoil.returnSpeed = recoil.returnSpeed;
        camRecoil.RecoilRotation = recoil.RecoilRotation;
        camRecoil.Fire();

        float x = Random.Range(minSpread, maxSpread);
        float y = Random.Range(minSpread, maxSpread);
        Destroy(Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity, transform.GetChild(0)), 0.2f);
        if (Physics.Raycast(cam.ViewportPointToRay(new Vector3(0.5f + x, 0.5f + y, 0f)),out _rayHit))
        {
            if (_rayHit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                _rayHit.collider.GetComponent<Enemy>().Health -= damage;
            }
        }

        //CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);
        
        _bulletsLeft--;
        bulletsClip--;
        onWeaponShot.Raise();
        textRefresh.Raise(this);
        
        Invoke(nameof(ResetShot), waitBeforeNextShot);
        
        if (_bulletsLeft > 0 && bulletsClip > 0)
        {
            Shoot();
        }
    }

    private void ResetShot()
    {
        canShoot = true;
    }

    private void Reload()
    {
        if (interruptReload) return;
        _bulletsLeft = bulletsInTime;
        if (bulletsMagazineTotal <= clipCapacity - bulletsClip)
        {
            bulletsClip += bulletsMagazineTotal;
            bulletsMagazineTotal = 0;
        }else 
        {
            bulletsMagazineTotal -= clipCapacity - bulletsClip;
            bulletsClip = clipCapacity;
        }

        isReloading = false;
        textRefresh.Raise(this);
    }
}
