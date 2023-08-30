using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused;
    public GameObject pauseCanvas;

    private void Update()
    {
        isPaused = Input.GetKeyDown(KeyCode.Escape) ? !isPaused : isPaused;

        if (isPaused)
        {
            Pause();
        }else if (Time.timeScale == 0f)
        {
            UnPause();
        }
    }

    private void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        Player.player.GetComponent<PlayerLook>().enabled = false;
        Player.player.GetComponent<PlayerMove>().enabled = false;
        Player.player.GetComponent<AudioSource>().enabled = false;
        if (WeaponManager.weaponManager.curWeaponRanged)
            WeaponManager.weaponManager.curWeaponRanged.enabled = false;
        Player.player.isPaused = true;
        
        pauseCanvas.SetActive(true);
    }

    public void UnPause()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
        Player.player.GetComponent<PlayerLook>().enabled = true;
        Player.player.GetComponent<PlayerMove>().enabled = true;
        Player.player.GetComponent<AudioSource>().enabled = true;
        if (WeaponManager.weaponManager.curWeaponRanged)
            WeaponManager.weaponManager.curWeaponRanged.enabled = true;
        Player.player.isPaused = false;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
