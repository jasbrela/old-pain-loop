using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Chunk
{
    public class DisableOnStart : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(Hide());
        }

        IEnumerator Hide()
        {
            yield return new WaitForSeconds(1);
            gameObject.SetActive(false);
        }
    }
}
