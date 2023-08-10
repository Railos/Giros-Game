using UnityEngine;

public class ChangeWeaponsRanged : MonoBehaviour
{

    public GameEvent WeaponsRangedChanged;
    
    private void Update()
    {
        if (WeaponManager.weaponManager.WeaponsRanged.Count == 0) return;
        if (Input.GetKeyDown(KeyCode.Alpha1) && WeaponManager.weaponManager.curWeaponRanged != WeaponManager.weaponManager.WeaponsRanged[0])
        {
            Debug.Log("Pistol");
            WeaponsRangedChanged.Raise(WeaponManager.weaponManager.WeaponsRanged[0]);
            WeaponManager.weaponManager.WeaponsRanged[0].SwitchToThis(0);
            return;
        }
        if (WeaponManager.weaponManager.WeaponsRanged.Count == 1) return;
        if (Input.GetKeyDown(KeyCode.Alpha2) && WeaponManager.weaponManager.curWeaponRanged != WeaponManager.weaponManager.WeaponsRanged[1])
        {
            Debug.Log("Assault Rifle");
            WeaponManager.weaponManager.WeaponsRanged[1].SwitchToThis(1);
            WeaponsRangedChanged.Raise(WeaponManager.weaponManager.WeaponsRanged[1]);
            return;
        }
        if (WeaponManager.weaponManager.WeaponsRanged.Count == 2) return;
        if (Input.GetKeyDown(KeyCode.Alpha3) && WeaponManager.weaponManager.curWeaponRanged != WeaponManager.weaponManager.WeaponsRanged[2])
        {
            Debug.Log("Shotgun");
            WeaponManager.weaponManager.WeaponsRanged[2].SwitchToThis(2);
            WeaponsRangedChanged.Raise(WeaponManager.weaponManager.WeaponsRanged[2]);
            return;
        }
        if (WeaponManager.weaponManager.WeaponsRanged.Count == 3) return;
        if (Input.GetKeyDown(KeyCode.Alpha4) && WeaponManager.weaponManager.curWeaponRanged != WeaponManager.weaponManager.WeaponsRanged[3])
        {
            Debug.Log("Rifle");
            WeaponManager.weaponManager.WeaponsRanged[3].SwitchToThis(3);
            WeaponsRangedChanged.Raise(WeaponManager.weaponManager.WeaponsRanged[3]);
            return;
        }
    }
}
