using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningChip : MonoBehaviour, IPickable
{

    public AudioClip pickUpSound;
    public GameEvent pickedUpEvent;
    
    public void PickUp()
    {
        pickedUpEvent.Raise(this, pickUpSound);
        Player.player.GetComponent<PlayerMove>().sprintSpeed += 3f;
        
        Destroy(this.gameObject);
    }
}
