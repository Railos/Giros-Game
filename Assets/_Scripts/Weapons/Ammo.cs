using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour, IPickable
{

    public AudioClip pickUpSound;
    public GameEvent onAmmoPickedUp;
    public GameEvent onTextRefresh;
    public int index;
    public int ammoWhenPickedUp;
    
    public void PickUp()
    {
        if (WeaponManager.weaponManager.WeaponsRanged.Count >= index + 1)
        {
            onAmmoPickedUp.Raise(this, pickUpSound);
            WeaponManager.weaponManager.WeaponsRanged[index].bulletsMagazineTotal += ammoWhenPickedUp;
            onTextRefresh.Raise(WeaponManager.weaponManager.WeaponsRanged[index].GetComponent<PlayerWeaponRanged>());
            Destroy(gameObject);
        }
    }
}
