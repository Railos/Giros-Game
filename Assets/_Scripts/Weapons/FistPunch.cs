using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class FistPunch : MonoBehaviour
{

    [Header("Stats")] 
    public KeyCode punchButton;
    public bool canPunch;
    public float damage;
    public float reloadTime;
    public float punchSphereRadius;
    public float punchDistance;

    [Header("Camera Shake")] 
    public float magnitude;
    public float roughness;
    public float fadeInTime;
    public float fadeOutTime;
    
    [Header("Refs")] 
    public Camera cam;

    public Animator animator;

    private RaycastHit _out;
    
    private void Update()
    {
        Inputing();
    }

    private void Inputing()
    {
        if (!canPunch) return;
        if (!Input.GetKeyDown(punchButton)) return;
        StartCoroutine(nameof(Punch));
    }

    private IEnumerator Punch()
    {
        canPunch = false;
        animator.gameObject.SetActive(true);
        animator.CrossFade("FistPunch", 0,0);

        CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);
        StartCoroutine(nameof(Waitings));
        for (int i = 0; i < 5; i++)
        {
            if (Physics.Raycast(cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out _out, punchDistance))
            {
                if (_out.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    _out.collider.GetComponent<Enemy>().Health -= damage;
                }
                break;
            }
            yield return new WaitForSeconds(0.02f);
        }
        
        Invoke(nameof(ReloadPunch), reloadTime);
    }

    private IEnumerator Waitings()
    {
        yield return new WaitForSeconds(0.4f);
        animator.CrossFade("FistIdle",0.1f,0);
        yield return new WaitForSeconds(.1f);
        animator.gameObject.SetActive(false);
    }
    
    private void ReloadPunch()
    {
        canPunch = true;
    }
}
