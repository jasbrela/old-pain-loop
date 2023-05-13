using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace PanicPlayhouse.Scripts.UI
{
    public class DiaryText : MonoBehaviour
    {
        [SerializeField] private List<TextMeshProUGUI> paragraphs;
        [SerializeField] private float fadeDuration;
        [SerializeField] private GameObject buttons;
        void Start()
        {
            buttons.SetActive(false);
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(0.5f);
        
            foreach (TextMeshProUGUI paragraph in paragraphs)
            {
                seq.Append(paragraph.DOFade(1, fadeDuration));
                seq.AppendInterval(fadeDuration);
            }
        
            seq.onComplete += () =>
            {
                buttons.SetActive(true);
            };
        }
    }
}
