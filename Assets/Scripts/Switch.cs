using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour, IInteractable
{
    [Header("Prefab Variables")]
    public Material m_switchOffMaterial;
    public Material m_switchOnMaterial;
    public GameObject m_model;

    [Header("ButtonMode")]
    [SerializeField] bool m_ButtonMode;
    [SerializeField] float m_pushedWait;
    private bool m_ButtonPushed = false;

    [Header("Components")]
    private Renderer m_modelRenderer;

    [Header("RuntimeVariables")]
    [SerializeField, ReadOnly]
    bool m_switchActive = false;
    public UnityEvent m_switchEnabled, m_switchDisabled;

    void Start()
    {
        m_modelRenderer = m_model.GetComponent<Renderer>();
    }

    public void ToggleSwitch()
    {
        m_switchActive = !m_switchActive;

        if (m_switchActive)
        {
            m_modelRenderer.material = m_switchOnMaterial;
            m_switchEnabled.Invoke();
        } else
        {
            m_modelRenderer.material = m_switchOffMaterial;
            m_switchDisabled.Invoke();
        }
    }

    IEnumerator ButtonCoroutine()
    {
        m_ButtonPushed = true;
        m_modelRenderer.material = m_switchOnMaterial;
        m_switchEnabled.Invoke();
        yield return new WaitForSecondsRealtime(m_pushedWait);
        m_modelRenderer.material = m_switchOffMaterial;
        m_ButtonPushed = false;
    }

    public void Interact()
    {
        if(m_ButtonMode)
        {
            if (!m_ButtonPushed)
                StartCoroutine(ButtonCoroutine());
        }
        else
            ToggleSwitch();
        // Play sound effect
        // Play particle effect
        // Not in toggle switch so we can silently change states if we need to reset
    }
}
