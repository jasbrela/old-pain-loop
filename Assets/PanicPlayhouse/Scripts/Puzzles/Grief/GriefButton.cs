using PanicPlayhouse.Scripts.Chunk;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Puzzles.Grief
{
    public class GriefButton : Interactable
    {
        [SerializeField] private AudioClip rotate;
        [SerializeField] private AudioClip click;
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
            _puzzle.PlayClip(click);
            _puzzle.PlayClip(rotate);
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
