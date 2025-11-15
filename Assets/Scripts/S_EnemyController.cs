using UnityEngine;

public class S_EnemyController : MonoBehaviour
{
    public HealthManager m_health = new HealthManager(); 
    [Header("D�g�ts inflig�s au joueur")]
    public int m_damage = 1;

    [Header("Layer Mask pour d�tecter le joueur")]
    [SerializeField] LayerMask m_playerLayer;
    [SerializeField] private float m_castRadius = 1;
    [SerializeField] private float m_speed = 1;
    [SerializeField] private float m_stopDistance = 1.5f;
    [SerializeField] Vector3 m_currentPos;
    [SerializeField] Vector3 m_castOffset;
    [SerializeField] GameObject player;
    [SerializeField] Rigidbody m_rb;

    void Start()
    {
        m_health.Init();
        m_currentPos = this.gameObject.transform.position;
        
    }

    void Update()
    {
        GetMovement();

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

    private void GetMovement()
    {
        if (m_rb == null)
            Debug.Log("Pas de rigidbody");

        float distance = Vector3.Distance(m_rb.position, player.transform.position);

        if (m_rb != null && player != null) 
        {
            if (distance > m_stopDistance)
            {
                Vector3 direction = (player.transform.position - m_rb.position).normalized;
                direction.y = 0;

                transform.position += direction * m_speed * Time.deltaTime;
            }
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
