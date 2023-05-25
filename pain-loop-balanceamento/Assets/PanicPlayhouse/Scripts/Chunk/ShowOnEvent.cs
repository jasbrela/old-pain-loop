using System.Collections.Generic;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Chunk
{
    public class ShowOnEvent : MonoBehaviour
    {
        [SerializeField] private List<GameObject> toShow = new List<GameObject>();

        public void Show()
        {
            foreach (GameObject obj in toShow)
            {
                obj.SetActive(true);
            }
        }
    }
}
