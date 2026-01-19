using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class S_MenuInGame : MonoBehaviour
{
    public GameObject m_MainMenuUI;
    public GameObject m_PauseMenuUI;
    public GameObject m_OptionsMenuUI;
    public GameObject m_CreditsMenuUI;

    public GameObject m_MainFirstButton;
    public GameObject m_PauseFirstButton;
    public GameObject m_OptionsFirstButton;
    public GameObject m_CreditsFirstButton;

    public Slider m_VolumeSlider;

    public S_CharaController m_Player;

    private bool m_IsPaused;
    private bool m_IsMainMenu;

    void Start()
    {
        m_IsMainMenu = SceneManager.GetActiveScene().name == "SC_MainMenu";

        Time.timeScale = 1f;
        CloseAllMenus();

        if (!m_IsMainMenu && m_Player == null)
            m_Player = FindObjectOfType<S_CharaController>();

        if (m_VolumeSlider != null)
            m_VolumeSlider.value = S_AudioManager.Instance.GetVolume();

        if (m_IsMainMenu && m_MainMenuUI != null)
        {
            m_MainMenuUI.SetActive(true);
            SelectUI(m_MainFirstButton);
        }
    }

    void Update()
    {
        if (m_IsMainMenu)
            return;

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            if (m_IsPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void StartGame(int levelIndex)
    {
        LoadScene("SC_Level_0" + levelIndex);
    }

    public void PauseGame()
    {
        m_IsPaused = true;
        Time.timeScale = 0f;

        m_PauseMenuUI.SetActive(true);
        SelectUI(m_PauseFirstButton);

        if (m_Player != null)
            m_Player.isPaused = true;
    }

    public void ResumeGame()
    {
        CloseAllMenus();
        Time.timeScale = 1f;
        m_IsPaused = false;

        if (m_Player != null)
            m_Player.isPaused = false;
    }

    public void OpenOptions()
    {
        HideAllBut(m_OptionsMenuUI);
        SelectUI(m_OptionsFirstButton);
    }

    public void CloseOptions()
    {
        if (m_IsMainMenu)
        {
            HideAllBut(m_MainMenuUI);
            SelectUI(m_MainFirstButton);
        }
        else
        {
            HideAllBut(m_PauseMenuUI);
            SelectUI(m_PauseFirstButton);
        }
    }

    public void OpenCredits()
    {
        HideAllBut(m_CreditsMenuUI);
        SelectUI(m_CreditsFirstButton);
    }

    public void CloseCredits()
    {
        CloseOptions();
    }

    public void OnVolumeChanged(float value)
    {
        S_AudioManager.Instance.SetVolume(value);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SC_MainMenu");
    }

    private void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    private void CloseAllMenus()
    {
        if (m_MainMenuUI != null) m_MainMenuUI.SetActive(false);
        if (m_PauseMenuUI != null) m_PauseMenuUI.SetActive(false);
        if (m_OptionsMenuUI != null) m_OptionsMenuUI.SetActive(false);
        if (m_CreditsMenuUI != null) m_CreditsMenuUI.SetActive(false);
    }

    private void HideAllBut(GameObject menu)
    {
        CloseAllMenus();
        if (menu != null)
            menu.SetActive(true);
    }

    private void SelectUI(GameObject element)
    {
        if (element == null)
            return;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(element);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
