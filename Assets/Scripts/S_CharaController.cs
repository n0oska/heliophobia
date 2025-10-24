using UnityEngine;

public class S_CharaController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Rigidbody m_rb;    
    [SerializeField] private float m_speed;
    [SerializeField] private float m_damage;
    [SerializeField] private SpriteRenderer m_spriteRenderer;

    private Vector3 m_currentDirection;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_currentDirection = Vector3.zero;
        m_currentDirection = Vector3.right * Input.GetAxisRaw("Horizontal") + Vector3.forward * Input.GetAxisRaw("Vertical");

        if (m_currentDirection != Vector3.zero)
        {
            OnStartMoving();
        }
    }

    private void OnStartMoving()
    {
        //display animations
    }

    void FixedUpdate()
    {
        Vector3 dir = Vector3.right * m_currentDirection.x + Vector3.forward * m_currentDirection.z;
        m_rb.MovePosition(m_rb.position + dir * m_speed * Time.fixedDeltaTime);
    }

    private void SetOrientation()
    {
        if (m_currentDirection.x !=0)
        {
            m_spriteRenderer.flipX = m_currentDirection.x < 0 ;
        }
    }
}
