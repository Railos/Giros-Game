using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hitmarker : MonoBehaviour
{
    public RawImage hitmarker;

    public void CheckHitmarker()
    {
        if (hitmarker.enabled)
        {
            StopCoroutine(ShowHitmarker());
            StartCoroutine(ShowHitmarker());
        }
        else
        {
            StartCoroutine(ShowHitmarker());
        }
    }

    IEnumerator ShowHitmarker()
    {
        hitmarker.enabled = true;
        
        yield return new WaitForSeconds(0.25f);

        hitmarker.enabled = false;
    }
}
