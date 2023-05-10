using UnityEngine;

namespace PanicPlayhouse.Scripts.Chunk
{
    public class Wall : MonoBehaviour
    {
        [SerializeField] private GameObject toHide;
        [SerializeField] private GameObject toShow;
        
        public void Unlock()
        {
            toHide.SetActive(false);
            toShow.SetActive(true);
        }
    }
}
