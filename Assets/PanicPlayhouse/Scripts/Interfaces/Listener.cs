using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using Event = PanicPlayhouse.Scripts.ScriptableObjects.Event;

namespace PanicPlayhouse.Scripts.Interfaces
{
    public abstract class Listener : MonoBehaviour
    {
        [SerializeField] protected Event @event;
        [HideInInspector] [SerializeField] protected UnityEvent response;

        //private bool IsEventNull => @event == null;

        protected virtual void OnEnable()
        {
            @event.RegisterListener(this);
        }

        protected virtual void OnDisable()
        {
            @event.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            response.Invoke();
        }
    }
}