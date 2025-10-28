using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Inventory/m_Item")]
public class m_Item : ScriptableObject
{
    [Header("Propriétés de l'objet")]
    [SerializeField] private string m_ItemName = "Nouvel objet";
    [SerializeField] private Sprite m_Icon;
    [SerializeField] private bool m_IsStackable = false;
    [SerializeField, TextArea] private string m_Description;

    public string ItemName => m_ItemName;
    public Sprite Icon => m_Icon;
    public bool IsStackable => m_IsStackable;
    public string Description => m_Description;
}
