using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour
{
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
    private string levelToLoad;
    [SerializeField] private GameObject noSavedGame = null;

    [Header("Resolution Dropdown")]
    public TMP_Dropdown resolutionDropdown;
    private  Resolution[] resolutions;

    private void Start()
    {
        resolutionDropdown.ClearOptions();
        resolutions = Screen.resolutions;

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length ; i++)
            {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

    }

    public void SetResolution(int resolutionDropdownIndex)
    {
        Resolution resolution = resolutions[resolutionDropdownIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
    }

    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(_newGameLevel);
    }

    public void LoadGameDialogYes()
    {
       if(PlayerPrefs.HasKey ("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
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
        PlayerPrefs.SetInt("masterQuality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);

        PlayerPrefs.SetInt("masterFullScreen", (_isFullScreen ? 1 : 0));
        Screen.fullScreen = _isFullScreen;

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
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreenMode);
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
