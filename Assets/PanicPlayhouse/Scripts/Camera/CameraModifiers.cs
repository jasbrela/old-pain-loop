using Cinemachine;
using NaughtyAttributes;
using PanicPlayhouse.Scripts.Interfaces;
using PanicPlayhouse.Scripts.ScriptableObjects;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Camera
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraModifiers : Listener
    {
        [Space(5)]
        [Header("Insanity")]
        [Label("Value")][SerializeField] private FloatVariable insanity;
        [SerializeField] private float multiplier = 0.1f;
        
        private CinemachineVirtualCamera _cam;
        private CinemachineBasicMultiChannelPerlin _noise;
        private void Start()
        {
            if (insanity == null) return;
            if (!TryGetComponent(out _cam)) return;
            
            response.AddListener(OnInsanityChange);

            _noise = _cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            _noise.m_AmplitudeGain = 0;
        }

        private void OnInsanityChange()
        {
            _noise.m_AmplitudeGain = insanity.Value * multiplier;
        }
    }
}
