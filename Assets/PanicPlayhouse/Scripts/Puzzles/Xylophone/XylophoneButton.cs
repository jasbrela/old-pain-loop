using FMODUnity;
using PanicPlayhouse.Scripts.Chunk;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Puzzles.Xylophone
{
    public class XylophoneButton : Interactable
    {
        [SerializeField] private EventReference click;
        private bool _isBlocked;
        
        public bool IsBlocked
        {
            get => _isBlocked;
            set
            {
                _isBlocked = value;
                if (_isBlocked)
                {
                    OnQuitRange();
                }
            }
        }

        public XylophonePuzzle Puzzle
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

        private XylophonePuzzle _puzzle;
        
        public override void OnInteract()
        {
            if (IsBlocked) return;
            Puzzle.OnPressButton(this, click);
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