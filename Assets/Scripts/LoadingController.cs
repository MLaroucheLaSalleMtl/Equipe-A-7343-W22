using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{
    //public static LoadingController instance = null;
    GameManager manager;
    MenuController menuController;

    [Header("--- Scene Settings ---")]
    [SerializeField] public float loadTime = 10f;
    //[SerializeField] public string nameOfScene;

    [Header("--- Loading Images ---")]
    [SerializeField] private Image imageRNG;    
    [SerializeField] private Sprite[] images;

    [Header("--- Loading Quick Tips ---")]
    [SerializeField] private TMP_Text quickTipText;
    [SerializeField] private static readonly string[] quickTip = {
        "Random Message A", 
        "Random Message B",
        "Random Message C",
        "Random Message D"
    };    

    IEnumerator WaitForScene()
    {
        yield return new WaitForSeconds(loadTime);        
        LoadScene(menuController.levelToLoad);
    }

    public void LoadScene()
    {
        SceneManager.LoadSceneAsync("Loading B (Main Menu To Game)", LoadSceneMode.Single);
    }

    public void LoadScene(string sceneName/*, float loadTime*/)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        //Invoke("LoadScene", loadTime);
    }

    // Start is called before the first frame update
    void Start()
    {
        manager = GameManager.instance;
        menuController = MenuController.instance;
        StartCoroutine(WaitForScene());        
        RandomizeLoadingScene();
    }

    void RandomizeLoadingScene()
    {
        quickTipText.text = quickTip[RNG.GetInstance().Next(0, quickTip.Length)];
        imageRNG.sprite   = images[RNG.GetInstance().Next(0, images.Length)];         
    }
}