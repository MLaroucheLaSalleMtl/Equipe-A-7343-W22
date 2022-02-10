using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    private AsyncOperation _asyncLoad;
    /*[HideInInspector][SerializeField]public*/ private bool sceneIsReady;

    [Header("--- Scene Settings ---")]
    [SerializeField] private float loadTimeDelay = 0f;
    [SerializeField] private string nameOfScene;
    [SerializeField]private bool randomizeItems = false;
    [Header("--- Only Use If 'Use User Input' Is Checked ---")]
    [Tooltip("Use Continue Button if you want the player to press a Button on start (Ex. Start Loading Scene)")]
    [SerializeField] public bool useContinueBtn = false;
    [SerializeField] public GameObject continueBtn;
    [SerializeField] public GameObject userInputTxt;
    [SerializeField] public GameObject loadingIcon;
 
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

    IEnumerator LoadingIconCoroutine() 
    {
        yield return new WaitForSeconds(loadTimeDelay);
        continueBtn.SetActive(true);
        loadingIcon.SetActive(false);
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