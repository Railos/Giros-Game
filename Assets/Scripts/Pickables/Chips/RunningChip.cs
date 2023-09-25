using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningChip : MonoBehaviour, IPickable
{
    
    public GameEvent pickedUpEvent;
    
    public void PickUp()
    {
        pickedUpEvent.Raise(this);
        Player.player.GetComponent<PlayerMove>().sprintSpeed = 13;
        
        Destroy(this.gameObject);
    }
}
