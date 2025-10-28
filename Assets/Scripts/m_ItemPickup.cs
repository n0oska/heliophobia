using UnityEngine;

public class m_ItemPickup : MonoBehaviour
{
    [Header("Pickup Data")]
    [SerializeField] private m_ItemPickupSO m_PickupData;
    [SerializeField] private bool m_DestroyOnPickup = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        S_CharaController player = collision.GetComponent<S_CharaController>();
        if (player != null && m_PickupData != null)
        {
            bool added = player.AddItem(m_PickupData.m_ItemData, m_PickupData.m_Amount);
            if (added && m_DestroyOnPickup)
            {
                Destroy(gameObject);
            }
        }
    }
}
