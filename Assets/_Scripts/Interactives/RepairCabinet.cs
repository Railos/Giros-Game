using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RepairCabinet : Interactive
{
    private bool isInteracting;
    [SerializeField] private GameObject canvasObject;
    [SerializeField] private PlayerWeaponRaycast curWeapon = null;
    private GameObject curCanvas;
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject interactText;
    [SerializeField] private GameObject noWeaponsUnlockedText;
    private int price;

    private List<TMP_Text> texts = new List<TMP_Text>();
    
    [SerializeField] private List<GameObject> buttons = new List<GameObject>();

    private void Start()
    {
        curCanvas = mainCanvas;
    }
    
    private void Update()
    {
        if (!CheckInteract()) 
        {
            interactText.SetActive(false);
            return;
        }

        if (!isInteracting) interactText.SetActive(true);
        
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
            interactText.SetActive(false);
            if (WeaponManager.weaponManager.WeaponsRanged.Count > 0)
            {
                WeaponManager.weaponManager.curWeaponRanged.enabled = false;
            }
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            texts.Clear();
            curCanvas.SetActive(false);
            curCanvas = mainCanvas;
            curCanvas.SetActive(true);
            if (WeaponManager.weaponManager.WeaponsRanged.Count > 0)
            {
                WeaponManager.weaponManager.curWeaponRanged.enabled = true;
            }
        }
        RenderMainCanvasButtons();
        PauseMenu.Instance.canPause = !PauseMenu.Instance.canPause;
        canvasObject.SetActive(!canvasObject.activeSelf);
        Player.player.GetComponent<PlayerLook>().enabled = !Player.player.GetComponent<PlayerLook>().enabled;
        Player.player.GetComponent<PlayerMove>().enabled = !Player.player.GetComponent<PlayerMove>().enabled;
    }

    public void RenderMainCanvasButtons()
    {
        for (int i = 0; i < WeaponManager.weaponManager.WeaponsRanged.Count; i++)
        {
            buttons[i].SetActive(true);
        }

        for (int i = WeaponManager.weaponManager.WeaponsRanged.Count; i < buttons.Count; i++)
        {
            buttons[i].SetActive(false);
        }
        if (WeaponManager.weaponManager.WeaponsRanged.Count == 0)
        {
            noWeaponsUnlockedText.SetActive(true);
            return;
        }
        noWeaponsUnlockedText.SetActive(false);
    }
    
    public void ChangeCanvases(GameObject targetCanvas)
    {
        texts.Clear();
        curCanvas.SetActive(false);
        curCanvas = targetCanvas;
        curCanvas.SetActive(true);
        if (curCanvas == mainCanvas)
        {
            RenderMainCanvasButtons();
        }
    }

    public void UpdateTexts()
    {
        foreach (var Z in texts)
        {
            if (Player.player.money < int.Parse(Z.text))
                Z.color = Color.red;
            else
                Z.color = Color.green;
        }
    }
    
    public void SetPrice(int cost)
    {
        price = cost;
    }

    public void SetCurGun(PlayerWeaponRaycast weapon)
    {
        curWeapon = weapon;
    }

    public void AddText(TMP_Text text)
    {
        texts.Add(text);
        UpdateTexts();
    }

    public void UpgradeDamage(float upgrade)
    {
        if (Player.player.money >= price)
        {
            Player.player.money -= price;
            curWeapon.damage += upgrade;
            UpdateTexts();
        }
    }
    
    public void UpgradeAmmo(int upgrade)
    {
        if (Player.player.money >= price)
        {
            Player.player.money -= price;
            curWeapon.clipCapacity += upgrade;
            UpdateTexts();
        }
    }
    
    public void UpgradeShootSpeed(float upgrade)
    {
        if (Player.player.money >= price)
        {
            Player.player.money -= price;
            curWeapon.waitBeforeNextShot -= upgrade;
            UpdateTexts();
        }
    }
}
