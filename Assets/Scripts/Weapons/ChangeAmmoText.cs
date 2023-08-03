using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeAmmoText : MonoBehaviour
{

    public TMP_Text clipText;
    public TMP_Text magazineText;
    

    public void RefreshText(Component sender, object data)
    {
        clipText.text = sender.GetComponent<PlayerWeapon>().bulletsClip.ToString();
        magazineText.text = sender.GetComponent<PlayerWeapon>().bulletsMagazineTotal.ToString();
    }
}
