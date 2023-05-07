using Cinemachine;
using NaughtyAttributes;
using PanicPlayhouse.Scripts.Interfaces;
using PanicPlayhouse.Scripts.ScriptableObjects;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Camera
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraInsanity : MonoBehaviour
    {
        [SerializeField] private FloatVariable insanity;
        [SerializeField] private float multiplier = 0.1f;
        
        private CinemachineVirtualCamera _cam;
        private CinemachineBasicMultiChannelPerlin _noise;
        private void Start()
        {
            if (insanity == null) return;
            if (!TryGetComponent(out _cam)) return;
            
            _noise = _cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            _noise.m_AmplitudeGain = 0;
        }

        public void OnInsanityChange()
        {
            _noise.m_AmplitudeGain = insanity.Value * multiplier;
        }
    }
}
