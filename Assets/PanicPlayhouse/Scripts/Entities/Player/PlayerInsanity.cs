using System.Collections;
using PanicPlayhouse.Scripts.ScriptableObjects;
using UnityEngine;
using Event = PanicPlayhouse.Scripts.ScriptableObjects.Event;

namespace PanicPlayhouse.Scripts.Entities.Player
{
    public class PlayerInsanity : MonoBehaviour
    {
        [SerializeField] private FloatVariable insanity;
        [Range(0, 100)] [SerializeField] private float insanityOnRespawn;
        [SerializeField] private Vector3Variable checkpoint;
        [SerializeField] private Event onRespawn;
        [SerializeField] private Event onGoInsane;
        private bool _goneInsane;

        private void Start()
        {
            if (!checkpoint.IsValid) checkpoint.Value = transform.position;
        }

        public void OnGoInsane() // max insanity
        {
            if (_goneInsane) return;
            _goneInsane = true;
            onGoInsane.Raise();
            StartCoroutine(Respawn());
        }

        IEnumerator Respawn()
        {
            yield return new WaitForSeconds(2);
            transform.position = checkpoint.Value;
            yield return new WaitForSeconds(1);
            _goneInsane = false;
            onRespawn.Raise();
            insanity.Value = insanityOnRespawn;
        }
    }
    
}
