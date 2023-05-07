using PanicPlayhouse.Scripts.Interfaces;
using UnityEngine;

namespace PanicPlayhouse.Scripts
{
    public class Hideout : MonoBehaviour, IInteractable
    {
        [SerializeField] private Material hoverMaterial;
        private Material _defaultMaterial;
        private SpriteRenderer _renderer;
        void Start()
        {
            _renderer = GetComponentInChildren<SpriteRenderer>();
            _defaultMaterial = _renderer.material;
        }

        public void OnInteract()
        {
            Debug.Log("Interacted!", this);
        }

        public void OnEnterRange()
        {
            _renderer.material = hoverMaterial;
        }

        public void OnQuitRange()
        {
            _renderer.material = _defaultMaterial;
        }
    }
}
