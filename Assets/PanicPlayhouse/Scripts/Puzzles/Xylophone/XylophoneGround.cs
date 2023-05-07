using UnityEngine;

namespace PanicPlayhouse.Scripts.Puzzles.Xylophone
{
    public class XylophoneGround : MonoBehaviour
    {
        public void Rotate()
        {
            transform.Rotate(Vector3.up, 90);
        }
    }
}
