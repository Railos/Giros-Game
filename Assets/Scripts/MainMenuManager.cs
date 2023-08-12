using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    private GameObject curCanvas;
    
    [Header("Canvas")]
    public GameObject mainCanvas;
    public GameObject settingsCanvas;

    [Header("Sliders")] 
    public GameObject sliderX;
    public GameObject sliderY;

    [Header("Dropdowns")] 
    public GameObject resolution;

    private int[] resolutionsX = new [] {3840, 2560, 1920, 1600, 1366, 1280, 1024, 800};
    private int[] resolutionsY = new [] {2160, 1440, 1080, 900, 768, 720, 768, 600};

    private void Start()
    {
        curCanvas = mainCanvas;
        sliderX.transform.GetChild(1).GetComponent<Slider>().value = PlayerPrefs.GetFloat("SensX");
        sliderY.transform.GetChild(1).GetComponent<Slider>().value = PlayerPrefs.GetFloat("SensY");

        sliderX.transform.GetChild(2).GetComponent<TMP_Text>().text = PlayerPrefs.GetFloat("SensX").ToString();
        sliderY.transform.GetChild(2).GetComponent<TMP_Text>().text = PlayerPrefs.GetFloat("SensY").ToString();

        if (!PlayerPrefs.HasKey("Resolution"))
        {
            PlayerPrefs.SetInt("Resolution", 2);
            PlayerPrefs.Save();
            
            Screen.SetResolution(resolutionsX[2], resolutionsY[2], FullScreenMode.ExclusiveFullScreen);
        }
        resolution.GetComponent<TMP_Dropdown>().value = PlayerPrefs.GetInt("Resolution");
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Settings()
    {
        curCanvas = settingsCanvas;
        mainCanvas.SetActive(false);
        settingsCanvas.SetActive(true);
    }

    public void Back()
    {
        curCanvas = mainCanvas;
        settingsCanvas.SetActive(false);
        mainCanvas.SetActive(true);
    }

    public void ChangeSensitivityX(float value)
    {
        PlayerPrefs.SetFloat("SensX", Mathf.Round(value * 10.0f) * 0.1f);
        PlayerPrefs.Save();

        sliderX.transform.GetChild(2).GetComponent<TMP_Text>().text = PlayerPrefs.GetFloat("SensX").ToString();
    }

    public void ChangeSensitivityY(float value)
    {
        PlayerPrefs.SetFloat("SensY", Mathf.Round(value * 10.0f) * 0.1f);
        PlayerPrefs.Save();
        
        sliderY.transform.GetChild(2).GetComponent<TMP_Text>().text = PlayerPrefs.GetFloat("SensY").ToString();
    }

    public void ChangeResolution(int value)
    {
        Screen.SetResolution(resolutionsX[value], resolutionsY[value], FullScreenMode.ExclusiveFullScreen);
        PlayerPrefs.SetInt("Resolution", value);
        PlayerPrefs.Save();
    }
}
