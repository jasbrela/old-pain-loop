using System.Collections.Generic;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Chunk
{
    public class ChangeMaterialOnEvent : MonoBehaviour
    {
        [SerializeField] private Material changeTo;
        [SerializeField] private List<MeshRenderer> toChange = new();

        public void ChangeMaterial()
        {
            foreach (MeshRenderer obj in toChange)
            {
                obj.material = changeTo;
            }
        }
    }
}
