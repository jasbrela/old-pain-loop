using System.Collections.Generic;
using NaughtyAttributes;
using PanicPlayhouse.Scripts.Interfaces;
using UnityEngine;

namespace PanicPlayhouse.Scripts.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Data/Event", fileName = "New Event")]
    public class Event : ScriptableObject
    {
        private readonly List<Listener> _listeners = new();
        
        [Button]
        public void Raise()
        {
            Debug.Log("Raised event: ".Bold().Color("#FFD700") + name);
            
            for(int i = _listeners.Count -1; i >= 0; i--)
                _listeners[i].OnEventRaised();
        }

        public void RegisterListener(Listener listener)
        {   
            _listeners.Add(listener);
        }

        public void UnregisterListener(Listener listener)
        {
            _listeners.Remove(listener);
        }
    }
}