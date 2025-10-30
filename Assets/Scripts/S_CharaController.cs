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

    private bool m_isBuffActive = false;
    private float m_buffTimer = 0f;

    void Start()
    {
        m_rb = gameObject.GetComponent<Rigidbody>();
        UpdateCoinUI();
        UpdateDamage();
    }

    void Update()
    {
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
