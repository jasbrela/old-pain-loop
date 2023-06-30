using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event = PanicPlayhouse.Scripts.ScriptableObjects.Event;

namespace PanicPlayhouse.Scripts.Chunk
{
    public class TutorialHideout : Hideout
    {
        [SerializeField] private float delayToOpen = 0.5f;
        [SerializeField] private Event onEnterTutorialHideout;
        
        private bool _interacted;
        
        public override void OnInteract()
        {
            if (_interacted) return;
            
            _interacted = true;
            base.OnInteract();
            StartCoroutine(AfterOnInteract());
        }

        private IEnumerator AfterOnInteract()
        {
            yield return new WaitForSeconds(delayToOpen);
            
            if (onEnterTutorialHideout != null) onEnterTutorialHideout.Raise();
        }
    }
}
