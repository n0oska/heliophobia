using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class S_DeathSCreen : MonoBehaviour
{
    [SerializeField] Canvas m_deathScreen;
    [SerializeField] Button m_restartButton;
    [SerializeField] GameObject m_player;

    private S_CharaController m_charaCon;

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
        if (m_charaCon.m_playerHealth.isDead())
        {
            Debug.Log("isDead");
            m_deathScreen.enabled = true;
            m_restartButton.Select();
            //Time.timeScale = 0;
        }
    }

    public void OnRestartClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}