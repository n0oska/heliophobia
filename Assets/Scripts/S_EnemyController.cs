using UnityEngine;

public class S_EnemyController : MonoBehaviour
{
    public HealthManager m_health = new HealthManager(); 
    [Header("Dégâts infligés au joueur")]
    public int m_damage = 1;

    [Header("Layer Mask pour détecter le joueur")]
    public LayerMask m_playerLayer;

    void Start()
    {
        m_health.Init();
    }

    void Update()
    {
        if (m_health.isDead())
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, 0.5f, m_playerLayer);
        if (playerCollider != null)
        {
            S_CharaController player = playerCollider.GetComponent<S_CharaController>();
            if (player != null)
            {
                player.O_TakeDamage(m_damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
