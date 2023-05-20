using System.Collections;
using NaughtyAttributes;
using PanicPlayhouse.Scripts.Audio;
using PanicPlayhouse.Scripts.Entities.Player;
using PanicPlayhouse.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;

namespace PanicPlayhouse.Scripts.Entities.Monster
{
    public class MonsterMovement : MonoBehaviour
    {
        [SerializeField] private AudioClip knockSfx;
        [SerializeField] private AudioSource twoDSource;
        [SerializeField] private AudioClip attackSound;
        [SerializeField] private AudioSource heartbeatSource;
        [SerializeField] private AudioSource followingMusicSource;
        [SerializeField] private PlayerHiddenStatus player;
        [SerializeField] private FloatVariable playerInsanity;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private float speed;
        [SerializeField] private float insanityPenalty;
        [SerializeField] private float insanityDistance;
        [SerializeField] private float visionDistance;
        [SerializeField] private float giveUpDistance;
        [SerializeField] private float killDistance;
        [SerializeField] private FootstepsAudio footsteps;
        [SerializeField] private NavMeshAgent agent;
        [Label("Animation")][SerializeField] private EntityAnimation anim;
        private Vector3 _defaultPos;
        [SerializeField] private float delayBetweenChecks;
        
        [Header("DEBUG")]
        [SerializeField] [ReadOnly] private float distanceFromDestination;
        [SerializeField] [ReadOnly] private float distanceFromPlayer;
        [SerializeField] [ReadOnly] private float distanceFromDefaultPos;
        [SerializeField] [ReadOnly] private bool isComingBack;
        [SerializeField] [ReadOnly] private bool isCheckingPlayer;
        [SerializeField] [ReadOnly] private bool wasCheckingPlayer;
        [SerializeField] [ReadOnly] private bool wasPathComplete;
        [SerializeField] [ReadOnly] private bool canKillHiddenPlayer;
        [SerializeField] [ReadOnly] private bool isFollowingPlayer;

        private void Start()
        {
            agent.speed = speed;
            _defaultPos = transform.position;
            StartCoroutine(CheckPathStatus());
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
            isComingBack = true;
            anim.Walking.SetBool(false);
            agent.destination = _defaultPos;
            transform.position = _defaultPos;
            agent.speed = speed;
        }

        public void OnPlayerRespawn()
        {
            // safe checks
            heartbeatSource.Stop();
            followingMusicSource.Stop();
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
                distanceFromDefaultPos = Vector3.Distance(monster, _defaultPos);

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
                    footsteps.IsMoving = true;
                    anim.Walking.SetBool(true);
                    isComingBack = false;
                    spriteRenderer.flipX = monster.x - agent.destination.x > 0;
                }
                else if (distanceFromPlayer <= killDistance && (!player.IsHidden || canKillHiddenPlayer))
                {
#if UNITY_EDITOR
                    Debug.Log("KillPlayer");
#endif
                    if (player.IsHidden && knockSfx != null)
                    {
                        twoDSource.PlayOneShot(knockSfx);
                    }
                    twoDSource.PlayOneShot(attackSound);
                    // KILL PLAYER
                    playerInsanity.Value = playerInsanity.MaxValue;
                    followingMusicSource.Stop();
                    anim.Attack.SetTrigger();
                    if (canKillHiddenPlayer) canKillHiddenPlayer = false;
                    agent.speed = 0;
                }
                else if ((player.IsHidden && canKillHiddenPlayer || !player.IsHidden) &&
                           (distanceFromPlayer <= visionDistance || isFollowingPlayer && distanceFromPlayer <= giveUpDistance))
                {
#if UNITY_EDITOR
                    Debug.Log("FollowingPlayer");
#endif
                    // FOLLOW PLAYER
                    wasPathComplete = false;
                    if (!followingMusicSource.isPlaying) followingMusicSource.Play();
                    if (!heartbeatSource.isPlaying) heartbeatSource.Play();
                    isFollowingPlayer = true;
                    anim.Walking.SetBool(true);
                    agent.destination = player.transform.position;
                    footsteps.IsMoving = true;
                    isComingBack = false;
                }
                else if (!wasPathComplete && distanceFromDestination <= agent.stoppingDistance)
                {
                    wasPathComplete = true;
#if UNITY_EDITOR
                    Debug.Log("PathComplete");
#endif
                    // PATH COMPLETED
                    footsteps.IsMoving = false;
                    anim.Walking.SetBool(false);
                    if (wasCheckingPlayer || isFollowingPlayer)
                    {
                        yield return new WaitForSeconds(3);
                        if (!isFollowingPlayer && !isCheckingPlayer)
                        {
                            wasPathComplete = false;
                            wasCheckingPlayer = false;
                            isComingBack = true;
                            agent.destination = _defaultPos;
                            anim.Walking.SetBool(true);
                            footsteps.IsMoving = true;
                            if (followingMusicSource.isPlaying) followingMusicSource.Stop();
                            if (heartbeatSource.isPlaying) heartbeatSource.Stop();
                        }
                    }
                }
                
                if (distanceFromPlayer <= insanityDistance)
                    playerInsanity.Value += insanityPenalty * Time.deltaTime;
            }
        }
        
    }
    
}
