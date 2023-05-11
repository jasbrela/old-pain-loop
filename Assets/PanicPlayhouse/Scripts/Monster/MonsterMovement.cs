using System;
using System.Collections;
using PanicPlayhouse.Scripts.Audio;
using PanicPlayhouse.Scripts.Player;
using PanicPlayhouse.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace PanicPlayhouse.Scripts.Monster
{
    public class MonsterMovement : MonoBehaviour
    {
        [SerializeField] private PlayerHiddenStatus player;
        [SerializeField] private FloatVariable playerInsanity;
        [SerializeField] private float speed;
        [SerializeField] private float insanityPenalty;
        [SerializeField] private float insanityDistance;
        [SerializeField] private float visionDistance;
        [SerializeField] private float giveUpDistance;
        [SerializeField] private float killDistance;
        [SerializeField] private FootstepsAudio footsteps;
        [SerializeField] private Vector3Variable lastKnownPos;
        [SerializeField] private NavMeshAgent agent;
        private Vector3 _defaultPos;

        private bool IsMoving { get; set; } = true;
        private bool IsComingBack { get; set; } = true;
        private bool IsFollowingPlayer { get; set; } = false;
        private bool CanKillHiddenPlayer { get; set; } = false;
        
        private void Start()
        {
            agent.speed = speed;
            _defaultPos = transform.position;
        }

        public void OnTriggerMonster()
        {
            StartCoroutine(WaitThenMove(lastKnownPos.Value));
            IsComingBack = false;
        }

        private void Update()
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance <= visionDistance || IsFollowingPlayer && distance <= giveUpDistance && !player.IsHidden)
            {
                IsFollowingPlayer = true;
                StartCoroutine(WaitThenMove(player.transform.position, 0));
            }
            else
            {
                IsComingBack = true;
                footsteps.IsMoving = false;
                StartCoroutine(WaitThenMove(_defaultPos, 3));
            }
            
            if (distance <= insanityDistance)
                playerInsanity.Value += insanityPenalty * Time.deltaTime;

            if (distance <= killDistance && (!player.IsHidden || CanKillHiddenPlayer))
            {
                playerInsanity.Value = playerInsanity.MaxValue;
                if (CanKillHiddenPlayer) CanKillHiddenPlayer = false;
            }

            // movement stuff
            if (agent.pathPending) return;
            if (!(agent.remainingDistance <= agent.stoppingDistance)) return;
            if (agent.hasPath && agent.velocity.sqrMagnitude != 0f) return;
            if (IsComingBack) return;
            IsComingBack = true;
            footsteps.IsMoving = false;
            StartCoroutine(WaitThenMove(_defaultPos, 3));
        }

        public void OnPlayerHide()
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance <= visionDistance)
            {
                CanKillHiddenPlayer = true;
            }
        }

        IEnumerator WaitThenMove(Vector3 to, int delay = 1)
        {
            yield return new WaitForSeconds(delay);
            IsMoving = true;
            agent.destination = to;
            footsteps.IsMoving = true;
        }
    }
}
