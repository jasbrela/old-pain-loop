using PanicPlayhouse.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PanicPlayhouse.Scripts.Player
{
    public class PlayerInteractionDetector : MonoBehaviour
    {
        [SerializeField] private float maxRange;
        [SerializeField] private LayerMask interactableMask;
        [SerializeField] private bool debug;
    
        private PlayerInput _input;
        private Vector3 _forward = Vector3.forward;

        private IInteractable _currentTarget;

        private void Start()
        {
            TryGetComponent(out _input);
            SetUpControls();
        }

        protected void OnDisable()
        {
            UnsubscribeControls();
        }

        private void Update()
        {
            RaycastForInteractable();
        }

        private void RaycastForInteractable()
        {
            Ray ray = new Ray(transform.position + Vector3.up, _forward);

            if (debug) Debug.DrawRay(ray.origin, ray.direction * maxRange, Color.cyan);

            if (!Physics.Raycast(ray, out RaycastHit hit, maxRange, interactableMask))
            {
                ResetTarget();
                return;
            }

            if (hit.collider != null)
            {
                hit.collider.TryGetComponent(out IInteractable interactable);
                
                if (_currentTarget == interactable) return;

                _currentTarget?.OnQuitRange();
                _currentTarget = interactable;
                _currentTarget.OnEnterRange();
            }
            else
            {
                ResetTarget();
            }
        }

        private void ResetTarget()
        {
            if (_currentTarget == null) return;

            _currentTarget.OnQuitRange();
            _currentTarget = null;
        }
    
        private void SetUpControls()
        {
            _input.actions["Interact"].performed += Interact;
            _input.actions["Movement"].performed += OnPlayerMove;
        }

        private void OnPlayerMove(InputAction.CallbackContext obj)
        {
            var forward = obj.ReadValue<Vector3>().normalized;
            if (forward != Vector3.zero) _forward = forward;
        }

        private void UnsubscribeControls()
        {
            _input.actions["Interact"].performed -= Interact;
            _input.actions["Movement"].performed -= OnPlayerMove;
        }
    
        private void Interact(InputAction.CallbackContext callbackContext)
        {
            _currentTarget?.OnInteract();
        }
    }
}
