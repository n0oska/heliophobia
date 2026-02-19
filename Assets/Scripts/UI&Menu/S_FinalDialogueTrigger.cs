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
        if (!hasBeenDestroyed)
        {
            if (m_script == null)
            {
                Debug.Log("ziz");
                return;
            } 
        
            if (m_script.hasDialogueEnded)
            {
                hasBeenDestroyed = true;
                m_dialogueUI.enabled = false;
                Debug.Log("canvas disabled");
            }
        }
            

        if (hasBeenDestroyed && m_dialogueUI.enabled == false)
        {
            m_dialogueUI.enabled = false;
            m_winScreen.enabled = true;
            Debug.Log("carré");
            m_button.Select();
            Cursor.visible = true;
            var trigger = this.GetComponent<BoxCollider>();
            Destroy(trigger);
        }
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
