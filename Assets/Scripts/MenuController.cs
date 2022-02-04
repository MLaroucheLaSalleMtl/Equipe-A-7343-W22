using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour
{
    public static MenuController instance = null;
    ResolutionSettings ResSettings;
    LoadingController loadingController;

    [Header("volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    //[SerializeField] private GameObject confirmationPrompt = null;
    [SerializeField] private float defaulVolume = 1.0f;

    [Header("Graphics Setting")]
    private int _qualityLevel;
    private bool _isFullScreen;

    [SerializeField] private TMP_Dropdown qualtyDropdown;
    [SerializeField] Toggle fullScreenToggle;

    [Header("Levels To load")]
    public string _newGameLevel;
    public string levelToLoad;
    [SerializeField] private GameObject noSavedGame = null;

    [Header("Resolution Dropdown")]
    [SerializeField] public TMP_Dropdown resolutionDropdown;
    private Dropdown resDropdown;
    private Resolution[] resolutions;

    private void Start()
    {
        loadingController = GetComponent<LoadingController>();
        ResSettings = gameObject.GetComponent<ResolutionSettings>();       
    }    

    public void NewGameDialogYes()
    {
        //SceneManager.LoadScene(_newGameLevel);
        //loadingController.GetComponent<LoadingController>().LoadScene();
        loadingController.LoadScene(levelToLoad);
    }

    public void LoadGameDialogYes()
    {
        if(PlayerPrefs.HasKey ("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            //SceneManager.LoadScene(levelToLoad);
            loadingController.LoadScene();
            loadingController.LoadScene(levelToLoad);
        }
        else
        {
            noSavedGame.SetActive(true);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        //StartCoroutine(ConfirmationBox());
        //show Promt
    }

    public void SetFullScreen(bool isFullScreen)
    {
        _isFullScreen = isFullScreen;
    }

    public void SetQuality(int isQuality)
    {
        _qualityLevel = isQuality;
    }

    public void GraphicsApply()
    {        
        //Temporary
        QualitySettings.SetQualityLevel(_qualityLevel);
        PlayerPrefs.SetInt("masterQuality", _qualityLevel);

        Screen.fullScreen = _isFullScreen;
        PlayerPrefs.SetInt("masterFullScreen", (_isFullScreen ? 1 : 0));
    }

    public void ResetButton(string Menutype)
    {
        if (Menutype == "Graphics")
        {
            qualtyDropdown.value = 1;
            QualitySettings.SetQualityLevel(1);

            fullScreenToggle.isOn = false;
            Screen.fullScreen = false;

            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreenMode, currentResolution.refreshRate);
            resolutionDropdown.value = resolutions.Length;
            GraphicsApply();
        }
        if (Menutype == "Audio")
        {
            AudioListener.volume = defaulVolume;
            volumeSlider.value = defaulVolume;
            volumeTextValue.text = defaulVolume.ToString("0.0");
            VolumeApply();
        }
    }
        
}
