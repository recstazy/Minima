using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.Navigation
{
    public class NavPointDistanceComparer : IComparer<NavPoint>
    {
        public NavPoint Origin { get; set; }

        public NavPointDistanceComparer(NavPoint origin)
        {
            Origin = origin;
        }

        public NavPointDistanceComparer(Vector2 origin)
        {
            Origin = new NavPoint(origin);
        }

        public int Compare(NavPoint x, NavPoint y)
        {
            var xDistance = Vector2.Distance(Origin.Position, x.Position);
            var yDistance = Vector2.Distance(Origin.Position, y.Position);

            if (xDistance > yDistance)
            {
                return 1;
            }
            else if (xDistance == yDistance)
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
