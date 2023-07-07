using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using PanicPlayhouse.Scripts.ScriptableObjects;

public enum PlayerDetectorState
{
    Inactive,
    PlayerOffVision,
    PlayerInVision
}

namespace PanicPlayhouse.Scripts.Entities
{
    public class PlayerDetector : MonoBehaviour
    {

        [Header("Dependencies")]
        public Player.PlayerHiddenStatus player;
        public MonsterStates scriptableObject;
        public NavMeshAgent agent;

        [Header("Events")]
        [SerializeField] PanicPlayhouse.Scripts.ScriptableObjects.Event onPlayerDetectorStateChange;


        [HideInInspector] public Vector3 lastPlayerSeenPosition = Vector3.zero;
        private Coroutine currentSearchCoroutine;

        private bool _sawPlayerHiding = false;
        public bool sawPlayerHiding
        {
            get
            {
                return _sawPlayerHiding;
            }
        }

        private PlayerDetectorState _currentState = PlayerDetectorState.PlayerOffVision;
        public PlayerDetectorState currentState
        {
            get
            {
                return _currentState;
            }
            set
            {
                _currentState = value;
                onPlayerDetectorStateChange.Raise();
            }
        }


        void Start()
        {
            currentSearchCoroutine = StartCoroutine(SearchForPlayer());
        }

        public void OnPlayerHide()
        {
            _sawPlayerHiding = currentState == PlayerDetectorState.PlayerInVision;
        }

        public void SetCurrentStateWithoutInvoke(PlayerDetectorState stateValue)
        {
            _currentState = stateValue;
        }

        private IEnumerator SearchForPlayer()
        {
            float distanceFromPlayer = 0;

            while (true)
            {
                yield return new WaitForSeconds(scriptableObject.delayBetweenChecks);

                if (currentState == PlayerDetectorState.Inactive)
                    continue;

                if (!player.IsHidden)
                    _sawPlayerHiding = false;

                // TODO: Implementar bloqueio de visão por paredes e afins.

                bool playerInVision = false;


                distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
                NavMeshHit navMeshRaycastHit;
                if (!agent.Raycast(player.transform.position, out navMeshRaycastHit))
                {
                    if (currentState == PlayerDetectorState.PlayerInVision)
                        playerInVision = distanceFromPlayer <= scriptableObject.giveUpDistance;
                    else if (currentState == PlayerDetectorState.PlayerOffVision)
                        playerInVision = distanceFromPlayer <= scriptableObject.visionDistance;
                }

                // Checks if player is hidden and was seen hiding, or is not hiding.
                playerInVision =
                playerInVision && (
                    (player.IsHidden && _sawPlayerHiding) ||
                    !player.IsHidden
                );

                if (playerInVision)
                {
                    lastPlayerSeenPosition = player.transform.position;

                    if (currentState != PlayerDetectorState.PlayerInVision)
                        currentState = PlayerDetectorState.PlayerInVision;
                }
                else if (!playerInVision)
                {
                    if (currentState != PlayerDetectorState.PlayerOffVision)
                        currentState = PlayerDetectorState.PlayerOffVision;
                }
            }

            // currentSearchCoroutine = null;
        }
    }
}
