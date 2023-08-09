using CodeBase.Game;
using UnityEngine;

namespace CodeBase.Utility.Extension
{
    public static class DirectionExtension
    {
        private static Quaternion[] _rotations =
        {
            Quaternion.identity,
            Quaternion.Euler(0f, 90f, 0f),
            Quaternion.Euler(0f, 180f, 0f),
            Quaternion.Euler(0f, 270f, 0f)
        };

        public static Quaternion GetRotation(this Direction direction) =>
            _rotations[(int)direction];

        public static DirectionChange GetChangeDirectionTo(this Direction current, Direction next)
        {
            if (current == next)
                return DirectionChange.None;

            if(current + 1  == next || current - 3 == next)
                return DirectionChange.TurnRight;

            if(current - 1 == next || current + 3 == next)
                return DirectionChange.TurnLeft;

            return DirectionChange.TurnArround;
        }

        public static float GetAngle(this Direction direction) =>
            (float)direction * 90f;
     }
}