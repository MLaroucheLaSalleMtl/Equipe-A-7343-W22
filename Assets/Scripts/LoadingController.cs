using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{
    GameManager manager;

    [Header("--- Scene Settings ---")]
    [SerializeField] public float loadTime = 5f;
    [SerializeField] public string nameOfScene;

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
        LoadScene();
    }

    private void LoadScene()
    {
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
    }

    public void LoadScene(string sceneName, float loadTime)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        Invoke("LoadScene", loadTime);
    }

    // Start is called before the first frame update
    void Start()
    {
        manager = GameManager.instance;
        StartCoroutine(WaitForScene());        
        RandomizeLoadingScene();
    }

    void RandomizeLoadingScene()
    {
        quickTipText.text = quickTip[RNG.GetInstance().Next(0, quickTip.Length)].ToString();
        imageRNG.sprite   = images[RNG.GetInstance().Next(0, images.Length)];         
    }
}