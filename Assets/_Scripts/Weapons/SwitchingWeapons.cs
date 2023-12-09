using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchingWeapons : MonoBehaviour
{
    [SerializeField] private int index;
    [SerializeField] private KeyCode switchKey;
    [SerializeField] private PlayerWeaponRanged weaponScript;
    
    private void Update()
    {
        if (WeaponManager.weaponManager.WeaponsRanged.Count < index + 1) return;
        if (Input.GetKeyDown(switchKey) && WeaponManager.weaponManager.curWeaponRanged != weaponScript)
        {
            SwitchToThis();
        }
    }
    
    private void SwitchToThis()
    {
        if (WeaponManager.weaponManager.curWeaponRanged.isReloading)
        {
            WeaponManager.weaponManager.curWeaponRanged.interruptReload = true;
            WeaponManager.weaponManager.curWeaponRanged.isReloading = false;
        }
        transform.GetChild(0).gameObject.SetActive(true);
        weaponScript.textRefresh.Raise(this);
        WeaponManager.weaponManager.curWeaponRanged.transform.GetChild(0).gameObject.SetActive(false);
        WeaponManager.weaponManager.curWeaponRanged.isChosen = false;
        WeaponManager.weaponManager.curWeaponRanged = WeaponManager.weaponManager.WeaponsRanged[index];
        weaponScript.isChosen = true;
    }
}
