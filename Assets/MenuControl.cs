using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{


    public void Play()
    {
        SceneManager.LoadScene(1);
    }
   
    public void Rules()
    {
        SceneManager.LoadScene(2);
    }

    public void Returm()
    {
        SceneManager.LoadScene(0);
    }

     public void QuitGame()
    {
        Application.Quit();
    }

}
