using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoublePunchDamageChip : MonoBehaviour, IPickable
{
    
    public AudioClip pickUpSound;
    public GameEvent pickedUpEvent;
    
    public void PickUp()
    {
        pickedUpEvent.Raise(this, pickUpSound);
        WeaponManager.weaponManager.fistPunch.damage *= 1.5f;
        Destroy(this.gameObject);
    }
}
