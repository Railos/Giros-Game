using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AmmoCylinder : MonoBehaviour
{
    [SerializeField] private PlayerWeaponRaycast weaponScript;
    [SerializeField] private Color color;
    [SerializeField] private Material ammoMat;

    private const string EMISSIONCOLOR = "_EmissionColor";
    private const string EMISSION = "_EMISSION";

    private void Start()
    {
        ammoMat = GetComponent<Renderer>().material;
        ammoMat.EnableKeyword(EMISSION);
        ammoMat.SetColor(EMISSIONCOLOR, color * Mathf.Pow(2, 3f));
    }

    public void ChangeColor()
    {
        ammoMat.SetColor(EMISSIONCOLOR,color * Mathf.Pow(2, 3 * ((float)weaponScript.bulletsClip / (float)weaponScript.clipCapacity)));
    }
}
