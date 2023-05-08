using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Event = PanicPlayhouse.Scripts.ScriptableObjects.Event;

namespace PanicPlayhouse.Scripts.Chunk
{
    public class Chunk : MonoBehaviour
    {
        [SerializeField] private bool useCamPerRoom;
        [SerializeField] private Event onEnterChunk;
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
            gameObject.name = "Chunk (Visible)";
            if (onEnterChunk != null) onEnterChunk.Raise();
            
            foreach (ChunkEntity obj in _entities)
            {
                obj.FadeIn();
            }

            if (!useCamPerRoom) return;
            if (other.CompareTag("Player") && !other.isTrigger) _virtualCam.gameObject.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            gameObject.name = "Chunk (Hidden)";

            foreach (ChunkEntity obj in _entities)
            {
                obj.FadeOut();
            }
            
            if (!useCamPerRoom) return;
            if (other.CompareTag("Player") && !other.isTrigger) _virtualCam.gameObject.SetActive(false);
        }
    }
}
