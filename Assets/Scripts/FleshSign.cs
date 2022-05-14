using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FleshSign : Sign
{
    private TextMeshProUGUI m_nameText;

    [SerializeField] string m_displyName;
    [SerializeField] private Animator m_animator;

    public override void AltInteract()
    {
        base.AltInteract();
        if (m_reading)
        {
            m_nameText.text = m_displyName;
            m_animator.SetBool("Talking", true);
        }
        else
        {
            m_nameText.text = "";
            m_animator.SetBool("Talking", false);
        }
    }

    protected override void Start()
    {
        base.Start();
        m_nameText = m_signCanvasScript.m_nameText;
    }

    public override InteractInfo CanInteract()
    {
        return new InteractInfo(true, "Talk", 1); ;
    }
}
