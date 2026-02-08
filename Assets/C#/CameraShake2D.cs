using UnityEngine;

public class CameraShake2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform cameraTransform;

    [Header("Preset Shakes")]
    public float moderateIntensity = 0.15f;
    public float moderateDuration  = 0.25f;

    public float extremeIntensity  = 0.4f;
    public float extremeDuration   = 0.45f;

    [Header("Light Shake Accumulation")]
    public float lightShakeStep = 0.08f;
    public float lightShakeMax  = 1.0f;
    public float lightShakeDecayPerSecond = 0.6f;
    public float lightShakeExponent = 2f;


    [Header("Shake Frequency")]
    public float shakeFrequency = 25f;

    Vector3 _originalPos;

    float _shakeTimer;
    float _shakeDuration;
    float _shakeIntensity;

    float _accumulatedShake;
    float _accumulatedTimer;
    
    float _lightShakeTrauma;

    void Awake()
    {
        if (cameraTransform == null)
            cameraTransform = transform;
    }

    void LateUpdate()
    {
        _originalPos = cameraTransform.localPosition;
        
        float dt = Time.unscaledDeltaTime;

        Vector3 shakeOffset = Vector3.zero;

        // ----- Preset / immediate shake -----
        if (_shakeTimer > 0f)
        {
            _shakeTimer -= dt;

            float t = _shakeTimer / _shakeDuration;
            float strength = _shakeIntensity * t;

            shakeOffset += (Vector3)Random.insideUnitCircle * strength;
        }

        // ----- Continuous accumulated shake -----
        _lightShakeTrauma = Mathf.Max(
            0f,
            _lightShakeTrauma - lightShakeDecayPerSecond * dt
        );

        if (_lightShakeTrauma > 0f)
        {
            float strength = Mathf.Pow(_lightShakeTrauma, lightShakeExponent);
            shakeOffset += (Vector3)Random.insideUnitCircle * strength;
        }

        cameraTransform.localPosition = _originalPos + shakeOffset;
    }

    // ================= PUBLIC API =================

    public void PlayModerate()
    {
        PlayShake(moderateIntensity, moderateDuration);
    }

    public void PlayExtreme()
    {
        PlayShake(extremeIntensity, extremeDuration);
    }

    public void AddLightShake()
    {
        _lightShakeTrauma =
            Mathf.Clamp(_lightShakeTrauma + lightShakeStep, 0f, lightShakeMax);
    }


    // ================= INTERNAL =================

    void PlayShake(float intensity, float duration)
    {
        _shakeIntensity = Mathf.Max(_shakeIntensity, intensity);
        _shakeDuration  = duration;
        _shakeTimer     = duration;
    }
}
