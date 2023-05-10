using DG.Tweening;
using PanicPlayhouse.Scripts.Interfaces;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Puzzles.GoldenBeadMaterial
{
    public class Pushable : Interactable
    {
        [SerializeField] private int value;
        [SerializeField] private Transform toMove;
        [SerializeField] private float radius;
        [SerializeField] private float multiplier = 3f;
        [SerializeField] private LayerMask avoidOverlap;
        [SerializeField] private float duration = 1f;

        public int Value => value;
        public bool IsBlocked { get; set; }
        
        public void Push(Vector3 forward)
        {
            if (IsBlocked) return;
            
            Collider[] results = { };
            var size = Physics.OverlapSphereNonAlloc(toMove.transform.position + forward, radius, results, avoidOverlap);
            if (size > 0) return;

            toMove.DOMove(toMove.position + forward * multiplier, duration);
        }
        
    }
}