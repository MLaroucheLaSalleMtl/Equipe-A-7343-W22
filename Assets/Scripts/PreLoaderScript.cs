using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreLoaderScript : MonoBehaviour
{
    private AsyncOperation asyncLoad;
    private const float LOADTIME = 5f;

    IEnumerator PreLoaderScene()
    {
        yield return new WaitForSeconds(LOADTIME);
        asyncLoad = SceneManager.LoadSceneAsync(1);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PreLoaderScene());
    }    
}