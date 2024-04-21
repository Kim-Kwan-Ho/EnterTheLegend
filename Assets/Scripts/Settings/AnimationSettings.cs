using UnityEngine;

namespace StandardData
{
    public static class AnimationSettings
    {
        public static int Direction = Animator.StringToHash("Direction");
        public static int Speed = Animator.StringToHash("Speed");
        public static int Die = Animator.StringToHash("Die");
        public static int BowLoad = Animator.StringToHash("BowLoad");
        public static int BowRelease = Animator.StringToHash("BowRelease");
        public static int Attack = Animator.StringToHash("Attack");
        public static float PlayerAnimationSpeed = 1.8f;
    }
}


