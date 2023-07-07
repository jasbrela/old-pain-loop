using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPlayhouse.Scripts.Audio;
using PanicPlayhouse.Scripts.Entities.Player;
using PanicPlayhouse.Scripts.ScriptableObjects;

namespace PanicPlayhouse.Scripts.Entities.Monster
{
    public class ChasePlayerMonsterMovementState : BaseMonsterMovementState
    {
        public bool shouldChasePlayer { get; private set; }

        private PlayerHiddenStatus playerHiddenStatus;
        private PlayerDetector playerDetector;

        private Coroutine currentCoroutine;

        public override void OnEnterState()
        {
            Debug.Log("OnEnterState!");
            // base.OnEnterState();
            audio.ToggleChaseAudiosOn(true);

            agent.speed = scriptableObject.chaseSpeed;
            currentCoroutine = movement.StartCoroutine(ChasePlayerCoroutine());
        }

        IEnumerator ChasePlayerCoroutine()
        {
            Debug.Log("ChasePlayerCoroutine!");
            Vector3 finalAgentDestination = playerHiddenStatus.transform.position;
            shouldChasePlayer = true;

            while (shouldChasePlayer)
            {
                if (playerDetector.currentState == PlayerDetectorState.PlayerOffVision)
                {
                    Debug.Log("PlayerOffVision!");
                    // Se o player está fora de visão, vai para sua ultima posição.
                    SetAgentDestination(playerDetector.lastPlayerSeenPosition);
                    float timeIdle = 0;
                    float distanceToDestination = Vector3.Distance(movement.transform.position, playerDetector.lastPlayerSeenPosition);

                    while (playerDetector.currentState == PlayerDetectorState.PlayerOffVision)
                    {
                        if (distanceToDestination <= agent.stoppingDistance)
                        {
                            agent.isStopped = true;
                            timeIdle += Time.deltaTime;

                            if (timeIdle >= scriptableObject.idleTime)
                            {
                                shouldChasePlayer = false;
                                break;
                            }
                        }

                        yield return new WaitForEndOfFrame();
                    }

                    if (!shouldChasePlayer)
                    {
                        playerDetector.currentState = PlayerDetectorState.PlayerOffVision;
                        break;
                    }
                }
                else if (playerDetector.currentState == PlayerDetectorState.PlayerInVision)
                {
                    Debug.Log("PlayerInVision!");
                    // Caso o player esteja em visão, corre atrás dele.
                    SetAgentDestination(playerHiddenStatus.transform.position);
                }

                yield return new WaitForEndOfFrame();
            }
        }

        public override void OnExitState()
        {
            // base.OnExitState();
            Debug.Log("Exiting state!");

            if (currentCoroutine != null)
                movement.StopCoroutine(currentCoroutine);

            audio.ToggleChaseAudiosOn(false);

            currentCoroutine = null;
        }

        public ChasePlayerMonsterMovementState(
            // Base class vars
            MonsterMovement monsterMovement,
            EntityAnimation entityAnimation,
            MonsterAudio monsterAudio,
            UnityEngine.AI.NavMeshAgent agent,
            MonsterStates scriptableObject,

            // Class-specific vars
            PlayerHiddenStatus hiddenStatus,
            PlayerDetector playerDetector
        )
        : base(monsterMovement, entityAnimation, monsterAudio, agent, scriptableObject)
        {
            this.playerHiddenStatus = hiddenStatus;
            this.playerDetector = playerDetector;

            this.stateType = MonsterMovementStates.ChasePlayer;

            OnEnterState();
        }
    }
}