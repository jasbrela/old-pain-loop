using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace PanicPlayhouse.Scripts.Camera
{
    public class CameraFade : MonoBehaviour
    {
        private Image _renderer;
        private Material _defaultMat;

        private void Start()
        {
            _renderer = GetComponentInChildren<Image>();
            
            var temp = _renderer.color;
            temp.a = 0f;
            _renderer.color = temp;
        }

        public void FadeOut()
        {
        
            _renderer.DOFade(0, 0.25f);
        }
        
        public void FadeIn()
        {
            _renderer.DOFade(1, 0.25f);
        }
    }
}
