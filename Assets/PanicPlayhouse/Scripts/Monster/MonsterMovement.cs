using System;
using System.Collections;
using PanicPlayhouse.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;

namespace PanicPlayhouse.Scripts.Monster
{
    public class MonsterMovement : MonoBehaviour
    {
        [SerializeField] private Vector3Variable lastKnownPos;
        private NavMeshAgent _monster;

        private void Start()
        {
            if (TryGetComponent(out _monster)) return;
            
            Debug.LogWarning("The monster is missing a NavMeshAgent Component", this);
            gameObject.SetActive(false);
        }

        public void OnTriggerMonster()
        {
            StartCoroutine(WaitThenMove());
        }

        IEnumerator WaitThenMove()
        {
            yield return new WaitForSeconds(1);
            _monster.destination = lastKnownPos.Value;
        }
    }
}
