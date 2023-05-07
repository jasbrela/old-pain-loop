using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Chunk
{
    public class Chunk : MonoBehaviour
    {
        [SerializeField] private bool useCamPerRoom;
        private CinemachineVirtualCamera _virtualCam;
        private List<ChunkEntity> _entities = new();
        
        private void Start()
        {
            _virtualCam = GetComponentInChildren<CinemachineVirtualCamera>();
            if (_virtualCam != null && !useCamPerRoom) _virtualCam.gameObject.SetActive(false);
            
            _entities = new List<ChunkEntity>(GetComponentsInChildren<ChunkEntity>());
        }

        private void OnTriggerEnter(Collider other)
        {
            foreach (ChunkEntity obj in _entities)
            {
                obj.FadeIn();
            }

            if (!useCamPerRoom) return;
            if (other.CompareTag("Player") && !other.isTrigger) _virtualCam.gameObject.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            foreach (ChunkEntity obj in _entities)
            {
                obj.FadeOut();
            }
            
            if (!useCamPerRoom) return;
            if (other.CompareTag("Player") && !other.isTrigger) _virtualCam.gameObject.SetActive(false);
        }
    }
}
