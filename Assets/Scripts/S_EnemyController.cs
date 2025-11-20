using System.Collections;
using UnityEditor.SearchService;
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
    [SerializeField] private float m_timerAttack = 0;
    [SerializeField] private float m_coolDown = 1.5f;
    [SerializeField] private bool canAttack = true;
    [SerializeField] Vector3 m_currentPos;
    [SerializeField] Vector3 m_castOffset;
    [SerializeField] GameObject player;
    [SerializeField] Rigidbody m_rb;

    void Start()
    {
        m_health.Init();
        m_currentPos = this.gameObject.transform.position;
        m_rb = this.GetComponent<Rigidbody>();
        GameObject chara = GameObject.FindGameObjectWithTag("Player");
        player = chara;
       
    }

    void Update()
    {
        GetMovement();
        StartCoroutine(CombatDmg());

        if (this.gameObject.transform.position.x != 0 && this.gameObject.transform.position.x > 0)
            m_castOffset = new Vector3(1, 0, 0);
        if (this.gameObject.transform.position.x != 0 && this.gameObject.transform.position.x < 0)
            m_castOffset = new Vector3(-1, 0, 0);

        if (m_health.isDead())
        {
            var charaCon = player.GetComponentInChildren<S_CharaController>();
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

    private IEnumerator CombatDmg()
    {
        Collider[] playerCollider = Physics.OverlapSphere(transform.position + m_castOffset, m_castRadius, m_playerLayer);

        foreach (var player in playerCollider)
        {
            var chara = player.GetComponentInChildren<S_CharaController>();

            if (chara != null && canAttack)
            {
                chara.m_playerHealth.TakeDamage(1);
                canAttack = false;
                yield return new WaitForSeconds(m_coolDown);
                canAttack = true;
            }
        }
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
