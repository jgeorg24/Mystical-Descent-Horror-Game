using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1); // Replace "YourGameScene" with the actual name of your game scene
    }

    public void InstructionsLoad()
    {
        SceneManager.LoadScene(2); // Replace "YourGameScene" with the actual name of your game scene    }
    }
}


