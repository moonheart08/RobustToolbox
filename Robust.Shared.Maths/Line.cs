using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Robust.Shared.Maths
{
    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public struct Line
    {
        [FieldOffset(sizeof(float) * 0)] public float x1;
        [FieldOffset(sizeof(float) * 1)] public float y1;
        [FieldOffset(sizeof(float) * 0)] public float x2;
        [FieldOffset(sizeof(float) * 1)] public float y2;

        [FieldOffset(sizeof(float) * 0)] public Vector2 a;
        [FieldOffset(sizeof(float) * 2)] public Vector2 b;

        public float Length
        {
            get => (float) Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        public float LengthSquared
        {
            get => (float)(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        public Line(Vector2 _a, Vector2 _b)
        {
            Unsafe.SkipInit(out this);
            a = _a;
            b = _b;
        }

        public float Run()
        {
            if (x1 > x2)
            {
                return x1 - x2;
            }

            return x2 - x1;
        }

        public float Rise()
        {
            if (y1 > y2)
            {
                return y1 - y2;
            }

            return y2 - y1;
        }

        public float Slope()
        {
            return Rise() / Run();
        }

        /// <summary>
        /// Finds a co-linear point 'distance' away from the start of the line segment.
        /// </summary>
        /// <param name="distance">Distance from line segment start (a)</param>
        /// <returns>A co-linear point along the full line.</returns>
        public Vector2 GetColinearPoint(float distance)
        {
            return ((b-a).Normalized * distance) + a;
        }

        /// <summary>
        /// Calculates the nearest subdivison to the requested one.
        /// </summary>
        /// <param name="distance">Distance to the colinear point that matches the wanted subdivison</param>
        /// <returns>A subdivision distance that evenly divides the whole line</returns>
        public float NearestSubdivison(float distance)
        {
            return (Length / distance) >= distance ? Length / distance : Length;
        }

        public IEnumerable<Vector2> NearestPointsAlongLine(float distance)
        {
            var evenSubdiv = NearestSubdivison(distance);
            var icount = (int)Math.Round(Length / evenSubdiv);

            // Guaranteed to only return points on the line
            foreach (var i in Enumerable.Range(0, icount))
            {
                yield return GetColinearPoint(evenSubdiv * i);
            }
        }
    }
}
