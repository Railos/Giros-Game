using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoublePunchDamageChip : MonoBehaviour, IPickable
{
    
    public GameEvent pickedUpEvent;
    
    public void PickUp()
    {
        pickedUpEvent.Raise(this);
        WeaponManager.weaponManager.fistPunch.damage *= 1.5f;
        Destroy(this.gameObject);
    }
}
