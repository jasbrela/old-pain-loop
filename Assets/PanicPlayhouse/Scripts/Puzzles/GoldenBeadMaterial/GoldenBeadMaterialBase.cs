using System.Collections.Generic;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Puzzles.GoldenBeadMaterial
{
    public class GoldenBeadMaterialBase : MonoBehaviour
    {
        [SerializeField] private int correctValue;

        public GoldenBeadMaterialPuzzle Puzzle
        {
            get => _puzzle;
            set
            {
                if (_puzzle == null)
                    _puzzle = value;
            }
        }

        private GoldenBeadMaterialPuzzle _puzzle;
        private int _currentValue;

        public bool IsCorrect => correctValue == CurrentValue;

        private int CurrentValue
        {
            get
            {
                _currentValue = 0;
                foreach (PuzzlePickupable pickupable in _inside)
                    _currentValue += pickupable.Value;

                return _currentValue;
            }
        }

        private readonly List<PuzzlePickupable> _inside = new();

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out PuzzlePickupable pickup)) return;

            if (pickup.pickedUp)
                return;

            _inside.Add(pickup);
            Puzzle.OnPressBase(this);
        }
        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out PuzzlePickupable pickup)) return;

            if (pickup.pickedUp)
                return;

            _inside.Remove(pickup);
            Puzzle.OnReleaseBase(this);
        }
    }
}