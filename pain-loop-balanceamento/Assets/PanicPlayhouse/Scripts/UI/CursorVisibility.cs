using UnityEngine;

namespace PanicPlayhouse.Scripts.UI
{
    public class CursorVisibility : MonoBehaviour
    {
        [Tooltip("When false, will show on start")]
        [SerializeField] private bool hideOnStart = true;
        
        void Start()
        {
            Cursor.visible = !hideOnStart;
        }
    }
}
