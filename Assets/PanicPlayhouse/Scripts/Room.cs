using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace PanicPlayhouse.Scripts
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private bool useCamPerRoom;
        [SerializeField] private GameObject virtualCam;
        private List<GameSprite> _objects = new();

        private void Start()
        {
            if (!useCamPerRoom) virtualCam.SetActive(false);
            _objects = new List<GameSprite>(GetComponentsInChildren<GameSprite>());
        }

        private void OnTriggerEnter(Collider other)
        {
            foreach (GameSprite obj in _objects)
            {
                obj.FadeIn();
            }

            if (!useCamPerRoom) return;
            if (other.CompareTag("Player") && !other.isTrigger) virtualCam.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            foreach (GameSprite obj in _objects)
            {
                obj.FadeOut();
            }
            
            if (!useCamPerRoom) return;
            if (other.CompareTag("Player") && !other.isTrigger) virtualCam.SetActive(false);
        }
    }
}
