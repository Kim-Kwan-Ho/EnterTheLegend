using UnityEngine;

namespace StandardData
{
    public static class PlayerMaximumStats
    {
        public const ushort MaxHp = 30;
        public const ushort MaxAttack = 30;
        public const ushort MaxDefense = 30;
        public const ushort MaxAttackDistance = 30;
    }

    public static class TextColors
    {
        public static readonly Color PlayerColor = new Color(0, 209, 8);
        public static readonly Color TeamColor = new Color(0, 111, 209);
        public static readonly Color EnemyColor = new Color(255, 35, 0);
    }

    public static class ImageColors
    {
        public static readonly Color TeamColor = new Color(0, 255, 9);
        public static readonly Color EnemyColor = new Color(255, 35, 0);
    }
}