using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("Overworld");
    }
    public void OptionsButton()
    {
        Debug.Log("check");
        SceneManager.LoadScene("Options");
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
}
