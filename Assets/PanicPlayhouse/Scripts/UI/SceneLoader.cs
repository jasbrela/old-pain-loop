using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PanicPlayhouse.Scripts.UI
{
    public class SceneLoader : MonoBehaviour
    {
        public void LoadNextScene()
        {
            StartCoroutine(WaitThenLoad(SceneManager.GetActiveScene().buildIndex + 1));
        }

        public void LoadMenuScene()
        {
            StartCoroutine(WaitThenLoad(0));
        }

        IEnumerator WaitThenLoad(int index)
        {
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene(index);
        }
    }
}
