using UnityEngine;
using FMODUnity;
using FMOD.Studio;

namespace PanicPlayhouse.Scripts.Puzzles.MusicBox
{
    public class MusicBox : Pickupable
    {
        [Header("MusicBox: SFX")]
        [SerializeField] private float volumeReduceOnPickup;

        [Header("MusicBox: SFX")]
        [SerializeField] private EventReference musicBoxEventReference;
        [SerializeField] private EventInstance musicBoxEventInstance;

        private float originalMusicBoxVolume;

        protected override void Awake()
        {
            base.Awake();
        }

        public void ToggleMusicOn(bool isOn)
        {
            if (isOn)
            {
                Audio?.PlayAudioInLoop(ref musicBoxEventInstance, musicBoxEventReference, _rigidbody);
                // musicBoxEventInstance.getVolume(out originalMusicBoxVolume);
            }
            else
                Audio?.StopAudioInLoop(musicBoxEventInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        public override void OnInteract()
        {
            base.OnInteract();

            // if (PickedUp)
            // {
            //     musicBoxEventInstance.setVolume(originalMusicBoxVolume - volumeReduceOnPickup);
            // }
            // else
            // {
            //     musicBoxEventInstance.setVolume(originalMusicBoxVolume);
            // }
        }
    }
}