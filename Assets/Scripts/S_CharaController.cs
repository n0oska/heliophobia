using UnityEngine;

public class S_CharaController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Rigidbody m_rb;    
    [SerializeField] private float m_speed;
    [SerializeField] private float m_damage;
    [SerializeField] private SpriteRenderer m_sR;

    private Vector3 m_currentDirection;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(x, 0, y);
        m_rb.linearVelocity = dir * m_speed;

        if (x != 0 && x < 0)
            m_sR.flipX = true;

        else if (x!=0 && x > 0)
            m_sR.flipX = false;

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
        
    }
    
}
