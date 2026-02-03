using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class S_DeathSCreen : MonoBehaviour
{
    [SerializeField] private Canvas m_deathScreen;
    [SerializeField] private Button m_restartButton;
    [SerializeField] private GameObject m_player;

    private S_CharaController m_charaCon;
    private bool m_isGameOver = false;

    void Start()
    {
        m_deathScreen.enabled = false;
        m_charaCon = m_player.GetComponent<S_CharaController>();
    }

    void Update()
    {
        CheckDeath();
    }

    private void CheckDeath()
    {
        if (m_charaCon.m_playerHealth.isDead() && !m_isGameOver)
        {
            m_isGameOver = true;

            Debug.Log("isDead");

            // Ici je check l'affichage
            m_deathScreen.enabled = true;

            // je désactive du joueur (plus d'inputs détecté)
            m_charaCon.enabled = false;

            // on force l'affochage du curseur
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Le bouton est automatiquement sélectionné
            m_restartButton.Select();

            // Le jeu se met en pause
            Time.timeScale = 0f;
        }
    }

    public void OnRestartClick()
    {
        // Reprise du temps
        Time.timeScale = 1f;

        // Re-lock du curseur
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Reload de la scène
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}