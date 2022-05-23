using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class ExitDoor : DungenDoor
{
    [FormerlySerializedAs("ExitScene")]
    [SerializeField] private string m_exitScene;
    public Animator m_transition;
    public float m_transitionTime = 1f;
    public string m_textForNextScene;


    private void Start()
    {
        
    }

    protected override IEnumerator MoveRoomCoroutine(Collider other)
    {
        m_transition.SetTrigger("Start");

        yield return new WaitForSeconds(m_transitionTime);

        SceneManager.LoadScene(m_exitScene);
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

    public override void AltInteract()
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
            foreach (GameObject bar in m_bars)
            {
                bar.active = false;
            }
            m_doorCollider.isTrigger = true;

        }
        else if (m_locked)
        {
            return;
        }
    }
}
