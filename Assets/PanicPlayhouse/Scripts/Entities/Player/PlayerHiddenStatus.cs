using FMOD.Studio;
using FMODUnity;
using PanicPlayhouse.Scripts.Audio;
using PanicPlayhouse.Scripts.ScriptableObjects;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Entities.Player
{
    public class PlayerHiddenStatus : MonoBehaviour
    {
        [SerializeField] private EventReference tired;
        [SerializeField] private EventReference exhausted;
        [SerializeField] private EventReference hide;
        [SerializeField] private EventReference leave;
        [SerializeField] private float percentageToExhausted;
        [SerializeField] private float insanityReward;
        [SerializeField] private FloatVariable insanity;
        public bool IsHidden { get; private set; }
        private AudioManager _audio;
        
        private EventInstance _tiredInstance;
        private EventInstance _exhaustedInstance;

        private void Start()
        {
            _audio = FindObjectOfType<AudioManager>();
        }

        private void Update()
        {
            if (IsHidden)
                insanity.Value -= insanityReward * Time.deltaTime;
        }

        public void ChangePlayerHiddenStatus(bool value)
        {
            IsHidden = value;

            if (IsHidden)
            {
                if (insanity.Value > insanity.MaxValue * percentageToExhausted / 100)
                {
                    _audio.PlayOneShot(hide);
                    _audio.PlayAudioInLoop(ref _exhaustedInstance, exhausted);
                }
                else
                {
                    _audio.PlayOneShot(leave);
                    _audio.PlayAudioInLoop(ref _tiredInstance, tired);
                }
            }
            else
            {
                _audio.StopAudioInLoop(_exhaustedInstance);
                _audio.StopAudioInLoop(_tiredInstance);
            }
        }
    }
}