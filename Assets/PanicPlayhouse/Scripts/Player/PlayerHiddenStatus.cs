using System;
using PanicPlayhouse.Scripts.ScriptableObjects;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Player
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
        private float defaultVolume;

        private void Start()
        {
            defaultVolume = source.volume;
            source.loop = true;
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
                source.volume = defaultVolume;
            }
        }
    }
}