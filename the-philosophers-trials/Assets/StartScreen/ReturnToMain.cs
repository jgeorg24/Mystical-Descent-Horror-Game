using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ReturnToMain : MonoBehaviour
{
    private void Update()
    {
        if(Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (SceneManager.GetActiveScene().name == "Main Scene")
        {
            SceneManager.UnloadSceneAsync("Main Scene");
        }
       
    }
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("StartScene"); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}


