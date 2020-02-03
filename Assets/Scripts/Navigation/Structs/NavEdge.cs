using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.Navigation
{
    public struct NavEdge
    {
        #region Properties

        public NavPoint Start { get; set; }
        public NavPoint End { get; set; }
        public bool IsValid { get; set; }

        #endregion

        public NavEdge(NavPoint start, NavPoint end)
        {
            Start = start;
            End = end;
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
    }
}
