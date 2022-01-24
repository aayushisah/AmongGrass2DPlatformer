using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    public Button [] lvlButtons;

    void Start()
    {
        int levelAt = PlayerPrefs.GetInt("levelAt", 4);
        for (int i = 0; i < lvlButtons.Length; i++)
        {
            if (i + 4 > levelAt)
            lvlButtons[i].interactable = false;
        }
    }
    
    public void Level1()
    {
        SceneManager.LoadScene(4);
    }

    public void Level2()
    {
        SceneManager.LoadScene(5);
    }
   
    public void Rules()
    { 
        SceneManager.LoadScene(3);
    }

    public void MainMenu()
    {        
        SceneManager.LoadScene(0);
    }

    public void GoBack()
    {
        SceneManager.LoadScene(0);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        //PlayerPrefs.DeleteAll(); 
        Application.Quit();
    }
}

