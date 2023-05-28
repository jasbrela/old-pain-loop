using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace PanicPlayhouse.Scripts.UI
{
    public class Boot : MonoBehaviour
    {
        [Serializable]
        public class EssentialSystem
        {
            public GameObject gameObject;
            [HideInInspector] public bool ready;
        }

        [SerializeField] private SceneLoader loader;
        [SerializeField] private List<EssentialSystem> systems;

        public void Ready(GameObject obj, bool failed = false)
        {
            bool ready = true;

            foreach (EssentialSystem sys in systems)
            {
                if (sys.gameObject == obj)
                {
#if UNITY_EDITOR
                    if (failed)
                    {
                        Debug.Log($"{obj.name}".Bold().Color("#FF4500") + " failed to load.");
                    }
                    else
                    {
                        Debug.Log($"{obj.name}".Bold().Color("#00FA9A") + " system is ready");
                    }
#endif
                    sys.ready = true;
                }

                if (!sys.ready) ready = false;
            }
        
            if (ready) loader.LoadNextScene();
        }
    }
}
