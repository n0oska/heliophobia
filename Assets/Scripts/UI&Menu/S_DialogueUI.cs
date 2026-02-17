using UnityEngine;
//using UnityEditor.UI;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class S_DialogueUI : MonoBehaviour
{
    [SerializeField] private Canvas m_canvas;
    [SerializeField] private Image m_image;
    [SerializeField] private GameObject m_Namiaus;
    [SerializeField] private ParticleSystem m_namiausParticles;
    [SerializeField] private List<TextMeshProUGUI> m_textList;
    
    private int currentIndex = 0;
    //[SerializeField] private TextMeshProUGUI nextTextToshow;

    public bool hasDialogueEnded = false;
    private bool hasSkipped = true;
    private bool firstDialogue = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartCoroutine(C_EcrisTaMere());
        if (m_textList == null || m_textList.Count == 0) return;

        foreach (TextMeshProUGUI text in  m_textList)
        {
            text.enabled = false;
        }

        if (m_textList.Count > 0)
        {
            m_textList[0].enabled = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_canvas.enabled && Input.GetKeyDown(KeyCode.Space) || m_canvas.enabled && Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            //hasSkipped = true;
            Debug.Log("input");
            StartNextText();
        }

        if (hasDialogueEnded)
        {
            Instantiate(m_namiausParticles, m_Namiaus.transform.position, Quaternion.identity);
            m_canvas.enabled = false;            
            m_Namiaus.SetActive(false);
            Destroy(this);
        }

        //if (hasDialogueEnded)
        //{
        //    hasDialogueEnded = false;
        //    StartCoroutine(C_EcrisTaMere());
        //}
    }

    private void StartNextText()
    {
        Debug.Log("next"); 
        m_textList[currentIndex].enabled = false;
        currentIndex++;

        if (currentIndex < m_textList.Count)
        {
            m_textList[currentIndex].enabled = true;
        }

        else
        {
            hasDialogueEnded = true;
        }

        //for (int i = 0; i < m_textList.Count; i++)
        //{
        //    Debug.Log("for");
            
        //    while (!hasSkipped)
        //    {
        //        yield return null;
        //    }

        //    //hasSkipped = false;         
            
        //}

        //hasDialogueEnded = true;
        //Debug.Log("fin coroutine");
    }
}
