using TMPro;
using UnityEngine;

namespace PanicPlayhouse.Scripts.UI
{
    public class GameVersion : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
    
        void Start()
        {
            if (text == null) TryGetComponent(out text);
            text.text = $"Pain Loop {Application.version}";
        }

    }
}
