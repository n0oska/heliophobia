using UnityEngine;

public class S_TriggerCam : MonoBehaviour
{
    [SerializeField] GameObject m_chara;
    [SerializeField] Collider m_trigger;
    public int m_waveNumber;
    public int m_ennemyNumber;
    public bool hasEnteredTriggerCam = false;
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

        if (!hasEnteredTriggerCam)
        {
            spawner.enabled = false;
        }

        if (hasEnteredTriggerCam)
        {
            spawner.enabled = true;
            charaCon.hasClearedAllWaves = false;
            m_trigger.enabled = false;            
        }

        if (spawner.hasClearedAllWaves)
        {
            if (!this.gameObject.CompareTag("TestTrigger"))
            {
                Debug.Log("testTrigger");
                return;
            }
                
            else
                Destroy(this.gameObject);

            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("ziz");
            hasEnteredTriggerCam = true;
        }
    }
}
    
