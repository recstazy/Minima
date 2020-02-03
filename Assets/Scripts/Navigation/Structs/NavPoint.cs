using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Minima.Navigation
{
    public struct NavPoint
    {
        #region Properties

        public SpriteRenderer Sprite { get; set; }
        public Vector2 Position { get; set; }
        public List<NavEdge> ConnectedEdges { get; set; }
        public bool IsValid { get; set; }

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

        public bool Activated { get; set; }


        #endregion

        public NavPoint(Vector2 position)
        {
            Position = position;
            ConnectedEdges = new List<NavEdge>();
            Activated = false;
            Sprite = null;
            IsValid = true;
        }

        public NavPoint ClosestVertex(Vector2 target, params NavPoint[] except)
        {
            var connected = ConnectedPoints.Except(except).ToArray();
            var comparer = new NavPointDistanceComparer(target);
            System.Array.Sort(connected, comparer);
            return connected.FirstOrDefault();
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
            bool xEqual = a.Position.x.Equal(b.Position.x);
            bool yEqual = a.Position.y.Equal(b.Position.y);
            return xEqual && yEqual;
        }

        #endregion

    }
}
