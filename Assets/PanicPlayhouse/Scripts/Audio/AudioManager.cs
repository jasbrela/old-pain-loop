using System.Collections;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using PanicPlayhouse.Scripts.UI;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace PanicPlayhouse.Scripts.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private SceneLoader loader;
        
        private Bus _music;
        private Bus _sfx;
        private Bus _master;
        
        private void Awake()
        {
            if (FindObjectOfType<AudioManager>() != this)
            {
                Destroy(this);
                return;
            }
            
            DontDestroyOnLoad(this);
        }
        
        void Start()
        {
            _master = RuntimeManager.GetBus("bus:/");
            _music = RuntimeManager.GetBus("bus:/Music");
            _sfx = RuntimeManager.GetBus("bus:/SFX");
            _master = RuntimeManager.GetBus("bus:/UI");
            _master = RuntimeManager.GetBus("bus:/Ambient");

            StartCoroutine(CheckIfAllBanksLoaded());
        }

        private IEnumerator CheckIfAllBanksLoaded()
        {
#if UNITY_EDITOR
            Debug.Log("Loading banks...");
#endif
            while (!RuntimeManager.HaveAllBanksLoaded) yield return null;
#if UNITY_EDITOR
            Debug.Log("Banks Loaded");
#endif
            loader.LoadNextScene();
        }

        public void PlayOneShot(EventReference reference, Vector3 pos)
        {
            RuntimeManager.PlayOneShot(reference, pos);
        }
    
        public void PlayOneShot(EventReference reference)
        {
            RuntimeManager.PlayOneShot(reference);
        }

        public void PlayAudioInLoop(ref EventInstance instance, EventReference reference, Rigidbody attachTo = null) {
            PLAYBACK_STATE state = PLAYBACK_STATE.STOPPED;

            if (instance.isValid())
                instance.getPlaybackState(out state);
        
            if (state != PLAYBACK_STATE.STOPPED) return;

            instance = RuntimeManager.CreateInstance(reference);
            
            if (attachTo != null)
            {
                instance.set3DAttributes(attachTo.position.To3DAttributes());
                RuntimeManager.AttachInstanceToGameObject(instance, attachTo.transform, attachTo);
            }

            instance.start();
        }
        
        public void StopAudioInLoop(EventInstance instance, FMOD.Studio.STOP_MODE mode = FMOD.Studio.STOP_MODE.IMMEDIATE)
        {
            if (!instance.isValid()) return;
        
            instance.getPlaybackState(out PLAYBACK_STATE state);
        
            if (state != PLAYBACK_STATE.PLAYING) return;
        
            instance.stop(mode);
            instance.release();
        }
        
        public void SetMasterVolume(float volume)
        {
            _master.setVolume(volume);
        }
    
        public void SetMusicVolume(float volume)
        {
            _music.setVolume(volume);
        }
    
        public void SetSfxVolume(float volume)
        {
            _sfx.setVolume(volume);
        }
        
        public void SetAmbientVolume(float volume)
        {
            _music.setVolume(volume);
        }
    
        public void SetUIVolume(float volume)
        {
            _sfx.setVolume(volume);
        }
    }
}
