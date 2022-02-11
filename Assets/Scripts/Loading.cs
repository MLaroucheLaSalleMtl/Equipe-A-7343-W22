using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using System.Linq;

public class Loading : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private AsyncOperation _asyncLoad;
    private bool sceneIsReady;

    [Header("--- Scene Settings ---")]
    [SerializeField] private float loadTimeDelay = 0f;
    [SerializeField] private string nameOfScene;
    [SerializeField] private bool randomizeItems = false;
    [Header("--- Only Use If 'Use User Input' Is Checked ---")]
    [Tooltip("Use Continue Button if you want the player to press a Button on start (Ex. Start Loading Scene)")]
    [SerializeField] public bool useContinueBtn = false;
    [SerializeField] public GameObject continueBtn;
    [SerializeField] public GameObject userInputTxt;
    [SerializeField] public GameObject loadingIcon;
    [Header("-- Continue Icon depending on the Controller Scheme --")]
    [SerializeField] public GameObject UserButtonPC;
    [SerializeField] public GameObject UserButtonXbox;
    [SerializeField] public GameObject UserButtonPlaystation;

    [Header("--- Loading Images ---")]
    [Tooltip("Add an Image for it to be Randomized")]
    [SerializeField] private Image imageRNG;    
    [SerializeField] private Sprite[] images;

    [Header("--- Loading Quick Tips ---")]
    [SerializeField] private TMP_Text quickTipText;
    [Tooltip("Add an QuickTip for it to be Randomized in the")]
    [SerializeField] private static readonly string[] quickTip = 
    {
        "Random Message A", 
        "Random Message B",
        "Random Message C",
        "Random Message D"
    };

    //InputDevice device;

    private void OnEnable()
    {
        InputSystem.onDeviceChange += InputSystem_onDeviceChange;
    }

    IEnumerator LoadingIconCoroutine() 
    {
        yield return new WaitForSeconds(loadTimeDelay);
        if (InputSystem.GetDevice("Keyboard").enabled)
        {
            InputSystem.GetDevice("Keyboard").MakeCurrent();
            continueBtn.SetActive(true);
            UserButtonPC.SetActive(true);
            UserButtonXbox.SetActive(false);
            UserButtonPlaystation.SetActive(false);
        }
        //if (/*InputSystem.GetDevice("Joystick").enabled &&*/ device.name == "Controller (Xbox One For Windows)")
        //{
        //    InputSystem.GetDevice("Joystick").MakeCurrent();
        //    continueBtn.SetActive(true);
        //    UserButtonXbox.SetActive(true);
        //    UserButtonPC.SetActive(false);
        //    UserButtonPlaystation.SetActive(false);
        //}        
        loadingIcon.SetActive(false);        
    }

    private void InputSystem_onDeviceChange(InputDevice device, InputDeviceChange deviceChange)
    {
        switch (deviceChange)
        {
            case InputDeviceChange.Added:
                Debug.Log(device.displayName + " has been added. ");
                break;
            case InputDeviceChange.Removed:
                Debug.Log(device.displayName + " has been removed. ");
                break;
            case InputDeviceChange.Disconnected:
                Debug.Log(device.displayName + " has been disconnected. "); 
                break;
            case InputDeviceChange.Reconnected:
                Debug.Log(device.displayName + " has been reconnected. "); 
                break;
            case InputDeviceChange.Enabled:
                Debug.Log(device.displayName + " has been enabled. ");
                break;
            case InputDeviceChange.Disabled:
                Debug.Log(device.displayName + " has been disabled. ");
                break;
            case InputDeviceChange.UsageChanged:
                break;
            case InputDeviceChange.ConfigurationChanged:
                break;                      
            default:
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {        
        StartSettings();
        if (randomizeItems)
        {
            RandomizeLoadingScene();
            LoadScene();
        }
        else
            LoadScene();
    }

    void StartSettings()
    {
        Time.timeScale = 1.0f;
        if (Gamepad.current == InputSystem.GetDevice("XInputControllerWindows"))
        {
            UserButtonXbox.SetActive(false);
        }
        Input.ResetInputAxes();
        System.GC.Collect();
    }    

    public void LoadScene()
    {
        _asyncLoad = SceneManager.LoadSceneAsync(nameOfScene);

        _asyncLoad.allowSceneActivation = false;
        if (!useContinueBtn)
        {
            Invoke("isActive", loadTimeDelay);            
        }
    }      

    public void isActive() 
    {
        sceneIsReady = true;
    }

    void Update()
    {
        if (useContinueBtn)
        {                
            if (userInputTxt && continueBtn)
            {
                StartCoroutine("LoadingIconCoroutine");             
            }
        }

        if (sceneIsReady)
        {
            _asyncLoad.allowSceneActivation = true;
        }
    }    

    void RandomizeLoadingScene()
    {
        quickTipText.text = quickTip[RNG.GetInstance().Next(0, quickTip.Length)];
        imageRNG.sprite   = images[RNG.GetInstance().Next(0, images.Length)];         
    }
}