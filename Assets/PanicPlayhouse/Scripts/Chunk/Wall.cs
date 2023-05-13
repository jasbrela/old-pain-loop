using UnityEngine;

namespace PanicPlayhouse.Scripts.Chunk
{
    public class Wall : MonoBehaviour
    {
        [SerializeField] private GameObject toHide;
        [SerializeField] private GameObject toShow;
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        public void Unlock()
        {
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            toHide.SetActive(false);
            toShow.SetActive(true);
        }
    }
}
