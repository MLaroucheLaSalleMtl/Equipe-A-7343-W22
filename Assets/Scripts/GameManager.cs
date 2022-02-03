using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    LoadingController SceneController;

    //[SerializeField] public float loadTime;
    //[SerializeField] public string sceneName;

    // Start is called before the first frame update
    public void Start()
    {
        SceneController = GetComponent<LoadingController>();
        SceneController.LoadScene(SceneController.nameOfScene, SceneController.loadTime);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }              
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
