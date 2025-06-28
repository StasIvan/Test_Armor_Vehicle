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
            if (_animator == null) return;
            
            _animator.SetBool(name, value);
        }
    }
}