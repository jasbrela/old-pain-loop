using DG.Tweening;
using FMODUnity;
using NaughtyAttributes;
using PanicPlayhouse.Scripts.Audio;
using PanicPlayhouse.Scripts.Chunk;
using PanicPlayhouse.Scripts.Entities.Player;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Puzzles.GoldenBeadMaterial
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Pickupable : Interactable
    {
        [Header("Gameplay")]
        [SerializeField] private float tweenDuration = 1f;

        [Header("Collision")]
        [SerializeField] private LayerMask roomMask;

        [Header("Rendering")]
        [SerializeField] [SortingLayer] private int abovePlayerLayer = 1748606149;

        [Header("SFX")]
        [SerializeField] private EventReference pickup;
        [SerializeField] private EventReference drop;

        [Header("Components")]
        [SerializeField] private PlayerInteractionDetector interactionDetector;

        private int _defaultSortingLayer;
        private Collider _collider;
        private SpriteRenderer _spriteRenderer;
        private Rigidbody _rigidbody;

        private bool _pickedUp = false;
        public bool PickedUp => _pickedUp;

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

        private Tween _currentTween;

        private void Awake()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>(); // problematico hj pq os objetos tem varios sprites... acho que os proximos devem ser só um p facilitar nossa vida
            _collider = GetComponentInChildren<Collider>();
            _rigidbody = GetComponentInChildren<Rigidbody>();
            _defaultSortingLayer = _spriteRenderer.sortingLayerID;
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
            
            SetSortingLayer(abovePlayerLayer);
            
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

            if (_currentTween != null)
                _currentTween.Kill(true);

            _currentTween = transform.DOMove(interactionDetector.transform.position, tweenDuration)
            .OnStart(() =>
            {
                _collider.enabled = false;
                _rigidbody.useGravity = false;
            })
            .OnComplete(() =>
            {
                _collider.enabled = true;
                _rigidbody.useGravity = true;
                _currentTween = null;
                
            });

            gameObject.layer = LayerMask.NameToLayer("Interactable");
            transform.rotation = Quaternion.identity;
            
            SetSortingLayer(_defaultSortingLayer);

            _pickedUp = false;
        }

        private void SetSortingLayer(int id)
        {
            _spriteRenderer.sortingLayerID = id;
            _spriteRenderer.sortingLayerName = SortingLayer.IDToName(id);
        }
    }
}