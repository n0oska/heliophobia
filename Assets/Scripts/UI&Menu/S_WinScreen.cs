using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class S_WinScreen : MonoBehaviour
{
    [SerializeField] private Button m_nextLvlButton;
    [SerializeField] private Button m_restartButton;
    [SerializeField] private  Canvas m_canvas;

    private Scene loadedScene;
    private int loadedSceneIndex; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_canvas.enabled = false;
        loadedScene = SceneManager.GetActiveScene();
        m_nextLvlButton.onClick.AddListener(OnNextLVLClick);
        m_restartButton.onClick.AddListener(OnRestartClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnNextLVLClick()
    {
        Cursor.visible = false;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // Vérifie que le niveau existe
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene("SC_Menu");
        }
    }

    public void OnRestartClick()
    {        
        Cursor.visible = false;       
        SceneManager.LoadScene(loadedScene.buildIndex);
    }
}
