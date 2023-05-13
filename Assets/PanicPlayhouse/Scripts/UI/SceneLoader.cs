using UnityEngine;
using UnityEngine.SceneManagement;

namespace PanicPlayhouse.Scripts.UI
{
    public class SceneLoader : MonoBehaviour
    {
        public void LoadNextScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
