using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class S_MenuManager : MonoBehaviour
{
    [SerializeField] private Button m_startButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_startButton.Select();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartClick()
    {
        SceneManager.LoadScene("SC_3C");
    }

    public void OnQuitClick()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // Quitte en build
#endif
    }
}
