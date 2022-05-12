using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Portal : MonoBehaviour
{
    [SerializeField] [FormerlySerializedAs("SceneToGoTo")]string m_destinationScene;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            SceneManager.LoadSceneAsync("DungeonBase");
            SceneManager.LoadSceneAsync(m_destinationScene,LoadSceneMode.Additive);
        }
    }
}
