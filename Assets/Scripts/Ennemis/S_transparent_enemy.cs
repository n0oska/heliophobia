using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class S_transparent_enemy : MonoBehaviour
{
    [Range(0f, 1f)]
    public float O_alpha = 0.5f;
    public bool O_disableShadows = true;

    [SerializeField] float timer = 0.3f;

    SpriteRenderer O_spriteRenderer;
    Color m_color;
    S_EnemyController m_ennemyScript;

    void Start()
    {
        O_spriteRenderer = GetComponent<SpriteRenderer>();
        
        m_ennemyScript = this.GetComponent<S_EnemyController>();

        //m_color = O_spriteRenderer.color;

        // ApplyTransparency();
        //DisableShadows();
    }

    void Update()
    {
        if (m_ennemyScript == null)
            return;

        if (m_ennemyScript.m_health.isTakingDamage)
        {
            var newColor = Color.darkRed;
            O_spriteRenderer.color = Color.darkRed;
            StartTimer();
        }

        if (!m_ennemyScript.m_health.isTakingDamage && timer < 0)
        {
            Debug.Log("ziz");
            O_spriteRenderer.color = Color.white;
            timer = 0.3f;
        }
        
        Debug.Log(timer);
        
    }
    
    private void StartTimer()
    {
        while (timer > 0)
        {
            timer -=Time.deltaTime;
        }
    }

    // void ApplyTransparency()
    // {
    //     if (O_spriteRenderer != null)
    //     {
    //         Color c = O_spriteRenderer.color;
    //         c.a = Mathf.Clamp01(O_alpha);
    //         O_spriteRenderer.color = c;
    //     }
    //     else if (O_renderer != null && O_renderer.material.HasProperty("_Color"))
    //     {
    //         Color c = O_renderer.material.color;
    //         c.a = Mathf.Clamp01(O_alpha);
    //         O_renderer.material.color = c;
    //     }
    // }

    // void DisableShadows()
    // {
    //     if (!O_disableShadows) return;

    //     if (O_renderer != null)
    //     {
    //         O_renderer.shadowCastingMode = ShadowCastingMode.Off;
    //         O_renderer.receiveShadows = false;
    //     }

    //     Light[] lights = Light.FindObjectsOfType<Light>();
    //     foreach (Light light in lights)
    //     {
    //         if (light.shadows != LightShadows.None)
    //         {
    //             light.shadowBias = 1f;
    //             light.shadowNormalBias = 1f;
    //         }
    //     }
    // }
}
