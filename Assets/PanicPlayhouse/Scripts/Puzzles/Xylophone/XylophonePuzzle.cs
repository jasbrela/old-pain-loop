﻿using System.Collections.Generic;
using PanicPlayhouse.Scripts.ScriptableObjects;
using UnityEngine;
using Event = PanicPlayhouse.Scripts.ScriptableObjects.Event;

namespace PanicPlayhouse.Scripts.Puzzles.Xylophone
{
    public class XylophonePuzzle : MonoBehaviour
    {
        [Header("Insanity")]
        [SerializeField] private float insanityPenalty;
        [SerializeField] private float insanityReward;
        [SerializeField] private FloatVariable insanity;

        [Header("Puzzle")]
        [SerializeField] private Event onFinish;
        [SerializeField] private List<XylophoneButton> order;
        private List<XylophoneButton> _uniqueButtons;
        private int _buttonCount;
        
        private bool IsActivated { get; set; } = false;
        private bool IsFinished { get; set; } = false;

        private void Start()
        {
            _uniqueButtons = new List<XylophoneButton>(FindObjectsOfType<XylophoneButton>());
            
            if (order.Count == 0)
            {
                gameObject.SetActive(false);
                Debug.Log(name + " has been deactivated.");
                return;
            }
            
            foreach (XylophoneButton button in order)
            {
                button.Puzzle = this;
            }
        }

        public void ActivatePuzzle()
        {
            if (IsActivated || IsFinished) return;
            
            Debug.Log(name + "has been activated.");
            
            foreach (XylophoneButton button in _uniqueButtons)
            {
                button.IsBlocked = false;
            }
        }

        public void OnPressButton(XylophoneButton button)
        {
            if (IsFinished || !IsActivated) return;
            
            if (order[_buttonCount] == button)
            {
                _buttonCount++;
            }
            else
            {
                _buttonCount = 0;
            }
            
            insanity.Value += insanityPenalty;

            if (_buttonCount != order.Count) return;
            
            IsFinished = true;
            IsActivated = false;
            insanity.Value -= insanityReward;
            if (onFinish != null) onFinish.Raise();
                
            foreach (XylophoneButton btn in _uniqueButtons) btn.IsBlocked = true;
                
            Debug.Log("Xylophone Finished!");
        }
    }
}