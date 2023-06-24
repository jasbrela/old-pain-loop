using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PanicPlayhouse.Scripts.Entities.Player
{
    public class PlayerTooltip : MonoBehaviour
    {
        [Serializable]
        public class ActionKey
        {
            public Keys key;
            public SpriteRenderer tooltip;
        }
        
        [SerializeField] private PlayerInput input;
        [SerializeField] private List<ActionKey> actions;

        [SerializeField] private float minimumInactiveSeconds;
        [SerializeField] private PlayerMovement movement;
        public float MinimumInactiveSeconds => minimumInactiveSeconds;
        public bool IsCloseToInteraction { get; set; }

        private bool _isShowingInteractionTooltip;
        private Keys _movementKey = Keys.WASD;
        private Keys _interactionKey = Keys.E;
        private const float HideDelay = 0.1f;

        void Start()
        {
            SetUpControls();
            HideTooltip();
            
            StartCoroutine(IdleTimer());
        }
    
        private void SetUpControls()
        {
            input.actions["Arrows"].performed += OnArrowKeyPerformed;
            input.actions["WASD"].performed += OnWASDKeysPerformed;
            input.actions["Movement"].performed += OnMove;
        }

        private void OnMove(InputAction.CallbackContext obj)
        {
            ResetIdleTimer();
        }

        private void ShowMovementTooltip()
        {
            if (_isShowingInteractionTooltip) return;

            ShowTooltip();
        }

        private void ShowTooltip()
        {
            var key = _movementKey;
            if (_isShowingInteractionTooltip) key = _interactionKey;

            foreach (ActionKey action in actions)
                action.tooltip.DOFade(action.key == key ? 1 : 0, HideDelay);
        }
        
        public void ShowInteractionTooltip()
        {
            _isShowingInteractionTooltip = true;
            
            ShowTooltip();
            ResetIdleTimer();
        }

        public void HideTooltip()
        {
            _isShowingInteractionTooltip = false;
            
            foreach (ActionKey action in actions)
                action.tooltip.DOFade(0, HideDelay);
        }

        private void OnArrowKeyPerformed(InputAction.CallbackContext ctx)
        {
            ResetIdleTimer();
            _movementKey = Keys.Arrows;
        }
        
        private void OnWASDKeysPerformed(InputAction.CallbackContext ctx)
        {
            ResetIdleTimer();
            _movementKey = Keys.WASD;
        }

        public void SetInteractionKey(string displayName)
        {
            var key = displayName.ToUpper() switch
            {
                "F" => Keys.F,
                "SPACE" => Keys.Space,
                _ => Keys.E
            };

            SetInteractionKey(key);
        }

        private void SetInteractionKey(Keys key) => _interactionKey = key;

        private void ResetIdleTimer() {
            if (!_isShowingInteractionTooltip) HideTooltip();
            StopCoroutine(IdleTimer());
            StartCoroutine(IdleTimer());
        }
        
        private IEnumerator IdleTimer()
        {
            while (_isShowingInteractionTooltip || IsCloseToInteraction) yield return null;
            yield return new WaitForSeconds(minimumInactiveSeconds);
            if (movement.IsMoving || IsCloseToInteraction)
            {
                ResetIdleTimer();
                yield break;
            }
            
            ShowMovementTooltip();
        }
    }
}
