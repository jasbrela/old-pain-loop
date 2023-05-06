using UnityEngine;

namespace PanicPlayhouse.Scripts
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private GameObject virtualCam;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !other.isTrigger) virtualCam.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && !other.isTrigger) virtualCam.SetActive(false);
        }
    }
}
