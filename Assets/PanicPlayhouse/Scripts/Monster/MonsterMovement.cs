using System;
using System.Collections;
using PanicPlayhouse.Scripts.Audio;
using PanicPlayhouse.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace PanicPlayhouse.Scripts.Monster
{
    public class MonsterMovement : MonoBehaviour
    {
        [SerializeField] private FootstepsAudio footsteps;
        [SerializeField] private Vector3Variable lastKnownPos;
        [SerializeField] private NavMeshAgent agent;

        public void OnTriggerMonster()
        {
            StartCoroutine(WaitThenMove());
        }

        IEnumerator WaitThenMove()
        {
            yield return new WaitForSeconds(1);
            agent.destination = lastKnownPos.Value;
        }
    }
}
