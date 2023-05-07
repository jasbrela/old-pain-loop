using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using Event = PanicPlayhouse.Scripts.ScriptableObjects.Event;

namespace PanicPlayhouse.Scripts.Interfaces
{
    public class Listener : MonoBehaviour
    {
        [SerializeField] private Event @event;
        [HideIf("IsEventNull")] [SerializeField] private UnityEvent response;

        private bool IsEventNull => @event == null;

        protected void OnEnable()
        {
            @event.RegisterListener(this);
        }

        protected void OnDisable()
        {
            @event.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            response.Invoke();
        }
    }
}