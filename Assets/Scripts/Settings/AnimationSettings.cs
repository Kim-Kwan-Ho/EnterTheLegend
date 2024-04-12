using UnityEngine;

namespace StandardData
{
    public static class AnimationSettings
    {

        public static int IsIdle = Animator.StringToHash("IsIdle");
        public static int IsMoving = Animator.StringToHash("IsMoving");
        public static int IsAttacking = Animator.StringToHash("IsAttacking");
        public static int Vertical = Animator.StringToHash("Vertical");
        public static int Horizontal = Animator.StringToHash("Horizontal");
        public static float PlayerAnimationSpeed = 1.8f;
    }
}


