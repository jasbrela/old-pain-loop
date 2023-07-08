using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPlayhouse.Scripts.Entities.Player;

namespace PanicPlayhouse.Scripts.Shading
{
    public class ShaderPlayerPositionGetter : MonoBehaviour
    {
        public PlayerMovement player;
        public List<Material> materialsToUpdate = new List<Material>();

        void Update()
        {
            foreach (Material material in materialsToUpdate)
            {
                material.SetVector("_PlayerPosition", player.transform.position);
            }
        }
    }
}

