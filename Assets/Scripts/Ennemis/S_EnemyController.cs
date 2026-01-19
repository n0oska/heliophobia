using System.Collections;
//using UnityEditor.SearchService;
using UnityEngine;
//using Unity.VisualScripting;
using Unity.Cinemachine;
using UnityEngine.SocialPlatforms.Impl;

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
    [SerializeField] private float m_timerAttack = 0;
    [SerializeField] private float m_coolDown = 1.5f;
    [SerializeField] private bool canAttack = true;
    [SerializeField] Vector3 m_currentPos;
    [SerializeField] Vector3 m_castOffset;
    [SerializeField] GameObject player;
    [SerializeField] GameObject spawner;
    [SerializeField] GameObject ScoreManager;
    [SerializeField] private CinemachineCamera m_cam;
    [SerializeField] Rigidbody m_rb;
    [SerializeField] private CinemachineBasicMultiChannelPerlin m_channels;
    [SerializeField] private ParticleSystem m_ennemyHitPs;

    private ParticleSystem m_ennemyHitPsInstance;
    public int m_Value;

    private CapsuleCollider col;
    private bool hasSpawned = false;
    private bool canMove = true;
    private bool isStunned = false;

    private SpriteRenderer m_spriteRenderer;
    
    private float stunTime;
    private float stunTimer = 0.5f;
    private float previousHeathValue;
    private Quaternion PsRotation;
    private bool hasPsSpawned = false;

    


    void Start()
    {
        m_health.Init();
        m_currentPos = this.gameObject.transform.position;
        m_rb = this.GetComponent<Rigidbody>();
        GameObject chara = GameObject.FindGameObjectWithTag("Player");
        player = chara;
        spawner = GameObject.FindGameObjectWithTag("Spawner");
        var cam = GameObject.FindFirstObjectByType<CinemachineCamera>();
        m_cam = cam;
        Debug.Log(m_cam);
        col = gameObject.GetComponent<CapsuleCollider>();
        col.enabled = false;
        hasSpawned = true;
        stunTime = stunTimer;
        previousHeathValue = m_health.m_maxValue;
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (hasSpawned)
            col.enabled = true;
        GetMovement();
        StartCoroutine(CombatDmg());
        var spawnerScript = spawner.GetComponent<S_EnemySpawner>();

        Vector3 dirToPlayer = (player.transform.position - m_rb.transform.position).normalized;
        if (dirToPlayer.x != 0 && dirToPlayer.x > 0)
        {
            m_castOffset = new Vector3(1, 0, 0);
            m_spriteRenderer.flipX = false;
        }
        if (dirToPlayer.x != 0 && dirToPlayer.x < 0)
        {
            m_castOffset = new Vector3(-1, 0, 0);
            m_spriteRenderer.flipX = true;            
        }
        //Debug.Log($"PlayerX: {player.transform.position.x} | EnemyX: {transform.position.x} | dirX: {dirToPlayer.x}");

        if (m_health.isDead())
        { 
            var scriptScore = GameObject.FindAnyObjectByType<S_ScoreManager>();
            if (scriptScore != null)
            {
                Debug.Log("score activé sah");
                scriptScore.m_score += m_Value;
            }
            //GameObject parent = m_rb.GetComponentInParent<GameObject>();          
            spawnerScript.isDead = true;            
            Destroy(gameObject);
        }

        if (m_health.isTakingDamage && !hasPsSpawned)
        {
            hasPsSpawned = true;
            StartCoroutine(C_ParticlesInstance());            
            var m_sprite = this.gameObject.GetComponent<SpriteRenderer>().color;
            m_sprite.b = 250f;
            m_sprite.r = 250f;
            m_sprite.g= 250f;
        }

        if (!m_health.isTakingDamage)
        {
            var m_sprite = this.gameObject.GetComponent<SpriteRenderer>().color;
            m_sprite.r = 250;
            m_sprite.g = 0;
            m_sprite.b = 0;
        }
        
        if (m_health.isTakingDamage && !isStunned)
        {
            isStunned = true;
            m_health.isTakingDamage = false;
            Debug.Log("hastakendmg");
        }
        
        if (isStunned)
        {
            stunTime -= Time.deltaTime;

            if (stunTime >= 0)
            {
                StunTarget();
            }

            else
            {
                canMove = true;
                canAttack = true;
                stunTime = stunTimer;
                isStunned = false;
                Debug.Log("fin stun");
            }
        }       
    }

    private void StunTarget()
    {
        Debug.Log("inStun");
        canMove = false;
        canAttack = false;            
        
        //mettre anim dégats        

    }

    private void GetMovement()
    {
        if (m_rb == null)
            Debug.Log("Pas de rigidbody");

        var spriteChara = player.GetComponentInChildren<SpriteRenderer>();

        float distance = Vector3.Distance(m_rb.position, spriteChara.transform.position);

        if (m_rb != null && spriteChara != null && canMove)
        {
            if (distance > m_stopDistance)
            {
                Vector3 direction = (spriteChara.transform.position - m_rb.position).normalized;
                direction.y = 0;

                transform.position += direction * m_speed * Time.deltaTime;
            }
        }
    }

    private IEnumerator CombatDmg()
    {
        Collider[] playerCollider = Physics.OverlapSphere(transform.position + m_castOffset, m_castRadius, m_playerLayer);

        foreach (var player in playerCollider)
        {
            var chara = player.GetComponentInChildren<S_CharaController>();

            if (chara != null && canAttack)
            {                            
                canAttack = false;
                chara.m_playerHealth.TakeDamage(1);              
                
                yield return new WaitForSeconds(m_coolDown);
                canAttack = true;
            }
        }
    }

    private IEnumerator C_ParticlesInstance()
    {
        //offset si besoin*
        Quaternion rotation;

        if (m_spriteRenderer.flipX)
            rotation = Quaternion.Euler(0, 0, 0);
        else 
            rotation = Quaternion.Euler(0, 180, 0);

        m_ennemyHitPsInstance = Instantiate(m_ennemyHitPs, transform.position, rotation);
        yield return new WaitForSeconds(0.2f);
        hasPsSpawned = false;        
    }

    
    //private void FixedUpdate()
    //{
    //    Collider[] playerCollider = Physics.OverlapSphere(m_currentPos + m_castOffset, m_castRadius, m_playerLayer);       

    //    foreach (var player in playerCollider)
    //    {
    //        var chara = player.GetComponent<S_CharaController>();

    //        if (chara != null)
    //        {
    //            Debug.Log(chara);
    //            chara.m_playerHealth.TakeDamage(1);
    //        }
    //    }
    //}


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.gameObject.transform.position + m_castOffset, m_castRadius);
    }
}
