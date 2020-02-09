using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.Navigation
{
    public class CostComparer : IComparer<NavPoint>
    {
        public Vector2 Origin { get; set; }
        public Vector2 Target { get; set; }

        public CostComparer(Vector2 origin, Vector2 target)
        {
            Target = target;
            Origin = origin;
        }

        public int Compare(NavPoint x, NavPoint y)
        {
            float costX = (Target - x.Position).sqrMagnitude * (Origin - x.Position).sqrMagnitude;
            float costY = (Target - y.Position).sqrMagnitude * (Origin - y.Position).sqrMagnitude;

            if (costY > costX)
            {
                return 1;
            }
            else if (costY == costX)
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
