using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;
using TMPro;

public class S_DialogueUI : MonoBehaviour
{
    [SerializeField] private Canvas m_canvas;
    [SerializeField] private Image m_image;
    [SerializeField] private TextMeshProUGUI m_text;

    private bool hasDialogueEnded = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_canvas.enabled = true;
        m_image.enabled = true;
        m_text.enabled = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (hasDialogueEnded && Input.GetKey(KeyCode.Mouse0))
        {
            m_canvas.enabled = false;
        }
    }
}
