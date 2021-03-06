﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.Navigation
{
    public struct NavPoint
    {
        #region Properties

        public Vector2 Position { get; set; }

        public List<NavEdge> ConnectedEdges { get; set; }

        public List<NavPoint> ConnectedPoints
        {
            get
            {
                var connected = new List<NavPoint>();

                foreach (var e in ConnectedEdges)
                {
                    var p = e.GetAnotherEnd(this);
                    connected.Add(p);
                }

                return connected;
            }
        }


        #endregion

        public NavPoint(Vector2 position)
        {
            Position = position;
            ConnectedEdges = new List<NavEdge>();
        }

        #region Operators

        public static bool operator== (NavPoint a, NavPoint b)
        {
            return IsEqual(a, b);
        }

        public static bool operator!= (NavPoint a, NavPoint b)
        {
            return !IsEqual(a, b);
        }

        private static bool IsEqual(NavPoint a, NavPoint b)
        {
            return a.Position == b.Position;
        }

        #endregion

    }
}
