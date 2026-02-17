using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    [Header("Other")]
    [SerializeField] private Rigidbody m_rb;
    private S_CharaController m_charaCon;

    void Start()
    {
        m_charaCon = m_rb.GetComponent<S_CharaController>();
        m_canvasMenu.enabled = false;
        m_optionsMenu.enabled = false;
        m_controlsPanel.SetActive(false);
        m_soundPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            ToggleMenu();
        }
    }

    // ✅ NOUVELLE MÉTHODE : sélectionne automatiquement le bouton le plus haut actif
    void SelectTopButton(GameObject parent)
    {
        Button[] buttons = parent.GetComponentsInChildren<Button>(true);

        foreach (Button btn in buttons)
        {
            if (btn.gameObject.activeInHierarchy && btn.interactable)
            {
                EventSystem.current.SetSelectedGameObject(null);
                btn.Select();
                return;
            }
        }
    }

    void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        Cursor.visible = !Cursor.visible;

        if (isMenuOpen)
        {
            Time.timeScale = 0;
            m_canvasMenu.enabled = true;
            m_optionsMenu.enabled = false;

            SelectTopButton(m_canvasMenu.gameObject);
        }
        else
        {
            Time.timeScale = 1;
            m_canvasMenu.enabled = false;
            m_optionsMenu.enabled = false;

            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void OnContinueClick()
    {
        m_canvasMenu.enabled = false;
        Time.timeScale = 1;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnQuitClick()
    {
        Time.timeScale = 1;
        m_canvasMenu.enabled = false;
        EventSystem.current.SetSelectedGameObject(null);
        SceneManager.LoadScene("SC_Menu");
    }

    public void OnRestartClick()
    {
        Time.timeScale = 1;
        m_canvasMenu.enabled = false;
        EventSystem.current.SetSelectedGameObject(null);
        SceneManager.LoadScene("LV1");
    }

    public void OnOptionsClick()
    {
        m_optionsMenu.enabled = true;
        m_canvasMenu.enabled = false;

        SelectTopButton(m_optionsMenu.gameObject);
    }

    public void OnOptionsBackClick()
    {
        m_optionsMenu.enabled = false;
        m_canvasMenu.enabled = true;

        SelectTopButton(m_canvasMenu.gameObject);
    }

    public void OnSoundClick()
    {
        m_soundPanel.SetActive(true);
        SelectTopButton(m_soundPanel);
    }

    public void OnControlsClick()
    {
        m_controlsPanel.SetActive(true);
        SelectTopButton(m_controlsPanel);
    }

    public void OnBackClickSound()
    {
        m_soundPanel.SetActive(false);
        m_optionsMenu.enabled = true;

        SelectTopButton(m_optionsMenu.gameObject);
    }

    public void OnBackClickControls()
    {
        m_controlsPanel.SetActive(false);
        m_optionsMenu.enabled = true;

        SelectTopButton(m_optionsMenu.gameObject);
    }
}
