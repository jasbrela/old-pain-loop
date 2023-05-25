using UnityEngine;

namespace PanicPlayhouse.Scripts.UI
{
    public class DisableIfWebGLBuild : MonoBehaviour
    {
        void Start()
        {
#if UNITY_WEBGL
            gameObject.SetActive(false);
#else
            gameObject.SetActive(true);
#endif
        }
    }
}
