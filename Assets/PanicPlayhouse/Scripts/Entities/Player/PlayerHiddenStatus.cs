using PanicPlayhouse.Scripts.ScriptableObjects;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Entities.Player
{
    public class PlayerHiddenStatus : MonoBehaviour
    {
        [SerializeField] private AudioSource source;
        [SerializeField] private AudioClip tiredAudio;
        [SerializeField] private AudioClip exhaustedAudio;
        [Range(0, 1)][SerializeField] private float volume;
        [SerializeField] private float percentageToExhausted;
        [SerializeField] private float insanityReward;
        [SerializeField] private FloatVariable insanity;
        public bool IsHidden { get; private set; }
        private float _defaultVolume;
        private bool _defaultLoop;

        private void Start()
        {
            _defaultLoop = source.loop;
            _defaultVolume = source.volume;
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
                source.volume = volume;
                source.loop = true;
                
                if (insanity.Value > insanity.MaxValue * percentageToExhausted / 100)
                {
                    source.clip = exhaustedAudio;
                    source.Play();
                }
                else
                {
                    source.clip = tiredAudio;
                    source.Play();
                }
            }
            else
            {
                source.Stop();
                source.loop = _defaultLoop;
                source.volume = _defaultVolume;
            }
        }
    }
}