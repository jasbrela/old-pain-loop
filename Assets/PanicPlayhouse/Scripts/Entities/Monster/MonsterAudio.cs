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
        [SerializeField] private FMODAudioEventReference footstepsReference;
        [SerializeField] private FMODAudioEventReference knockReference;
        [SerializeField] private FMODAudioEventReference attackReference;
        [SerializeField] private FMODAudioEventReference breathReference;
        [SerializeField] private FMODAudioEventReference heartbeatReference;
        [SerializeField] private FMODAudioEventReference chasingReference;

        [Header("Dependencies")]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private NavMeshAgent agent;


        private EventInstance _footstepsInstance;
        public EventInstance footstepsInstance
        {
            get
            {
                return _footstepsInstance;
            }
        }

        private EventInstance _knockInstance;
        public EventInstance knockInstance
        {
            get
            {
                return _knockInstance;
            }
        }

        private EventInstance _attackInstance;
        public EventInstance attackInstance
        {
            get
            {
                return _attackInstance;
            }
        }

        private EventInstance _breathInstance;
        public EventInstance breathInstance
        {
            get
            {
                return _breathInstance;
            }
        }

        private EventInstance _heartbeatInstance;
        public EventInstance heatbeatInstance
        {
            get
            {
                return _heartbeatInstance;
            }
        }

        private EventInstance _chasingInstance;
        public EventInstance chasingInstance
        {
            get
            {
                return _chasingInstance;
            }
        }


        private AudioManager _audioManager;
        private AudioManager audioManager
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
            audioManager.PlayOneShot(knockReference.eventReference);
        }

        public void PlayAttackOneShot()
        {
            audioManager.PlayOneShot(attackReference.eventReference);
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

        public void ToggleFootstepsOn(bool on)
        {
            ToggleLoopAudio(
                on,
                ref _footstepsInstance,
                footstepsReference.eventReference,
                footstepsReference.stopMode,
                true
            );
        }

        void ToggleLoopAudio(bool on, ref EventInstance eventInstance, EventReference reference, STOP_MODE stopMode, bool attachRb = false)
        {
            if (on)
            {
                if (attachRb)
                    audioManager.PlayAudioInLoop(ref eventInstance, reference, rb);
                else
                    audioManager.PlayAudioInLoop(ref eventInstance, reference);
            }
            else
                audioManager.StopAudioInLoop(eventInstance, stopMode);
        }

        public void StopAudiosInLoop()
        {
            audioManager.StopAudioInLoop(_footstepsInstance, footstepsReference.stopMode);
            audioManager.StopAudioInLoop(_knockInstance, knockReference.stopMode);
            audioManager.StopAudioInLoop(_attackInstance, attackReference.stopMode);
            audioManager.StopAudioInLoop(_breathInstance, breathReference.stopMode);
            audioManager.StopAudioInLoop(_heartbeatInstance, heartbeatReference.stopMode);
            audioManager.StopAudioInLoop(_chasingInstance, chasingReference.stopMode);
        }
    }
}