using UnityEngine;
using UnityEngine.UI;

public class S_FinalDialogueTrigger : MonoBehaviour
{
    [SerializeField] private Canvas m_dialogueUI;
    [SerializeField] private GameObject m_namiaus;
    [SerializeField] private Canvas m_winScreen;
    [SerializeField] private Button m_button;
    private S_DialogueUI m_script;
    private bool hasBeenDestroyed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_dialogueUI.enabled = false;
        m_script = m_dialogueUI.GetComponent<S_DialogueUI>();
        m_namiaus.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

  

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_dialogueUI.enabled = true;
            m_namiaus.SetActive(true);
        }
    }
}
