using UnityEngine;

public class S_EnemyController : MonoBehaviour
{
<<<<<<< Updated upstream
    public HealthManager m_health = new HealthManager(); 
    private S_CharaController m_charaController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
=======
    [SerializeField] private HealthManager m_healthManager; // Health de l'ennemi

    [Header("Enemy Attack Settings")]
    [SerializeField] private int mDamageToPlayer = 1;
    [SerializeField] private float mAttackCooldown = 0.8f;

    private float mAttackTimer;

>>>>>>> Stashed changes
    void Start()
    {
        m_healthManager.Init();
        mAttackTimer = 0f;
    }

    void Update()
    {
        if (m_healthManager.isDead())
        {
            Destroy(gameObject);
        }

<<<<<<< Updated upstream

=======
        mAttackTimer -= Time.deltaTime;
>>>>>>> Stashed changes
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && mAttackTimer <= 0f)
        {
            S_CharaController player = other.GetComponent<S_CharaController>();

            if (player != null && player.GetHealthManager() != null)
            {
                player.GetHealthManager().TakeDamage(mDamageToPlayer);
                mAttackTimer = mAttackCooldown;
            }
        }
    }

    // ✅ Getter pour HealthManager
    public HealthManager GetHealthManager() => m_healthManager;
}
