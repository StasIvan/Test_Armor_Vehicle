using Interfaces;
using UnityEngine;

namespace GameItems
{
    public class AnimatorController : IAnimationSetter
    {
        private Animator _animator;

        public AnimatorController(Animator animator)
        {
            _animator = animator;
        }

        public void SetAnimation(string name, bool value)
        {
            _animator.SetBool(name, value);
        }
    }
}