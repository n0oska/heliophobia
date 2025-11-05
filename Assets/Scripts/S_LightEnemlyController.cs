using UnityEngine;

public class S_LightEnemyController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light m_light; 
    [SerializeField] private float m_effectRadius = 5f;

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
    }
}
