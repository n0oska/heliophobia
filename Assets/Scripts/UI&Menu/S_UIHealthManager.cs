using UnityEngine;
using UnityEngine.UI;

public class S_UIHealthManager : MonoBehaviour
{

    [SerializeField] S_CharaController m_charaCon;
    [SerializeField] GameObject m_chara;
    [SerializeField] Slider m_healthSlider;
    [SerializeField] Slider m_manaSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {        
        m_charaCon = m_chara.GetComponent<S_CharaController>();
        m_healthSlider.maxValue = m_charaCon.m_playerHealth.m_maxValue;
        m_healthSlider.value = m_charaCon.m_playerHealth.m_maxValue;

        m_manaSlider.maxValue = m_charaCon.m_coinsRequiredForBuff;
        m_manaSlider.value = 0;
    }

    private void Update()
    {
        
        UpdateUIHealth();
        UpdateUIMana();
    }

    private void UpdateUIHealth()
    {
        float smoothSpeed = 10f;
        m_healthSlider.value = Mathf.Lerp(m_healthSlider.value, m_charaCon.m_playerHealth.m_value, Time.deltaTime * smoothSpeed);
    }

    private void UpdateUIMana()
    {
        if (m_charaCon != null)
        {
            Debug.Log("UI/Characon validé");
            float smoothSpeed = 10f;
            m_manaSlider.value = Mathf.Lerp(m_manaSlider.value, m_charaCon.m_coinCount, Time.deltaTime * smoothSpeed);
            Debug.Log(m_charaCon.m_coinCount);
        }
    }


}
