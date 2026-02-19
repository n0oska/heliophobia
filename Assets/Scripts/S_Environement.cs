using System.Collections;
using UnityEngine;

public class S_Environement : MonoBehaviour
{
    public HealthManager m_health = new HealthManager();
    [SerializeField] GameObject m_coin;
    [SerializeField] private ParticleSystem m_ennemyHitPs;
    [SerializeField] private int m_lowProba;
    [SerializeField] private int m_highProba;
    [SerializeField] private int m_targetProba;
    private ParticleSystem m_ennemyHitPsInstance;

    private bool hasPsSpawned;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_health.Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_health.isDead())
        {
            StartCoroutine(DropRandom());
            Destroy(this.gameObject);
        }

        if (m_health.isTakingDamage && !hasPsSpawned)
        {
            hasPsSpawned = true;
            StartCoroutine(C_ParticlesInstance());
        }

        m_health.EndOfFrame();
    }

    private IEnumerator C_ParticlesInstance()
    {
        m_ennemyHitPsInstance = Instantiate(m_ennemyHitPs, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        m_ennemyHitPsInstance.Clear();
        hasPsSpawned = false;

        //m_anim.SetTrigger("TakeDamage");

    }

    private IEnumerator DropRandom()
    {
        int randomInt = Random.Range(m_lowProba, m_highProba);

        if (randomInt >= m_targetProba)
        {
            Debug.Log(randomInt);
            m_coin.transform.position = this.transform.position;
            Instantiate(m_coin);
            yield return new WaitForFixedUpdate();
        }
    }
}
//Dans l'inspector sur l'enviro de base : low proba =1 high proba = 10 target proba =6