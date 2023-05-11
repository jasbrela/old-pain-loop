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
            float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
            float distanceFromDefaultPos = Vector3.Distance(transform.position, _defaultPos);


            if (!player.IsHidden && (distanceFromPlayer <= visionDistance || IsFollowingPlayer && distanceFromPlayer <= giveUpDistance))
            {
                Debug.Log("A");
                IsFollowingPlayer = true;
                StartCoroutine(WaitThenMove(player.transform.position, 0));
            }
            else if (distanceFromDefaultPos > 0.5)
            {
                IsComingBack = true;
                footsteps.IsMoving = false;
                StartCoroutine(WaitThenMove(_defaultPos, 3));
            }
            
            if (distanceFromPlayer <= insanityDistance)
                playerInsanity.Value += insanityPenalty * Time.deltaTime;

            if (distanceFromPlayer <= killDistance && (!player.IsHidden || CanKillHiddenPlayer))
            {
                playerInsanity.Value = playerInsanity.MaxValue;
                if (CanKillHiddenPlayer) CanKillHiddenPlayer = false;
            }

            // movement stuff
            Debug.Log($"{agent.pathPending}, {!(agent.remainingDistance <= agent.stoppingDistance)}, {agent.hasPath}, {agent.velocity.sqrMagnitude != 0f}, {IsComingBack} ");
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
