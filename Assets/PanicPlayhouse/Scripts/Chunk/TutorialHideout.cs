using System.Collections;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Chunk
{
    public class TutorialHideout : Hideout
    {
        [SerializeField] private float delayToOpen = 0.5f;
        [SerializeField] private Wall openWall;
        private bool _interacted = false;
        public override void OnInteract()
        {
            if (_interacted) return;
            
            _interacted = true;
            base.OnInteract();
            StartCoroutine(OpenWall());
        }

        private IEnumerator OpenWall()
        {
            yield return new WaitForSeconds(delayToOpen);
            
            openWall.Unlock();
        }
    }
}
