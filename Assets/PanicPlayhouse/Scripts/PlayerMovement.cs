using UnityEngine;
using UnityEngine.InputSystem;

namespace PanicPlayhouse.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float defaultForce;
        [SerializeField] private float defaultMaxVel;

        private SpriteRenderer _sr;
        private Rigidbody _rb;
        private PlayerInput _input;
        private Vector3 _previousInput;

        private void Start()
        {
            _input = GetComponent<PlayerInput>();
            _rb = GetComponent<Rigidbody>();
            _sr = GetComponentInChildren<SpriteRenderer>();
        
            SetUpControls();
        }

        private void SetUpControls()
        {
            _input.actions["Movement"].performed += SetMovement;
            _input.actions["Movement"].canceled += ResetMovement;
        }

        private void OnDisable()
        {
            _input.actions["Movement"].performed -= SetMovement;
            _input.actions["Movement"].canceled -= ResetMovement;
        }

        private void SetMovement(InputAction.CallbackContext ctx)
        {
            _previousInput = ctx.ReadValue<Vector3>();
            if (_previousInput.x != 0) _sr.flipX = _previousInput.x > 0;
        }


        private void FixedUpdate()
        {
            Move();
        }
    
        private void Move()
        {
            if (_rb.velocity.magnitude >= defaultMaxVel) return;
        
            _rb.AddRelativeForce(_previousInput * defaultForce, ForceMode.Force);
        }
    
        private void ResetMovement(InputAction.CallbackContext obj)
        {
            _previousInput = Vector3.zero;
            Vector3 vel = _rb.velocity;
        
            vel = new Vector2(vel.x * 0.15f, vel.y);
        
            _rb.velocity = vel;
        }
    }
}
