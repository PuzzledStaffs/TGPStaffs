using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] string SceneToGoTo;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            SceneManager.LoadSceneAsync("DungeonBase");
            SceneManager.LoadSceneAsync(SceneToGoTo,LoadSceneMode.Additive);
        }
    }
}
