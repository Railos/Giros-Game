using System;
using System.Collections;
using EZCameraShake;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    [Header("Stats")]
    public bool canShoot;

    public float explosionForce;
    public float explosionRadius;
    public float maxDistance;
    public float reloadTime;
    public float beamDamage;
    public float explosionDamage;

    public KeyCode shootButton;

    public LayerMask hitMask;

    [Header("Refs")] 
    public GameEvent explosionEvent;
    public LineRenderer laser;
    public Transform attackPoint;
    public Camera cam;

    [Header("Camera Shake")]
    public float magnitude;
    public float roughness;
    public float fadeInTime;
    public float fadeOutTime;

    private Animator animator;

    private RaycastHit[] hit;
    private bool drawLaser;

        private void Start()
    {
        canShoot = true;
        animator = GetComponent<Animator>();
        animator.CrossFade("RocketLauncher_Idle",0,0,0);
        laser.positionCount = 0;
    }
    
    private void Update()
    {
        Checkings();
    }

    private void Checkings()
    {
        if (!Input.GetKeyDown(shootButton)) return;
        if (!canShoot) return;
        if (!Physics.Raycast(cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), maxDistance)) return;
        canShoot = false;
        StartCoroutine(nameof(Shoot));
        Invoke(nameof(ResetShot), reloadTime);
    }

    private IEnumerator Shoot()
    {
        animator.CrossFade("RocketLauncher_Shoot",0,0,0);
        
        yield return new WaitForSeconds(0.47f);

        CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);
        laser.positionCount = 2;
        hit = Physics.RaycastAll(cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), maxDistance);

        if (hit.Length > 0)
        {
            laser.positionCount = 2;
            laser.SetPosition(0, attackPoint.position);
            laser.SetPosition(1, hit[hit.Length-1].point);
        }
            
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].transform.TryGetComponent(out Enemy enemy))
                enemy.Health -= beamDamage;
            else
            {
                Instantiate(ObjectsPool.objectPool.bigExplosion, hit[i].point, Quaternion.identity);
                explosionEvent.Raise(this);
                Collider[] colliders = Physics.OverlapSphere(hit[i].point, explosionRadius);

                for (int j = 0; j < colliders.Length; j++)
                {
                    if (colliders[j].TryGetComponent(out Enemy enemyy))
                    {
                        enemyy.Health -= explosionDamage;
                    }
                    else if(colliders[j].TryGetComponent(out Rigidbody rb))
                    {
                        rb.AddExplosionForce(explosionForce, hit[i].point, explosionRadius);
                    }
                }
                break;
            }
        }
        
        yield return new WaitForSeconds(0.4f);

        laser.positionCount = 0;

    }

    private void ResetShot()
    {
        canShoot = true;
    }
    
}
