using UnityEngine;

namespace PanicPlayhouse.Scripts.Puzzles.Grief
{
    public class GriefGround : MonoBehaviour
    {
        [SerializeField] private float correctY;

        private float CorrectY => Quaternion.Euler(0, correctY, 0).eulerAngles.y;
        public bool IsCorrect => transform.eulerAngles.y == CorrectY;

        public void Rotate()
        {
            transform.Rotate(Vector3.up, 90);
            Debug.Log("Rotated ground to: ".Bold() + transform.eulerAngles.y + ". Should be:".Bold() + CorrectY + "Is it Correct? ".Bold() + IsCorrect);
        }
    }
}
