using PanicPlayhouse.Scripts.Interfaces;
using UnityEngine;
using Event = PanicPlayhouse.Scripts.ScriptableObjects.Event;


namespace PanicPlayhouse.Scripts
{
    public class Hideout : MonoBehaviour, IInteractable
    {
        [SerializeField] private Event enter;
        [SerializeField] private Event exit;
        [SerializeField] private Material hoverMaterial;
        private Material _defaultMaterial;
        private SpriteRenderer _renderer;
        private bool _isHidden;
        
        void Start()
        {
            _renderer = GetComponentInChildren<SpriteRenderer>();
            _defaultMaterial = _renderer.material;
        }

        public void OnInteract()
        {
            _isHidden = !_isHidden;
            
            if (_isHidden)
            {
                OnEnterHideout();
            }
            else
            {
                OnExitHideout();
            }
        }

        public void OnEnterRange()
        {
            _renderer.material = hoverMaterial;
        }

        public void OnQuitRange()
        {
            _renderer.material = _defaultMaterial;
        }

        public void OnEnterHideout()
        {
            enter.Raise();
        }
        
        public void OnExitHideout()
        {
            exit.Raise();
        }
    }
}
