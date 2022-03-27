using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private CinemachineVirtualCamera _cinemachineVCAM;
    private float _shakeTimer;
    private float _startingIntensity;
    private float _shakeTimerTotal;

    private void Awake()
    {
        Instance = this;
        _cinemachineVCAM = GetComponent<CinemachineVirtualCamera>();
    }

    public void DashShake(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin =
            _cinemachineVCAM.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        _startingIntensity = intensity;
        _shakeTimer = time;
        _shakeTimerTotal = time;
    }

    private void Update()
    {
        if (_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime;
            if (_shakeTimer <= 0)
            {
                CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin =
            _cinemachineVCAM.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(_startingIntensity, 0f, 1 - (_shakeTimer / _shakeTimerTotal));
            }
        }
    }

}
