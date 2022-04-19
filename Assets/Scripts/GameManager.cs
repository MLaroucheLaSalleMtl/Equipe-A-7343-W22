using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject completLevelUi;
    public void CompletTheGame()
    {        
        completLevelUi.SetActive(true);
    }
    
    public void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
