using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PanicPlayhouse.Scripts.UI
{
    public class SceneLoader : MonoBehaviour
    {
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

        IEnumerator WaitThenLoad(int index)
        {
            yield return new WaitForSeconds(0.5f);

#if !UNITY_WEBGL
            if (_richPresence != null) {
                switch (index)
                {
                    case 0:
                        _richPresence.Details = "In the menu";
                        break;
                    case 1:
                        _richPresence.Details = "Getting scared";
                        break;
                    case 2:
                        _richPresence.Details = "Reading the diary";
                        break;
                }
            }
#endif
            SceneManager.LoadScene(index);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
