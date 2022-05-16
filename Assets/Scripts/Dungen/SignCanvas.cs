using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SignCanvas : MonoBehaviour
{
    [SerializeField] public Canvas m_canvas;

    public TextMeshProUGUI m_textBody;

    public TextMeshProUGUI m_nameText;
    // Start is called before the first frame update
    void Start()
    {
        m_canvas.enabled = false;
        m_textBody.text = "";
        m_nameText.text = "";
    }


}
