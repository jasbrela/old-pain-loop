using System.Collections;
using FMOD;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PanicPlayhouse.Scripts.UI
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private GameObject disableOnLoad;
        private RichPresence _richPresence;
        private AsyncOperation _asyncOperation;

        private int NextScene => SceneManager.GetActiveScene().buildIndex + 1;

        private void Start()
        {
            _richPresence = FindObjectOfType<RichPresence>();
        }

        public void LoadNextScene()
        {
            if (_asyncOperation is {isDone: true})
            {
                DisableObjectOnLoad();
                _asyncOperation.allowSceneActivation = true;
                return;
            }
            
            StartCoroutine(WaitThenLoad(NextScene));
        }

        public void PreLoadNextScene()
        {
            StartCoroutine(LoadSceneAsyncProcess());
        }
        
        private IEnumerator LoadSceneAsyncProcess()
        {
            _asyncOperation = SceneManager.LoadSceneAsync(NextScene);
            _asyncOperation.allowSceneActivation = false;

            while (!_asyncOperation.isDone) yield return null;
        }

        public void LoadMenuScene()
        {
            StartCoroutine(WaitThenLoad(0));
        }

        private void DisableObjectOnLoad()
        {
#if UNITY_WEBGL
            if (disableOnLoad != null) disableOnLoad.SetActive(false);
#endif
        }

        IEnumerator WaitThenLoad(int index)
        {

            DisableObjectOnLoad();

            yield return new WaitForSeconds(0.5f);

#if !UNITY_WEBGL
            if (_richPresence != null) {
                switch (index)
                {
                    case 0: // BOOT
                        _richPresence.State = "Loading...";
                        _richPresence.Details = "\"Why is it taking so long?\"";
                        break;
                    case 1: // MAIN MENU
                        _richPresence.State = "In the menu";
                        _richPresence.Details = "\"What am I doing here?\"";
                        break;
                    case 2: // CUTSCENE, TUTORIAL
                        _richPresence.State = "In the tutorial";
                        _richPresence.Details = "Getting ready!";
                        break;
                    case 3: // FIRST PHASE
                        _richPresence.State = "I. Denial and Isolation";
                        _richPresence.Details = "In my room";
                        break;
                    case 4: // LAST PHASE
                        _richPresence.State = "II. Anger";
                        _richPresence.Details = "Getting angrier...";
                        break;
                    case 5: // END GAME
                        _richPresence.State = "In the menu";
                        _richPresence.Details = "Just finished the game!";
                        break;

                }
            }
#endif
            StopAllSounds();
            SceneManager.LoadScene(index);
        }

        private static void StopAllSounds()
        {
            FMODUnity.RuntimeManager.CoreSystem.getMasterChannelGroup(out ChannelGroup group);
            group.stop();
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
