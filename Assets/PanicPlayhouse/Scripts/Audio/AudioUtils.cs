using System;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Audio
{
    public class AudioUtils : MonoBehaviour
    {
        private AudioManager _audio;
        private void Start()
        {
            _audio = FindObjectOfType<AudioManager>();
        }
        
        public void SetMasterVolume(float volume)
        {
            if (_audio != null) _audio.SetMasterVolume(volume);
        }
    
        public void SetMusicVolume(float volume)
        {
            if (_audio != null) _audio.SetMusicVolume(volume);
        }
    
        public void SetSfxVolume(float volume)
        {
            if (_audio != null) _audio.SetSfxVolume(volume);
            
        }
        
        public void SetAmbientVolume(float volume)
        {
            if (_audio != null) _audio.SetAmbientVolume(volume);
        }
    
        public void SetUIVolume(float volume)
        {
            if (_audio != null) _audio.SetUIVolume(volume);
                
        }
    }
}