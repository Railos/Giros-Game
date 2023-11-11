using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{

    public AudioSource audioSource;
    
    public void PlaySound(Component sender, object data)
    {
        if (data is AudioClip)
            audioSource.PlayOneShot((AudioClip)data);
    }
}
