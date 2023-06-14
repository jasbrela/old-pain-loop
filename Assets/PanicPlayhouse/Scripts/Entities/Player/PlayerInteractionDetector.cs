using System.Collections;
using PanicPlayhouse.Scripts.Chunk;
using PanicPlayhouse.Scripts.Puzzles.GoldenBeadMaterial;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PanicPlayhouse.Scripts.Entities.Player
{
    public class PlayerInteractionDetector : MonoBehaviour
    {
        [SerializeField] private PlayerTooltip tooltip;

        [Space(10)]
        [SerializeField] private float interactionRadius;
        [SerializeField] private LayerMask interactableMask;
    
        [SerializeField] private PlayerInput input;
        private Vector3 _forward = Vector3.forward; // TODO: Remove forward

        private Interactable _currentTarget;

        private void Start()
        {
            SetUpControls();
        }

        protected void OnDisable()
        {
            UnsubscribeControls();
        }

        private void Update()
        {
            CheckForInteractable();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, interactionRadius);
        }

        private void CheckForInteractable()
        {
            var results = new Collider[5];
            var size = Physics.OverlapSphereNonAlloc(transform.position, interactionRadius, results, interactableMask);
            
            if (size == 0)
            {
                ResetTarget();
                return;
            }

            Interactable interactable = null;
            
            foreach (Collider col in results)
            {
                if (interactable != null) break;
                col.TryGetComponent(out interactable);
            }
            
            if (interactable != null)
            {
                if (_currentTarget == interactable) return;

                if (_currentTarget != null)
                {
                    tooltip.IsCloseToInteraction = false;
                    _currentTarget.OnQuitRange();
                    tooltip.HideTooltip();
                }
                
                _currentTarget = interactable;

                if (_currentTarget == null) return;
                
                _currentTarget.OnEnterRange();
                ResetIdleTimer();
                tooltip.IsCloseToInteraction = true;
            }
            else ResetTarget();
            
        }

        private void ResetTarget()
        {
            if (_currentTarget == null) return;

            tooltip.IsCloseToInteraction = false;
            _currentTarget.OnQuitRange();
            _currentTarget = null;
            
            StopCoroutine(IdleTimer());
            tooltip.HideTooltip();
        }
    
        private void SetUpControls()
        {
            input.actions["Interact"].performed += Interact;
            input.actions["Movement"].performed += OnPlayerMove;
        }

        private void OnPlayerMove(InputAction.CallbackContext obj)
        {
            var forward = obj.ReadValue<Vector3>().normalized;
            if (forward != Vector3.zero) _forward = forward;
        }

        private void UnsubscribeControls()
        {
            input.actions["Interact"].performed -= Interact;
            input.actions["Movement"].performed -= OnPlayerMove;
        }
    
        private void Interact(InputAction.CallbackContext ctx)
        {
            if (_currentTarget == null) return;
            
            tooltip.SetInteractionKey(ctx.action.activeControl.displayName);
            
            ResetIdleTimer();
            tooltip.HideTooltip();
            
            _currentTarget.OnInteract();

            if (!_currentTarget.TryGetComponent(out Pushable pushable)) return;
            
            pushable.Push(_forward);
        }
        
        private void ResetIdleTimer() {
            StopCoroutine(IdleTimer());
            StartCoroutine(IdleTimer());
        }
        
        private IEnumerator IdleTimer()
        {
            yield return new WaitForSeconds(tooltip.MinimumInactiveSeconds);
            if (_currentTarget == null)
            {
                ResetIdleTimer();
                yield break;
            }

            tooltip.ShowInteractionTooltip();
        }
    }
}
