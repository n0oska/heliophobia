using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class S_ScoreManager : MonoBehaviour
{

    public int m_score = 0;

    [SerializeField] int m_scoreCible = 0;
    [SerializeField] TextMeshProUGUI m_text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (m_text != null)
        {
            m_text.text = m_score.ToString();
        }
    }
}
