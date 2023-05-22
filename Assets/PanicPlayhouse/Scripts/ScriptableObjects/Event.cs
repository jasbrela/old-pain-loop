using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace PanicPlayhouse.Scripts.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Data/Event", fileName = "New Event")]
    public class Event : ScriptableObject
    {
        [SerializeField] private bool debug = true;
        private readonly List<Listener> _listeners = new();
        
        [Button]
        public void Raise()
        {
#if UNITY_EDITOR
            if (debug) Debug.Log("Raised event: ".Bold().Color("#FFD700") + name);
#endif
            
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