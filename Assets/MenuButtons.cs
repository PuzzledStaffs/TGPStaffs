using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MenuButtons : MonoBehaviour
{
    [FormerlySerializedAs("MainMenu")]
    public GameObject m_mainMenu;
    [FormerlySerializedAs("OptionsMenu")]
    public GameObject m_optionsMenu;
    public GameObject m_newGame;
   

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
        m_optionsMenu.SetActive(false);
        m_mainMenu.SetActive(true);
      
    }

    public void OpenOptions()
    {
        m_mainMenu.SetActive(false);
        m_optionsMenu.SetActive(true);
       
    }
    public void OpenNewGame()
    {
        Debug.Log("function called");
        m_mainMenu.SetActive(false);
        m_newGame.SetActive(true);

    }
}
