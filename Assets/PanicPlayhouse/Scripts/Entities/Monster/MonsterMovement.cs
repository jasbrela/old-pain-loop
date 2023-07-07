using System.Collections;
using FMOD.Studio;

using NaughtyAttributes;
using PanicPlayhouse.Scripts.Audio;
using PanicPlayhouse.Scripts.Chunk;
using PanicPlayhouse.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;
using Event = PanicPlayhouse.Scripts.ScriptableObjects.Event;

namespace PanicPlayhouse.Scripts.Entities.Monster
{
    public class MonsterMovement : MonoBehaviour
    {
        [Header("General")]
        private Vector3 _defaultPos;

        [Header("Component Dependencies")]
        [SerializeField] private Player.PlayerHiddenStatus player;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [Label("Rigidbody")][SerializeField] private Rigidbody rb;
        [SerializeField] private EntityAnimation entityAnimation;
        [SerializeField] private PlayerDetector playerDetector;
        [SerializeField] private new MonsterAudio audio;
        [SerializeField] private MapWaypoints mapWaypoints;

        [Header("SO Dependencies")]
        [SerializeField] private Event arrivedAtDefaultPosition;
        [SerializeField] private FloatVariable playerInsanity;
        [SerializeField] private MonsterStates scriptableObject;

        [Header("DEBUG")]
        [SerializeField][ReadOnly] private float distanceFromPlayer;
        [SerializeField][ReadOnly] private bool killedPlayer = false;
        [SerializeField][ReadOnly] private bool shouldRoam;
        [SerializeField][ReadOnly] private EventInstance footstepsEventInstance;
        // [SerializeField][ReadOnly] private bool canKillHiddenPlayer;
        // [SerializeField][ReadOnly] private float distanceFromDestination;
        // [SerializeField][ReadOnly] private float distanceFromDefault;
        // [SerializeField][ReadOnly] private bool isCheckingPlayer;
        // [SerializeField][ReadOnly] private bool wasCheckingPlayer;
        // [SerializeField][ReadOnly] private bool wasPathComplete;
        // [SerializeField][ReadOnly] private bool isFollowingPlayer;
        // private bool _firstEvent = true;


        private BaseMonsterMovementState _currentState;
        private BaseMonsterMovementState currentState
        {
            get
            {
                return _currentState;
            }
            set
            {
                if (_currentState != null)
                    _currentState.OnExitState();

                _currentState = value;
            }
        }

        private void Start()
        {
            _defaultPos = transform.position;
            agent.isStopped = true;
            GoToWaypoint(_defaultPos);
            // StartCoroutine(CheckPathStatus());
        }

        void Update()
        {
            if (killedPlayer)
                return;

            entityAnimation["is_moving"].SetValue(!agent.isStopped);
            spriteRenderer.flipX = transform.position.x - agent.destination.x > 0;

            footstepsEventInstance = audio.GetEventInstance(MonsterAudioConstants.FOOTSTEPS_AUDIO_KEY);
            footstepsEventInstance.getPlaybackState(out PLAYBACK_STATE state);
            if (!agent.isStopped && (footstepsEventInstance.isValid() && state == PLAYBACK_STATE.STOPPED))
            {
                audio.PlayAudioInLoop(MonsterAudioConstants.FOOTSTEPS_AUDIO_KEY, true);
            }
            else if (agent.isStopped && (footstepsEventInstance.isValid() && state != PLAYBACK_STATE.STOPPED))
            {
                audio.StopAudioInLoop(MonsterAudioConstants.FOOTSTEPS_AUDIO_KEY);
            }

            if (currentState != null)
            {
                currentState.OnUpdate();
            }

            distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceFromPlayer <= scriptableObject.insanityDistance)
                playerInsanity.Value += scriptableObject.insanityPenalty * Time.deltaTime;

            if (distanceFromPlayer <= scriptableObject.killDistance)
                KillPlayer();
        }

        private void GoToWaypoint(Vector3 waypointPosition)
        {
            currentState = new WaypointMonsterMovementState(
                this, entityAnimation, audio, agent, scriptableObject,
                waypointPosition
            );
        }

        public void OnTriggerMonster()
        {
            GoToWaypoint(player.transform.position);
        }

        void KillPlayer()
        {
#if UNITY_EDITOR
            Debug.Log("MonsterMovement: ".Bold() + "KillPlayer");
#endif
            // KILL PLAYER
            audio.StopAudiosInLoop();
            audio.PlayOneShot(playerDetector.sawPlayerHiding ? MonsterAudioConstants.KNOCK_AUDIO_KEY : MonsterAudioConstants.ATTACK_AUDIO_KEY);

            currentState = null;
            agent.speed = 0;
            killedPlayer = true;

            playerInsanity.Value = playerInsanity.MaxValue;
            entityAnimation["attack"].SetValue();
        }

        private void SetAgentDestination(Vector3 pos)
        {
            agent.destination = new Vector3(pos.x, transform.position.y, pos.z);
        }

        public void OnPlayerRespawn()
        {
            transform.position = _defaultPos;
            agent.Warp(_defaultPos);
            DefaultBehaviour();

            audio.StopAudiosInLoop();
            killedPlayer = false;
        }

        public void OnPlayerDetectorStateChange()
        {
            if (playerDetector.currentState == PlayerDetectorState.Inactive)
                return;

            if (killedPlayer)
            {
                playerDetector.SetCurrentStateWithoutInvoke(PlayerDetectorState.PlayerOffVision);
                return;
            }

            if (currentState != null && currentState.stateType == MonsterMovementStates.ChasePlayer)
            {
                if (((ChasePlayerMonsterMovementState)currentState).shouldChasePlayer)
                    return;
            }

            // if (playerDetector.currentState == PlayerDetectorState.PlayerOffVision)
            // {
            //     DefaultBehaviour();
            // }
            // else 
            if (playerDetector.currentState == PlayerDetectorState.PlayerInVision)
            {
                ChasePlayer();
            }
        }

        void ChasePlayer()
        {
            Debug.Log("Chasing player!");
            currentState = new ChasePlayerMonsterMovementState(
                    this, entityAnimation, audio, agent, scriptableObject,
                    player, playerDetector
                );
        }

        void DefaultBehaviour()
        {
            if (shouldRoam && currentState.stateType != MonsterMovementStates.Roam)
            {
                currentState = new RoamMonsterMovementState(
                    this, entityAnimation, audio, agent, scriptableObject,
                    mapWaypoints
                );
            }
            else
            {
                GoToWaypoint(_defaultPos);
            }
        }

        // private IEnumerator CheckPathStatus()
        // {
        //     while (true)
        //     {
        //         while (agent == null) yield return null;


        //         if (distanceFromDefault < 1f && !_firstEvent)
        //         {
        //             if (arrivedAtDefaultPosition != null) arrivedAtDefaultPosition.Raise();
        //         }
        //         if (_firstEvent) _firstEvent = false;
        //     }
        // }
    }
}
