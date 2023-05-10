using System;
using NaughtyAttributes;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Player
{
    public class EntityAnimation : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] private Animator animator;

        [HideIf("AnimatorIsNull")] [SerializeField] private Animation walking;
        
        public bool AnimatorIsNull => animator == null;
        
        private void OnValidate()
        {
            if (animator == null) return;
            walking.SetAnimator(animator);
        }
        
        private void Awake()
        {
            if (animator == null) return;
            walking.SetAnimator(animator);
        }

        public Animation Walking => walking;
        
        [Serializable]
        public class Animation
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
    }
}