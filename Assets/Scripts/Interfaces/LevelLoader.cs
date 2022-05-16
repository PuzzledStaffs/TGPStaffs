using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LevelLoader : MonoBehaviour
{
    [FormerlySerializedAs("transition")]
    public Animator m_transition;
    [FormerlySerializedAs("transitionTime")]
    public float m_transitionTime = 1f;


    // Update is called once per frame
    void Update()
    {
      
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
       StartCoroutine(LoadLevelCoroutine(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevelCoroutine(int levelIndex)
    {
        m_transition.SetTrigger("Start");

        yield return new WaitForSeconds(m_transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
