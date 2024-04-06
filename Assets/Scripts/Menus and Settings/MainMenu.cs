using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
        SceneManager.UnloadSceneAsync("Main Menu");
    }

    public void OpenTutorial()
    {
        SceneManager.LoadScene("Tutorial",LoadSceneMode.Additive);
    }

    public void CloseTutorial()
    {
        SceneManager.UnloadSceneAsync("Tutorial");
    }

    public void OpenOptions()
    {
        SceneManager.LoadScene("Options",LoadSceneMode.Additive);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
