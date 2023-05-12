using NaughtyAttributes;
using PanicPlayhouse.Scripts.Audio;
using PanicPlayhouse.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PanicPlayhouse.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float defaultForce;
        [SerializeField] private float defaultMaxVel;
        [SerializeField] private Vector3Variable lastKnownPos;
        [SerializeField] private FootstepsAudio footsteps;

        [Header("Components")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [Label("Rigidbody")] [SerializeField] private Rigidbody rb;
        [SerializeField] private PlayerInput input;
        [Label("Animation")][SerializeField] private EntityAnimation anim;
        
        private Vector3 _previousInput;
        private bool _isHidden;

        private void Start()
        {
            SetUpControls();
        }

        public void ChangeVisibility()
        {
            _isHidden = !_isHidden;
            
            if (_isHidden)
            {
                input.actions["Movement"].Disable();
            }
            else
            {
                input.actions["Movement"].Enable();
            }
        }

        public void UnlockMovement()
        {
            input.actions["Movement"].Enable();
        }

        private void SetUpControls()
        {
            input.actions["Movement"].performed += SetMovement;
            input.actions["Movement"].canceled += ResetMovement;
        }

        private void OnDisable()
        {
            input.actions["Movement"].performed -= SetMovement;
            input.actions["Movement"].canceled -= ResetMovement;
        }

        private void SetMovement(InputAction.CallbackContext ctx)
        {
            _previousInput = ctx.ReadValue<Vector3>();
            
            footsteps.IsMoving = _previousInput != Vector3.zero;
            anim.Walking.SetBool(_previousInput != Vector3.zero);            
            
            if (_previousInput.x != 0) spriteRenderer.flipX = _previousInput.x > 0;
        }


        private void FixedUpdate()
        {
            Move();
        }
    
        private void Move()
        {
            if (_isHidden) return;
            if (rb.velocity.magnitude >= defaultMaxVel) return;
        
            rb.AddRelativeForce(_previousInput * defaultForce, ForceMode.Force);
        }
    
        private void ResetMovement(InputAction.CallbackContext obj)
        {
            _previousInput = Vector3.zero;
            Vector3 vel = rb.velocity;
        
            vel = new Vector2(vel.x * 0.15f, vel.y);
        
            rb.velocity = vel;
            anim.Walking.SetBool(false);            
            footsteps.IsMoving = false;
        }

        public void SaveLastKnownPosition()
        {
            lastKnownPos.Value = transform.position;
        }
    }
}
