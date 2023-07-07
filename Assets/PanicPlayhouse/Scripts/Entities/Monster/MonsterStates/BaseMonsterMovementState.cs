using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPlayhouse.Scripts.Audio;
using UnityEngine.AI;
using PanicPlayhouse.Scripts.ScriptableObjects;

namespace PanicPlayhouse.Scripts.Entities.Monster
{
    public enum MonsterMovementStates
    {
        Waypoint,
        Roam,
        ChasePlayer
    }
    public class BaseMonsterMovementState
    {
        public MonsterMovementStates stateType;

        protected MonsterMovement movement;
        protected EntityAnimation animation;
        protected MonsterAudio audio;
        protected NavMeshAgent agent;
        protected MonsterStates scriptableObject;


        public virtual void OnEnterState()
        { }

        public virtual void OnUpdate()
        { }

        public virtual void OnExitState()
        { }

        protected void SetAgentDestination(Vector3 pos)
        {
            agent.isStopped = false;
            agent.destination = new Vector3(pos.x, movement.transform.position.y, pos.z);
        }

        public BaseMonsterMovementState(
            MonsterMovement monsterMovement,
            EntityAnimation entityAnimation,
            MonsterAudio monsterAudio,
            NavMeshAgent agent,
            MonsterStates scriptableObject
        )
        {
            this.movement = monsterMovement;
            this.animation = entityAnimation;
            this.audio = monsterAudio;
            this.agent = agent;
            this.scriptableObject = scriptableObject;
        }

        // public virtual void OnFixedUpdate()
        // { }
    }
}
