using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Audio
{
    public class FootstepsAudio : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> footsteps;
        [SerializeField] private float interval;
    
        private int _footstep;

        private AudioSource _src;
        public bool IsMoving { get; set; }
        void Start()
        {
            if (!TryGetComponent(out _src))
                _src = gameObject.AddComponent<AudioSource>();

            StartCoroutine(Footsteps());
        }

        private IEnumerator Footsteps()
        {
            while (true)
            {
                while (!IsMoving) yield return null;
                
                _src.PlayOneShot(footsteps[_footstep]);
                _footstep++;
                if (_footstep >= footsteps.Count) _footstep = 0;
            
                yield return new WaitForSeconds(interval);
            }
        }
    }
}
