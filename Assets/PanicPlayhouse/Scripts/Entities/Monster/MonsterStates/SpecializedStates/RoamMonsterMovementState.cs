using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPlayhouse.Scripts.Audio;
using PanicPlayhouse.Scripts.Chunk;
using PanicPlayhouse.Scripts.ScriptableObjects;
using UnityEngine.AI;

namespace PanicPlayhouse.Scripts.Entities.Monster
{
    public class RoamMonsterMovementState : BaseMonsterMovementState
    {
        // TODO: Receber listas de todos os pontos do mapa.
        private MapWaypoints mapWaypoints;

        private float currentIdleTime;
        private Vector3? currentPoint = null;

        public override void OnUpdate()
        {
            // base.OnUpdate();

            if (currentPoint == null)
                SortNewPoint();

            float distanceToPoint = Vector3.Distance(movement.transform.position, currentPoint.Value);

            if (distanceToPoint <= agent.stoppingDistance)
            {
                currentIdleTime += Time.deltaTime;

                if (currentIdleTime >= scriptableObject.idleTime)
                    SortNewPoint();
            }
        }

        private void SortNewPoint()
        {
            currentIdleTime = 0;
            List<Vector3> excludedPoints = new List<Vector3>();
            if (currentPoint.HasValue)
                excludedPoints.Add(currentPoint.Value);

            SortNewPoint(ref excludedPoints);
        }

        private void SortNewPoint(ref List<Vector3> excludedPoints)
        {
            Vector3 newPointCandidate = mapWaypoints.GetRandomWaypoint(excludedPoints);

            NavMeshPath path = new NavMeshPath();
            bool canReachEndpoint = agent.CalculatePath(newPointCandidate, path);
            if (!canReachEndpoint || path.status != NavMeshPathStatus.PathComplete)
            {
                excludedPoints.Add(newPointCandidate);
                SortNewPoint(ref excludedPoints);
                return;
            }

            currentPoint = newPointCandidate;
            SetAgentDestination(currentPoint.Value);
        }

        public RoamMonsterMovementState(
            // Base class vars
            MonsterMovement monsterMovement,
            EntityAnimation entityAnimation,
            MonsterAudio monsterAudio,
            UnityEngine.AI.NavMeshAgent agent,
            MonsterStates scriptableObject,

            // Class-specific vars
            MapWaypoints mapWaypoints
        )
        : base(monsterMovement, entityAnimation, monsterAudio, agent, scriptableObject)
        {
            this.mapWaypoints = mapWaypoints;

            this.stateType = MonsterMovementStates.Roam;
            OnEnterState();
        }
    }
}