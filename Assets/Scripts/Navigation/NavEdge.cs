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

        #endregion

        public NavEdge(NavPoint start, NavPoint end)
        {
            Start = start;
            End = end;
            ConnectedTriangles = new List<NavTriangle>();
        }
    }
}
