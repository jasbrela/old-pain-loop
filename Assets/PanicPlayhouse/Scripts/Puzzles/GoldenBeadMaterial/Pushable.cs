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
        [SerializeField] private LayerMask avoidOverlap;
        [SerializeField] private float duration = 1f;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, radius);
        }
        
        public int Value => value;
        public bool IsBlocked { get; set; }
        
        public void Push(Vector3 forward)
        {
            if (IsBlocked) return;

            Collider[] results = new Collider[10];
            var size = Physics.OverlapSphereNonAlloc(toMove.transform.position + forward, radius, results, avoidOverlap);
            if (size > 0) return;

            toMove.DOMove(toMove.position + forward * radius/2, duration);
        }
        
    }
}