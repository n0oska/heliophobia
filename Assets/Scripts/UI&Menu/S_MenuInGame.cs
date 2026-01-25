using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class S_MenuInGame : MonoBehaviour
{
    [SerializeField] Canvas m_canvasMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            SceneManager.LoadScene("SC_Menu");
        }
    }
}
