using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpChip : MonoBehaviour, IPickable
{
    public void PickUp()
    {
        Player.player.GetComponent<PlayerMove>().MaxJumps += 1;
        
        Destroy(this.gameObject);
    }
}
