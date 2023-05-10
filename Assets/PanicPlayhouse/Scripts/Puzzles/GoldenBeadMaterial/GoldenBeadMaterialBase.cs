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
                foreach (Pushable pushable in _inside)
                    _currentValue += pushable.Value;

                return _currentValue;
            }
        }

        private readonly List<Pushable> _inside = new();
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Pushable push)) return;
            
            _inside.Add(push);
            Puzzle.OnPressBase(this);
        }
        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Pushable push)) return;
            
            _inside.Remove(push);
        }
    }
}