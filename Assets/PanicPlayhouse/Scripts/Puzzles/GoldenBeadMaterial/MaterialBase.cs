using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace PanicPlayhouse.Scripts.Puzzles.GoldenBeadMaterial
{
    public class MaterialBase : MonoBehaviour
    {
        [SerializeField] private int correctValue;
        
        public bool IsCorrect => correctValue == CurrentValue;
        
        private int CurrentValue => _inside[0].Value;
        private readonly List<Pushable> _inside = new();
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Pushable push))
            {
                _inside.Add(push);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Pushable push))
            {
                _inside.Remove(push);
            }
        }
    }
}