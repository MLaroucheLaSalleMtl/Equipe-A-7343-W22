using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForSeconds());
    }
}
