using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace PanicPlayhouse.Scripts.Camera
{
    public class CameraFade : MonoBehaviour
    {
        [SerializeField] private bool fadeOnStart = true;
        [SerializeField] private float fadeDuration = 0.25f;
        [SerializeField] private Image blackImage;
        private Material _defaultMat;

        public float Duration => fadeDuration;
        private void Start()
        {
            if (!fadeOnStart) return;
            var temp = blackImage.color;
            temp.a = 1f;
            blackImage.color = temp;
            
            blackImage.DOFade(0, 1f);
        }
        
        /// <summary>
        /// Set's the black image's alpha to 0
        /// </summary>
        public void FadeOut()
        {
            blackImage.DOFade(0, fadeDuration);
        }
        
        /// <summary>
        /// Set's the black image's alpha to 1
        /// </summary>
        public void FadeIn()
        {
            blackImage.DOFade(1, fadeDuration);
        }
    }
}
