using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.LevelGeneration
{
    public class CornerDistanceComparer : IComparer<WallCorner>
    {
        public Vector2 Origin { get; set; }

        public CornerDistanceComparer(Vector2 origin)
        {
            Origin = origin;
        }

        public int Compare(WallCorner x, WallCorner y)
        {
            var distanceX = Vector2.Distance(x.position, Origin);
            var distanceY = Vector2.Distance(y.position, Origin);

            if (distanceY > distanceX)
            {
                return 1;
            }
            else if (distanceY == distanceX)
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
