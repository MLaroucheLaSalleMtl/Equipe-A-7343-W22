using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    private AsyncOperation asyncLoad;
    private const float LOADTIME = 5f;
    
    public IEnumerator InGameLoadingScene()
    {        
        asyncLoad = SceneManager.LoadSceneAsync(2);
        yield return new WaitForSeconds(LOADTIME);
    }    
}