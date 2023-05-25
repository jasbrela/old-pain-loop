using NaughtyAttributes;
using UnityEngine;
using Event = PanicPlayhouse.Scripts.ScriptableObjects.Event;

namespace PanicPlayhouse.Scripts.Chunk
{
    public class Hideout : Interactable
    {
        [SerializeField] private Event enter;
        [SerializeField] [ReadOnly] private bool isHidden;

        public override void OnInteract()
        {
            if (isHidden) return;
            
            isHidden = true;
            enter.Raise();
        }

        public void OnExitHideout()
        {
            isHidden = false;
        }
    }
}
