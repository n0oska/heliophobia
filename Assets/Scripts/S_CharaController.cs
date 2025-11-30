using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class S_CharaController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Rigidbody m_rb;
    [SerializeField] private float m_speed;
    [SerializeField] private float m_damage;
    [SerializeField] private float m_baseDamage;
    [SerializeField] private SpriteRenderer m_sR;
    //[SerializeField] private HealthManager m_healthManager;

    [Header("Dash system")]
    [SerializeField] private float m_dashForce;
    [SerializeField] private float m_dashCooldown;
    [SerializeField] private float m_dashRadius;
    [SerializeField] private float m_dashDamage;
    [SerializeField] private float m_dashDuration = 0.2f;
    [SerializeField] private AnimationCurve m_dashCurve;
    private bool isDashing;
    private bool canDash;
    private Vector3 m_dashDirection;
    private float m_dashTimer;
    private float m_dashCooldownTimer;

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
    [SerializeField] private Vector3 m_attackOffset;
    
    [SerializeField] private float m_attackRadius = 1f;
    [SerializeField] private float m_cooldownTime = 0.5f;
    [SerializeField] private float m_cooldownTimer = 0;
    [SerializeField] private LayerMask m_enemyMask;

    [Header("Health and death system")]
    public HealthManager m_playerHealth = new HealthManager();
    [SerializeField] private GameObject m_startPos;
    public bool hasRespawned = false;

    [Header("Camera")]
    [SerializeField] private CinemachineCamera m_mainCam;
    //[SerializeField] private CinemachineBasicMultiChannelPerlin m_channels;
    [SerializeField] private GameObject m_triggerCam;
    [SerializeField] private GameObject m_lookTriggerCam;
    public int killCount;
    public int waveCount;
    public bool hasEnteredTriggerCam = false;


    [Header("Other")]
    private bool m_isBuffActive = false;
    //private bool hasDashed = false;
    public bool hasDashHit = false;
    public bool hasHit = false;
    private float m_buffTimer = 0f;

    public Animator m_animator;

    [SerializeField] private float m_holdThreshold = 0.5f;
    private float m_buttonPressTime;
    private bool m_isHolding;
    private bool m_isCharging;
    public bool hasClearedAllWaves = false;
    public bool setTrigger = false;
    private S_TriggerCam m_trigger;
    private S_EnemySpawner m_spawner;

    void Start()
    {
        m_rb = gameObject.GetComponent<Rigidbody>();
        m_playerHealth.Init();
        UpdateCoinUI();
        UpdateDamage();
        m_attackOffset = new Vector3(1, 0, 0);
        m_spawner = m_triggerCam.GetComponentInChildren<S_EnemySpawner>();
        m_trigger = m_triggerCam.GetComponent<S_TriggerCam>();
        //m_channels.enabled = false;
        
    }

    void Update()
    {
        CheckDamage();
        CheckDeath();
        CameraControl();
        ReloadScene();
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        m_currentDirection = new Vector3(x, 0, y);
        m_rb.linearVelocity = m_currentDirection * m_speed;

        if (x != 0 && x < 0)
        {
            m_sR.flipX = true;
            m_attackOffset = new Vector3(-1, 0, 0);
        }

        else if (x != 0 && x > 0)
        {
            m_sR.flipX = false;
            m_attackOffset = new Vector3(1, 0, 0);
        }

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

        if (!canDash)
        {
            m_dashCooldownTimer -= Time.deltaTime;
            if (m_dashCooldownTimer <= 0f)
                canDash = true;
        }
        else
            CheckDash();

        CheckAttackInput();


        

        if (m_trigger.hasEnteredTriggerCam)
        {
            CameraTriggerSet();
        }
    }

    private void CameraControl()
    {
        //var m_trigger = FindAnyObjectByType<S_TriggerCam>(FindObjectsInactive.Exclude);

        //if (!m_trigger.hasEnteredTriggerCam)
        //    return;
        if (m_triggerCam != null)
        {
            var spawner = m_triggerCam.GetComponentInChildren<S_EnemySpawner>();
            Debug.Log("zizi");
            Debug.Log(spawner.hasClearedAllWaves);
            if (spawner.hasClearedAllWaves)
            {
            
                var pChara = m_rb.GetComponent<SpriteRenderer>();
                Debug.Log("Cam follow");             
                m_mainCam.Follow = pChara.transform;
             
                hasEnteredTriggerCam = false;
                setTrigger = false;
                Debug.Log(setTrigger);
            }        
        }

        else 
            return;
        
    }

    private void CheckDeath()
    {
        if (m_playerHealth.isDead())
        {
            //death + réinitialiser hp
            StartCoroutine(C_CheckDeath());
        }
    }
    
    private IEnumerator C_CheckDeath()
    {
        m_rb.transform.position = m_startPos.transform.position;

        if (hasRespawned)
        {
            m_playerHealth.Init();
            yield return null;
        }
    }

    private IEnumerator C_Dash()
    {
        isDashing = true;
        canDash = false;
        m_dashDirection = m_currentDirection;

        //float time = 0f;
        m_dashTimer = 0f;

        while (m_dashTimer < m_dashDuration)
        {
            float t = m_dashTimer / m_dashDuration;
            float curveMultiplier = m_dashCurve.Evaluate(t);

            m_rb.linearVelocity = m_dashDirection * (m_dashForce * curveMultiplier);

            m_dashTimer += Time.deltaTime;
            yield return null;
        }

        if (isDashing)
        {
            Collider[] ennemiesInRange = Physics.OverlapSphere(m_rb.position, m_dashRadius, m_enemyMask);

            foreach (var enemy in ennemiesInRange)
            {
                var enemyCtrl = enemy.GetComponent<S_EnemyController>();
                hasDashHit = true;

                if (enemyCtrl != null && hasDashHit)
                {
                    enemyCtrl.m_health.TakeDamage(m_dashDamage);
                    Debug.Log(m_dashDamage);
                }
            }
        }

        isDashing = false;
        m_dashCooldownTimer = m_dashCooldown;
    }

    private void OnStartMoving()
    {
        // display animations
    }

    private void CheckDash()
    {
        if (Input.GetKey(KeyCode.LeftShift) && canDash || Input.GetKey(KeyCode.Joystick1Button1))
        {
            StartCoroutine(C_Dash());
        }

        else
            canDash = true;
    }

    private void CheckDamage()
    {
        if (m_cooldownTimer <= 0)
        {
            if (Input.GetKey(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Joystick1Button2))
            {
                Collider[] ennemiesInRange = Physics.OverlapSphere(m_rb.position + m_attackOffset, m_attackRadius, m_enemyMask);

                m_animator.SetTrigger("Attack");

                foreach (var enemy in ennemiesInRange)
                {
                    var enemyCtrl = enemy.GetComponent<S_EnemyController>();
                    if (enemyCtrl != null)
                    {
                        enemyCtrl.m_health.TakeDamage(m_damage);
                        hasHit = true;
                    }
                    var envirmtCtrl = enemy.GetComponent<S_Environement>();
                    if (envirmtCtrl!=null)
                    {
                        envirmtCtrl.m_health.TakeDamage(m_damage);
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

    private void CheckAttackInput()
    {
        if(m_cooldownTimer > 0)
        {
            m_cooldownTimer -= Time.deltaTime;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button3))
        {
            m_buttonPressTime = Time.time;
            m_isHolding = true;
            m_isCharging = false;
        }

        if(m_isHolding && Input.GetKeyDown(KeyCode.Joystick1Button3))
        {
            float heldTime = Time.time - m_buttonPressTime;

            if (heldTime >= m_holdThreshold)
            {
                m_isCharging = true;
            } 
        }

        if (Input.GetKeyUp(KeyCode.Joystick1Button3))
        {
            m_isHolding = false;
            float totalHoldTime = Time.time - m_buttonPressTime;
            
            if(totalHoldTime >= m_holdThreshold)
            {
                PerformChargedAttack();
            }
            else
            {
                CheckDamage();
            }

            m_isCharging = false;
        }
    }


    private void PerformChargedAttack()
    {
        m_animator.SetTrigger("Attack2");
        Collider[] enemiesInRange = Physics.OverlapSphere(m_rb.position + m_attackOffset, m_attackRadius * 1.5f, m_enemyMask);

        foreach(var enemy in enemiesInRange)
        {
            var enemyCtrl = enemy.GetComponent<S_EnemyController>();
            if(enemyCtrl != null)
            {
                enemyCtrl.m_health.TakeDamage(m_damage * 2f);
            }
        }
    }
// public void O_TakeDamage(float damage)
// {
//     if (m_healthManager != null)
//     {
//         m_healthManager.TakeDamage(damage);
//         Debug.Log($"Le joueur prend {damage} dégâts");
//     }
// }


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

        if (m_rb != null)
        {
            var sprite = m_rb.GetComponent<SpriteRenderer>().color;
            sprite.b = 0.5f;
            sprite.r = 0.5f;
            sprite.g = 0.5f;

            m_rb.GetComponent<SpriteRenderer>().color = sprite;
        }

        ShadowForce();
    }

    private void OnExitShadow()
    {
        isInShadow = false;

        if (m_rb != null)
        {
            var sprite = m_rb.GetComponent<SpriteRenderer>().color;
            sprite.b = 1;
            sprite.r = 1;
            sprite.g = 1;

            m_rb.GetComponent<SpriteRenderer>().color = sprite;
        }
        NormalForce();
    }

    private void ShadowForce()
    {
        UpdateDamage();
        //Debug.Log("ombre → damage boosted");
    }

    private void NormalForce()
    {
        UpdateDamage();
        //Debug.Log("plus ombre → damage normal");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(m_rb.position + m_attackOffset, m_attackRadius);

        if (!m_light) return;

        Gizmos.color = isInShadow ? Color.red : Color.green;
        Gizmos.DrawLine(m_rb.transform.position + m_offset,
                        m_rb.transform.position + (-m_light.transform.forward) * m_rayLength);  

        if (canDash)
        {
            Gizmos.DrawWireSphere(m_rb.position, m_dashRadius);
            Gizmos.color = isDashing ? Color.red : Color.green;
        }      
    }
    public void ForceExitShadow()
    {
        if (isInShadow)
        {
            isInShadow = false;
            UpdateDamage(); // recalcul du damage normal
            //Debug.Log("Anti-ombre active → damage normal");
        }
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

        var sprite = m_rb.GetComponent<SpriteRenderer>().color;
        sprite.b = 0.5f;
        sprite.r = 0.5f;
        sprite.g = 0.5f;

        m_rb.GetComponent<SpriteRenderer>().color = sprite;


        UpdateDamage();

        //Debug.Log($"BUFF ACTIVATED! Damage x{m_damageMultiplierDuringBuff} for {m_buffDuration} seconds");

        m_coinCount = 0;
        UpdateCoinUI();
    }

    private void DeactivateBuff()
    {
        m_isBuffActive = false;
        UpdateDamage();

        var sprite = m_rb.GetComponent<SpriteRenderer>().color;
        sprite.b = 1;
        sprite.r = 1;
        sprite.g = 1;

        m_rb.GetComponent<SpriteRenderer>().color = sprite;
        //Debug.Log("Buff ended → Damage back to normal");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            m_coinCount++;

            if (m_coinCount >= m_coinsRequiredForBuff && !m_isBuffActive)
                ActivateBuff(); //display particle system + buff

            UpdateCoinUI();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("TestCoin"))
        {
            m_coinCount++;

            if (m_coinCount >= m_coinsRequiredForBuff && !m_isBuffActive)
                ActivateBuff(); //display particle system + buff            
        }

        if (other.CompareTag("TriggerCam") || other.CompareTag("TestTrigger"))
        {
            m_triggerCam = other.gameObject;
            m_trigger = m_triggerCam.GetComponent<S_TriggerCam>();
        }

        // if (other.CompareTag("Ennemy") || other.CompareTag("DestroyableEvmt"))
        // {
        //     hasDashHit = true;
        // }        
    }

    public void CameraTriggerSet()
    {
        m_mainCam.Follow = null;           

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

    private void ReloadScene()
    {
        if (Input.GetKey(KeyCode.P))
        {
            SceneManager.LoadScene("SC_3C");
        }
    }
}

[System.Serializable]
public class HealthManager
{
    public float m_value;
    public float m_maxValue = 10;
    public bool isTakingDamage;

    public void Init()
    {
        m_value = m_maxValue;
    }
    public void TakeDamage(float damage)
    {
        m_value = Mathf.Max(0, m_value - damage);
        isTakingDamage = true;
        
        if (isTakingDamage)
        {
            isTakingDamage = false;
        }
    }

    public bool isDead() => m_value <= 0;

}

