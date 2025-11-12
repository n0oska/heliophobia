using UnityEngine;
using UnityEngine.Rendering;

public class S_transparent_enemy : MonoBehaviour
{
    [Range(0f, 1f)]
    public float O_alpha = 0.5f;
    public bool O_disableShadows = true;

    SpriteRenderer O_spriteRenderer;
    Renderer O_renderer;

    void Start()
    {
        O_spriteRenderer = GetComponent<SpriteRenderer>();
        O_renderer = GetComponent<Renderer>();

        ApplyTransparency();
        DisableShadows();
    }

    void Update()
    {
    }

    void ApplyTransparency()
    {
        if (O_spriteRenderer != null)
        {
            Color c = O_spriteRenderer.color;
            c.a = Mathf.Clamp01(O_alpha);
            O_spriteRenderer.color = c;
        }
        else if (O_renderer != null && O_renderer.material.HasProperty("_Color"))
        {
            Color c = O_renderer.material.color;
            c.a = Mathf.Clamp01(O_alpha);
            O_renderer.material.color = c;
        }
    }

    void DisableShadows()
    {
        if (!O_disableShadows) return;

        if (O_renderer != null)
        {
            O_renderer.shadowCastingMode = ShadowCastingMode.Off;
            O_renderer.receiveShadows = false;
        }

        Light[] lights = FindObjectsOfType<Light>();
        foreach (Light light in lights)
        {
            if (light.shadows != LightShadows.None)
            {
                light.shadowBias = 1f;
                light.shadowNormalBias = 1f;
            }
        }
    }
}
