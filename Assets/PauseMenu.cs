using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool m_gamePaused = false;
    public GameObject m_pauseMenu;

    public void Toggle()
    {
        if (m_gamePaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void ResumeGame()
    {
        Debug.Log("Resume the game");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        m_pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        m_gamePaused = false;
    }

    public void PauseGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        m_pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        m_gamePaused = true;
    }

    public void BackToMainMenu()
    {
        Debug.Log("Main menu the game");
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
#if UNITY_EDITOR
        Debug.Log("Exited the game");
#endif
        Application.Quit();
    }
}
