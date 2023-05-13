using System.Collections;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Chunk
{
    public class DisableOnStart : MonoBehaviour
    {
        void Awake()
        {
            if (TryGetComponent(out SpriteRenderer sp))
            {
                var color = sp.color;
                color.a = 0.0f;
                sp.color = color;
            }
            
            StartCoroutine(Hide());
        }

        IEnumerator Hide()
        {
            yield return new WaitForSeconds(0.1f);
            if (TryGetComponent(out SpriteRenderer sp))
            {
                var color = sp.color;
                color.a = 0.25f;
                sp.color = color;
            }
            gameObject.SetActive(false);
        }
    }
}
