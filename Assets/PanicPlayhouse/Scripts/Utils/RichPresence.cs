using System;
using PanicPlayhouse.Scripts.UI;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Utils
{
    public class RichPresence : MonoBehaviour
    {
        private const long AppId = 1107520580774805534;
        [SerializeField] private Boot boot;
#if !UNITY_WEBGL
        private string _details = "Loading...";
        private string _state = "\"Why is it taking so long?\"";

        public string Details
        {
            get => _details;
            set
            {
#if UNITY_EDITOR
                Debug.Log("Discord Rich Presence: ".Bold() + "Changed the details");
#endif
                _details = value;
                UpdateStatus();
            }
        }

        public string State
        {
            get => _state;
            set
            {
#if UNITY_EDITOR
                Debug.Log("Discord Rich Presence: ".Bold() + "Changed the state");
#endif
                _state = value;
                UpdateStatus();
            }
        }

        private Discord.Discord _discord;

        private long _time;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        void Start()
        {
            try
            {
                _discord = new Discord.Discord(AppId, (UInt64)Discord.CreateFlags.NoRequireDiscord);
            }
            catch (Exception e)
            {
                Debug.LogWarning("Discord Rich Presence:".Bold() + " Fail".Bold().Color("#FF4500") + "\nMessage: ".Bold() + e.Message, this);
                boot.Ready(gameObject, true);
                Destroy(gameObject);
                return;
            }

            _time = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            UpdateStatus();

            boot.Ready(gameObject);
        }

        void Update()
        {
            if (_discord == null) return;

            try
            {
                _discord.RunCallbacks();
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Discord Rich Presence:".Bold() + " Fail".Bold().Color("#FF4500") + "\nMessage: ".Bold() + e.Message, this);
#endif
                Destroy(gameObject);
            }
        }

        void UpdateStatus()
        {
            try
            {
                var activityManager = _discord.GetActivityManager();

                var activity = new Discord.Activity
                {
                    Details = _details,
                    State = _state,
                    Assets =
                    {
                        LargeImage = "logo"
                    },
                    Timestamps =
                    {
                        Start = _time
                    }
                };

                activityManager.UpdateActivity(activity, res =>
                {
#if UNITY_EDITOR
                    if (res != Discord.Result.Ok) Debug.LogWarning("Discord Rich Presence:".Bold() + " Fail".Bold().Color("#FF4500") + "\nResult: ".Bold() + res, this);

#endif
                });
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Discord Rich Presence:".Bold() + " Fail".Bold().Color("#FF4500") + "\nMessage: ".Bold() + e.Message, this);
#endif
                Destroy(gameObject);
            }
        }
#else
		private void Start()
		{
			boot.Ready(gameObject, true);
		}
#endif
    }
}
