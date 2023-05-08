using System.Collections.Generic;
using Cinemachine;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using Event = PanicPlayhouse.Scripts.ScriptableObjects.Event;

namespace PanicPlayhouse.Scripts.Chunk
{
    public class Chunk : MonoBehaviour
    {
        [SerializeField] private Event onEnterChunk;
        [SerializeField] private bool useCamPerRoom;
        [ShowIf("useCamPerRoom")] [SerializeField] private CinemachineVirtualCamera virtualCam;
        
        private List<ChunkEntity> _entities = new();
        
        private void Start()
        {
            if (virtualCam != null && !useCamPerRoom) virtualCam.gameObject.SetActive(false);
            
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
            if (other.CompareTag("Player") && !other.isTrigger) virtualCam.gameObject.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            gameObject.name = "Chunk (Hidden)";

            foreach (ChunkEntity obj in _entities)
            {
                obj.FadeOut();
            }
            
            if (!useCamPerRoom) return;
            if (other.CompareTag("Player") && !other.isTrigger) virtualCam.gameObject.SetActive(false);
        }
    }
}
