using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour, IHealth
{
    [Header("Components")]
    public GameObject m_model;
    public Rigidbody m_rigidbody;
    private PlayerInput m_playerInput;

    [Header("Health and Death")]
    public Vector3 m_respawnPosition;
    public Action m_Death;
    public Scene currentScene;

    [SerializeField]
    int m_health = 5;

    public TextMeshProUGUI HealthText;

    [Header("Movement")]
    [SerializeField, ReadOnly]
    Vector2 m_moveDir = new Vector2();
    [SerializeField, ReadOnly]
    private bool m_movementFrozen = false;
    public float m_speed = 5.0f;
    public float m_pushBackForce;

    [Header("Weapon Wheel")]
    [SerializeField, ReadOnly]
    Vector2 m_pointerPos;
    public WeaponWheelController m_weaponWheelController;
    public Transform spawnPoint;
    public bool m_buttonHeld = false;

    [Header("Alt Interact")]
    public bool m_altButtonHeld = false;
    public Box m_grabbedBox;
    public Vector3 m_boxLerpDirection;
    public Vector3 m_boxLerpStart;
    public Vector3 m_boxLerpEnd;
    public float m_boxLerpTime;

    [Header("Weapon Models & Stuff")]
    public GameObject Sword, Bow;
    public ParticleSystem SwordTrailParticle, SecondarySwordTrail;

    [Header("SFX")]
    public AudioClip m_damageSound;
    public AudioClip m_deathSound;

    [Header("Animations")]
    public Animator animator;
    public GameObject playerKnockbackUI;

    [Header("Player UI")]
    public TMPro.TextMeshProUGUI m_gameOverText;
    private float m_deathLerpTime = 0.0f;
    public LineRenderer BowLineRenderer;

    [Header("Currency")]
    public int m_coins;

    [Header("Alt Interact")]
    [SerializeField] float m_altInteractArea;
    [SerializeField] Canvas m_altInteractToolTip;
    [SerializeField] private TextMeshProUGUI m_altInteractToolTipText;
    private IAltInteractable m_currentInteract;

    private float m_timeLeftUntilTick = 1.0f;

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_playerInput = GetComponent<PlayerInput>();
        HealthText.text = "x " + m_health.ToString();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        m_respawnPosition = transform.position;

        // Loading Save Data
        SetHealth(PersistentPrefs.GetInstance().m_currentSaveFile.m_currentHealth);
        m_weaponWheelController.m_weaponWheel.transform.GetChild(0).GetComponent<WeaponButtonInfo>().ItemBlocked = !PersistentPrefs.GetInstance().m_currentSaveFile.m_item1Unlocked;
        m_weaponWheelController.m_weaponWheel.transform.GetChild(1).GetComponent<WeaponButtonInfo>().ItemBlocked = !PersistentPrefs.GetInstance().m_currentSaveFile.m_item2Unlocked;
        m_weaponWheelController.m_weaponWheel.transform.GetChild(2).GetComponent<WeaponButtonInfo>().ItemBlocked = !PersistentPrefs.GetInstance().m_currentSaveFile.m_item3Unlocked;
        m_weaponWheelController.m_weaponWheel.transform.GetChild(3).GetComponent<WeaponButtonInfo>().ItemBlocked = !PersistentPrefs.GetInstance().m_currentSaveFile.m_item4Unlocked;
        m_weaponWheelController.m_weaponWheel.transform.GetChild(4).GetComponent<WeaponButtonInfo>().ItemBlocked = !PersistentPrefs.GetInstance().m_currentSaveFile.m_item5Unlocked;
        m_weaponWheelController.m_weaponWheel.transform.GetChild(5).GetComponent<WeaponButtonInfo>().ItemBlocked = !PersistentPrefs.GetInstance().m_currentSaveFile.m_item6Unlocked;
        m_weaponWheelController.m_weaponWheel.transform.GetChild(6).GetComponent<WeaponButtonInfo>().ItemBlocked = !PersistentPrefs.GetInstance().m_currentSaveFile.m_item7Unlocked;
        m_weaponWheelController.m_weaponWheel.transform.GetChild(7).GetComponent<WeaponButtonInfo>().ItemBlocked = !PersistentPrefs.GetInstance().m_currentSaveFile.m_item8Unlocked;
        if (gameObject.scene.name.Equals("DungeonBase"))
        {
            PersistentPrefs.GetInstance().m_currentSaveFile.m_currentScene = SceneManager.GetSceneAt(1).name;
        }
        else
            PersistentPrefs.GetInstance().m_currentSaveFile.m_currentScene = gameObject.scene.name;
        PersistentPrefs.GetInstance().m_currentSaveFile.m_isInDungeon = gameObject.scene.name.Equals("DungeonBase");

        if (PersistentPrefs.GetInstance().m_currentSaveFile.m_saveLoaded)
        {
            PersistentPrefs.GetInstance().m_currentSaveFile.m_saveLoaded = false;
            transform.position = new Vector3(
                PersistentPrefs.GetInstance().m_currentSaveFile.m_savePositionX,
                PersistentPrefs.GetInstance().m_currentSaveFile.m_savePositionY,
                PersistentPrefs.GetInstance().m_currentSaveFile.m_savePositionZ);
        }
        else
        {
            PersistentPrefs.GetInstance().m_currentSaveFile.m_savePositionX = transform.position.x;
            PersistentPrefs.GetInstance().m_currentSaveFile.m_savePositionY = transform.position.y;
            PersistentPrefs.GetInstance().m_currentSaveFile.m_savePositionZ = transform.position.z;
        }
        // Save to Autosave
        PersistentPrefs.GetInstance().SaveSaveFile(0);

        m_coins = 0; //Do not use PlayerPrefs, ask Kit to add this

        m_altInteractToolTip.enabled = false;
    }

    void OnDestroy()
    {
        PersistentPrefs.GetInstance().m_currentSaveFile.m_currentHealth = IsDead() ? 5 : m_health;
    }

    void Update()
    {
        if (m_timeLeftUntilTick <= 0.0f)
        {
            m_timeLeftUntilTick += 1.0f;
            PersistentPrefs.GetInstance().m_currentSaveFile.AddSecond();
        }
        m_timeLeftUntilTick -= Time.deltaTime;

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            animator.SetBool("Attack1", false);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            animator.SetBool("Attack2", false);
            SwordItem.m_noOfClicks = 0;
            Sword.SetActive(false);
        }

        if (Time.time - SwordItem.m_lastClickedTime > SwordItem.m_maxComboDelay)
        {
            SwordItem.m_noOfClicks = 0;
            Sword.SetActive(false);
        }

        if (m_weaponWheelController.isWheelOpen)
        {
            Vector2 direction = m_pointerPos;
            switch (m_playerInput.currentControlScheme.ToString().ToLower())
            {
                case "controller":
                    break;
                default:
                    direction = new Vector2(m_pointerPos.x - Screen.width / 2, m_pointerPos.y - Screen.height / 2);
                    break;
            }
            if (direction.x != 0 && direction.y != 0)
            {
                float angle = Mathf.Atan2(direction.y, direction.x);
                angle += Mathf.PI;
                m_weaponWheelController.Pulse(angle);
            }
        }

        if (IsDead())
        {
            if (!m_gameOverText.gameObject.activeSelf)
                m_gameOverText.gameObject.SetActive(true);
            if (m_deathLerpTime < 1.0f)
            {
                Color c = m_gameOverText.color;
                c.a = m_deathLerpTime;
                m_gameOverText.color = c;

                m_deathLerpTime += Time.deltaTime / 2.0f;
            }
            else
            {
                if (m_gameOverText.color.a < 1.0f)
                {
                    Color c = m_gameOverText.color;
                    c.a = 1.0f;
                    m_gameOverText.color = c;
                }
            }
        }


        CheckForInterpretablesInRange();

    }

    void CheckForInterpretablesInRange()
    {
        //Check for interactibles
        List<IAltInteractable> interactInRange = new List<IAltInteractable>();
        foreach (Collider coll in Physics.OverlapSphere(transform.position, m_altInteractArea))
        {
            IAltInteractable interact = coll.GetComponent<IAltInteractable>();
            if (interact != null)
            {
                if (interact.CanInteract().canInteract)
                {
                    interactInRange.Add(interact);
                }
            }
        }
        //If there are none display nothing on screen
        if (interactInRange.Count == 0)
        {
            m_altInteractToolTip.enabled = false;
            m_currentInteract = null;
            return;
        }

        //Find the one with the highest priority
        IAltInteractable topPriority = null;
        foreach (var interactable in interactInRange)
        {
            if (topPriority == null)
            {
                topPriority = interactable;
            }
            else if (interactable.CanInteract().priority > topPriority.CanInteract().priority)
            {
                topPriority = interactable;
            }
        }
        //If a new one with hinger priority found use that
        if (m_currentInteract != topPriority)
        {
            m_currentInteract = topPriority;

            m_altInteractToolTip.enabled = true;
            m_altInteractToolTipText.text = m_currentInteract.CanInteract().text;
        }



    }

    void FixedUpdate()
    {
        if (!m_movementFrozen)
        {
            float width = BowLineRenderer.startWidth;
            BowLineRenderer.material.mainTextureScale = new Vector2(1f / width, 1.0f);

            if (m_grabbedBox != null)
            {
                if (m_grabbedBox.m_moving)
                {
                    if (m_boxLerpTime < 1.0f)
                    {
                        m_boxLerpEnd.y = transform.position.y;
                        transform.position = Vector3.Lerp(m_boxLerpStart, m_boxLerpEnd, m_boxLerpTime);
                        m_boxLerpTime += Time.deltaTime * 2.0f;
                    }
                    else
                    {
                        transform.position = m_boxLerpEnd;
                    }
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

                        //Swap mov around as a tile's local x is actually the grid y pos and local z grid x pos
                        mov = new Vector2(mov.y, mov.x);
                        Vector3 mov3 = new Vector3(mov.x, 0.0f, mov.y);
                        if (m_grabbedBox.IsValidTile(mov3))
                        {
                            bool valid = true;
                            foreach (Box box in m_grabbedBox.transform.parent.GetComponentsInChildren<Box>())
                                if (box.Occupies(m_grabbedBox.GetTileX(mov3), m_grabbedBox.GetTileY(mov3)))
                                {
                                    valid = false;
                                    break;
                                }

                            if (valid &&
                                (m_boxLerpDirection.x == 0 && m_boxLerpDirection.x == mov3.z ||
                                    m_boxLerpDirection.z == 0 && m_boxLerpDirection.z == mov3.x))
                            {
                                m_grabbedBox.Move(mov3);
                                m_boxLerpStart = transform.position;
                                m_boxLerpEnd = m_grabbedBox.m_boxLerpEnd + m_boxLerpDirection;
                                m_boxLerpEnd.y = transform.position.y;
                                m_boxLerpTime = 0.0f;
                            }
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
        animator.SetFloat("Speed", 0);
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
        if (PauseMenu.m_gamePaused)
            return;

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
                    m_weaponWheelController.SelectItem(m_weaponWheelController.m_currentIndex);
                    break;
                }

                if (!m_weaponWheelController.m_CurrentItem.ItemHold)
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
        if (PauseMenu.m_gamePaused)
            return;

        // Check the phase of the button press. Equivalent to if ctx.started else if ctx.performed else if ctx.canceled
        switch (ctx.phase)
        {
            // Button was pressed
            case InputActionPhase.Started:
                // Prevents the player from interacting when using or changing items
                if (m_buttonHeld || m_weaponWheelController.isWheelOpen)
                    break;

                foreach (Collider col in Physics.OverlapBox(transform.position + m_model.transform.forward, new Vector3(0.5f, 0.5f, 0.5f) / 2, m_model.transform.rotation))
                {
                    if (col.CompareTag("Box"))
                    {
                        m_grabbedBox = col.GetComponent<Box>();
                        m_boxLerpDirection = m_grabbedBox.transform.position - transform.position;
                        m_boxLerpDirection.y = 0.0f;
                        m_boxLerpDirection.Normalize();
                        if (Mathf.Abs(m_boxLerpDirection.x) > Mathf.Abs(m_boxLerpDirection.z))
                        {
                            m_boxLerpDirection.x = m_boxLerpDirection.x >= 0 ? -1 : 1;
                            m_boxLerpDirection.z = 0;
                        }
                        else
                        {
                            m_boxLerpDirection.x = 0;
                            m_boxLerpDirection.z = m_boxLerpDirection.z >= 0 ? -1 : 1;
                        }

                        m_boxLerpEnd = m_grabbedBox.transform.position + m_boxLerpDirection;
                        m_boxLerpEnd.y = transform.position.y;
                        transform.position = m_boxLerpEnd;
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
        if (PauseMenu.m_gamePaused)
            return;

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

    public void OnAltInteract()
    {
        m_currentInteract?.AltInteract();
        m_currentInteract = null;
        CheckForInterpretablesInRange();
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

    public void AddHealth(int Health)
    {
        SetHealth(m_health + Health);
    }

    public void SetHealth(int Health)
    {
        m_health = Health;
        HealthText.text = "x " + m_health.ToString();
    }

    public void TakeDamage(IHealth.Damage damage)
    {
        m_health -= damage.damageAmount;
        GetComponent<AudioSource>().PlayOneShot(m_damageSound);
        HealthText.text = "x " + m_health.ToString();
        if (IsDead())
        {
            StartCoroutine(DeathCoroutine());
        }

    }





    public bool IsDead()
    {
        return m_health <= 0;
    }

    IEnumerator DeathCoroutine()
    {
        m_playerInput.enabled = false;
        animator.SetBool("Dead", true);
        GetComponent<AudioSource>().PlayOneShot(m_deathSound);
        yield return new WaitForSeconds(3.6f);
        m_Death?.Invoke();
        // Do not destroy this object
    }

    public void Restart()
    {
        //Debug.Log("Player dead");



        /* /// Commented out so as not to randomly respawn people to the test scene
         * code works
        //TODO: Change this to appropriate scene or add other code
        // Play death animation
        // Reload Scene
        SceneManager.LoadScene("IzzyScene");
        */
    }
    #endregion


    public void AddCoins(int coins)
    {
        m_coins += coins;
        PlayerPrefs.SetInt("Coins", m_coins);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, m_altInteractArea);
    }

    Collider[] GetAnyInteract()
    {
        return Physics.OverlapBox(transform.position + m_model.transform.forward, new Vector3(0.5f, 0.5f, 0.5f) / 2, m_model.transform.rotation);
    }

    public void PlayerKnockBack(GameObject enemy)
    {
        Debug.Log("Knockback");
        m_rigidbody.AddForce(enemy.transform.forward * m_pushBackForce);
    }
}
