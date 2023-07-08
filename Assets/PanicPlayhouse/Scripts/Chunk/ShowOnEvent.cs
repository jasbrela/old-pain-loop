using System.Collections.Generic;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Chunk
{
    public class ShowOnEvent : MonoBehaviour
    {
        [SerializeField] private List<GameObject> toShow = new();

        public void Show()
        {
            foreach (GameObject obj in toShow)
            {
                if (obj == null) continue;
                obj.SetActive(true);
            }
        }
    }
}
