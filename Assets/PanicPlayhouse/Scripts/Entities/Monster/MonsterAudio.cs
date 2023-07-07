using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using PanicPlayhouse.Scripts.Audio;
using FMOD.Studio;
using FMODUnity;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace PanicPlayhouse.Scripts.Entities.Monster
{
    public static class MonsterAudioConstants
    {
        public static string FOOTSTEPS_AUDIO_KEY = "footsteps";
        public static string KNOCK_AUDIO_KEY = "knock";
        public static string ATTACK_AUDIO_KEY = "attack";
        public static string BREATH_AUDIO_KEY = "breath";
        public static string HEARTBEAT_AUDIO_KEY = "heartbeat";
        public static string CHASING_AUDIO_KEY = "chasing";
    }

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
        [SerializeField] private GenericDictionary<string, FMODAudioEventReference> eventReferences = new GenericDictionary<string, FMODAudioEventReference>();

        [Header("Dependencies")]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private NavMeshAgent agent;

        private Dictionary<string, EventInstance> eventInstances = new Dictionary<string, EventInstance>();

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

        void Awake()
        {
            foreach (string eventReferenceKey in eventReferences.Keys)
            {
                eventInstances.Add(eventReferenceKey, new EventInstance());
            }
        }

        void Start()
        {
            PlayAudioInLoop(MonsterAudioConstants.BREATH_AUDIO_KEY, true);
        }

        public EventInstance GetEventInstance(string audioKey)
        {
            if (!eventInstances.TryGetValue(audioKey, out EventInstance eventInstance))
            {
#if UNITY_EDITOR
                Debug.LogWarning("MonsterAudio: ".Bold() + $"No event instance for audio \"{audioKey}\"");
#endif
            }

            return eventInstance;
        }

        public void StopAudiosInLoop()
        {
            foreach (KeyValuePair<string, EventInstance> eventInstanceKvp in eventInstances)
            {
                STOP_MODE stopMode = eventReferences.TryGetValue(eventInstanceKvp.Key, out FMODAudioEventReference eventReference) ? eventReference.stopMode : STOP_MODE.IMMEDIATE;
                audioManager?.StopAudioInLoop(eventInstanceKvp.Value, stopMode);
            }
        }

        public void StopAudioInLoop(string audioKey)
        {
            if (!eventInstances.TryGetValue(audioKey, out EventInstance eventInstance))
                return;

            audioManager?.StopAudioInLoop(eventInstance);
        }

        public void PlayOneShot(string audioKey)
        {
            if (!eventReferences.TryGetValue(audioKey, out FMODAudioEventReference audioReference))
                return;

            audioManager?.PlayOneShot(audioReference.eventReference);
        }

        public void PlayAudioInLoop(string audioKey, bool useRb = false)
        {
            if (!eventReferences.TryGetValue(audioKey, out FMODAudioEventReference audioReference))
            {
#if UNITY_EDITOR
                Debug.LogWarning("MonsterAudio: ".Bold() + $"No event reference for audio \"{audioKey}\"");
#endif
                return;
            }

            if (!eventInstances.TryGetValue(audioKey, out EventInstance eventInstance))
            {
#if UNITY_EDITOR
                Debug.LogWarning("MonsterAudio: ".Bold() + $"No event instance for audio \"{audioKey}\"");
#endif
                return;
            }
            eventInstance.getPlaybackState(out PLAYBACK_STATE state);
            if (state != PLAYBACK_STATE.STOPPED)
            {
#if UNITY_EDITOR
                Debug.LogWarning("MonsterAudio: ".Bold() + $"Audio \"{audioKey}\" is already playing!");
#endif
                return;
            }

            audioManager?.PlayAudioInLoop(ref eventInstance, audioReference.eventReference, useRb ? rb : null);
        }
    }
}