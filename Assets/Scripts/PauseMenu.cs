using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUi;
    [SerializeField] private bool isPaused;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }
            if (isPaused)
            {
                MenuActive();
            }
            else
            {
                MenuDesactive();
            }
        
    }

   public void MenuActive()
    {
        Time.timeScale = 0f;
        pauseMenuUi.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;        
    }

    public void MenuDesactive()
    {
        Time.timeScale = 1f;
        pauseMenuUi.SetActive(false);
        isPaused = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
