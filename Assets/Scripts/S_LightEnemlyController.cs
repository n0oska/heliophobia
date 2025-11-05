using UnityEngine;

public class S_LightEnemyController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light m_light; // Lumière de l'ennemi
    [SerializeField] private float m_effectRadius = 5f;

    private void OnDrawGizmosSelected()
    {
        // Visualiser la zone de neutralisation
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_effectRadius);
    }

    private void Update()
    {
        // On peut faire un trigger manuel si pas de collider
        Collider[] players = Physics.OverlapSphere(transform.position, m_effectRadius);
        foreach (var col in players)
        {
            S_CharaController player = col.GetComponent<S_CharaController>();
            if (player != null)
            {
                player.ForceExitShadow(); // méthode publique à créer dans le player
            }
        }
    }
}
