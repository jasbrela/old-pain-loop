using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPlayhouse.Scripts.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Variables/MonsterScriptableObject", fileName = "MonsterSO")]
    public class MonsterStates : ScriptableObject
    {
        [Header("Insanity Variables")]
        public float insanityPenalty;
        public float insanityDistance;

        [Header("Player Detection")]
        public float visionDistance;
        public float giveUpDistance;
        public float killDistance;
        public float delayBetweenChecks;

        // [Header("Idle State")]

        [Header("Roam State")]
        public float roamSpeed;
        public float idleTime;

        [Header("Chase Player State")]
        public float chaseSpeed;
    }
}
