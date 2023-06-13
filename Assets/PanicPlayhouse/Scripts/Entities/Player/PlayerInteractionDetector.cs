using System;
using PanicPlayhouse.Scripts.Chunk;
using PanicPlayhouse.Scripts.Puzzles.GoldenBeadMaterial;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace PanicPlayhouse.Scripts.Entities.Player
{
    public class PlayerInteractionDetector : MonoBehaviour
    {
        [SerializeField] private bool debug;
        [FormerlySerializedAs("maxRange")]
        [Space(10)]
        [SerializeField] private float interactionRadius;
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
            
            Debug.Log(size);
            
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
                
                if (_currentTarget != null) _currentTarget.OnQuitRange();
                
                _currentTarget = interactable;
                
                if (_currentTarget != null) _currentTarget.OnEnterRange();
            }
            else ResetTarget();
            
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
