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

        public List<NavEdge> Edges
        {
            get
            {
                return new List<NavEdge> { AB, BC, AC };
            }
        }

        public List<NavTriangle> Connected { get => GetConnected(); }
        public bool IsValid { get; private set; }
        public Vector2 Center { get; private set; }
        
        #endregion

        public NavTriangle(NavPoint a, NavPoint b, NavPoint c)
        {
            A = a;
            B = b;
            C = c;

            AB = new NavEdge(a, b);
            BC = new NavEdge(b, c);
            AC = new NavEdge(a, c);

            Center = StaticHelpers.GetTriangleCenter(A.Position, B.Position, C.Position);
            IsValid = true;
            AddSelfToConnected();
        }

        public NavTriangle(NavEdge ab, NavEdge bc, NavEdge ac)
        {
            AB = ab;
            BC = bc;
            AC = ac;

            A = AB.Start;
            B = BC.Start;
            C = AC.End;

            Center = StaticHelpers.GetTriangleCenter(A.Position, B.Position, C.Position);
            IsValid = true;
            AddSelfToConnected();
        }

        public NavTriangle(NavEdge edge, NavPoint point)
        {
            A = edge.Start;
            B = edge.End;
            C = point;

            AB = edge;
            BC = new NavEdge(B, C);
            AC = new NavEdge(A, C);

            Center = StaticHelpers.GetTriangleCenter(A.Position, B.Position, C.Position);
            IsValid = true;
            AddSelfToConnected();
        }

        public NavPoint ClosestVertex(Vector2 origin)
        {
            var vertices = new List<NavPoint> { A, B, C };
            var comparer = new NavPointDistanceComparer(origin);
            vertices.Sort(comparer);
            return vertices.FirstItem();
        }

        private List<NavTriangle> GetConnected()
        {
            List<NavTriangle> connected = new List<NavTriangle>();

            AddConnectedToList(AB, connected);
            AddConnectedToList(BC, connected);
            AddConnectedToList(AC, connected);
            return connected;
        }

        private void AddSelfToConnected()
        {
            AB.AddConnected(this);
            BC.AddConnected(this);
            AC.AddConnected(this);
        }

        private void AddConnectedToList(NavEdge edge, List<NavTriangle> list)
        {
            if (edge.IsValid)
            {
                list.Add(edge.GetAnotherTriangle(this));
            }
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
