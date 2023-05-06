using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace PanicPlayhouse.Scripts
{
    public class GameSprite : MonoBehaviour
    {
        [SerializeField] private Material outline;

        private SpriteRenderer _renderer;
        private Material _defaultMat;

        private void Start()
        {
            gameObject.TryGetComponent(out _renderer);
            _defaultMat = _renderer.material;
        }

        [Button]
        private void FadeOut()
        {
            _renderer.material = outline;
            _renderer.DOFade(0, 1);
        }
        
        [Button]
        private void FadeIn()
        {
            _renderer.material = _defaultMat;
            _renderer.DOFade(1, 1);
        }
    }
}
