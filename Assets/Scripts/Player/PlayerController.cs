using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IHealth
{
    [Header("Components"), SerializeField]
    private Rigidbody m_rigidbody;
    public GameObject m_model;

    [Header("Health and Death")]
    public Vector3 m_respawnPosition;
    int m_health = 100;

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

    public bool m_buttonHeld = false;

    [Header("Alt Interact")]
    public bool m_altButtonHeld = false;
    public Box m_grabbedBox;
    public Vector3 m_boxLerpEnd;

    [Header("Weapon Models")]
    public GameObject Sword;

    [Header("Animations")]
    public Animator animator;



    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
#if !UNITY_EDITOR
        Cursor.visible = false;
#endif
        m_respawnPosition = transform.position;
    }

    void Update()
    {
        if (m_weaponWheelController.isWheelOpen)
        {
            Vector2 direction = new Vector2(m_pointerPos.x - Screen.width / 2, m_pointerPos.y - Screen.height / 2);
            if (direction.x != 0 && direction.y != 0)
            {
                float angle = Mathf.Atan2(direction.y, direction.x);
                angle += Mathf.PI;
                m_weaponWheelController.Pulse(angle);
            }
        }
    }

    void FixedUpdate()
    {
        if (!m_movementFrozen)
        {
            if (m_grabbedBox != null)
            {
                if (m_grabbedBox.m_moving)
                {
                    //m_boxLerpTime += Time.deltaTime * 5.0f;
                    //transform.position = Vector3.Lerp(m_boxLerpStart, m_boxLerpEnd, Mathf.Clamp(m_boxLerpTime, 0.0f, 1.0f));
                    //m_boxLerpEnd.y = transform.position.y;
                    //transform.position = Vector3.MoveTowards(transform.position, m_boxLerpEnd, Time.deltaTime * 2.0f);
                }
                else
                {
                    Vector2 mov = m_moveDir;
                    if (mov.x != 0.0f || mov.y != 0.0f)
                    {
                        if (Mathf.Abs(mov.x) > Mathf.Abs(mov.y))
                        {
                            mov.x = mov.x >= 0 ? 1 : -1;
                            mov.y = 0;
                        }
                        else
                        {
                            mov.x = 0;
                            mov.y = mov.y >= 0 ? 1 : -1;
                        }
                        /*float temp = mov.y;
                        mov.y = mov.x;
                        mov.x = -temp;*/

                        Debug.Log(mov);
                        Vector3 mov3 = new Vector3(mov.x, 0.0f, mov.y);
                        if (m_grabbedBox.GetTile(mov3))
                        {
                            m_grabbedBox.Move(mov3);

                            /*Vector3 direction = m_grabbedBox.transform.position - transform.position;
                            direction.Normalize();

                            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                            {
                                direction.x = direction.x >= 0 ? 1 : -1;
                                direction.y = 0;
                            }
                            else
                            {
                                direction.x = 0;
                                direction.y = direction.y >= 0 ? 1 : -1;
                            }

                            Debug.Log("");
                            Debug.Log(mov);
                            Debug.Log(direction);

                            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
                                m_boxLerpEnd = new Vector3(direction.x >= 0 ? -1 : 1, 0.0f, 0.0f);
                            else
                                m_boxLerpEnd = new Vector3(0.0f, 0.0f, direction.z >= 0 ? -1 : 1);
                            */
                            //m_boxLerpEnd = m_grabbedBox.transform.localPosition + new Vector3(mov.x, 0.0f, mov.y);
                        }
                    }
                }
            }
            else
            {
                // Gravity is handled by Rigidbody
                m_rigidbody.velocity = new Vector3(m_moveDir.x * m_speed, m_rigidbody.velocity.y, m_moveDir.y * m_speed);

                // Rotates model to face the direction of movement
                if (m_moveDir.x != 0 || m_moveDir.y != 0)
                    m_model.transform.rotation = Quaternion.LookRotation(new Vector3(m_moveDir.x, 0.0f, m_moveDir.y), Vector3.up);

                float currentSpeed = m_rigidbody.velocity.magnitude / 10;
                animator.SetFloat("Speed", currentSpeed);
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
                // Prevents the player from using items whilst interacting
                if (m_altButtonHeld)
                    break;

                if (m_weaponWheelController.isWheelOpen)
                {
                    m_weaponWheelController.SelectItem(m_weaponWheelController.currentIndex);
                    break;
                }

                if (!m_weaponWheelController.CurrentItem.ItemHold)
                    m_weaponWheelController.LeftClickAction();
                else
                    m_buttonHeld = true;
                break;
            // Button is being held
            case InputActionPhase.Performed:
                break;
            // Button was released
            case InputActionPhase.Canceled:
                if (m_buttonHeld)
                {
                    m_buttonHeld = false;
                    m_weaponWheelController.HoldActionCooldown();
                }
                break;
            case InputActionPhase.Disabled:
            case InputActionPhase.Waiting:
            default:
                break;
        }

    }

    /// <summary>
    /// Unity Input Action callback for alternate use button
    /// </summary>
    public void OnAltInteract(InputAction.CallbackContext ctx)
    {
        // Check the phase of the button press. Equivalent to if ctx.started else if ctx.performed else if ctx.canceled
        switch (ctx.phase)
        {
            // Button was pressed
            case InputActionPhase.Started:
                // Prevents the player from interacting when using or changing items
                if (m_buttonHeld || m_weaponWheelController.isWheelOpen)
                    break;

                foreach (Collider col in Physics.OverlapBox(transform.position + m_model.transform.forward, new Vector3(1.0f, 1.0f, 1.0f) / 2, m_model.transform.rotation))
                {
                    if (col.CompareTag("Box"))
                    {
                        m_grabbedBox = col.GetComponent<Box>();
                        Vector3 direction = m_grabbedBox.transform.position - transform.position;
                        direction.Normalize();
                        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
                            m_boxLerpEnd = m_grabbedBox.transform.position + new Vector3(direction.x >= 0 ? -1 : 1, 0.0f, 0.0f);
                        else
                            m_boxLerpEnd = m_grabbedBox.transform.position + new Vector3(0.0f, 0.0f, direction.z >= 0 ? -1 : 1);
                        m_boxLerpEnd.y = transform.position.y;
                        m_altButtonHeld = true;
                        break;
                    }
                }
                break;
            // Button is being held
            case InputActionPhase.Performed:
                break;
            // Button was released
            case InputActionPhase.Canceled:
                if (m_altButtonHeld)
                {
                    m_altButtonHeld = false;
                    m_grabbedBox = null;
                }
                break;
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
                // Prevents the player from opening the weapon wheel whilst interacting or using an item
                if (m_buttonHeld || m_altButtonHeld)
                    break;

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

    public int GetHealth()
    {
        int health = m_health;

        return health;
    }

    public void TakeDamage(int damage)
    {
        m_health -= damage;

        if (isDead())
        {
            StartCoroutine(DeathCoroutine());
        }

    }

    public bool isDead()
    {
        if (m_health <= 0)
        {
            return true;
        }
        else
        {
            return false;

        }
    }

    IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(1);
        Restart();
    }

    public void Restart()
    {
        Debug.Log("Player dead");
        //TODO: Change this to appropriate scene or add other code
        // Play death animation
        // Reload Scene
        SceneManager.LoadScene("IzzyScene");
    }
    #endregion
}
