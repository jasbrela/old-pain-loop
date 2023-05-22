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

        public void Ready(GameObject obj)
        {
            bool ready = true;
        
            foreach (EssentialSystem sys in systems)
            {
                if (sys.gameObject == obj)
                {
#if UNITY_EDITOR
                    Debug.Log($"{obj.name}".Bold().Color("#00FA9A") + " system is ready");
#endif
                    sys.ready = true;
                }

                ready = sys.ready;
            }
        
            if (ready) loader.LoadNextScene();
        }
    }
}
