using DG.Tweening;
using TMPro;
using UnityEngine;

namespace PanicPlayhouse.Scripts.UI
{
    public class TextFadeAfterDelay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI toFade;

        [SerializeField] private float delay;

        void Start()
        {
            toFade.DOFade(0, delay);
        }
    }
}
