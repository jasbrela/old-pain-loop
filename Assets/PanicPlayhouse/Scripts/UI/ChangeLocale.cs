using System.Collections;
using FMODUnity;
using PanicPlayhouse.Scripts.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace PanicPlayhouse.Scripts.UI
{
    public class ChangeLocale : MonoBehaviour
    {
        [SerializeField] private EventReference click;
        [SerializeField] private TextMeshProUGUI display;
        [SerializeField] private Button left;
        [SerializeField] private Button right;
        
        private int _index;
        private AudioManager _audio;
        
        void Start()
        {
            _audio = FindObjectOfType<AudioManager>();
            UpdateDisplay();
        }

        private IEnumerator EnableButtons()
        {
            yield return new WaitForSeconds(0.6f);
            left.enabled = true;
            right.enabled = true;
        }

        public void OnClickLeft()
        {
            left.enabled = false;
            right.enabled = false;
            
            _audio.PlayOneShot(click);
            _index--;
            if (_index < 0) _index = LocalizationSettings.AvailableLocales.Locales.Count - 1;
            SetLocale(_index);
        }
        
        public void OnClickRight()
        {
            left.enabled = false;
            right.enabled = false;
            
            _audio.PlayOneShot(click);
            _index++;
            if (_index >= LocalizationSettings.AvailableLocales.Locales.Count) _index = 0;
            SetLocale(_index);
        }

        private void SetLocale(int index)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
            PlayerPrefs.SetInt("locale", index);
            UpdateDisplay();
            StartCoroutine(EnableButtons());
        }

        private void UpdateDisplay()
        {
            if (display != null)
                display.text = LocalizationSettings.SelectedLocale.LocaleName;
        }
    }
}
