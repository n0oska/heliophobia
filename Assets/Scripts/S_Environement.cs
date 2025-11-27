using System.Collections;
using UnityEngine;

public class S_Environement : MonoBehaviour
{
    public HealthManager m_health = new HealthManager();
    [SerializeField] GameObject m_coin;
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
            
    }

    private IEnumerator DropRandom()
    {
        int randomInt = Random.Range(1, 10);

        if (randomInt >=6 )
        {
            Debug.Log(randomInt);
            m_coin.transform.position = this.transform.position;
            Instantiate(m_coin);
            yield return new WaitForFixedUpdate();
        }
    }
}
