using DataStructures;
using UnityEngine;

namespace Extensions
{
    /// <summary>
    /// Extension methods class.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Returns Vector3 with replaced X value.
        /// </summary>
        public static Vector3 WithX(this Vector3 v, float x)
        {
            return new Vector3(x, v.y, v.z);
        }

        /// <summary>
        /// Returns Vector3 with replaced Y value.
        /// </summary>
        public static Vector3 WithY(this Vector3 v, float y)
        {
            return new Vector3(v.x, y, v.z);
        }

        /// <summary>
        /// Returns Vector3 with replaced Z value.
        /// </summary>
        public static Vector3 WithZ(this Vector3 v, float z)
        {
            return new Vector3(v.x, v.y, z);
        }

        /// <summary>
        /// Returns Vector3 with coordinates rounded to nearest integer.
        /// </summary>
        public static Vector3 Round(this Vector3 v)
        {
            return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
        }

        /// <summary>
        /// Convert Point to Vector3 optionally giving Y value of Vector3.
        /// </summary>
        /// <param name="point">Point to convert.</param>
        /// <param name="yValue">Y value of target Vector3.</param>
        /// <returns>Vector3 from specified Point with optional custom Y value.</returns>
        public static Vector3 ToVector3(this Point point, float yValue = 0.0f)
        {
            return new Vector3(point.X, yValue, point.Y);
        }

        /// <summary>
        /// Convert Vector3 to Point.
        /// </summary>
        /// <param name="vector">Vector3 to convert.</param>
        /// <returns>Point composed of X and Z values of Vector3.</returns>
        public static Point ToPoint(this Vector3 vector)
        {
            return new Point((int)vector.x, (int)vector.z);
        }
    }
}
