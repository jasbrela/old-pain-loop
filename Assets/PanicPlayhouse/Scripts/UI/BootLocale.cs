using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PanicPlayhouse.Scripts.UI
{
    public class BootLocale : MonoBehaviour
    {
        [SerializeField] private Boot boot;
        void Start()
        {
            var operation = LocalizationSettings.InitializationOperation;
            operation.Completed += Initialize;
        }
    
        private void Initialize(AsyncOperationHandle<LocalizationSettings> obj)
        {
            PlayerPrefs.DeleteKey("locale"); // TODO: Remove this line after fixing the localization
            var index = PlayerPrefs.GetInt("locale", LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale));
        
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
            PlayerPrefs.SetInt("locale", index);
            
            boot.Ready(gameObject);
        }
    }
}
