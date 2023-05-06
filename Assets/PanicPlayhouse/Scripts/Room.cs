using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PanicPlayhouse.Scripts
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private GameObject virtualCam;
        private List<GameSprite> _objects = new();

        private void Start()
        {
            _objects = new List<GameSprite>(GetComponentsInChildren<GameSprite>());
        }

        private void OnTriggerEnter(Collider other)
        {
            foreach (GameSprite obj in _objects)
            {
                obj.FadeIn();
            }

            if (other.CompareTag("Player") && !other.isTrigger) virtualCam.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            foreach (GameSprite obj in _objects)
            {
                obj.FadeOut();
            }
            
            if (other.CompareTag("Player") && !other.isTrigger) virtualCam.SetActive(false);
        }
    }
}
