using PanicPlayhouse.Scripts.Interfaces;

namespace PanicPlayhouse.Scripts.Puzzles.Xylophone
{
    public class XylophoneButton : Interactable
    {
        private XylophonePuzzle _puzzle;
        private XylophoneGround _ground;
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
        
        public override void OnInteract()
        {
            if (_ground == null) _ground = transform.parent.GetComponentInChildren<XylophoneGround>();
            _ground.Rotate();
            Puzzle.OnPressButton(this);
        }
    }
}
