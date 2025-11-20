using UnityEngine;

public class S_Environement : MonoBehaviour
{
    public HealthManager m_health = new HealthManager();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_health.Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_health.isDead())
            Destroy(this.gameObject);
    }
}
