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

        public NavEdge AB { get; private set; }
        public NavEdge BC { get; private set; }
        public NavEdge AC { get; private set; }

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
        
        #endregion

        public NavTriangle(NavPoint a, NavPoint b, NavPoint c)
        {
            A = a;
            B = b;
            C = c;

            AB = new NavEdge(a, b);
            BC = new NavEdge(b, c);
            AC = new NavEdge(a, c);
        }

        public NavTriangle(NavEdge edge, NavPoint point)
        {
            A = edge.Start;
            B = edge.End;
            C = point;

            AB = edge;
            BC = new NavEdge(B, C);
            AC = new NavEdge(A, C);
        }

        private List<NavTriangle> GetConnected()
        {
            List<NavTriangle> connected = new List<NavTriangle>();

            connected = connected.Concat(AB.ConnectedTriangles).ToList();
            connected = connected.Concat(BC.ConnectedTriangles).ToList();
            connected = connected.Concat(AC.ConnectedTriangles).ToList();

            var thisTriangle = this;

            connected.RemoveAll(t => t == thisTriangle);

            return connected;
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

        private static bool IsEqual(NavTriangle a, NavTriangle b)
        {
            if (a.A == b.A && a.B == b.B && a.C == b.C)
            {
                return true;
            }

            return false;
        }

        #endregion

    }
}
