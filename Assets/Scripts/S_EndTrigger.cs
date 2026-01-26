using UnityEngine;

public class S_EndTrigger : MonoBehaviour
{
    [SerializeField] Canvas EndScreen; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EndScreen.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EndScreen.enabled = true;
        }
    }
}
