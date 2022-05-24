using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EndScreenControler : MonoBehaviour
{
    [SerializeField] private Animator m_scroll;
    [SerializeField] private AudioSource m_lightning;
    [SerializeField]
    private TextMeshProUGUI m_text;

    [SerializeField][TextArea] private string m_digoryMessage;
    [SerializeField] private Color m_digoryMessageColor;
    [SerializeField] [TextArea] private string m_credits;
    [SerializeField] private Color m_creditsColor;
    private bool m_creditsShowing;

    [SerializeField] private string m_nextScene;

    // Start is called before the first frame update
    void Start()
    {
        m_creditsShowing = false;
        m_text.text = m_digoryMessage;
        m_text.color = m_digoryMessageColor;
        m_scroll.SetTrigger("Scroll");
        m_lightning.Play();
    }

    private void CreditsDone()
    {
        if (m_creditsShowing)
        {
            SceneManager.LoadScene(m_nextScene, LoadSceneMode.Single);
        }
    }

    private void ScrollEnd()
    {
        m_creditsShowing = true;
        m_text.text = m_credits;
        m_text.color = m_creditsColor;
        m_scroll.SetTrigger("Scroll");


    }
}
