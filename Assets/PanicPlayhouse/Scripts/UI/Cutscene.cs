using System.Collections;
using System.Collections.Generic;
using PanicPlayhouse.Scripts.Camera;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PanicPlayhouse.Scripts.UI
{
    public class Cutscene : MonoBehaviour
    {
        [SerializeField] private PlayerInput input;
        [SerializeField] private GameObject imagesParent;
        [SerializeField] private List<GameObject> images;
        [SerializeField] private int secondsPerScene;
        [SerializeField] private GameObject tutorial;
        [SerializeField] private CameraFade fade;
        [SerializeField] private SceneLoader loader;

        private int _currentScene;
        private Coroutine _coroutine;

        private void Start()
        {
            tutorial.SetActive(false);
            loader.PreLoadNextScene();
            _coroutine = StartCoroutine(StartCutscene());
            
            SetUpControls();
        }
        
        private void SetUpControls()
        {
            input.actions["Skip"].performed += Skip;
        }

        private void OnDisable()
        {
            if (input == null) return;
            DisableControls();
        }

        private void DisableControls()
        {
            input.actions["Skip"].performed -= Skip;
        }

        private void Skip(InputAction.CallbackContext ctx)
        {
#if UNITY_EDITOR
            Debug.Log("Cutscene: ".Bold() + "Skipping cutscene...");
#endif
            DisableControls();
            StopCoroutine(_coroutine);
            StartCoroutine(OnSkip());
        }


        private IEnumerator StartCutscene()
        {
            while (_currentScene < images.Count - 1)
            {
                _currentScene++;
                fade.FadeOut();
                yield return new WaitForSeconds(fade.Duration);
                yield return new WaitForSeconds(secondsPerScene);
                fade.FadeIn();
                yield return new WaitForSeconds(fade.Duration);
                images[_currentScene].SetActive(true);
            }

            ShowTutorial();
        }

        private IEnumerator OnSkip()
        {
            fade.FadeIn();
            yield return new WaitForSeconds(fade.Duration);
            imagesParent.SetActive(false);
            ShowTutorial();
            fade.FadeOut();
            yield return new WaitForSeconds(fade.Duration);
        }

        private void ShowTutorial()
        {
            Cursor.visible = true;
            DisableControls();
            tutorial.SetActive(true);
        }
    }
}
