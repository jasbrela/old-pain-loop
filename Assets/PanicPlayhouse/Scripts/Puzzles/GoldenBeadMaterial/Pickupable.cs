using DG.Tweening;
using FMODUnity;
using PanicPlayhouse.Scripts.Audio;
using PanicPlayhouse.Scripts.Chunk;
using PanicPlayhouse.Scripts.Entities.Player;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Puzzles.GoldenBeadMaterial
{
    public class Pickupable : Interactable
    {
        [Header("Gameplay")]
        [SerializeField] private float radius;
        [SerializeField] private float tweenDuration = 1f;

        [Header("Collision")]
        [SerializeField] private LayerMask roomMask;

        [Header("SFX")]
        [SerializeField] private EventReference pickup;
        [SerializeField] private EventReference drop;

        [Header("Components")]
        [SerializeField] private PlayerInteractionDetector interactionDetector;


        private Collider _collider;
        private Rigidbody _rigidbody;

        private bool _pickedUp = false;
        public bool pickedUp
        {
            get
            {
                return _pickedUp;
            }
        }

        private AudioManager _audio;
        private AudioManager Audio
        {
            get
            {
                if (_audio == null)
                    _audio = FindObjectOfType<AudioManager>();

                return _audio;
            }
        }

        private Tween currentTween;

        private void Awake()
        {
            _collider = GetComponentInChildren<Collider>();
            _rigidbody = GetComponentInChildren<Rigidbody>();

        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, radius);
        }

        public bool IsBlocked { get; set; }

        public override void OnInteract()
        {
            // if (IsBlocked) return;

            if (!_pickedUp) OnPickup();
            else OnDrop();
        }


        void OnPickup()
        {
            Debug.Log("OnPickup!");
            if (interactionDetector == null) // algo de errado não está certo
            {
                Debug.LogError($"Pickupable {gameObject.name}:".Bold() + " No Interaction Detector found!");
                return;
            }

            if (interactionDetector.pickupInteractablePosition == null) // algo de errado não está certo
            {
                Debug.LogError($"Pickupable {gameObject.name}:".Bold() + " No pickup holder found on interaction detector!");
                return;
            }

            // if (_pickedUp)
            //     return;

            Audio?.PlayOneShot(pickup);
            transform.SetParent(interactionDetector.pickupInteractablePosition);
            transform.localPosition = Vector3.zero;
            // if (currentTween != null)
            //     currentTween.Kill(true);

            // currentTween = transform.DOLocalMove(Vector3.zero, tweenDuration)
            // .OnStart(() =>
            // {
            //     _collider.enabled = false;
            //     _rigidbody.useGravity = false;
            // })
            // .OnComplete(() =>
            // {
            //     _collider.enabled = true;
            //     _rigidbody.useGravity = true;
            //     currentTween = null;
            // });

            gameObject.layer = LayerMask.NameToLayer("PickedupInteractable");

            _pickedUp = true;
        }

        void OnDrop()
        {
            Debug.Log("OnDrop!");
            if (interactionDetector == null) // algo de errado não está certo
            {
                Debug.LogError($"Pickupable {gameObject.name}:".Bold() + " No Interaction Detector found!");
                return;
            }

            // if (!_pickedUp)
            //     return;

            Transform roomTransform = null;
            Audio?.PlayOneShot(drop);


            if (Physics.Raycast(interactionDetector.transform.position, Vector3.down, out RaycastHit hitInfo, 99, roomMask))
            {
                roomTransform = hitInfo.transform;
            }
            transform.SetParent(roomTransform);

            if (currentTween != null)
                currentTween.Kill(true);

            currentTween = transform.DOMove(interactionDetector.transform.position, tweenDuration)
            .OnStart(() =>
            {
                _collider.enabled = false;
                _rigidbody.useGravity = false;
            })
            .OnComplete(() =>
            {
                _collider.enabled = true;
                _rigidbody.useGravity = true;
                currentTween = null;
            });

            gameObject.layer = LayerMask.NameToLayer("Interactable");
            transform.rotation = Quaternion.identity;

            _pickedUp = false;
        }
    }
}