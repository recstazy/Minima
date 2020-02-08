using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.Navigation
{
    public struct NavTriangle
    {
        #region Fields

        #endregion

        #region Properties

        public NavEdge AB { get; set; }
        public NavEdge BC { get; set; }
        public NavEdge AC { get; set; }

        public NavPoint A { get; }
        public NavPoint B { get; }
        public NavPoint C { get; }

        public bool IsValid { get; private set; }
        public Vector2 Center { get; private set; }
        
        #endregion

        public NavTriangle(NavEdge ab, NavEdge bc, NavEdge ac)
        {
            AB = ab;
            BC = bc;
            AC = ac;

            A = AB.Start;
            B = BC.Start;
            C = AC.End;

            Center = Helpers.GetTriangleCenter(A.Position, B.Position, C.Position);
            IsValid = true;
        }

        public NavTriangle(NavEdge edge, NavPoint point)
        {
            A = edge.Start;
            B = edge.End;
            C = point;

            AB = edge;
            BC = new NavEdge(B, C);
            AC = new NavEdge(A, C);

            Center = Helpers.GetTriangleCenter(A.Position, B.Position, C.Position);
            IsValid = true;
        }

        public NavPoint ClosestVertex(Vector2 origin)
        {
            var vertices = new List<NavPoint> { A, B, C };
            var comparer = new NavPointDistanceComparer(origin);
            vertices.Sort(comparer);
            return vertices.FirstOrDefault();
        }

        #region Operators

        public static bool operator== (NavTriangle a, NavTriangle b)
        {
            return IsEqual(a, b);
        }

        public static bool operator!= (NavTriangle a, NavTriangle b)
        {
            return !IsEqual(a, b);
        }

        /// <summary>
        /// Two triangles can't intersect, so for equality it's enough to their centers be equal
        /// </summary>
        private static bool IsEqual(NavTriangle a, NavTriangle b)
        {
            bool xEqual = a.Center.x.Equal(b.Center.x);
            bool yEqual = a.Center.y.Equal(b.Center.y);

            return xEqual && yEqual;
        }

        #endregion

    }
}
