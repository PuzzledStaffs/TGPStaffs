using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour, IAltInteractable
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

    [Header("LockedSwitch")]
    [SerializeField] private bool m_locked;

    [SerializeField] private List<GameObject> m_locks;
    private DungenManager m_dungenManager;

    void Start()
    {
        m_modelRenderer = m_model.GetComponent<Renderer>();
        m_dungenManager = GameObject.FindObjectOfType<DungenManager>();
        ToggleLocks(m_locked);
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

    public void AltInteract()
    {
        if (m_locked)
        {
            if (m_dungenManager.UseKey())
            {
                m_locked = false;
                ToggleLocks(false);
            }
            else
            {
                return;
            }
        }
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

    void ToggleLocks(bool show)
    {
        foreach (GameObject lockObject in m_locks)
        {
            lockObject.SetActive(show);
        }
    }

    public InteractInfo CanInteract()
    {
        if (m_locked)
        {
            return new InteractInfo(true, "Unlock Switch", 2);
        }
        else
        {
            return new InteractInfo(true, "Use Switch", 2);
        }
    }
}
