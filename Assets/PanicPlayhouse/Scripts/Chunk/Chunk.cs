using System.Collections.Generic;
using Cinemachine;
using NaughtyAttributes;
using PanicPlayhouse.Scripts.Puzzles.GoldenBeadMaterial;
using UnityEngine;
using Event = PanicPlayhouse.Scripts.ScriptableObjects.Event;
using Random = UnityEngine.Random;

namespace PanicPlayhouse.Scripts.Chunk
{
    public class Chunk : MonoBehaviour
    {
        [SerializeField] private Event onEnterChunk;
        [SerializeField] private bool useCamPerRoom;
        [ShowIf("useCamPerRoom")] [SerializeField] private CinemachineVirtualCamera virtualCam;
        
        private List<ChunkEntity> _entities = new();
        private int _id;
        private bool _isPlayerInside;
        
        private void Start()
        {
            _id = Random.Range(0, 1000);
            if (virtualCam != null && !useCamPerRoom) virtualCam.gameObject.SetActive(false);
            
            _entities = new List<ChunkEntity>(GetComponentsInChildren<ChunkEntity>());
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag("Player"))
            {
                _isPlayerInside = true;
                
                gameObject.name = "Chunk (Visible) " + _id;
                
                if (onEnterChunk != null)
                    onEnterChunk.Raise();

                foreach (ChunkEntity obj in _entities)
                    obj.FadeIn();

                if (!useCamPerRoom)
                    return;
                
                if (!other.isTrigger)
                    virtualCam.gameObject.SetActive(true);
                
            } else if (other.TryGetComponent(out Pushable entity))
            {
                foreach (ChunkEntity child in entity.GetComponentsInChildren<ChunkEntity>())
                {
                    if (!_entities.Contains(child)) _entities.Add(child);
                    if (_isPlayerInside) child.FadeIn();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _isPlayerInside = false;
                gameObject.name = "Chunk (Hidden) " + _id;
                
                foreach (ChunkEntity obj in _entities)
                    obj.FadeOut();

                if (!useCamPerRoom)
                    return;
                
                if (!other.isTrigger)
                    virtualCam.gameObject.SetActive(false);
                
            } else if (other.TryGetComponent(out Pushable entity))
            {
                foreach (ChunkEntity child in entity.GetComponentsInChildren<ChunkEntity>())
                {
                    _entities.Remove(child);
                    if (_isPlayerInside) child.FadeOut();
                }
            }
        }
    }
}
