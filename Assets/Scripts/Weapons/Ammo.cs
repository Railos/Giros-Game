using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{

    public GameEvent onAmmoPickedUp;
    public GameEvent onTextRefresh;
    public int index;
    public int ammoWhenPickedUp;

    public void CanPickUp()
    {
        if (WeaponManager.weaponManager.WeaponsRanged.Count >= index + 1) PickUp();
    }

    private void PickUp()
    {
        onAmmoPickedUp.Raise();
        WeaponManager.weaponManager.WeaponsRanged[index].bulletsMagazineTotal += ammoWhenPickedUp;
        onTextRefresh.Raise(WeaponManager.weaponManager.WeaponsRanged[index].GetComponent<PlayerWeaponRanged>());
        Destroy(gameObject);
    }
}
