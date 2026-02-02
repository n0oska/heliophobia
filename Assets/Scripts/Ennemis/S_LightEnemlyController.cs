using System.Collections;
using UnityEngine;

public class S_LightEnemyController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light m_light; 
    [SerializeField] private float m_effectRadius = 5f;
    [SerializeField] private GameObject m_coins;

    private bool isLight = false;

    private void OnDrawGizmosSelected()
    {
      
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_effectRadius);
    }

    private void Update()
    {       
        Collider[] players = Physics.OverlapSphere(transform.position, m_effectRadius);
        foreach (var col in players)
        {
            S_CharaController player = col.GetComponent<S_CharaController>();
            if (player != null)
            {
                player.ForceExitShadow(); 
            }
        }

        S_EnemyController scriptennemy = this.gameObject.GetComponent<S_EnemyController>();

        if (scriptennemy != null)
        {
            isLight = true;
        }

        if (isLight && scriptennemy.m_health.isDead())
        {
            StartCoroutine(C_Drop());
        }

    }

    private IEnumerator C_Drop()
    {
        Debug.Log("in coroutine drop");
        int randomInt = Random.Range(1, 10);
        Debug.Log(randomInt);

        if (randomInt >= 6)
        {
            m_coins.transform.position = this.transform.position;
            Instantiate(m_coins);
            yield return new WaitForFixedUpdate();
        }        
    }
}
