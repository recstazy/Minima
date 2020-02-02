using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.Navigation
{
    public class EdgeDistanceComparer : IComparer<NavPoint>
    {
        public NavEdge Edge { get; set; }

        public EdgeDistanceComparer(NavEdge edge)
        {
            Edge = edge;
        }

        public int Compare(NavPoint x, NavPoint y)
        {
            var center = Edge.Start.Position + (Edge.End.Position - Edge.Start.Position) / 2;

            var distanceX = Vector2.Distance(center, x.Position);
            var distanceY = Vector2.Distance(center, y.Position);

            if (distanceX > distanceY)
            {
                return 1;
            }
            else if (distanceX == distanceY)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
    }
}
