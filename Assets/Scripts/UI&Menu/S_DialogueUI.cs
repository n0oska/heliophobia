using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class S_DialogueUI : MonoBehaviour
{
    [SerializeField] private Canvas m_canvas;
    [SerializeField] private Image m_image;
    [SerializeField] private TextMeshProUGUI m_textToShow;
    [SerializeField] private List<TextMeshProUGUI> m_textList;
    

    //[SerializeField] private TextMeshProUGUI nextTextToshow;

    private bool hasDialogueEnded = false;
    private bool hasSkipped = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(C_EcrisTaMere());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            hasSkipped = true;
        }

        if (hasDialogueEnded)
        {
            StartCoroutine(C_EcrisTaMere());
        }
    }

    private IEnumerator C_EcrisTaMere()
    {
        hasSkipped = false;
        for (int i = 0; i < m_textList.Count;)
        {
            if (i == m_textList.Count)
            {
                m_textToShow = m_textList.ElementAt(m_textList.Count);
            }

            // if (hasDialogueEnded)
            // {
                
            // }

            if (hasSkipped)
            {
                hasDialogueEnded = true;
                i++;
                yield return new WaitForEndOfFrame();
            }
            
        }
    }
}
