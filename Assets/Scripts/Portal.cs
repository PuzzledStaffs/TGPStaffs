using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    public Animator m_transition;
    public float m_transitionTime = 1f;
    public string m_textForNextScene;
    [SerializeField] [FormerlySerializedAs("SceneToGoTo")]string m_destinationScene;


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            LoadNextLevel();
            Text name = GameObject.Find("LevelLoader/RotatingMove/Image/Loading").GetComponent<Text>();
            name.text = m_textForNextScene;
            //SceneManager.LoadScene("DungeonBase");
            //SceneManager.LoadSceneAsync(m_destinationScene,LoadSceneMode.Additive);
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevelCoroutine("DungeonBase"));
    }

    IEnumerator LoadLevelCoroutine(string currentLevel)
    {
        if(m_transition != null)
        {
            m_transition.SetTrigger("Start");
        }

        yield return new WaitForSeconds(m_transitionTime);

        SceneManager.LoadScene(currentLevel);
        SceneManager.LoadSceneAsync(m_destinationScene, LoadSceneMode.Additive);
    }
}
