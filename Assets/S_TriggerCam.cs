using UnityEngine;

public class S_TriggerCam : MonoBehaviour
{
    [SerializeField] GameObject m_chara;
    public int m_waveNumber;
    public int m_ennemyNumber;
    void Start()
    {
    }

    void Update()
    {
        var charaCon = m_chara.GetComponent<S_CharaController>();

        if (charaCon.hasClearedAllWaves)
        {
            Destroy(this.gameObject);
        }
    }
}
    
