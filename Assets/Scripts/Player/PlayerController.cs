using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 m_moveDir = new Vector2();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(m_moveDir.x, 0.0f, m_moveDir.y);
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
