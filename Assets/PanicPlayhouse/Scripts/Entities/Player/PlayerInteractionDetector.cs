using System.Collections;
using PanicPlayhouse.Scripts.Chunk;
using PanicPlayhouse.Scripts.Puzzles.MusicBox;
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
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private EntityAnimation anim;

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
            if (_currentInteractionState == PlayerInteractionState.InteractablePickedUp)
            {
                // Caso não tenhamos interagível nas mãos, volta o estado do player para None e chama a função novamente.
                if (_pickedUpInteractable == null || !_pickedUpInteractable.PickedUp)
                {
                    ResetTarget();
                    CheckForInteractable();
                }
                else SetupCurrentTarget(_pickedUpInteractable);

                return;
            }

            // Debug.Log("Checking for Interactable!");
            // Get nearest interactable.
            if (!GetNearbyInteractable(out Interactable nearbyInteractable) && _pickedUpInteractable == null)
            {
                ResetTarget();
            }

            // We didn't have any nearby interactables last check;
            if (_currentInteractionState == PlayerInteractionState.None && nearbyInteractable != null)
            {
                // Setup current target with nearby interactable.
                _currentInteractionState = PlayerInteractionState.InteractableNearby;
                SetupCurrentTarget(nearbyInteractable);
            }
            else if (_currentInteractionState == PlayerInteractionState.InteractableNearby)
            {
                if (_currentTarget != nearbyInteractable)
                {
                    ResetTarget();

                    _currentInteractionState = PlayerInteractionState.InteractableNearby;
                    SetupCurrentTarget(nearbyInteractable);
                }
            }
        }

        private bool GetNearbyInteractable(out Interactable interactable)
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

        private void SetupCurrentTarget(Interactable nearbyInteractable)
        {
            if (nearbyInteractable == null) return;
#if UNITY_EDITOR
            Debug.Log($"Setting up current target: {(nearbyInteractable != null ? nearbyInteractable.name : "null")}");
#endif
            if (_currentTarget != null)
                ResetTarget();

            _currentTarget = nearbyInteractable;
            _currentTarget.OnEnterRange();
            ResetIdleTimer();
            tooltip.IsCloseToInteraction = true;
        }

        private void ResetTarget()
        {
            // #if UNITY_EDITOR
            //             Debug.Log($"Resetting current target: {(_currentTarget != null ? _currentTarget.name : "null")}");
            // #endif
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
        }

        private void UnsubscribeControls()
        {
            input.actions["Interact"].performed -= Interact;
        }

        private void Interact(InputAction.CallbackContext ctx)
        {
            if (_currentTarget == null) return;

            tooltip.SetInteractionKey(ctx.action.activeControl.displayName);

            ResetIdleTimer();
            tooltip.HideTooltip();

            _currentTarget.OnInteract();

            if (!_currentTarget.TryGetComponent(out Pickupable pickupable)) return;

            if (pickupable.PickedUp)
            {
                anim["on_pickup_item"].SetValue();
                anim["item_picked_up"].SetValue(true);
                movement.OnPickUp();
                _pickedUpInteractable = pickupable;
                _currentInteractionState = PlayerInteractionState.InteractablePickedUp;
            }
            else
            {
                anim["item_picked_up"].SetValue(false);
                movement.OnReleaseItem();
                _pickedUpInteractable = null;
                _currentInteractionState = PlayerInteractionState.None;
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
