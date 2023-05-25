using PanicPlayhouse.Scripts.Audio;
using UnityEngine;

namespace PanicPlayhouse.Scripts.UI
{
    public class AudioUtils : MonoBehaviour
    {
        private AudioManager _audio;
        private void Start()
        {
            _audio = FindObjectOfType<AudioManager>();
            
            if (_audio == null) Destroy(gameObject);
            
            SetMasterVolume(PlayerPrefs.GetFloat("master_volume", 1f));
            SetMusicVolume(PlayerPrefs.GetFloat("music_volume", 1f));
            SetSFXVolume(PlayerPrefs.GetFloat("sfx_volume", 1f));
            SetAmbientVolume(PlayerPrefs.GetFloat("ambient_volume", 1f));
            SetUIVolume(PlayerPrefs.GetFloat("ui_volume", 1f));
        }
        
        public void SetMasterVolume(float volume)
        {
            if (_audio != null) _audio.SetMasterVolume(volume);
            PlayerPrefs.SetFloat("master_volume", volume);
        }
    
        public void SetMusicVolume(float volume)
        {
            if (_audio != null) _audio.SetMusicVolume(volume);
            PlayerPrefs.SetFloat("music_volume", volume);
        }
    
        public void SetSFXVolume(float volume)
        {
            if (_audio != null) _audio.SetSFXVolume(volume);
            PlayerPrefs.SetFloat("sfx_volume", volume);
        }
        
        public void SetAmbientVolume(float volume)
        {
            if (_audio != null) _audio.SetAmbientVolume(volume);
            PlayerPrefs.SetFloat("ambient_volume", volume);
        }
    
        public void SetUIVolume(float volume)
        {
            if (_audio != null) _audio.SetUIVolume(volume);
            PlayerPrefs.SetFloat("ui_volume", volume);
        }
    }
}