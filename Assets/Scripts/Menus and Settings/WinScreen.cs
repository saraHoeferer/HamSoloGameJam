using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        if (SceneManager.GetSceneByName("Player1Win").IsValid())
        {
            SceneManager.UnloadSceneAsync("Player1Win");
        }
        else
        {
            SceneManager.UnloadSceneAsync("Player2Win");
        }
    }
}