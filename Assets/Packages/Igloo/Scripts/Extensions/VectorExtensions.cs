using UnityEngine;

namespace Packages.Igloo.Scripts.Extensions
{
    public static class VectorExtensions
    {
        public static Vector2 RandomInsideAnnulus(float lowerBound, float upperBound)
        {
            float distance = Random.Range(lowerBound, upperBound);
            float angle = Random.Range(0, 360);

            Vector2 randomPosition = new Vector2(distance * Mathf.Cos(angle), distance * Mathf.Sin(angle));
            return randomPosition;
        }

        public static Vector2 X(this Vector2 vector, float v)
        {
            vector.x = v;
            return vector;
        }

        public static Vector2 Y(this Vector2 vector, float v)
        {
            vector.y = v;
            return vector;
        }

        public static Vector3 X(this Vector3 vector, float v)
        {
            vector.x = v;
            return vector;
        }

        public static Vector3 Y(this Vector3 vector, float v)
        {
            vector.y = v;
            return vector;
        }

        public static Vector3 Z(this Vector3 vector, float v)
        {
            vector.z = v;
            return vector;
        }

        public static Vector4 X(this Vector4 vector, float v)
        {
            vector.x = v;
            return vector;
        }

        public static Vector4 Y(this Vector4 vector, float v)
        {
            vector.y = v;
            return vector;
        }

        public static Vector4 Z(this Vector4 vector, float v)
        {
            vector.z = v;
            return vector;
        }

        public static Vector4 W(this Vector4 vector, float v)
        {
            vector.w = v;
            return vector;
        }
    }
}