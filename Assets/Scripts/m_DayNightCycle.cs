using UnityEngine;

public class mDayNightCycle : MonoBehaviour
{
    [Header("Cycle Settings")]
    [SerializeField] private float mDayDuration = 60f;
    [SerializeField] private Gradient mLightColor;
    [SerializeField] private AnimationCurve mLightIntensityCurve;

    [Header("Light Reference")]
    [SerializeField] private Light mDirectionalLight;

    private float mTimer = 0f;

    void Start()
    {
        if (mDirectionalLight == null)
            mDirectionalLight = GetComponent<Light>();

        ResetDayCycle();
    }

    void Update()
    {
        mTimer += Time.deltaTime;
        float t = Mathf.Clamp01(mTimer / mDayDuration);

        float angle = Mathf.Lerp(0f, 360f, t);
        transform.rotation = Quaternion.Euler(angle, 0f, 0f);

        mDirectionalLight.color = mLightColor.Evaluate(t);
        mDirectionalLight.intensity = mLightIntensityCurve.Evaluate(t);
    }

    public void ResetDayCycle()
    {
        mTimer = 0f;

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        mDirectionalLight.color = mLightColor.Evaluate(0f);
        mDirectionalLight.intensity = mLightIntensityCurve.Evaluate(0f);
    }
}
