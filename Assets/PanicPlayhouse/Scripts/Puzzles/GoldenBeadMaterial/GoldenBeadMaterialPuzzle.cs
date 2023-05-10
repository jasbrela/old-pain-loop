using System.Collections.Generic;
using PanicPlayhouse.Scripts.ScriptableObjects;
using UnityEngine;
using Event = PanicPlayhouse.Scripts.ScriptableObjects.Event;

namespace PanicPlayhouse.Scripts.Puzzles.GoldenBeadMaterial
{
    public class GoldenBeadMaterialPuzzle : MonoBehaviour
    {
        [Header("Puzzle")]
        [SerializeField] private Event onFinish;
        
        [Header("Insanity")]
        [SerializeField] private float insanityPenalty;
        [SerializeField] private float insanityReward;
        [SerializeField] private FloatVariable insanity;

        private List<GoldenBeadMaterialBase> _matBases;
        private List<Pushable> _pushables;
        private List<bool> _areCorrect;
        
        private bool IsActivated { get; set; } = false;
        private bool IsFinished { get; set; } = false;
        
        private void Start()
        {
            _pushables = new(FindObjectsOfType<Pushable>());
            _matBases = new(FindObjectsOfType<GoldenBeadMaterialBase>());
            _areCorrect = new List<bool>(_matBases.Count) { false, false, false, false };
            
            if (_matBases.Count == 0)
            {
                gameObject.SetActive(false);
                Debug.Log(name.Bold().Color("#FF4500") + " has been deactivated.");
                return;
            }

            foreach (Pushable pushable in _pushables)
                pushable.IsBlocked = true;
            
            
            foreach (GoldenBeadMaterialBase matBase in _matBases)
                matBase.Puzzle = this;
            
        }
        
        public void ActivatePuzzle()
        {
            if (IsActivated || IsFinished) return;
            
            Debug.Log(name.Bold().Color("#00FA9A") + " has been activated.");

            foreach (Pushable pushable in _pushables)
                pushable.IsBlocked = false;
        }

        public void OnPressBase(GoldenBeadMaterialBase matBase)
        {
            _areCorrect[_matBases.IndexOf(matBase)] = matBase.IsCorrect;

            if (_areCorrect.IndexOf(false) == -1)
            {
                if (onFinish != null) onFinish.Raise();
                IsFinished = true;
                insanity.Value -= insanityReward;
            }

            foreach (var pushable in _pushables)
                pushable.IsBlocked = true;
            

            insanity.Value += insanityPenalty;
        }
    }
}
