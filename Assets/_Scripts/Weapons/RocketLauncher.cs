using System.Collections;
using EZCameraShake;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    [Header("Stats")]
    public bool canShoot;

    public float reloadTime;
    public float damage;

    public KeyCode shootButton;

    public LayerMask hitMask;

    [Header("Refs")] 
    public GameObject rocket;
    public Transform cam;
    public Transform attackPoint;

    [Header("Camera Shake")]
    public float magnitude;
    public float roughness;
    public float fadeInTime;
    public float fadeOutTime;

    private Animator animator;

    private void Start()
    {
        canShoot = true;
        animator = GetComponent<Animator>();
        animator.CrossFade("RocketLauncher_Idle",0,0,0);
    }
    
    private void Update()
    {
        Checkings();
    }
    
    private void Checkings()
    {
        if (!Input.GetKeyDown(shootButton)) return;
        if (!canShoot) return;
        canShoot = false;
        StartCoroutine(nameof(Shoot));
        Invoke(nameof(ResetShot), reloadTime);
    }

    private IEnumerator Shoot()
    {
        animator.CrossFade("RocketLauncher_Shoot",0,0,0);
        
        yield return new WaitForSeconds(0.5f);

        CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);

        yield return new WaitForSeconds(0.15f);
        
        Instantiate(rocket,attackPoint.position, Quaternion.Euler(cam.eulerAngles.x, cam.eulerAngles.y, cam.eulerAngles.z));
    }

    private void ResetShot()
    {
        canShoot = true;
    }
    
}
