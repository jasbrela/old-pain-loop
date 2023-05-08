using System;
using NaughtyAttributes;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Interfaces
{
    public abstract class Interactable : MonoBehaviour
    {
        [SerializeField] private Material hoverMaterial;
        private Material _defaultMaterial;
        private SpriteRenderer _renderer;

        private void Start()
        {
            _renderer = GetComponentInChildren<SpriteRenderer>();
            if (_renderer == null) return;
            _defaultMaterial = _renderer.material;
        }

        public virtual void OnInteract() { }

        public virtual void OnEnterRange()
        {
            if (_renderer == null) return;
            _renderer.material = hoverMaterial;

        }

        public virtual void OnQuitRange()
        {
            if (_renderer == null) return;
            _renderer.material = _defaultMaterial;
        }
    }
}
