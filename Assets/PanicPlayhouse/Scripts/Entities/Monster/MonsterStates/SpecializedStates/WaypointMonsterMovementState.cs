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
        private Vector3 waypointPosition;

        public override void OnEnterState()
        {
            // base.OnEnterState();

            SetAgentDestination(waypointPosition);
            agent.speed = scriptableObject.roamSpeed;
        }

        // TODO: Se encontrar o player no caminho, correr atrás dele.
        public override void OnUpdate()
        {
            // base.OnUpdate();

            float distanceFromDestination = Vector3.Distance(movement.transform.position, agent.destination);
            bool hasReachedDestination = distanceFromDestination <= agent.stoppingDistance;
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
