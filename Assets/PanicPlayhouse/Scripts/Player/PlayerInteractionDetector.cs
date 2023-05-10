using PanicPlayhouse.Scripts.Interfaces;
using PanicPlayhouse.Scripts.Puzzles.GoldenBeadMaterial;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PanicPlayhouse.Scripts.Player
{
    public class PlayerInteractionDetector : MonoBehaviour
    {
        [SerializeField] private bool debug;
        [Space(10)]
        [SerializeField] private float maxRange;
        [SerializeField] private LayerMask interactableMask;
    
        [SerializeField] private PlayerInput input;
        private Vector3 _forward = Vector3.forward;

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
                hit.collider.TryGetComponent(out Interactable interactable);
                
                
                if (_currentTarget == interactable) return;
                if (_currentTarget != null) _currentTarget.OnQuitRange();
                
                _currentTarget = interactable;
                if (_currentTarget != null) _currentTarget.OnEnterRange();
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
    
        private void Interact(InputAction.CallbackContext callbackContext)
        {
            if (_currentTarget == null) return;
            
            _currentTarget.OnInteract();

            if (!_currentTarget.TryGetComponent(out Pushable pushable)) return;
            
            pushable.Push(_forward);
        }
    }
}
