using System.Collections;
using FMOD.Studio;
using FMODUnity;
using NaughtyAttributes;
using PanicPlayhouse.Scripts.Audio;
using PanicPlayhouse.Scripts.Entities.Player;
using PanicPlayhouse.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace PanicPlayhouse.Scripts.Entities.Monster
{
    public class MonsterMovement : MonoBehaviour
    {
        [Header("Sound")]
        [SerializeField] private EventReference knock;
        [SerializeField] private EventReference breath;
        [SerializeField] private EventReference attack;
        [SerializeField] private EventReference heartbeat;
        [SerializeField] private EventReference chasingMusic;
        [SerializeField] private EventReference footstep;

        [Header("General")]
        [Label("Rigidbody")] [SerializeField] private Rigidbody rb;
        [SerializeField] private PlayerHiddenStatus player;
        [SerializeField] private FloatVariable playerInsanity;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private float speed;
        [SerializeField] private float insanityPenalty;
        [SerializeField] private float insanityDistance;
        [SerializeField] private float visionDistance;
        [SerializeField] private float giveUpDistance;
        [SerializeField] private float killDistance;
        [SerializeField] private NavMeshAgent agent;
        [Label("Animation")][SerializeField] private EntityAnimation anim;
        private Vector3 _defaultPos;
        [SerializeField] private float delayBetweenChecks;
        
        [Header("DEBUG")]
        [SerializeField] [ReadOnly] private float distanceFromDestination;
        [SerializeField] [ReadOnly] private float distanceFromPlayer;
        [SerializeField] [ReadOnly] private bool isCheckingPlayer;
        [SerializeField] [ReadOnly] private bool wasCheckingPlayer;
        [SerializeField] [ReadOnly] private bool wasPathComplete;
        [SerializeField] [ReadOnly] private bool canKillHiddenPlayer;
        [SerializeField] [ReadOnly] private bool killedPlayer;
        [SerializeField] [ReadOnly] private bool isFollowingPlayer;

        private EventInstance _footstepInstance;
        private EventInstance _heartbeatInstance;
        private EventInstance _chasingMusicInstance;
        private EventInstance _breathInstance;

        private AudioManager _audio;

        private void Start()
        {
            agent.speed = speed;
            _defaultPos = transform.position;
            StartCoroutine(CheckPathStatus());
            _audio = FindObjectOfType<AudioManager>();
            _audio.PlayAudioInLoop(ref _breathInstance, breath, rb);
        }

        public void OnTriggerMonster()
        {
            isCheckingPlayer = true;
        }

        public void OnPlayerHide()
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance <= visionDistance)
            {
                canKillHiddenPlayer = true;
            }
        }

        public void OnKillPlayer()
        {
            isFollowingPlayer = false;
            isCheckingPlayer = false;
            anim.Walking.SetBool(false);
            agent.destination = _defaultPos;
            //transform.position = _defaultPos;
            agent.speed = speed;
        }

        public void OnPlayerRespawn()
        {
            //transform.position = _defaultPos;
            agent.destination = _defaultPos;
            killedPlayer = false;
            _audio.StopAudioInLoop(_heartbeatInstance);
            _audio.StopAudioInLoop(_chasingMusicInstance);
        }

        private IEnumerator CheckPathStatus()
        {
            while (true)
            {
                yield return new WaitForSeconds(delayBetweenChecks);

                while (agent == null) yield return null;

                Vector3 monster = transform.position;

                distanceFromDestination = Vector3.Distance(monster, agent.destination);
                distanceFromPlayer = Vector3.Distance(monster, player.transform.position);
                Vector3.Distance(monster, _defaultPos);

                if (isCheckingPlayer)
                {
#if UNITY_EDITOR
                    Debug.Log("IsCheckingPlayer");
#endif
                    // CHECKING PLAYER
                    wasPathComplete = false;
                    isCheckingPlayer = false;
                    wasCheckingPlayer = true;
                    agent.destination = player.transform.position;
                    _audio.PlayAudioInLoop(ref _footstepInstance, footstep, rb);
                    anim.Walking.SetBool(true);
                    spriteRenderer.flipX = monster.x - agent.destination.x > 0;
                }
                else if (!killedPlayer && distanceFromPlayer <= killDistance && (!player.IsHidden || canKillHiddenPlayer))
                {
#if UNITY_EDITOR
                    Debug.Log("KillPlayer");
#endif
                    // KILL PLAYER
                    _audio.PlayOneShot(player.IsHidden ? knock : attack);
                    killedPlayer = true;
                    playerInsanity.Value = playerInsanity.MaxValue;
                    _audio.StopAudioInLoop(_chasingMusicInstance, STOP_MODE.ALLOWFADEOUT);
                    anim.Attack.SetTrigger();
                    if (canKillHiddenPlayer) canKillHiddenPlayer = false;
                    agent.speed = 0;
                }
                else if ((player.IsHidden && canKillHiddenPlayer || !player.IsHidden) &&
                           (distanceFromPlayer <= visionDistance || isFollowingPlayer && distanceFromPlayer <= giveUpDistance)
                           && !killedPlayer)
                {
#if UNITY_EDITOR
                    Debug.Log("FollowingPlayer");
#endif
                    // FOLLOW PLAYER
                    wasPathComplete = false;
                    _audio.PlayAudioInLoop(ref _chasingMusicInstance, chasingMusic);
                    _audio.PlayAudioInLoop(ref _heartbeatInstance, heartbeat);
                    isFollowingPlayer = true;
                    anim.Walking.SetBool(true);
                    agent.destination = player.transform.position;
                    _audio.PlayAudioInLoop(ref _footstepInstance, footstep, rb);
                }
                else if (!wasPathComplete && distanceFromDestination <= agent.stoppingDistance)
                {
                    wasPathComplete = true;
#if UNITY_EDITOR
                    Debug.Log("PathComplete");
#endif
                    // PATH COMPLETED
                    _audio.StopAudioInLoop(_footstepInstance);
                    anim.Walking.SetBool(false);
                    if (wasCheckingPlayer || isFollowingPlayer)
                    {
                        yield return new WaitForSeconds(3);
                        if (!isFollowingPlayer && !isCheckingPlayer)
                        {
                            wasPathComplete = false;
                            wasCheckingPlayer = false;
                            agent.destination = _defaultPos;
                            anim.Walking.SetBool(true);
                            _audio.PlayAudioInLoop(ref _footstepInstance, footstep, rb);
                            _audio.StopAudioInLoop(_chasingMusicInstance, STOP_MODE.ALLOWFADEOUT);
                            _audio.StopAudioInLoop(_heartbeatInstance);
                        }
                    }
                }
                
                if (distanceFromPlayer <= insanityDistance)
                    playerInsanity.Value += insanityPenalty * Time.deltaTime;
            }
        }
        
    }
    
}
