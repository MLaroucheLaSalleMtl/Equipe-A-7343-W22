using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [Header("Levels To load")]
    public string _newGameLevel;
    private string levelToLoad;
    [SerializeField] private GameObject noSavedGame = null;

    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(_newGameLevel);
    }

    public void LoadGameDialogYes()
    {
       if(PlayerPrefs.HasKey ("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            noSavedGame.SetActive(true);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }
    
}
