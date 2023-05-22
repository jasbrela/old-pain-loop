using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PanicPlayhouse.Scripts.UI
{
    public class AudioOption : MonoBehaviour
    {
        [SerializeField] private string key = "_volume";
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI text;

        private void Start()
        {
            slider.value = PlayerPrefs.GetFloat(key, 1f);
        }

        public void OnChangeValue(float value)
        {
            text.text = $"{Mathf.Ceil(value * 100)}%";
        }
    }
}
