using System;
using NaughtyAttributes;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Player
{
    public class EntityAnimation : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] private Animator animator;

        [HideIf("AnimatorIsNull")] [SerializeField] private BoolAnimation walking;
        [HideIf("AnimatorIsNull")] [SerializeField] private TriggerAnimation attack;
        
        public bool AnimatorIsNull => animator == null;
        
        private void OnValidate()
        {
            if (animator == null) return;
            walking.SetAnimator(animator);
            attack.SetAnimator(animator);
        }
        
        private void Awake()
        {
            if (animator == null) return;
            walking.SetAnimator(animator);
            attack.SetAnimator(animator);
        }

        public BoolAnimation Walking => walking;
        public TriggerAnimation Attack => attack;

        
        [Serializable]
        public class BoolAnimation : Animation
        {
            private Animator _animator;
            [HideIf("AnimatorIsNull")] [SerializeField] [AnimatorParam("_animator", AnimatorControllerParameterType.Bool)] private string param;
            
            public bool AnimatorIsNull => _animator == null;
            
            public Animation SetAnimator(Animator animator)
            {
                if (_animator == null) _animator = animator;
                return this;
            }
            
            public void SetBool(bool value)
            {
                _animator.SetBool(param, value);
            }
        }
        
        [Serializable]
        public class TriggerAnimation : Animation
        {
            private Animator _animator;
            [HideIf("AnimatorIsNull")] [SerializeField] [AnimatorParam("_animator", AnimatorControllerParameterType.Trigger)] private string param;
            
            public bool AnimatorIsNull => _animator == null;
            
            public Animation SetAnimator(Animator animator)
            {
                if (_animator == null) _animator = animator;
                return this;
            }
            
            public void SetTrigger()
            {
                _animator.SetTrigger(param);
            }
        }

        public class Animation { }
    }
}