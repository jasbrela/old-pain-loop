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

        public float MaxValue => maxValue;
        public float MinValue => minValue;
        public float Percentage => _value / maxValue;
        public float Value
        {
            get => _value;
            set
            {
                var temp = _value;
                _value = value;
#if UNITY_EDITOR
                //Debug.Log($"Insanity: {temp} -> {_value}");
#endif
                
                if (_value <= MinValue)
                {
                    _value = MinValue;
                    if (onMinValueReached != null) onMinValueReached.Raise();
                }
                else if (_value >= MaxValue)
                {
                    if (onMaxValueReached != null) onMaxValueReached.Raise();
                    _value = MaxValue;
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
