using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PanicPlayhouse.Scripts.UI
{
    public class SceneLoader : MonoBehaviour
    {
        private RichPresence _richPresence;
        
        private void Start()
        {
            _richPresence = FindObjectOfType<RichPresence>();
        }

        public void LoadNextScene()
        {
            int scene = SceneManager.GetActiveScene().buildIndex + 1;
            StartCoroutine(WaitThenLoad(scene));
        }

        public void LoadMenuScene()
        {
            StartCoroutine(WaitThenLoad(0));
        }

        IEnumerator WaitThenLoad(int index)
        {
            yield return new WaitForSeconds(0.5f);
            
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
            
            SceneManager.LoadScene(index);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
