using System.Collections;
using PanicPlayhouse.Scripts.Chunk;
using PanicPlayhouse.Scripts.Puzzles.GoldenBeadMaterial;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PanicPlayhouse.Scripts.Entities.Player
{
    public enum PlayerInteractionState
    {
        None,
        InteractableNearby,
        InteractablePickedUp
    }

    public class PlayerInteractionDetector : MonoBehaviour
    {
        [Header("Tooltip")]
        [SerializeField] private PlayerTooltip tooltip;

        [Header("Interaction")]
        [Space(10)]
        [SerializeField] private float interactionRadius;
        [SerializeField] private LayerMask interactableMask;
        public Transform pickupInteractablePosition;

        [SerializeField] private PlayerInput input;

        private Interactable _currentTarget;
        private Pickupable _pickedUpInteractable;
        private PlayerInteractionState _currentInteractionState = PlayerInteractionState.None;


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
            bool GetNearbyInteractable(out Interactable interactable)
            {
                interactable = null;
                Collider[] results = new Collider[5];
                int size = Physics.OverlapSphereNonAlloc(transform.position, interactionRadius, results, interactableMask);

                if (size == 0)
                    return false;

                foreach (Collider col in results)
                {
                    col.TryGetComponent(out interactable);
                    if (interactable != null) break;
                }

                if (interactable == null)
                    return false;

                return true;
            }
            void SetupCurrentTarget(Interactable nearbyInteractable)
            {
                _currentInteractionState = PlayerInteractionState.InteractableNearby;
                _currentTarget = nearbyInteractable;
                _currentTarget.OnEnterRange();
                ResetIdleTimer();
                tooltip.IsCloseToInteraction = true;
            }


            // Get nearest interactable.
            if (!GetNearbyInteractable(out Interactable nearbyInteractable))
            {
                ResetTarget();
            }

            // We didn't have any nearby interactables last check;
            if (_currentInteractionState == PlayerInteractionState.None && nearbyInteractable != null)
            {
                // Setup current target with nearby interactable.
                SetupCurrentTarget(nearbyInteractable);
            }
            else if (_currentInteractionState == PlayerInteractionState.InteractableNearby)
            {
                if (_currentTarget != nearbyInteractable)
                {
                    ResetTarget();
                    SetupCurrentTarget(nearbyInteractable);
                }
            }
            else if (_currentInteractionState == PlayerInteractionState.InteractablePickedUp)
            {
                // Caso não tenhamos interagível nas mãos, volta o estado do player para None e chama a função novamente.
                if (_pickedUpInteractable == null || !_pickedUpInteractable.pickedUp)
                {
                    _pickedUpInteractable = null;
                    ResetTarget();
                    CheckForInteractable();
                }
                else SetupCurrentTarget(_pickedUpInteractable);
            }
        }

        private void ResetTarget()
        {
            _currentInteractionState = PlayerInteractionState.None;
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

            if (!_currentTarget.TryGetComponent(out Pickupable pickupable))
            {
                if (pickupable.pickedUp)
                {
                    _pickedUpInteractable = pickupable;
                    _currentInteractionState = PlayerInteractionState.InteractablePickedUp;
                }
                else
                {
                    _pickedUpInteractable = null;
                    _currentInteractionState = PlayerInteractionState.None;
                }
            }
        }

        private void ResetIdleTimer()
        {
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
