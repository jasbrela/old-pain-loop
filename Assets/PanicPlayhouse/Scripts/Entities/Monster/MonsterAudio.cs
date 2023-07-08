using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using PanicPlayhouse.Scripts.Audio;
using FMOD.Studio;
using FMODUnity;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace PanicPlayhouse.Scripts.Entities.Monster
{
    [System.Serializable]
    public class FMODAudioEventReference
    {
        public EventReference eventReference;
        public STOP_MODE stopMode;

        public FMODAudioEventReference(EventReference eventReference, STOP_MODE stopMode)
        {
            this.eventReference = eventReference;
            this.stopMode = stopMode;
        }

        public FMODAudioEventReference(EventReference eventReference)
        {
            this.eventReference = eventReference;
            this.stopMode = STOP_MODE.IMMEDIATE;
        }
    }
    public class MonsterAudio : MonoBehaviour
    {
        [Header("FMOD")]
        [SerializeField] private FMODAudioEventReference knockReference;
        [SerializeField] private FMODAudioEventReference attackReference;
        [SerializeField] private FMODAudioEventReference breathReference;
        [SerializeField] private FMODAudioEventReference heartbeatReference;
        [SerializeField] private FMODAudioEventReference chasingReference;

        [Header("Dependencies")]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private NavMeshAgent agent;

        private EventInstance _knockInstance;
        public EventInstance KnockInstance => _knockInstance;

        private EventInstance _attackInstance;
        public EventInstance AttackInstance => _attackInstance;

        private EventInstance _breathInstance;
        public EventInstance BreathInstance => _breathInstance;

        private EventInstance _heartbeatInstance;
        public EventInstance HeatbeatInstance => _heartbeatInstance;

        private EventInstance _chasingInstance;
        public EventInstance ChasingInstance => _chasingInstance;


        private AudioManager _audioManager;
        private AudioManager AudioManager
        {
            get
            {
                if (_audioManager == null)
                    _audioManager = FindObjectOfType<AudioManager>();

                return _audioManager;
            }
        }

        void Start()
        {
            ToggleBreathOn(true);
        }

        public void PlayKnockOneShot()
        {
            AudioManager?.PlayOneShot(knockReference.eventReference);
        }

        public void PlayAttackOneShot()
        {
            AudioManager?.PlayOneShot(attackReference.eventReference);
        }

        public void ToggleChaseAudiosOn(bool on)
        {
            ToggleLoopAudio(
                on,
                ref _chasingInstance,
                chasingReference.eventReference,
                chasingReference.stopMode
            );

            ToggleLoopAudio(
                on,
                ref _heartbeatInstance,
                heartbeatReference.eventReference,
                heartbeatReference.stopMode
            );
        }

        public void ToggleBreathOn(bool on)
        {
            ToggleLoopAudio(
                on,
                ref _breathInstance,
                breathReference.eventReference,
                breathReference.stopMode,
                true
            );
        }

        void ToggleLoopAudio(bool on, ref EventInstance eventInstance, EventReference reference, STOP_MODE stopMode, bool attachRb = false)
        {
            if (on)
            {
                if (attachRb)
                    AudioManager?.PlayAudioInLoop(ref eventInstance, reference, rb);
                else
                    AudioManager?.PlayAudioInLoop(ref eventInstance, reference);
            }
            else
                AudioManager?.StopAudioInLoop(eventInstance, stopMode);
        }

        public void StopAudiosInLoop()
        {
            AudioManager?.StopAudioInLoop(_knockInstance, knockReference.stopMode);
            AudioManager?.StopAudioInLoop(_attackInstance, attackReference.stopMode);
            AudioManager?.StopAudioInLoop(_breathInstance, breathReference.stopMode);
            AudioManager?.StopAudioInLoop(_heartbeatInstance, heartbeatReference.stopMode);
            AudioManager?.StopAudioInLoop(_chasingInstance, chasingReference.stopMode);
        }
    }
}