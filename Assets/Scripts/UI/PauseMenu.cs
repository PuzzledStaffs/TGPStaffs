using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool m_gamePaused = false;
    public GameObject m_pauseMenu;
    public Action m_pause;
    public Action m_unPause;
    // Save File Menu Objects
    public GameObject m_savesMenu;
    public GameObject m_saveAutoButton;
    public SaveFileButton m_save1Button;
    public SaveFileButton m_save2Button;
    public SaveFileButton m_save3Button;

    public void Toggle()
    {
        if (m_gamePaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void ResumeGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        m_pauseMenu.SetActive(false);
        m_savesMenu.SetActive(false);
        Time.timeScale = 1f;
        m_unPause?.Invoke();
        m_gamePaused = false;
    }

    public void PauseGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        m_savesMenu.SetActive(false);
        m_pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        m_pause?.Invoke();
        m_gamePaused = true;
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1.0f;
        m_gamePaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Save File Stuff
    public void OpenSaveMenu()
    {
        if (PersistentPrefs.GetInstance().HasSaveFile(1))
        {
            SaveFile file = PersistentPrefs.GetInstance().LoadSaveFile(1);
            m_save1Button.m_playtime.text = file.m_saveHours + ":" + (file.m_saveMinutes < 10 ? "0" : "") + file.m_saveMinutes;
            m_save1Button.m_date.text = file.m_saveDate;
            m_save1Button.m_level.text = file.m_currentScene;
        }
        if (PersistentPrefs.GetInstance().HasSaveFile(2))
        {
            SaveFile file = PersistentPrefs.GetInstance().LoadSaveFile(2);
            m_save2Button.m_playtime.text = file.m_saveHours + ":" + (file.m_saveMinutes < 10 ? "0" : "") + file.m_saveMinutes;
            m_save2Button.m_date.text = file.m_saveDate;
            m_save2Button.m_level.text = file.m_currentScene;
        }
        if (PersistentPrefs.GetInstance().HasSaveFile(3))
        {
            SaveFile file = PersistentPrefs.GetInstance().LoadSaveFile(3);
            m_save3Button.m_playtime.text = file.m_saveHours + ":" + (file.m_saveMinutes < 10 ? "0" : "") + file.m_saveMinutes;
            m_save3Button.m_date.text = file.m_saveDate;
            m_save3Button.m_level.text = file.m_currentScene;
        }

        m_pauseMenu.SetActive(false);
        m_savesMenu.SetActive(true);
    }

    public void TrySaveSave(int save)
    {
        PersistentPrefs.GetInstance().m_currentSaveFile.m_savePosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        PersistentPrefs.GetInstance().SaveSaveFile(save);
        OpenSaveMenu();
    }
}
