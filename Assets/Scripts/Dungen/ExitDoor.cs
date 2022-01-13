using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : DungenDoor
{
    [SerializeField] private string ExitScene;

    protected override IEnumerator MoveRoomCoroutine(Collider other)
    {
        
        SceneManager.LoadScene(ExitScene);
        yield return new WaitForEndOfFrame();
    }
}
