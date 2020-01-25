using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.Navigation
{
    public struct NavEdge
    {
        #region Fields

        #endregion

        #region Properties

        public NavPoint Start { get; set; }
        public NavPoint End { get; set; }

        public List<NavTriangle> ConnectedTriangles { get; set; }

        public bool IsValid { get; set; }

        #endregion

        public NavEdge(NavPoint start, NavPoint end)
        {
            Start = start;
            End = end;
            ConnectedTriangles = new List<NavTriangle>();
            IsValid = true;
        }

        public bool Intersects(NavEdge otherEdge)
        {
            return StaticHelpers.EdgesIntersect(Start.Position, End.Position, otherEdge.Start.Position, otherEdge.End.Position);
        }

        public NavPoint GetAnotherEnd(NavPoint start)
        {
            if (start == Start)
            {
                return End;
            }
            else if (start == End)
            {
                return Start;
            }
            else
            {
                return new NavPoint();
            }
        }

        public void AddConnected(NavTriangle triangle)
        {
            if (!ConnectedTriangles.Contains(triangle))
            {
                ConnectedTriangles.Add(triangle);
            }
        }

        public NavTriangle GetAnotherTriangle(NavTriangle triangle)
        {
            var other = ConnectedTriangles.Find(t => t != triangle);
            return other;
        }
    }
}
