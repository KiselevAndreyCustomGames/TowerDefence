using UnityEngine;

namespace CodeBase.Utility.Extension
{
    public static class VectorExtension
    {
        public static Vector3 ApplyRotation(this Vector3 vec, float angle)
        {
            return Quaternion.Euler(0, 0, angle) * vec;
        }
    }
}