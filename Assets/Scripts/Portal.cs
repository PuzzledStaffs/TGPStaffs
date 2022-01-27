using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] string SceneToGoTo;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneToGoTo);
        }
    }
}
