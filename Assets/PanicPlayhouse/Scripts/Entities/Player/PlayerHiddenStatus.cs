using System.Collections;
using FMOD.Studio;
using FMODUnity;
using NaughtyAttributes;
using PanicPlayhouse.Scripts.Audio;
using PanicPlayhouse.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;
using Event = PanicPlayhouse.Scripts.ScriptableObjects.Event;

namespace PanicPlayhouse.Scripts.Entities.Player
{
    public class PlayerHiddenStatus : MonoBehaviour
    {
        [Header("SFX")]
        [SerializeField] private EventReference tired;
        [SerializeField] private EventReference exhausted;
        [SerializeField] private EventReference hide;
        [SerializeField] private EventReference leave;

        [Header("Hideout")]
        [SerializeField] private PlayerInput input;
        [SerializeField] private Event onLeaveHideout;
        [SerializeField] private float minDelayToLeave;
        [SerializeField][ReadOnly] private bool isHidden;

        [Header("Insanity")]
        [SerializeField] private float percentageToExhausted;
        [SerializeField] private float insanityReward;
        [SerializeField] private FloatVariable insanity;

        public bool IsHidden
        {
            get => isHidden;
            private set => isHidden = value;
        }

        private AudioManager _audio;
        private AudioManager Audio
        {
            get
            {
                if (_audio == null)
                    _audio = FindObjectOfType<AudioManager>();

                return _audio;
            }
            set
            {
                _audio = value;
            }
        }
        private bool _canLeave;

        private EventInstance _tiredInstance;
        private EventInstance _exhaustedInstance;

        private void Start()
        {
            Audio = FindObjectOfType<AudioManager>();
            SetUpControls();
        }

        private void SetUpControls()
        {
            input.actions["ExitHideout"].performed += LeaveHideout;
        }

        private void OnDisable()
        {
            input.actions["ExitHideout"].performed -= LeaveHideout;
        }

        private void LeaveHideout(InputAction.CallbackContext ctx)
        {
            if (!IsHidden || !_canLeave) return;
            if (onLeaveHideout != null) onLeaveHideout.Raise();
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
                _canLeave = false;
                StartCoroutine(AllowToLeave());

                if (insanity.Value > insanity.MaxValue * percentageToExhausted / 100)
                {
                    Audio?.PlayOneShot(hide);
                    Audio?.PlayAudioInLoop(ref _exhaustedInstance, exhausted);
                }
                else
                {
                    Audio?.PlayOneShot(hide);
                    Audio?.PlayAudioInLoop(ref _tiredInstance, tired);
                }
            }
            else
            {
                Audio?.PlayOneShot(leave);
                StopBreathingAudios();
            }
        }

        public void StopBreathingAudios()
        {
            isHidden = false;
            Audio?.StopAudioInLoop(_exhaustedInstance);
            Audio?.StopAudioInLoop(_tiredInstance);
        }

        private IEnumerator AllowToLeave()
        {
            yield return new WaitForSeconds(minDelayToLeave);
            _canLeave = true;
        }
    }
}