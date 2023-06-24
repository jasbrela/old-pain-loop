using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PanicPlayhouse.Scripts.Puzzles.GoldenBeadMaterial
{
    public class PuzzlePickupable : Pickupable
    {
        [SerializeField] private int value;
        public int Value => value;
    }
}
