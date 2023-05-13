using UnityEngine;
using UnityEngine.EventSystems;

namespace PanicPlayhouse.Scripts.UI
{
    public class SoundOnHover : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private AudioSource source;
        [SerializeField] private AudioClip clip;

        public void OnPointerEnter(PointerEventData eventData)
        {
            source.PlayOneShot(clip);
        }
    }
}
