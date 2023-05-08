using System;
using NaughtyAttributes;
using UnityEngine;

namespace PanicPlayhouse.Scripts.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Variables/Float", fileName = "New Float")]
    public class FloatVariable : ScriptableObject, ISerializationCallbackReceiver
    {
        [Header("Events")]
        [SerializeField] private Event onChangeValue;
        [SerializeField] private Event onMaxValueReached;
        [SerializeField] private Event onMinValueReached;
        
        [Header("Values")]
        [SerializeField] private float initialValue;
        [SerializeField] private float maxValue;
        [SerializeField] private float minValue;
        [ShowNonSerializedField] [NonSerialized] private float _value;

        public float Value
        {
            get => _value;
            set
            {
                var temp = _value;   
                _value = value;
                Debug.Log("Insanity set: ".Bold() + temp + " -> " + _value);
                
                if (_value <= minValue)
                {
                    _value = minValue;
                    if (onMinValueReached != null) onMaxValueReached.Raise();
                }
                else if (_value >= maxValue)
                {
                    if (onMaxValueReached != null) onMinValueReached.Raise();
                    _value = maxValue;
                }
                if (onChangeValue != null) onChangeValue.Raise();
            }
        }

        public void OnAfterDeserialize()
        {
            _value = initialValue;
        }

        public void OnBeforeSerialize() { }
    }
}
