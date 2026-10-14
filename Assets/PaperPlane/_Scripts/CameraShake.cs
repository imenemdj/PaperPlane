using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    [Header("Shake Settings")]
    [SerializeField] private float shakeIntensity = 0.05f;
    [SerializeField] private float shakeFrequency = 25f;

    private Vector3 originalPosition;
    private bool isShaking = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        if (isShaking && Player.Instance != null && Player.Instance.isAlive)
        {
            float offsetX = Mathf.PerlinNoise(Time.time * shakeFrequency, 0f) * 2f - 1f;
            float offsetY = Mathf.PerlinNoise(0f, Time.time * shakeFrequency) * 2f - 1f;

            transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0f) * shakeIntensity;
        }
        else if (!isShaking)
        {
            transform.localPosition = originalPosition;
        }
    }

    public void StartShake()
    {
        isShaking = true;
    }

    public void StopShake()
    {
        isShaking = false;
        transform.localPosition = originalPosition;
    }
}
