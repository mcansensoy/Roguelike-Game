using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineCam;

    private void Awake()
    {
        cinemachineCam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float duration, float amplitude, float frequency)
    {
        StartCoroutine(ScreenShake(duration, amplitude, frequency));
    }

    private IEnumerator ScreenShake(float duration, float amplitude, float frequency)
    {
        var noise = cinemachineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        // Set noise values
        noise.m_AmplitudeGain = amplitude;
        noise.m_FrequencyGain = frequency;

        // Wait for the shake duration
        yield return new WaitForSeconds(duration);

        // Reset noise values
        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
    }
}
