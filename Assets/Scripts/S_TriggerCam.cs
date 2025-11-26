using UnityEngine;

public class S_TriggerCam : MonoBehaviour
{
    [SerializeField] GameObject m_chara;
    [SerializeField] Collider m_trigger;
    public int m_waveNumber;
    public int m_ennemyNumber;
    void Start()
    {
        //var spawners = GameObject.FindGameObjectsWithTag("Spawner");
        var spawners = FindAnyObjectByType<S_EnemySpawner>(FindObjectsInactive.Exclude);
        Debug.Log(spawners);
        spawners.enabled = false;
    }

    void Update()
    {
        var charaCon = m_chara.GetComponent<S_CharaController>();
        m_trigger = m_trigger.GetComponent<BoxCollider>();
        var spawner = this.gameObject.GetComponentInChildren<S_EnemySpawner>();
        
        if (m_trigger == null)
            Debug.Log("pas trigger");

        if (!charaCon.hasEnteredTriggerCam)
        {
            spawner.enabled = false;
        }

        if (charaCon.hasEnteredTriggerCam)
        {
            spawner.enabled = true;
            m_trigger.enabled = false;            
        }

        if (spawner.hasClearedAllWaves)
        {
            Destroy(this.gameObject);
        }
    }
}
    
