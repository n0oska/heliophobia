using UnityEngine;

public class S_EnemyController : MonoBehaviour
{
    public HealthManager m_health = new HealthManager(); 
    [Header("D�g�ts inflig�s au joueur")]
    public int m_damage = 1;

    [Header("Layer Mask pour d�tecter le joueur")]
    [SerializeField] LayerMask m_playerLayer;
    [SerializeField] private float m_castRadius = 1;
    [SerializeField] Vector3 m_currentPos;
    [SerializeField] Vector3 m_castOffset;
    [SerializeField] GameObject player;

    void Start()
    {
        m_health.Init();
        m_currentPos = this.gameObject.transform.position;
        
    }

    void Update()
    {
        if (this.gameObject.transform.position.x != 0 && this.gameObject.transform.position.x > 0)
            m_castOffset = new Vector3(1, 0, 0);
        if (this.gameObject.transform.position.x != 0 && this.gameObject.transform.position.x < 0)
            m_castOffset = new Vector3(-1, 0, 0);

        if (m_health.isDead())
        {
            var charaCon = player.GetComponent<S_CharaController>();
            charaCon.killCount += 1;
            Debug.Log(charaCon.killCount);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Collider[] playerCollider = Physics.OverlapSphere(m_currentPos + m_castOffset, m_castRadius, m_playerLayer);
        // if (playerCollider != null)
        // {
        //     S_CharaController player = playerCollider.GetComponent<S_CharaController>();
        //     if (player != null)
        //     {
        //         player.O_TakeDamage(m_damage);
        //     }
        // }

        foreach (var player in playerCollider)
        {
            var chara = player.GetComponent<S_CharaController>();
            if (chara != null)
            {
                chara.m_playerHealth.TakeDamage(1);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.gameObject.transform.position + m_castOffset, m_castRadius);
    }
}
