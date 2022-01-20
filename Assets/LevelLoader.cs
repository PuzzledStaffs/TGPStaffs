using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1f;


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
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
