using FMODUnity;
using PanicPlayhouse.Scripts.Audio;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PanicPlayhouse.Scripts.UI
{
    public class ButtonAudio : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private EventReference chalk;
        [SerializeField] private EventReference click;
        private AudioManager _audio;
        
        private void Start()
        {
            _audio = FindObjectOfType<AudioManager>();
        }

        public void OnClick()
        {
            if (_audio != null) _audio.PlayOneShot(click);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_audio != null) _audio.PlayOneShot(chalk);
        }
    }
}
