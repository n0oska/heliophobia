using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class S_MenuInGame : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] Canvas m_canvasMenu;
    [SerializeField] Canvas m_optionsMenu;
    [SerializeField] Button m_quitButton;
    [SerializeField] Button m_ContinueButton;
    [SerializeField] Button m_OptionsButton;
    [SerializeField] Button m_RestartButton;
    private bool isMenuOpen = false;

    [Header("Options")]
    [SerializeField] Button m_backButton;
    [SerializeField] Button m_soundButton;
    [SerializeField] Button m_controlsButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_canvasMenu.enabled = false;
        m_optionsMenu.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
           ToggleMenu();                  
        }       
    }    

    void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;

        if (isMenuOpen)
        {
            Time.timeScale=0;
            m_canvasMenu.enabled = true;
            m_optionsMenu.enabled=false;
            m_ContinueButton.Select();
        }

        else
        {
            Time.timeScale = 1;
            m_canvasMenu.enabled = false;
            m_optionsMenu.enabled = false;
        }
    }

    public void OnContinueClick()
    {
        m_canvasMenu.enabled = false;
        Time.timeScale = 1;
    }

    public void OnQuitClick()
    {
        m_canvasMenu.enabled=false;
        SceneManager.LoadScene("SC_Menu");
    }

    public void OnRestartClick()
    {
        m_canvasMenu.enabled = false;
        SceneManager.LoadScene("LV1");
    }

    public void OnOptionsClick()
    {
        m_optionsMenu.enabled = true;
    }
}
