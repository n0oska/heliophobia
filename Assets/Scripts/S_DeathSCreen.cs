using UnityEngine;
using UnityEngine.UI;

public class S_DeathSCreen : MonoBehaviour
{
    [SerializeField] Canvas m_deathScreen;
    [SerializeField] Button m_restartButton;
    [SerializeField] GameObject m_player;

    private S_CharaController m_charaCon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_deathScreen.enabled = false;
        m_charaCon = m_player.GetComponent<S_CharaController>();
    }

    // Update is called once per frame
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
        m_charaCon.hasRespawned = true;
        Time.timeScale = 1;
        m_deathScreen.enabled = false;
    }
}
