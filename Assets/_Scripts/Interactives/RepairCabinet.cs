using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairCabinet : Interactive
{
    private bool isInteracting;
    [SerializeField] private GameObject canvasObject;
    private GameObject curCanvas;

    private void Start()
    {
        curCanvas = canvasObject;
    }

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
        curCanvas = canvasObject;
        canvasObject.SetActive(!canvasObject.activeSelf);
        Player.player.GetComponent<PlayerLook>().enabled = !Player.player.GetComponent<PlayerLook>().enabled;
        Player.player.GetComponent<PlayerMove>().enabled = !Player.player.GetComponent<PlayerMove>().enabled;
    }

    public void ChangeCanvases(GameObject targetCanvas)
    {
        if (curCanvas != canvasObject) curCanvas.SetActive(false);
        curCanvas = targetCanvas;
        curCanvas.SetActive(true);
    }
}
