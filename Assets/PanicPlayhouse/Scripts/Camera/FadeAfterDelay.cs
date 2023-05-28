using System.Collections;
using DG.Tweening;
using PanicPlayhouse.Scripts.Entities.Player;
using PanicPlayhouse.Scripts.UI;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace PanicPlayhouse.Scripts.Camera
{
    public class FadeAfterDelay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Image image;
        [SerializeField] private float delay;
        
        [Header("OPTIONAL")]
        [SerializeField] private TextMeshProUGUI soon;
        [SerializeField] private SceneLoader loader;
        [SerializeField] private PlayerMovement player;
        
        void Start()
        {
            StartCoroutine(Fade());
        }

        private IEnumerator Fade()
        {
            yield return new WaitForSeconds(delay);
            
            if (soon != null)
            {
                soon.DOFade(1, 1f);
                yield return new WaitForSeconds(2f);
                soon.DOFade(0, 0.5f);
            }
            
            text.DOFade(0, 0.5f);
            yield return new WaitForSeconds(0.5f);
            
            if (loader != null)
            {
                loader.LoadNextScene();
                yield break;
            }
            
            player.UnlockMovement();
            image.DOFade(0, 0.5f);
            image.gameObject.SetActive(false);
        }
    }
}
