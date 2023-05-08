using System.Collections.Generic;
using PanicPlayhouse.Scripts.ScriptableObjects;
using UnityEngine;
using Event = PanicPlayhouse.Scripts.ScriptableObjects.Event;

namespace PanicPlayhouse.Scripts.Puzzles.Grief
{
    public class GriefPuzzle : MonoBehaviour
    {
        [Header("Puzzle")]
        [SerializeField] private Event onFinish;

        [Header("Insanity")]
        [SerializeField] private float insanityPenalty;
        [SerializeField] private float insanityReward;
        [SerializeField] private FloatVariable insanity;
        private List<GriefButton> _buttons;
        private List<bool> _areCorrect;
        private bool IsActivated { get; set; } = false;
        private bool IsFinished { get; set; } = false;

        private void Start()
        {
            _buttons = new(FindObjectsOfType<GriefButton>());
            _areCorrect = new List<bool>(_buttons.Count) { false, false, false, false };
            
            if (_buttons.Count == 0)
            {
                gameObject.SetActive(false);
                Debug.Log(name.Bold().Color("#FF4500") + " has been deactivated.");
                return;
            }
            
            foreach (GriefButton button in _buttons)
            {
                button.IsBlocked = true;
                button.Puzzle = this;
            }
        }

        public void ActivatePuzzle()
        {
            if (IsActivated || IsFinished) return;
            
            Debug.Log(name.Bold().Color("#00FA9A") + " has been activated.");

            foreach (GriefButton button in _buttons) button.IsBlocked = false;
        }

        public void OnPressButton(GriefButton button)
        {
            _areCorrect[_buttons.IndexOf(button)] = button.IsCorrect;

            if (_areCorrect.IndexOf(false) == -1)
            {
                foreach (GriefButton btn in _buttons)
                {
                    btn.IsBlocked = true;
                }
                if (onFinish != null) onFinish.Raise();
                insanity.Value -= insanityReward;
            }

            insanity.Value += insanityPenalty;
        }
    }
}
