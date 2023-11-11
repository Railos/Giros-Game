using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpChip : MonoBehaviour, IPickable
{
    
    public AudioClip pickUpSound;
    public GameEvent pickedUpEvent;
    
    public void PickUp()
    {
        pickedUpEvent.Raise(this, pickUpSound);
        Player.player.GetComponent<PlayerMove>().MaxJumps += 1;
        
        Destroy(this.gameObject);
    }
}
