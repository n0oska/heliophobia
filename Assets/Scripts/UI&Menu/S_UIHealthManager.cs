using UnityEngine;
using UnityEngine.UI;

public class S_UIHealthManager : MonoBehaviour
{

    [SerializeField] S_CharaController m_healthManagerPlayer;
    [SerializeField] GameObject m_chara;
    [SerializeField] Slider m_healthSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {        
        m_healthManagerPlayer = m_chara.GetComponent<S_CharaController>();
        m_healthSlider.maxValue = m_healthManagerPlayer.m_playerHealth.m_maxValue;
        m_healthSlider.value = m_healthManagerPlayer.m_playerHealth.m_maxValue;
    }

    private void Update()
    {
        
        UpdateUI();
        
    }

    private void UpdateUI()
    {
        float smoothSpeed = 10f;
        m_healthSlider.value = Mathf.Lerp(m_healthSlider.value, m_healthManagerPlayer.m_playerHealth.m_value, Time.deltaTime * smoothSpeed);
    }


}
