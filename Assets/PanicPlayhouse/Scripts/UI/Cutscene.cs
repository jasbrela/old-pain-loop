using System.Collections;
using System.Collections.Generic;
using PanicPlayhouse.Scripts.Camera;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace PanicPlayhouse.Scripts.UI
{
    public class Cutscene : MonoBehaviour
    {
        [SerializeField] private List<Image> images;
        [SerializeField] private GameObject tutorial;
        [SerializeField] private CameraFade fade;
        [SerializeField] private int secondsPerScene;
        [SerializeField] private SceneLoader loader;
        
        private int _currentScene;

        private void Start()
        {
            tutorial.SetActive(false);
            StartCoroutine(StartCutscene());
        }
        
        private IEnumerator StartCutscene()
        {
            while (_currentScene != images.Count)
            {
                fade.FadeOut();
                yield return new WaitForSeconds(fade.Duration);
                yield return new WaitForSeconds(secondsPerScene);
                fade.FadeIn();
                yield return new WaitForSeconds(fade.Duration);
                images[_currentScene].gameObject.SetActive(false);
                _currentScene++;
            }
            loader.PreLoadNextScene();
            tutorial.SetActive(true);
        }
    }
}
