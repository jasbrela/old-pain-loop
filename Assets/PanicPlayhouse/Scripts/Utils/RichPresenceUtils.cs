using UnityEngine;

namespace PanicPlayhouse.Scripts.Utils
{
    public class RichPresenceUtils : MonoBehaviour
    {
#if !UNITY_WEBGL

        [SerializeField] private string sceneState;
        [SerializeField] private string sceneDetails;

        private RichPresence _presence;

        void Start()
        {
            _presence = FindObjectOfType<RichPresence>();
            if (_presence == null)
            {
                Destroy(gameObject);
                return;
            }

            _presence.State = sceneState;
            _presence.Details = sceneDetails;

        }

        public void SetDetails(string text)
        {
            _presence.Details = text;
        }
#endif
    }
}
