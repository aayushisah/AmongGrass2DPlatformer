using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{


    public void Level1()
    {
        SceneManager.LoadScene(1);
    }

    public void Level2()
    {
        SceneManager.LoadScene(5);
    }
   
    public void Rules()
    {
        SceneManager.LoadScene(2);
    }

    public void GoBack()
    {
        SceneManager.LoadScene(0);
    }

     public void QuitGame()
    {
        Application.Quit();
    }

}
