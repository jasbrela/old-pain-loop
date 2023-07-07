using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using FMODUnity;
using NaughtyAttributes;
using PanicPlayhouse.Scripts.Audio;
using PanicPlayhouse.Scripts.Chunk;
using PanicPlayhouse.Scripts.Entities.Player;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Puzzles.MusicBox
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
        private List<SpriteRenderer> _sprites;
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
            _sprites = GetComponentsInChildren<SpriteRenderer>().ToList();
            _collider = GetComponentInChildren<Collider>();
            _rigidbody = GetComponentInChildren<Rigidbody>();
            _defaultSortingLayer = _sprites[0].sortingLayerID;
        }

        public bool IsBlocked { get; set; }

        public override void OnInteract()
        {
            if (IsBlocked) return;

            if (!_pickedUp) OnPickup();
            else OnDrop();
        }


        void OnPickup()
        {
#if UNITY_EDITOR
            Debug.Log("OnPickup!");
#endif
            if (interactionDetector == null) // algo de errado não está certo
            {
#if UNITY_EDITOR
                Debug.LogError($"Pickupable {gameObject.name}:".Bold() + " No Interaction Detector found!");
#endif
                return;
            }

            if (interactionDetector.pickupInteractablePosition == null) // algo de errado não está certo
            {
#if UNITY_EDITOR
                Debug.LogError($"Pickupable {gameObject.name}:".Bold() + " No pickup holder found on interaction detector!");
#endif
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
#if UNITY_EDITOR
            Debug.Log("OnDrop!");
#endif
            if (interactionDetector == null) // algo de errado não está certo
            {
#if UNITY_EDITOR
                Debug.LogError($"Pickupable {gameObject.name}:".Bold() + " No Interaction Detector found!");
#endif
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
            foreach (var sprite in _sprites)
            {
                sprite.sortingLayerID = id;
                sprite.sortingLayerName = SortingLayer.IDToName(id);
            }
        }
    }
}