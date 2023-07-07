using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPlayhouse.Scripts.Audio;
using UnityEngine.AI;
using PanicPlayhouse.Scripts.ScriptableObjects;

namespace PanicPlayhouse.Scripts.Entities.Monster
{
    public class WaypointMonsterMovementState : BaseMonsterMovementState
    {
        public bool isDone = false;
        private Vector3 waypointPosition;
        private Coroutine idleCoroutine = null;

        public override void OnEnterState()
        {
            // base.OnEnterState();

            SetAgentDestination(waypointPosition);
            isDone = false;
            agent.speed = scriptableObject.roamSpeed;
        }

        // TODO: Se encontrar o player no caminho, correr atrás dele.
        public override void OnUpdate()
        {
            // base.OnUpdate();

            float distanceFromDestination = Vector3.Distance(movement.transform.position, agent.destination);

            if (distanceFromDestination <= agent.stoppingDistance && idleCoroutine == null)
            {
                idleCoroutine = movement.StartCoroutine(IdleCoroutine());
            }
        }

        IEnumerator IdleCoroutine()
        {
            float currentWaitTime = 0;

            while (currentWaitTime < scriptableObject.idleTime)
            {
                currentWaitTime += Time.deltaTime;

                if (currentWaitTime >= scriptableObject.idleTime)
                {
                    isDone = true;
                    break;
                }

                yield return new WaitForEndOfFrame();
            }

            idleCoroutine = null;
        }

        public override void OnExitState()
        {
            // base.OnExitState();

            if (idleCoroutine != null)
            {
                movement.StopCoroutine(idleCoroutine);
                idleCoroutine = null;
            }
        }

        public WaypointMonsterMovementState(
            // Base class vars
            MonsterMovement monsterMovement,
            EntityAnimation entityAnimation,
            MonsterAudio monsterAudio,
            UnityEngine.AI.NavMeshAgent agent,
            MonsterStates scriptableObject,

            // Class-specific vars
            Vector3 waypointPosition
        )
        : base(monsterMovement, entityAnimation, monsterAudio, agent, scriptableObject)
        {
            this.waypointPosition = waypointPosition;

            this.stateType = MonsterMovementStates.Waypoint;
            OnEnterState();
        }
    }
}
