using UnityEngine;

public class S_TriggerCam : MonoBehaviour
{
    [SerializeField] GameObject m_chara;
    [SerializeField] Collider m_trigger;
    public int m_waveNumber;
    public int m_ennemyNumber;
    void Start()
    {

    }

    void Update()
    {
        var charaCon = m_chara.GetComponent<S_CharaController>();
        m_trigger = m_trigger.GetComponent<BoxCollider>();

        if (m_trigger == null)
            Debug.Log("pas trigger");

        if (charaCon.hasEnteredTriggerCam)
        {
            m_trigger.enabled = false; 
        }

        if (charaCon.hasClearedAllWaves)
        {
            Destroy(this.gameObject);
        }
    }
}
    
