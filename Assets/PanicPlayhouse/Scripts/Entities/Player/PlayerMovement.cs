using FMOD.Studio;
using FMODUnity;
using NaughtyAttributes;
using PanicPlayhouse.Scripts.Audio;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PanicPlayhouse.Scripts.Entities.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private bool startLocked;
        [SerializeField] private float defaultForce;
        [SerializeField] private float defaultMaxVel;
        [SerializeField] private float heavyMaxVel;
        [SerializeField] private EventReference footsteps;

        [Header("Components")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Transform flipXGameObjects;

        [Label("Rigidbody")][SerializeField] private Rigidbody rb;
        [SerializeField] private PlayerInput input;

        [Label("Animation")][SerializeField] private EntityAnimation anim;
        [SerializeField] private PlayerHiddenStatus hiddenStatus;

        public bool IsMoving { get; private set; }
        private EventInstance _footstepInstance;
        private AudioManager _audio;
        private Vector3 _previousInput;
        private bool _isHidden;
        private float _currentMaxVel;

        private void Start()
        {
            _currentMaxVel = defaultMaxVel;
            _audio = FindObjectOfType<AudioManager>();
            SetUpControls();
        }

        public void ChangeVisibility()
        {
            if (hiddenStatus.IsHidden)
            {
                LockMovement();
            }
            else
            {
                UnlockMovement();
            }
        }

        public void OnPickUp()
        {
            _currentMaxVel = heavyMaxVel;
        }

        public void OnReleaseItem()
        {
            _currentMaxVel = defaultMaxVel;
        }

        public void UnlockMovement()
        {
            input.actions["Movement"].Enable();
        }

        public void LockMovement()
        {
            input.actions["Movement"].Disable();
        }

        private void SetUpControls()
        {
            input.actions["Movement"].performed += SetMovement;
            input.actions["Movement"].canceled += ResetMovement;

            if (startLocked) LockMovement();
        }

        private void OnDisable()
        {
            input.actions["Movement"].performed -= SetMovement;
            input.actions["Movement"].canceled -= ResetMovement;
        }

        private void SetMovement(InputAction.CallbackContext ctx)
        {
            IsMoving = true;
            _previousInput = ctx.ReadValue<Vector3>();

            // if (_previousInput != Vector3.zero)
            // {
            //     _audio?.PlayAudioInLoop(ref _footstepInstance, footsteps);
            // }
            // else
            // {
            //     _audio?.StopAudioInLoop(_footstepInstance);
            // }

            anim["is_moving"].SetValue(_previousInput != Vector3.zero);

            if (_previousInput.x != 0)
            {
                spriteRenderer.flipX = _previousInput.x > 0;
                flipXGameObjects.localScale = new Vector3(_previousInput.x > 0
                    ? -1
                    : 1, 1, 1);
            }
        }


        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            if (_isHidden) return;
            if (rb.velocity.magnitude >= _currentMaxVel) return;

            rb.AddRelativeForce(_previousInput * defaultForce, ForceMode.Force);
        }

        private void ResetMovement(InputAction.CallbackContext obj)
        {
            IsMoving = false;
            _previousInput = Vector3.zero;
            Vector3 vel = rb.velocity;

            vel = new Vector2(vel.x * 0.15f, vel.y);

            rb.velocity = vel;
            anim["is_moving"].SetValue(false);
            // _audio?.StopAudioInLoop(_footstepInstance);
        }
    }
}
