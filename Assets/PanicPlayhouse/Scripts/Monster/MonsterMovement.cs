using System;
using UnityEngine;
using UnityEngine.AI;

namespace PanicPlayhouse.Scripts.Monster
{
    public class MonsterMovement : MonoBehaviour
    {
        private NavMeshAgent _monster;

        private void Start()
        {
            if (TryGetComponent(out _monster)) return;
            
            Debug.LogWarning("The monster is missing a NavMeshAgent Component", this);
            gameObject.SetActive(false);
        }

        public void MoveTo(Vector3 position)
        {
            _monster.destination = position;
            
        }
    }
}
