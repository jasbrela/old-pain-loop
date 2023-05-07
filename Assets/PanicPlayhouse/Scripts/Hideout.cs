using PanicPlayhouse.Scripts.Interfaces;
using UnityEngine;
using Event = PanicPlayhouse.Scripts.ScriptableObjects.Event;


namespace PanicPlayhouse.Scripts
{
    public class Hideout : Interactable
    {
        [SerializeField] private Event enter;
        [SerializeField] private Event exit;
        private bool _isHidden;

        public override void OnInteract()
        {
            _isHidden = !_isHidden;

            if (_isHidden)
            {
                enter.Raise();
            }
            else
            {
                exit.Raise();
            }
        }
    }
}
