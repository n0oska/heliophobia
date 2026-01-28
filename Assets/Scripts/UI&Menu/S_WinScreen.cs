using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class S_WinScreen : MonoBehaviour
{
    [SerializeField] private Button m_nextLvlButton;
    [SerializeField] private  Canvas m_canvas;

    private Scene loadedScene;
    private int loadedSceneIndex; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_canvas.enabled = false;
        loadedScene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnNextLVLClick()
    {
        Cursor.visible = false;
        SceneManager.LoadScene(loadedScene.buildIndex+1);
    }

    public void OnRestartClick()
    {        
        Cursor.visible = false;       
        SceneManager.LoadScene(loadedScene.buildIndex);
    }
}
