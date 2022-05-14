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
    public GameObject m_savesMenu;

    public void StartButton()
    {
        if (!PersistentPrefs.HasSaveFile(1) && !PersistentPrefs.HasSaveFile(2) && !PersistentPrefs.HasSaveFile(3))
            NewGame();
        else
        {
            m_mainMenu.SetActive(false);
            m_savesMenu.SetActive(true);
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void BackToMainMenu()
    {
        m_optionsMenu.SetActive(false);
        m_savesMenu.SetActive(false);
        m_mainMenu.SetActive(true);
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

    public void NewGame()
    {
        PersistentPrefs.LoadDefaults();
        SceneManager.LoadScene("Overworld");
    }

    public void ChangeVolume(float volume)
    {
        SoundManager.m_instance.ChangeMasterVolume(volume);
    }
}
