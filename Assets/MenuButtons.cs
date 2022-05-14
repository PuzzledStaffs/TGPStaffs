using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MenuButtons : MonoBehaviour
{
    [Header("Main Menu")]
    [FormerlySerializedAs("MainMenu")]
    public GameObject m_mainMenu;
    public GameObject m_newGameButton;

    [Header("Options Menu")]
    [FormerlySerializedAs("OptionsMenu")]
    public GameObject m_optionsMenu;
    public Slider m_soundSlider;

    [Header("Saves Menu")]
    public GameObject m_savesMenu;
    public GameObject m_saveAutoButton;
    public GameObject m_save1Button;
    public GameObject m_save2Button;
    public GameObject m_save3Button;

    private void Start()
    {
        if (!PersistentPrefs.GetInstance().HasSaveFile(0) &&
            !PersistentPrefs.GetInstance().HasSaveFile(1) &&
            !PersistentPrefs.GetInstance().HasSaveFile(2) &&
            !PersistentPrefs.GetInstance().HasSaveFile(3))
            m_newGameButton.SetActive(false);

        m_soundSlider.SetValueWithoutNotify(SoundManager.m_instance.GetMasterVolume());

        OpenMainMenu();
    }

    // Main Menu Code
    public void OpenMainMenu()
    {
        m_optionsMenu.SetActive(false);
        m_savesMenu.SetActive(false);

        m_mainMenu.SetActive(true);
    }

    public void Play()
    {
        if (!PersistentPrefs.GetInstance().HasSaveFile(0) &&
            !PersistentPrefs.GetInstance().HasSaveFile(1) &&
            !PersistentPrefs.GetInstance().HasSaveFile(2) &&
            !PersistentPrefs.GetInstance().HasSaveFile(3))
            NewGame();
        else
            OpenSaveFileMenu();
    }

    public void NewGame()
    {
        PersistentPrefs.GetInstance().LoadDefaultSave();
        SceneManager.LoadScene("Overworld");
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Options Menu Code
    public void OpenOptions()
    {
        m_mainMenu.SetActive(false);
        m_optionsMenu.SetActive(true);
    }

    public void ChangeVolume(float volume)
    {
        SoundManager.m_instance.ChangeMasterVolume(volume);
    }

    // Save File Menu Code
    public void OpenSaveFileMenu()
    {
        m_saveAutoButton.SetActive(!PersistentPrefs.GetInstance().HasSaveFile(0));
        m_save1Button.SetActive(!PersistentPrefs.GetInstance().HasSaveFile(1));
        m_save2Button.SetActive(!PersistentPrefs.GetInstance().HasSaveFile(2));
        m_save3Button.SetActive(!PersistentPrefs.GetInstance().HasSaveFile(3));

        m_mainMenu.SetActive(false);
        m_savesMenu.SetActive(true);
    }

    public void TryLoadSave(int save)
    {
        if (PersistentPrefs.GetInstance().HasSaveFile(save))
        {
            PersistentPrefs.GetInstance().LoadSaveFile(save);
            SceneManager.LoadScene(PersistentPrefs.GetInstance().m_currentSaveFile.m_currentScene);
        }
    }

}
