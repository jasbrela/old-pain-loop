using PanicPlayhouse.Scripts.Interfaces;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Puzzles.Grief
{
    public class GriefButton : Interactable
    {
        [SerializeField] private GriefGround ground;
        public bool IsCorrect => ground.IsCorrect;
        public bool IsBlocked { get; set; } = false;

        public GriefPuzzle Puzzle
        {
            get => _puzzle;
            set
            {
                if (_puzzle == null)
                {
                    _puzzle = value;
                }
            }
        }

        private GriefPuzzle _puzzle;
        private GriefGround _ground;
        
        public override void OnInteract()
        {
            if (IsBlocked) return;
            ground.Rotate();
            Puzzle.OnPressButton(this);
        }

        public override void OnEnterRange()
        {
            if (IsBlocked) return;
            base.OnEnterRange();
        }

        public override void OnQuitRange()
        {
            if (IsBlocked) return;
            base.OnQuitRange();
        }
    }
}
