using PanicPlayhouse.Scripts.Interfaces;

namespace PanicPlayhouse.Scripts.Puzzles.Grief
{
    public class GriefButton : Interactable
    {
        public bool IsCorrect => Ground.IsCorrect;
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
        
        private GriefGround Ground
        {
            get
            {
                if (_ground == null) _ground = transform.parent.GetComponentInChildren<GriefGround>();
                return _ground;
            }
        }
        
        private GriefPuzzle _puzzle;
        private GriefGround _ground;
        
        public override void OnInteract()
        {
            if (IsBlocked) return;
            Ground.Rotate();
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
