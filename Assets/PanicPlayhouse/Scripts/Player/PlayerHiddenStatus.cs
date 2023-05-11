using System;
using PanicPlayhouse.Scripts.ScriptableObjects;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Player
{
    public class PlayerHiddenStatus : MonoBehaviour
    {
        [SerializeField] private float insanityReward;
        [SerializeField] private FloatVariable insanity;
        public bool IsHidden { get; private set; }

        private void Update()
        {
            if (IsHidden)
                insanity.Value -= insanityReward * Time.deltaTime;
        }

        public void ChangePlayerHiddenStatus(bool value)
        {
            IsHidden = value;
        }
    }
}