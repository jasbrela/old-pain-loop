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

            for (var index = 0; index < paragraphs.Count; index++)
            {
                var paragraph = paragraphs[index];
                seq.Append(paragraph.DOFade(1, fadeDuration));
                
                if (index != paragraphs.Count - 1)
                    seq.AppendInterval(fadeDuration);
            }

            seq.onComplete += () =>
            {
                buttons.SetActive(true);
            };
        }
    }
}
