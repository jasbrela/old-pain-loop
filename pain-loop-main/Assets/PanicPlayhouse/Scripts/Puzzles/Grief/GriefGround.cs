using System;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Puzzles.Grief
{
    public class GriefGround : MonoBehaviour
    {
        [SerializeField] private float correctY;

        private float CorrectY => Quaternion.Euler(0, correctY, 0).eulerAngles.y;
        public bool IsCorrect => Math.Abs(transform.localRotation.eulerAngles.y - CorrectY) < 1;

        public void Rotate()
        {
            transform.Rotate(Vector3.up, 90);
        }
    }
}
