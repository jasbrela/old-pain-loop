using System;
using UnityEngine;

namespace PanicPlayhouse.Scripts
{
	public class RichPresence : MonoBehaviour
	{
#if !UNITY_WEBGL
		
		[SerializeField] private long appId;
		[SerializeField] private string details = "In the menu";

		public string Details
		{
			get => details;
			set
			{
#if UNITY_EDITOR
				Debug.Log("Discord: ".Bold() + "Changed the details");
#endif
				details = value;
				UpdateStatus();
			}
		}

		private Discord.Discord _discord;

		private long _time;
		
		private void Awake()
		{
			DontDestroyOnLoad(this);
		}

		void Start () {
			_discord = new Discord.Discord(appId, (UInt64)Discord.CreateFlags.NoRequireDiscord);

#if UNITY_EDITOR
			_discord.GetActivityManager().ClearActivity(result => Debug.Log("ClearActivity: " + result));
#endif
			
			_time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
			
			UpdateStatus();
		}
    	
		void Update () {
			try
			{
				_discord.RunCallbacks();
			}
			catch
			{
#if UNITY_EDITOR
				Debug.LogWarning("Discord:" + " Fail".Bold().Color("#FF4500"));
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
					Details = details,
					Assets =
					{
						LargeImage = "logo"
					},
					Timestamps =
					{
						Start = _time
					}
				};

				activityManager.UpdateActivity(activity, (res) =>
				{
#if UNITY_EDITOR
					if (res != Discord.Result.Ok) Debug.LogWarning("Discord:" + " Fail".Bold().Color("#FF4500"));
#endif
				});
			}
			catch
			{
#if UNITY_EDITOR
				Debug.LogWarning("Discord:" + " Fail".Bold().Color("#FF4500"));
#endif
				Destroy(gameObject);
			}
		}
#endif
	}
}
