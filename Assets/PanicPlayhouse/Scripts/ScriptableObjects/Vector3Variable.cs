using System;
using NaughtyAttributes;
using UnityEngine;

namespace PanicPlayhouse.Scripts.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Variables/Vector3", fileName = "New Vector3")]
    public class Vector3Variable : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private Vector3 initialValue;
        [ReadOnly] [ShowNonSerializedField] [NonSerialized] private Vector3 _value;

        public bool IsValid => _value == initialValue;
        public Vector3 Value
        {
            get => _value;
            set => _value = value;
        }

        public void OnAfterDeserialize()
        {
            _value = initialValue;
        }

        public void OnBeforeSerialize() { }
    }
}
