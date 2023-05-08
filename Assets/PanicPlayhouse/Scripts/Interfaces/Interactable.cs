using System;
using NaughtyAttributes;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Interfaces
{
    public abstract class Interactable : MonoBehaviour
    {
        [SerializeField] private Material hoverMaterial;
        private Material _defaultMaterial;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private void Start()
        {
            if (spriteRenderer == null) return;
            _defaultMaterial = spriteRenderer.material;
        }

        public virtual void OnInteract() { }

        public virtual void OnEnterRange()
        {
            if (spriteRenderer == null) return;
            spriteRenderer.material = hoverMaterial;

        }

        public virtual void OnQuitRange()
        {
            if (spriteRenderer == null) return;
            spriteRenderer.material = _defaultMaterial;
        }
    }
}
