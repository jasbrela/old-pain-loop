﻿using PanicPlayhouse.Scripts.Interfaces;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Puzzles.Xylophone
{
    public class XylophoneButton : Interactable
    {
        [SerializeField] private AudioClip clip;
        public bool IsBlocked { get; set; } = false;

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