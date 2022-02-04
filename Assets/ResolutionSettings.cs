using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionSettings : MonoBehaviour
{
    private TMP_Dropdown resolutionDropdown;  
    private Resolution[] resolutions;

    // Start is called before the first frame update
    void Start()
    {
        resolutionDropdown = GetComponent<TMP_Dropdown>();        
        resolutions = Screen.resolutions;
        List<string> options = new List<string>();
        int currentResIndex = 0;
        int currentResPos = 0;
        Resolution currentRes = Screen.currentResolution;
        PlayerPrefs.GetInt(currentRes.refreshRate.ToString(), 60);
        foreach (Resolution resolution in resolutions)
        {
            string opt = resolution.ToString();
            options.Add(opt);

            if (resolution.width == currentRes.width && resolution.height == currentRes.height && resolution.refreshRate == currentRes.refreshRate)
            {
                currentResPos = currentResIndex;
            }
            currentResIndex++;
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResPos;
    }

    public void SetResolution()
    {
        Resolution resolution = resolutions[resolutionDropdown.value];       
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode, resolution.refreshRate);
        PlayerPrefs.SetInt(resolution.refreshRate.ToString(), resolution.refreshRate);
    }
}
