using System.Collections.Generic;
using FMODUnity;
using PanicPlayhouse.Scripts.Audio;
using PanicPlayhouse.Scripts.ScriptableObjects;
using UnityEngine;
using Event = PanicPlayhouse.Scripts.ScriptableObjects.Event;

namespace PanicPlayhouse.Scripts.Puzzles.MusicBox
{
    public class MusicBoxPuzzle : MonoBehaviour
    {
        [Header("Puzzle")]
        [SerializeField] private Event onFinish;

        [Header("Insanity")]
        [SerializeField] private float insanityPenalty;
        [SerializeField] private float insanityReward;
        [SerializeField] private FloatVariable insanity;

        [Header("SFX")]
        [SerializeField] private EventReference success;
        [SerializeField] private EventReference fit;

        private AudioManager _audio;
        private MusicBoxBase _base;
        private MusicBox _musicBox;

        private bool IsActivated { get; set; }
        private bool IsFinished { get; set; }

        private void Start()
        {
            _audio = FindObjectOfType<AudioManager>();
            
            ActivatePuzzle();
        }

        public void ActivatePuzzle()
        {
            if (IsActivated || IsFinished) return;

            IsActivated = true;

#if UNITY_EDITOR
            Debug.Log(name.Bold().Color("#00FA9A") + " has been activated.");
#endif
        }

        public void OnPressBase()
        {
            _audio.PlayOneShot(fit);

            IsFinished = true;
            insanity.Value -= insanityReward;
            _audio.PlayOneShot(success);
            if (onFinish != null) onFinish.Raise();
            _musicBox.IsBlocked = true;
            insanity.Value += insanityPenalty;
        }

        public void OnReleaseBase()
        {
            insanity.Value += insanityPenalty;
        }
    }
}
