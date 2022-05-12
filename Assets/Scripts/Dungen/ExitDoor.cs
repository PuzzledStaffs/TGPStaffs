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

    protected override void CheckDoorSet()
    {
        if (m_locked)
        {
            ShowLocks();
        }
        else
        {
            HideLocks();
        }

        if (m_closedOnStart || m_locked)
        {
            CloseDoor();

        }       
        else
        {
            OpenDoor();
        }
    }

    public override void Interact()
    {
        if (m_dungenManager.UseKey())
        {
            m_locked = false;
            HideLocks();
            OpenDoor();
        }
    }

    public override void OpenDoor()
    {
        if (!m_locked)
        {
            m_doorActive = true;
            m_doorRenderer.material = m_doorOpen;
            m_doorCollider.isTrigger = true;

        }
        else if (m_locked)
        {
            return;
        }
    }
}
