using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{

    public GameEvent onAmmoPickUp;
    public int index;
    public int ammoWhenPickedUp;
    
    public void CanPickUp()
    {
        if (WeaponManager.weaponManager.Weapons.Count >= index + 1) PickUp();
    }

    private void PickUp()
    {
        WeaponManager.weaponManager.Weapons[index].bulletsMagazineTotal += ammoWhenPickedUp;
        onAmmoPickUp.Raise(WeaponManager.weaponManager.Weapons[index].GetComponent<PlayerWeapon>());
        Destroy(gameObject);
    }
}
