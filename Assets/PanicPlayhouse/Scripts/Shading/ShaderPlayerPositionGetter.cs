using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPlayhouse.Scripts.Entities.Player;

namespace PanicPlayhouse.Scripts.Shading
{
    public class ShaderPlayerPositionGetter : MonoBehaviour
    {
        [SerializeField] PlayerMovement player;
        [SerializeField] List<Material> materialsToUpdate = new List<Material>();

        private Vector3 playerOriginalSceneLocation = Vector3.zero;

        void Awake()
        {
            playerOriginalSceneLocation = player.transform.position;
        }

        void Update()
        {
            foreach (Material material in materialsToUpdate)
            {
                material.SetVector("_PlayerPosition", player.transform.position);
            }
        }

        void OnDestroy()
        {
            foreach (Material material in materialsToUpdate)
            {
                material.SetVector("_PlayerPosition", playerOriginalSceneLocation);
            }
        }
    }
}

