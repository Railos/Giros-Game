using UnityEngine;

public class ChangeWeapons : MonoBehaviour
{

    public GameEvent weaponChanged;
    
    private void Update()
    {
        if (WeaponManager.weaponManager.Weapons.Count == 0) return;
        if (Input.GetKeyDown(KeyCode.Alpha1) && WeaponManager.weaponManager.curWeapon != WeaponManager.weaponManager.Weapons[0])
        {
            Debug.Log("Pistol");
            weaponChanged.Raise(WeaponManager.weaponManager.Weapons[0]);
            WeaponManager.weaponManager.Weapons[0].SwitchToThis(0);
            return;
        }
        if (WeaponManager.weaponManager.Weapons.Count == 1) return;
        if (Input.GetKeyDown(KeyCode.Alpha2) && WeaponManager.weaponManager.curWeapon != WeaponManager.weaponManager.Weapons[1])
        {
            Debug.Log("Assault Rifle");
            WeaponManager.weaponManager.Weapons[1].SwitchToThis(1);
            weaponChanged.Raise(WeaponManager.weaponManager.Weapons[1]);
            return;
        }
        if (WeaponManager.weaponManager.Weapons.Count == 2) return;
        if (Input.GetKeyDown(KeyCode.Alpha3) && WeaponManager.weaponManager.curWeapon != WeaponManager.weaponManager.Weapons[2])
        {
            Debug.Log("Shotgun");
            WeaponManager.weaponManager.Weapons[2].SwitchToThis(2);
            weaponChanged.Raise(WeaponManager.weaponManager.Weapons[2]);
            return;
        }
        if (WeaponManager.weaponManager.Weapons.Count == 3) return;
        if (Input.GetKeyDown(KeyCode.Alpha4) && WeaponManager.weaponManager.curWeapon != WeaponManager.weaponManager.Weapons[3])
        {
            Debug.Log("Rifle");
            WeaponManager.weaponManager.Weapons[3].SwitchToThis(3);
            weaponChanged.Raise(WeaponManager.weaponManager.Weapons[3]);
            return;
        }
    }
}
