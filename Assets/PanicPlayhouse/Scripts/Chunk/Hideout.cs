using UnityEngine;
using Event = PanicPlayhouse.Scripts.ScriptableObjects.Event;


namespace PanicPlayhouse.Scripts.Chunk
{
    public class Hideout : Interactable
    {
        [SerializeField] private Event enter;
        private bool _isHidden;

        public override void OnInteract()
        {
            _isHidden = !_isHidden;

            if (_isHidden)
            {
                enter.Raise();
            }
        }

        public void OnExitHideout()
        {
            _isHidden = false;
        }
    }
}
