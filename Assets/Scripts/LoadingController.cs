using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{
    //public static LoadingController instance = null;

    GameManager manager;

    [SerializeField] public float loadTime = 5f;
    [SerializeField] public string nameOfScene;

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
    }
}