using DG.Tweening;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Chunk
{
    public class ChunkEntity : MonoBehaviour
    {
        [SerializeField] private Material outline;
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Material _defaultMat;

        private void Start()
        {
            _defaultMat = spriteRenderer.material;
            
            var temp = spriteRenderer.color;
            temp.a = 0f;
            spriteRenderer.color = temp;
        }

        public void FadeOut()
        {
            spriteRenderer.material = outline;
            spriteRenderer.DOFade(0, 0.25f);
        }
        
        public void FadeIn()
        {
            spriteRenderer.material = _defaultMat;
            spriteRenderer.DOFade(1, 0.25f);
        }
    }
}
