using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EwanSceneController : MonoBehaviour
{
    [SerializeField] [FormerlySerializedAs("playerController")] PlayerController m_playerController;


    private void Start()
    {
        m_playerController.m_Death += Respawn;


    }

    void Respawn()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("EwanScene");
    }
}
