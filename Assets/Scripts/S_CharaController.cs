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

        if (Physics.Raycast(transform.position, dirLight, out RaycastHit hit, m_rayLength, m_obstacle))
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
        m_damage = m_baseDamage * 2; //équilibrage force à revoir
    }


    private void OnDrawGizmos()
    {
        Vector3 lightDir = m_light.transform.forward;

        Gizmos.color = isInShadow ? Color.red : Color.green;
        Gizmos.DrawLine(m_rb.transform.position, m_rb.transform.position + (- m_light.transform.forward) * m_rayLength);
    }



}
