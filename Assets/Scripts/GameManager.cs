using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    
    public void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainMenu(int number)
    {
        SceneManager.LoadSceneAsync(3);
    }
}
