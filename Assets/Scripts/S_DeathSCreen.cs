using UnityEngine;
using UnityEngine.UI;

public class S_DeathSCreen : MonoBehaviour
{
    [SerializeField] Canvas m_deathScreen;
    [SerializeField] Button m_restartButton;
    [SerializeField] GameObject m_player;

    private S_CharaController m_charaCon;
    [Header("Sound Effects")]
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioClip m_deathSFX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_deathScreen.enabled = false;
        m_charaCon = m_player.GetComponent<S_CharaController>();
        if (m_audioSource == null)
            m_audioSource = GetComponent<AudioSource>();

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
            if (m_audioSource && m_deathSFX)
                m_audioSource.PlayOneShot(m_deathSFX);
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
