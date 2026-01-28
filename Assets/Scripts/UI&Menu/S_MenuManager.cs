using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class S_MenuManager : MonoBehaviour
{
    [Header("Main menu")]
    [SerializeField] private Button m_startButton;
    [SerializeField] private Canvas m_canvasMain;
    private bool isInOptions = false;

    [Header("Options menu")]
    [SerializeField] private Canvas m_canvasOptions;
    [SerializeField] private Button m_buttonSound;
    [SerializeField] private GameObject m_panelSound;
    [SerializeField] private GameObject m_panelOptions;
    [SerializeField] private Button m_backButtonPanelOptions;
    [SerializeField] private Slider m_sliderSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_startButton.Select();
        m_canvasOptions.enabled = false;
        m_panelOptions.SetActive(false);
        m_panelSound.SetActive(false);
    }

    public void Options()
    {
        m_canvasOptions.enabled = true;
        m_canvasMain.enabled = false;
        m_buttonSound.Select();
    }

    public void OnBackClick()
    {
        m_canvasMain.enabled = true;
        m_canvasOptions.enabled = false;
        m_startButton.Select();
    }

    public void OnSoundClick()
    {
        m_panelSound.SetActive(true);
        m_sliderSound.Select();
    }

    public void OnSoundBackClick()
    {
        m_panelSound.SetActive(false);
        m_buttonSound.Select();
    }

    public void OnControlsClick()
    {
        m_panelOptions.SetActive(true);
        m_backButtonPanelOptions.Select();
    }

    public void OnControlsBackClick()
    {
        m_panelOptions.SetActive(false);
        m_buttonSound.Select();
    }

    public void OnStartClick()
    {
        SceneManager.LoadScene("LV1");
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
