using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace PanicPlayhouse.Scripts.UI
{
    public class InGameSettings : MonoBehaviour
    {
        [SerializeField] private PlayerInput input;
        [SerializeField] private UnityEvent onOpen;
        [SerializeField] private UnityEvent onClose;

        private bool _isOpen;

        private void Start()
        {
            SetUpControls();
        }

        private void SetUpControls()
        {
            input.actions["ToggleMenu"].performed += ToggleMenu;
        }
        
        private void OnDisable()
        {
            if (input != null) input.actions["ToggleMenu"].performed -= ToggleMenu;
        }

        private void ToggleMenu(InputAction.CallbackContext ctx)
        {
            if (_isOpen)
            {
                CloseMenu();
            }
            else
            {
                onOpen?.Invoke();
                _isOpen = true;
                Cursor.visible = true;
            }
        }

        public void CloseMenu()
        {
            if (!_isOpen) return;
            
            Cursor.visible = false;
            _isOpen = false;
            onClose?.Invoke();
        }
    }
}
