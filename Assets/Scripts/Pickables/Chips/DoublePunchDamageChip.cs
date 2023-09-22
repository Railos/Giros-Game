using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoublePunchDamageChip : MonoBehaviour, IPickable
{
    public void PickUp()
    {
        WeaponManager.weaponManager.fistPunch.damage *= 1.5f;
        Destroy(this.gameObject);
    }
}
