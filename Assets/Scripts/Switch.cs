using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour, IInteractable
{
    [Header("Prefab Variables")]
    public Material m_switchOffMaterial;
    public Material m_switchOnMaterial;
    public GameObject m_model;

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

    public void Interact()
    {
        ToggleSwitch();
        // Play sound effect
        // Play particle effect
        // Not in toggle switch so we can silently change states if we need to reset
    }
}
