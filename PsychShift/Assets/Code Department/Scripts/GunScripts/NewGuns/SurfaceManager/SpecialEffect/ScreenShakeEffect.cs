using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace ImpactSystem.Effects
{
    [CreateAssetMenu(menuName = "Impact System/Abstracted Special Effect", fileName = "AbstractedSpecialEffect")]
    public class ScreenShakeEffect : AbstractSpecialEffect
    {
        public float Duration = 1f;
        public float Magnitude = 0.1f;
        public float Frequency = 25f;
        public override void PlaySpecialEffect(SpecialEffectManager manager)
        {
            // Implement the special effect logic here
            manager.StopCoroutine(PlayScreenShake(manager));
            manager.StartCoroutine(PlayScreenShake(manager));
        }

        private IEnumerator PlayScreenShake(SpecialEffectManager manager)
    {
        CinemachineVirtualCamera cam = manager.currentCamera;
        CinemachineBasicMultiChannelPerlin perlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        ShakeCamera(Magnitude, Frequency, Duration, cam);

        float decreaseRate = Magnitude / Duration;

        while (perlin.m_AmplitudeGain > 0)
        {
            perlin.m_AmplitudeGain -= decreaseRate * Time.deltaTime;
            yield return null;
        }
        perlin.m_AmplitudeGain = 0;
    }
        private void ShakeCamera(float intensity, float frequency, float duration, CinemachineVirtualCamera cam)
        {
            CinemachineBasicMultiChannelPerlin perlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            perlin.m_AmplitudeGain = intensity;
            perlin.m_FrequencyGain = frequency;
        }
        
    }
}

