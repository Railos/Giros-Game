using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairCabinet : Interactive
{
    private bool isInteracting;
    [SerializeField] private GameObject canvasObject;
    [SerializeField] private PlayerWeaponRaycast curWeapon = null;
    [SerializeField] private GameObject curCanvas;
    private int price;
    
    private void Update()
    {
        if (!CheckInteract()) return;

        if (Input.GetKeyDown(KeyCode.E))
            Interact();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isInteracting)
            {
                Interact();
            }
        }
    }

    public override void Interact()
    {
        isInteracting = !isInteracting;
        if (isInteracting)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        PauseMenu.Instance.canPause = !PauseMenu.Instance.canPause;
        canvasObject.SetActive(!canvasObject.activeSelf);
        Player.player.GetComponent<PlayerLook>().enabled = !Player.player.GetComponent<PlayerLook>().enabled;
        Player.player.GetComponent<PlayerMove>().enabled = !Player.player.GetComponent<PlayerMove>().enabled;
    }

    public void ChangeCanvases(GameObject targetCanvas)
    {
        curCanvas.SetActive(false);
        curCanvas = targetCanvas;
        curCanvas.SetActive(true);
    }

    public void SetPrice(int cost)
    {
        price = cost;
    }

    public void SetCurGun(PlayerWeaponRaycast weapon)
    {
        curWeapon = weapon;
    }

    public void UpgradeDamage(float upgrade)
    {
        if (Player.player.money >= price)
        {
            Player.player.money -= price;
            curWeapon.damage += upgrade;
        }
    }
    
    public void UpgradeAmmo(int upgrade)
    {
        if (Player.player.money >= price)
        {
            Player.player.money -= price;
            curWeapon.clipCapacity += upgrade;
        }
    }
    
    public void UpgradeShootSpeed(float upgrade)
    {
        if (Player.player.money >= price)
        {
            Player.player.money -= price;
            curWeapon.waitBeforeNextShot -= upgrade;
        }
    }
}
