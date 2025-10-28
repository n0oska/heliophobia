using UnityEngine;

[CreateAssetMenu(fileName = "NewPickup", menuName = "Inventory/Pickup")]
public class m_ItemPickupSO : ScriptableObject
{
    public m_Item m_ItemData;    // L'item à donner
    public int m_Amount = 1;     // Quantité donnée
}