using System.Collections;
using PanicPlayhouse.Scripts.ScriptableObjects;
using UnityEngine;
using Event = PanicPlayhouse.Scripts.ScriptableObjects.Event;

namespace PanicPlayhouse.Scripts.Player
{
    public class PlayerInsanity : MonoBehaviour
    {
        [SerializeField] private FloatVariable insanity;
        [Range(0, 100)] [SerializeField] private float insanityOnRespawn;
        [SerializeField] private Vector3Variable checkpoint;
        [SerializeField] private Event onRespawn;

        private void Start()
        {
            if (!checkpoint.IsValid) checkpoint.Value = transform.position;
        }

        public void OnGoInsane() // max insanity
        {
            StartCoroutine(Respawn());
        }

        IEnumerator Respawn()
        {
            yield return new WaitForSeconds(2);
            transform.position = checkpoint.Value;
            yield return new WaitForSeconds(1);
            onRespawn.Raise();
            insanity.Value = insanityOnRespawn;
        }
    }
    
}
