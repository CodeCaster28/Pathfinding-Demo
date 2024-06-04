using System;

namespace DataStructures
{
    /// <summary>
    /// Simple point class holding x and y values, used to represent cartesian coordinates.
    /// </summary>
    public readonly struct Point
    {
        /// <summary>
        /// X coordinate.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Y coordinate.
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// Creates a new point.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Equal operator.
        /// </summary>
        /// <param name="a">Point A.</param>
        /// <param name="b">Point B.</param>
        /// <returns>TRUE if points are equal.</returns>
        public static bool operator ==(Point a, Point b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Not equal operator.
        /// </summary>
        /// <param name="a">Point A.</param>
        /// <param name="b">Point B.</param>
        /// <returns>TRUE if points are not equal.</returns>
        public static bool operator !=(Point a, Point b)
        {
            return !a.Equals(b);
        }

        /// <summary>
        /// Check if point is equal to object.
        /// </summary>
        /// <param name="obj">Object to inspect.</param>
        /// <returns>TRUE if object is equal to this Point.</returns>
        public override bool Equals(object obj)
        {
            return obj is Point other && Equals(other);
        }

        /// <summary>
        /// Combines two values into a hash code.
        /// </summary>
        /// <returns>The hash code that represents the two values.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        /// <summary>
        /// Check if point's X and Y coordinates both equals.
        /// </summary>
        /// <param name="other">Point to test equality with.</param>
        /// <returns></returns>
        private bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }

        /// <summary>
        /// Check if point's X and Y coordinates are equal to parameters coordinates.
        /// </summary>
        /// <param name="x">Coordinate X to test equality with.</param>
        /// <param name="y">Coordinate Y to test equality with.</param>
        /// <returns>TRUE if point lays on specified coordinates.</returns>
        public bool Equals(int x, int y)
        {
            return X == x && Y == y;
        }

        /// <summary>
        /// Check if any of point coordinates are negative.
        /// </summary>
        /// <returns>TRUE if X or Y or both coordinates are negative.</returns>
        public bool Negative()
        {
            return X < 0 || Y < 0;
        }
    }
}
