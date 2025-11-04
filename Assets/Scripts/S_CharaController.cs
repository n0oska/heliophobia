using UnityEngine;
using TMPro;

public class S_CharaController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Rigidbody m_rb;
    [SerializeField] private float m_speed;
    [SerializeField] private float m_damage;
    [SerializeField] private float m_baseDamage;
    [SerializeField] private SpriteRenderer m_sR;
    [SerializeField] private HealthManager m_healthManager;

    [Header("Shadow Manager")]
    [SerializeField] private Light m_light;
    [SerializeField] private bool isInShadow;
    [SerializeField] private float m_rayLength;
    [SerializeField] private LayerMask m_obstacle;
    [SerializeField] private Vector3 m_offset;

    private Vector3 m_currentDirection;

    [Header("Coins System")]
    [SerializeField] private TextMeshProUGUI m_coinText;
    private int m_coinCount = 0;

    [Header("Buff Collectibles")]
    [SerializeField] private int m_coinsRequiredForBuff = 5;
    [SerializeField] private float m_buffDuration = 10f;
    [SerializeField] private float m_damageMultiplierDuringBuff = 2f;

    [Header("Combat system")]
    [SerializeField] private Transform m_attackOrigin;
    [SerializeField] private float m_attackRadius = 1f;
    [SerializeField] private float m_cooldownTime = 0.5f;
    [SerializeField] private float m_cooldownTimer = 0;
    [SerializeField] private LayerMask m_enemyMask;
    [SerializeField] private int m_attackDmg = 1;

    private bool m_isBuffActive = false;
    public bool hasHit = false;
    private float m_buffTimer = 0f;

    void Start()
    {
        m_rb = gameObject.GetComponent<Rigidbody>();
        UpdateCoinUI();
        UpdateDamage();
        
    }

    void Update()
    {
        CheckDamage();
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(x, 0, y);
        m_rb.linearVelocity = dir * m_speed;

        if (x != 0 && x < 0)
            m_sR.flipX = true;
        else if (x != 0 && x > 0)
            m_sR.flipX = false;

        if (m_currentDirection != Vector3.zero)
            OnStartMoving();

        if (m_light != null)
            CheckLight();

        if (m_isBuffActive)
        {
            m_buffTimer -= Time.deltaTime;
            if (m_buffTimer <= 0f)
                DeactivateBuff();
        }
    }    

    private void OnStartMoving()
    {
        // display animations
    }

    private void CheckDamage()
    {
        if (m_cooldownTimer <= 0)
        {
            if (Input.GetKey(KeyCode.Joystick1Button15))
            {
                Collider[] ennemiesInRange = Physics.OverlapSphere(m_attackOrigin.position, m_attackRadius, m_enemyMask);

                foreach (var enemy in ennemiesInRange)
                {
                    var enemyCtrl = enemy.GetComponent<S_EnemyController>();
                    if (enemyCtrl != null)
                    {
                        enemyCtrl.m_health.TakeDamage(m_attackDmg);
                        hasHit = true;
                    }
                }

                m_cooldownTimer = m_cooldownTime;
                hasHit = false;
            }
        }

        else
        {
            m_cooldownTimer -= Time.deltaTime;
        }
    }

    private void CheckLight()
    {
        Vector3 dirLight = -m_light.transform.forward;

        if (Physics.Raycast(transform.position + m_offset, dirLight, out RaycastHit hit, m_rayLength, m_obstacle))
        {
            if (hit.collider != null && !isInShadow)
                OnEnterShadow();
        }
        else
        {
            if (isInShadow)
                OnExitShadow();
        }
    }

    private void OnEnterShadow()
    {
        isInShadow = true;
        ShadowForce();
    }

    private void OnExitShadow()
    {
        isInShadow = false;
        NormalForce();
    }

    private void ShadowForce()
    {
        UpdateDamage();
        Debug.Log("ombre → damage boosted");
    }

    private void NormalForce()
    {
        UpdateDamage();
        Debug.Log("plus ombre → damage normal");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(m_attackOrigin.position, m_attackRadius);

        if (!m_light) return;

        Gizmos.color = isInShadow ? Color.red : Color.green;
        Gizmos.DrawLine(m_rb.transform.position + m_offset,
                        m_rb.transform.position + (-m_light.transform.forward) * m_rayLength);        
    }

    // ✅ Buff > Ombre > Normal (PAS DE STACK)
    private void UpdateDamage()
    {
        if (m_isBuffActive)
        {
            m_damage = m_baseDamage * m_damageMultiplierDuringBuff;
            return;
        }

        if (isInShadow)
        {
            m_damage = m_baseDamage * 2f;
            return;
        }

        m_damage = m_baseDamage;
    }

    private void ActivateBuff()
    {
        m_isBuffActive = true;
        m_buffTimer = m_buffDuration;
        UpdateDamage();

        Debug.Log($"BUFF ACTIVATED! Damage x{m_damageMultiplierDuringBuff} for {m_buffDuration} seconds");

        m_coinCount = 0;
        UpdateCoinUI();
    }

    private void DeactivateBuff()
    {
        m_isBuffActive = false;
        UpdateDamage();
        Debug.Log("Buff ended → Damage back to normal");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            m_coinCount++;

            if (m_coinCount >= m_coinsRequiredForBuff && !m_isBuffActive)
                ActivateBuff();

            UpdateCoinUI();
            Destroy(other.gameObject);
        }
    }

    private void UpdateCoinUI()
    {
        if (m_coinText != null)
            m_coinText.text = m_coinCount.ToString();
    }

    public int GetCoinCount()
    {
        return m_coinCount;
    }
}

[System.Serializable]
public class HealthManager
{
    public float m_value;
    public float m_maxValue = 10;

    public void Init()
    {
        m_value = m_maxValue;
    }
    public void TakeDamage(float damage)
    {
        m_value = Mathf.Max(0, m_value - damage);
    }

    public bool isDead() => m_value <= 0;

}

