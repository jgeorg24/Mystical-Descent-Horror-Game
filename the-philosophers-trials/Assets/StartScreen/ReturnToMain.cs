using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ReturnToMain : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0); // Replace "YourGameScene" with the actual name of your game scene
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}


