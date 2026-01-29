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

    [Header("Sound")]
    [SerializeField] GameObject m_soundPanel;
    [SerializeField] Slider m_soundSlider;

    [Header("Controls")]
    [SerializeField] Button m_controlsBackButton;
    [SerializeField] GameObject m_controlsPanel;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_canvasMenu.enabled = false;
        m_optionsMenu.enabled = false;
        m_controlsPanel.SetActive(false);
        m_soundPanel.SetActive(false);
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
        Cursor.visible = !Cursor.visible;

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
        Time.timeScale = 1;
        m_canvasMenu.enabled=false;
        SceneManager.LoadScene("SC_Menu");
    }

    public void OnRestartClick()
    {
        Time.timeScale = 1;
        m_canvasMenu.enabled = false;
        SceneManager.LoadScene("LV1");
    }

    public void OnOptionsClick()
    {
        m_optionsMenu.enabled = true;
        m_canvasMenu.enabled = false;
    }

    public void OnOptionsBackClick()
    {
        m_optionsMenu.enabled = false;
        m_canvasMenu.enabled = true;
    }

    public void OnSoundClick()
    {
        m_soundPanel.SetActive(true);
        m_soundSlider.Select();
    }

    public void OnControlsClick()
    {
        m_controlsPanel.SetActive(true);
        m_controlsBackButton.Select();
    }

    public void OnBackClickSound()
    {
        m_soundPanel.SetActive(false);
        m_optionsMenu.enabled = true;
        m_soundButton.Select();
    }

    public void OnBackClickControls()
    {
        m_controlsPanel.SetActive(false);
        m_optionsMenu.enabled = true;
        m_soundButton.Select();
    }
}
