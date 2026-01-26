using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class S_WinScreen : MonoBehaviour
{
    [SerializeField] private Button m_nextLvlButton; 

    private Scene loadedScene;
    private int loadedSceneIndex; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
