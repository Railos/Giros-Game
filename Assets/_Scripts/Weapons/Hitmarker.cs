using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hitmarker : MonoBehaviour
{
    public RawImage hitmarker;
    public GameEvent tookDamageEvent;
    public AudioClip hitmarkerSound;

    public void CheckHitmarker(Component sender, object data)
    {
        if (data is AudioClip) return;
        
        if (hitmarker.enabled)
        {
            StopCoroutine(ShowHitmarker());
            StartCoroutine(ShowHitmarker());
        }
        else
        {
            StartCoroutine(ShowHitmarker());
        }
        tookDamageEvent.Raise(this, hitmarkerSound);
    }

    IEnumerator ShowHitmarker()
    {
        hitmarker.enabled = true;
        
        yield return new WaitForSeconds(0.25f);

        hitmarker.enabled = false;
    }
}
