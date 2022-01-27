using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{  
  
    public GameObject MainMenu;
    public GameObject OptionsMenu;
   

    public void StartButton()
    {
        SceneManager.LoadScene("Overworld");
    }

    public void Quit()
    {
        Debug.Log("Exited the game");
        Application.Quit();
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


    public void OpenMainMenu()
    {
        OptionsMenu.SetActive(false);
        MainMenu.SetActive(true);
      
    }

    public void OpenOptions()
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(true);
       
    }
}
