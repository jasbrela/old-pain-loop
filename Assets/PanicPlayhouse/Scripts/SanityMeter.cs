using Cinemachine;
using NaughtyAttributes;
using UnityEngine;

namespace PanicPlayhouse.Scripts
{
    public class SanityMeter : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCam;
        [SerializeField] private float newValue;
        private CinemachineBasicMultiChannelPerlin _noise;
        private float _sanity;
        
        public float Sanity
        {
            get => _sanity;
            set
            {
                _sanity = value;
                if (_noise == null) _noise = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                _noise.m_AmplitudeGain = _sanity;
            }
        }


        void Start()
        {
            Sanity = 0;
            _noise = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        [Button]
        void ChangeValue()
        {
            Sanity = newValue;
        }
    }
    
}
