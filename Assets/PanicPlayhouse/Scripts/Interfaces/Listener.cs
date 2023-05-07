using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using Event = PanicPlayhouse.Scripts.ScriptableObjects.Event;

namespace PanicPlayhouse.Scripts.Interfaces
{
    public abstract class Listener : MonoBehaviour
    {
        [SerializeField] protected List<Event> events;
        [HideInInspector] [SerializeField] protected UnityEvent response;

        private bool IsEventNull => events == null;

        private void OnEnable()
        {
            foreach (Event @event in events)
            {
                @event.RegisterListener(this);
            }
        }

        private void OnDisable()
        {
            foreach (Event @event in events)
            {
                @event.UnregisterListener(this);
            }
        }

        public void OnEventRaised()
        {
            response.Invoke();
        }
    }
}