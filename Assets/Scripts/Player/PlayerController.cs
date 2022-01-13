using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components"), SerializeField]
    private Rigidbody m_rigidbody;
    public GameObject m_model;

    [Header("Health and Death")]
    public Vector3 m_respawnPosition;

    [Header("Movement")]
    [SerializeField, ReadOnly]
    Vector2 m_moveDir = new Vector2();
    [SerializeField, ReadOnly]
    private bool m_movementFrozen = false;
    public float m_speed = 5.0f;

    [Header("Weapon Wheel")]
    [SerializeField, ReadOnly]
    Vector2 m_pointerPos;
    [SerializeField]
    WeaponWheelController m_weaponWheelController;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        m_respawnPosition = transform.position;
    }

    void Update()
    {
        if (!m_movementFrozen)
        {
            // Gravity is handled by Rigidbody
            m_rigidbody.velocity = new Vector3(m_moveDir.x * m_speed, m_rigidbody.velocity.y, m_moveDir.y * m_speed);

            // Rotates model to face the direction of movement
            if (m_moveDir.x != 0 || m_moveDir.y != 0)
                m_model.transform.rotation = Quaternion.LookRotation(new Vector3(m_moveDir.x, 0.0f, m_moveDir.y), Vector3.up);
        }

        if (m_weaponWheelController.isWheelOpen)
        {
            Vector2 direction = new Vector2(m_pointerPos.x - Screen.width / 2, m_pointerPos.y - Screen.height / 2);
            if (direction.x != 0 && direction.y != 0)
            {
                float angle = Mathf.Atan2(direction.y, direction.x);
                angle += Mathf.PI;
                angle /= 2;
                m_weaponWheelController.Pulse(angle);
            }
        }
    }

    #region Freezing Movement
    public void FreezeMovement()
    {
        m_movementFrozen = true;
        m_rigidbody.velocity = new Vector3(0, m_rigidbody.velocity.y, 0);
    }

    public void UnFreezeMovement()
    {
        m_movementFrozen = false;
    }
    #endregion

    #region Input Action Events
    /// <summary>
    /// Unity Input Action callback for movement
    /// </summary>
    public void OnMove(InputAction.CallbackContext ctx)
    {
        m_moveDir = ctx.ReadValue<Vector2>();
    }

    /// <summary>
    /// Unity Input Action callback for movement
    /// </summary>
    public void OnPointerMove(InputAction.CallbackContext ctx)
    {
        m_pointerPos = ctx.ReadValue<Vector2>();
    }

    /// <summary>
    /// Unity Input Action callback for use button
    /// </summary>
    public void OnUse(InputAction.CallbackContext ctx)
    {
        // Check the phase of the button press. Equivalent to if ctx.started else if ctx.performed else if ctx.canceled
        switch (ctx.phase)
        {
            // Button was pressed
            case InputActionPhase.Started:
                // Gets all objects with a collider in a box (halfExtents = scale / 2) in front of the player
                foreach (Collider col in Physics.OverlapBox(transform.position + m_model.transform.forward, new Vector3(1.0f, 1.0f, 1.0f) / 2, m_model.transform.rotation))
                {
                    if (col.CompareTag("Player"))
                        continue;
                    // If the collider also has a IInteractable script, interact with it
                    col.GetComponent<IInteractable>()?.Interact();
                    col.GetComponent<IHealth>()?.TakeDamage(1);
                }
                m_weaponWheelController.CurrentItem.LeftClickAction();
                break;
            // Button is being held
            case InputActionPhase.Performed:
            // Button was released
            case InputActionPhase.Canceled:
            case InputActionPhase.Disabled:
            case InputActionPhase.Waiting:
            default:
                break;
        }

    }

    /// <summary>
    /// Unity Input Action callback for use button
    /// </summary>
    public void OnToggleWeaponWheel(InputAction.CallbackContext ctx)
    {
        // Check the phase of the button press. Equivalent to if ctx.started else if ctx.performed else if ctx.canceled
        switch (ctx.phase)
        {
            // Button was pressed
            case InputActionPhase.Started:
                m_weaponWheelController.ToggleWheel();
                break;
            // Button is being held
            case InputActionPhase.Performed:
            // Button was released
            case InputActionPhase.Canceled:
            case InputActionPhase.Disabled:
            case InputActionPhase.Waiting:
            default:
                break;
        }

    }
    #endregion

    #region Player Health And Death
    /// <summary>
    /// Teleports the player to their repsawn position.
    /// Not to be confused with death functions.
    /// </summary>
    public void Respawn()
    {
        transform.position = m_respawnPosition;
    }
    #endregion
}
