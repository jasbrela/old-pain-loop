using System;
using NaughtyAttributes;
using UnityEngine;

namespace PanicPlayhouse.Scripts.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Variables/Float", fileName = "New Float")]
    public class FloatVariable : ScriptableObject
    {
        [SerializeField] private float initialValue;
        [ReadOnly] [ShowNonSerializedField] [NonSerialized] private float _value;
        [SerializeField] private float maxValue;
        [SerializeField] private float minValue;
        [SerializeField] private Event onChangeValue;

        public float Value => _value;

        [Button()]
        private void Increase()
        {
            Increase(10);
        }
        
        public void Increase(float add)
        {
            if (_value < maxValue)
                _value += add;

            if (_value > maxValue)
                _value = maxValue;

            onChangeValue.Raise();
        }
        
        public void Decrease(float sub)
        {
            if (_value > minValue)
                _value += sub;

            if (_value < minValue)
                _value = minValue;
            
            onChangeValue.Raise();
        }
        
        public void OnAfterDeserialize()
        {
            _value = initialValue;
        }

        public void OnBeforeSerialize() { }
    }
}
