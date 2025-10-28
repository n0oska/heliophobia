using System.Collections.Generic;
using UnityEngine;

public class S_CharaController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Rigidbody m_rb;
    [SerializeField] private float m_speed;
    [SerializeField] private float m_damage;
    [SerializeField] private float m_baseDamage;
    [SerializeField] private SpriteRenderer m_sR;

    [Header("Shadow Manager")]
    [SerializeField] private Light m_light;
    [SerializeField] private bool isInShadow;
    [SerializeField] private float m_rayLength;
    [SerializeField] private LayerMask m_obstacle;
    [SerializeField] private Vector3 m_offset;

    private Vector3 m_currentDirection;

    // ---------------- Inventory intégré ----------------
    [System.Serializable]
    public class InventoryItem
    {
        public m_Item item;
        public int quantity;
    }

    [Header("Inventory intégré")]
    [SerializeField] private List<InventoryItem> m_Items = new List<InventoryItem>();
    [SerializeField] private int m_MaxSize = 20;

    void Start()
    {
        m_rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(x, 0, y);
        m_rb.linearVelocity = dir * m_speed;

        if (x != 0 && x < 0)
            m_sR.flipX = true;
        else if (x != 0 && x > 0)
            m_sR.flipX = false;

        if (m_currentDirection != Vector3.zero)
            OnStartMoving();

        if (m_light == null)
            return;
        else
            CheckLight();
    }

    private void OnStartMoving()
    {
        //display animations
    }

    private void CheckLight()
    {
        Vector3 dirLight = -m_light.transform.forward;

        if (Physics.Raycast(transform.position + m_offset, dirLight, out RaycastHit hit, m_rayLength, m_obstacle))
        {
            if (hit.collider != null)
            {
                if (!isInShadow)
                    OnEnterShadow();
            }
        }
        else
        {
            if (isInShadow)
                OnExitShadow();
        }
    }

    private void OnEnterShadow()
    {
        isInShadow = true;
        Debug.Log("ombre");
        ShadowForce();
    }

    private void NormalForce()
    {
        m_damage = m_baseDamage;
    }

    private void OnExitShadow()
    {
        isInShadow = false;
        Debug.Log("plus ombre");
        NormalForce();
    }

    private void ShadowForce()
    {
        m_damage = m_baseDamage * 2;
    }

    private void OnDrawGizmos()
    {
        Vector3 lightDir = m_light.transform.forward;

        Gizmos.color = isInShadow ? Color.red : Color.green;
        Gizmos.DrawLine(m_rb.transform.position + m_offset, m_rb.transform.position + (-m_light.transform.forward) * m_rayLength);
    }

    // ---------------- Méthodes de l'inventaire ----------------

    public bool AddItem(m_Item newItem, int amount = 1)
    {
        if (newItem.IsStackable)
        {
            foreach (InventoryItem entry in m_Items)
            {
                if (entry.item == newItem)
                {
                    entry.quantity += amount;
                    Debug.Log($"Ajouté {amount} x {newItem.ItemName} (stackable)");
                    return true;
                }
            }
        }

        if (m_Items.Count >= m_MaxSize)
        {
            Debug.Log("Inventaire plein !");
            return false;
        }

        InventoryItem newEntry = new InventoryItem
        {
            item = newItem,
            quantity = amount
        };
        m_Items.Add(newEntry);
        Debug.Log($"Ajouté {amount} x {newItem.ItemName}");
        return true;
    }

    public void RemoveItem(m_Item itemToRemove, int amount = 1)
    {
        for (int i = 0; i < m_Items.Count; i++)
        {
            if (m_Items[i].item == itemToRemove)
            {
                if (itemToRemove.IsStackable)
                {
                    m_Items[i].quantity -= amount;
                    if (m_Items[i].quantity <= 0)
                        m_Items.RemoveAt(i);
                }
                else
                {
                    m_Items.RemoveAt(i);
                }
                Debug.Log($"Retiré {itemToRemove.ItemName}");
                return;
            }
        }
    }

    public List<InventoryItem> GetItems()
    {
        return m_Items;
    }
}
