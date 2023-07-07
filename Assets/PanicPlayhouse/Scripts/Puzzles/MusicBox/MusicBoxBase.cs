using System.Collections.Generic;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Puzzles.MusicBox
{
    public class MusicBoxBase : MonoBehaviour
    {
        [SerializeField] private MusicBoxPuzzle puzzle;

        private MusicBoxPuzzle _puzzle;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out MusicBox musicBox)) return;

            if (musicBox.PickedUp)
                return;

            puzzle.OnPressBase();
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out MusicBox pickup)) return;

            if (pickup.PickedUp)
                return;

            puzzle.OnReleaseBase();
        }
    }
}