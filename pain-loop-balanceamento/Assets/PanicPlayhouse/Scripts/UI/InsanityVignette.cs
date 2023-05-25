using System;
using PanicPlayhouse.Scripts.ScriptableObjects;
using UnityEngine;

namespace PanicPlayhouse.Scripts.UI
{
    public class InsanityVignette : MonoBehaviour
    {
        [SerializeField] private RectTransform rect;
        [SerializeField] private FloatVariable insanity;
        [SerializeField] private Vector2 minInsanityScale = new(7000, 7000);
        private readonly Vector2 _maxInsanityScale = new(1920, 1920);

        private void Start()
        {
            ChangeVignette();
        }

        public void ChangeVignette()
        {
            var width = Mathf.Lerp(minInsanityScale.x, _maxInsanityScale.x, insanity.Percentage);
            var height = Mathf.Lerp(minInsanityScale.y, _maxInsanityScale.y, insanity.Percentage);
                        
            rect.sizeDelta = new Vector2(width, height);
        }
    }
}
