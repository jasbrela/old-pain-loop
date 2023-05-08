using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace PanicPlayhouse.Scripts.Camera
{
    public class CameraFade : MonoBehaviour
    {
        [SerializeField] private Image blackImage;
        private Material _defaultMat;

        private void Start()
        {
            var temp = blackImage.color;
            temp.a = 0f;
            blackImage.color = temp;
        }

        public void FadeOut()
        {
        
            blackImage.DOFade(0, 0.25f);
        }
        
        public void FadeIn()
        {
            blackImage.DOFade(1, 0.25f);
        }
    }
}
