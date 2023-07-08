using NaughtyAttributes;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Chunk
{
    public abstract class Interactable : MonoBehaviour
    {
        [Tooltip("'False' will switch sprites instead of material")]
        [SerializeField] private bool switchMat = true;

        [HideIf("switchMat")][SerializeField] private Sprite hoverSprite;
        [ShowIf("switchMat")][SerializeField] private Material hoverMaterial;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private Material _defaultMaterial;
        private Sprite _defaultSprite;
        private void Start()
        {
            if (spriteRenderer == null) return;

            if (switchMat)
                _defaultMaterial = spriteRenderer.sharedMaterial;
            else
                _defaultSprite = spriteRenderer.sprite;
        }

        public virtual void OnInteract() { }

        public virtual void OnEnterRange()
        {
            if (spriteRenderer == null) return;

            if (switchMat)
                spriteRenderer.material = hoverMaterial;
            else
                spriteRenderer.sprite = hoverSprite;

        }

        public virtual void OnQuitRange()
        {
            if (spriteRenderer == null) return;

            if (switchMat)
                spriteRenderer.material = _defaultMaterial;
            else
                spriteRenderer.sprite = _defaultSprite;
        }
    }
}
