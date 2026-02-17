using UnityEngine;

public class mDayNightCycle : MonoBehaviour
{
    [Header("Cycle Settings")]
    [SerializeField] private float mDayDuration = 60f;
    [SerializeField] private Gradient mLightColor;
    [SerializeField] private AnimationCurve mLightIntensityCurve;
    [SerializeField] private AnimationCurve m_timeCurve;

    [Header("Light Reference")]
    [SerializeField] private Light mDirectionalLight;

    private bool hasCycleEnded = false;

    private float mTimer = 0f;

    void Start()
    {
        if (mDirectionalLight == null)
            mDirectionalLight = GetComponent<Light>();

        ResetDayCycle();
    }

    void Update()
    {
        CreateCycle();
    }

    public void CreateCycle()
    {
        mTimer += Time.deltaTime;
        float linearT = Mathf.Clamp01(mTimer / mDayDuration);

        float curvedT = m_timeCurve.Evaluate(linearT);

        float angle = Mathf.Lerp(0f, 360f, curvedT);
        transform.rotation = Quaternion.Euler(angle, 0f, 0f);

        mDirectionalLight.color = mLightColor.Evaluate(curvedT);
        mDirectionalLight.intensity = mLightIntensityCurve.Evaluate(curvedT);

        if (mTimer >= mDayDuration)
        {
            hasCycleEnded = true;
            ResetDayCycle();
        }
    }

    public void ResetDayCycle()
    {
        if (hasCycleEnded)
        {
            mTimer = 0f;

            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            mDirectionalLight.color = mLightColor.Evaluate(0f);
            mDirectionalLight.intensity = mLightIntensityCurve.Evaluate(0f);
        }
        
    }
}
