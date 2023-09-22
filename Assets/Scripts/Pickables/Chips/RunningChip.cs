using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningChip : MonoBehaviour, IPickable
{
    public void PickUp()
    {
        Player.player.GetComponent<PlayerMove>().sprintSpeed = 13;
        
        Destroy(this.gameObject);
    }
}
