using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EwanSceneController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    private void Start()
    {
        playerController.m_Death += Respawn;


    }

    void Respawn()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("EwanScene");
    }
}
