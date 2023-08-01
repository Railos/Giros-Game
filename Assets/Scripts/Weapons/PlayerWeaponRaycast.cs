using EZCameraShake;
using UnityEngine;

public class PlayerWeaponRaycast : PlayerWeapon
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
    

    [Header("Camera Shake")]
    public float magnitude;
    public float roughness;
    public float fadeInTime;
    public float fadeOutTime;

    [Header("Refs")] 
    public Transform attackPoint;
    
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
        if (!isPickedUp) return;
        if (!isChosen) return;
        if (!canShoot) return;
        if (!isShooting) return;
        Shoot();
    }

    private void Shoot()
    {
        canShoot = false;
        recoil.Fire();

        CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);

        float x = Random.Range(0.01f, 0.05f);
        float y = Random.Range(0.01f, 0.05f);
        
        if (Physics.Raycast(cam.ViewportPointToRay(new Vector3(0.5f + x, 0.5f + y, 0f)),out _rayHit, raycastRange))
        {
            if (_rayHit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Debug.Log("Shot Enemy");
            }
        }

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
