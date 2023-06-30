using System.Collections.Generic;
using System;
using NaughtyAttributes;
using UnityEngine;

namespace PanicPlayhouse.Scripts.Entities
{
    public class EntityAnimation : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] private Animator animator;

        [HideIf("AnimatorIsNull")]
        public GenericDictionary<string, Animation> animations = new GenericDictionary<string, Animation>();

        public bool AnimatorIsNull => animator == null;

        private void OnValidate()
        {
            if (animator == null) return;

            foreach (KeyValuePair<string, Animation> kvp in animations)
            {
                if (kvp.Value == null)
                    animations[kvp.Key] = new Animation();

                kvp.Value.SetAnimator(animator);
            }
        }

        private void Awake()
        {
            OnValidate();
        }

        public Animation this[string key]
        {
            get => animations[key];
            set => animations[key] = value;
        }



        public class Animation
        {
            [HideIf("AnimatorIsNull")]
            [SerializeField]
            [AnimatorParam("_animator")]
            protected string _param;
            protected Animator _animator;

            public virtual void SetValue() => _animator.SetTrigger(_param);
            public virtual void SetValue(bool value) => _animator.SetBool(_param, value);
            public virtual void SetValue(float value) => _animator.SetFloat(_param, value);
            public virtual void SetValue(int value) => _animator.SetInteger(_param, value);

            public Animation SetAnimator(Animator animator)
            {
                _animator = animator;
                return this;
            }

            public Animation()
            {
                _animator = null;
                _param = null;
            }

            public Animation(Animator animator)
            {
                _animator = animator;
                _param = null;
            }

            public Animation(Animator animator, string param)
            {
                _animator = animator;
                this._param = param;
            }
        }
    }
}