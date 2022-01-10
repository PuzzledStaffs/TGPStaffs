using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components"), SerializeField]
    public Rigidbody m_rigidbody;

    [Header("Movement")]
    [SerializeField, ReadOnly]
    Vector2 m_moveDir = new Vector2();

    public float m_speed = 5.0f;

    void Start()
    {

    }

    void Update()
    {
        // Gravity is handled by Rigidbody
        m_rigidbody.velocity = new Vector3(m_moveDir.x * m_speed, m_rigidbody.velocity.y, m_moveDir.y * m_speed);
    }

    /// <summary>
    /// Unity Input Action callback for movement
    /// </summary>
    /// <param name="ctx">Callback Context (Vector2 with values between -1 & 1)</param>
    public void OnMove(InputAction.CallbackContext ctx)
    {
        m_moveDir = ctx.ReadValue<Vector2>();
    }
}
