using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.Navigation
{
    public struct NavPath
    {
        #region Properties

        public List<NavPoint> NavPoints { get; set; }
        public List<Vector2> Points { get; set; }
        public bool IsValid { get; private set; }

        #endregion

        public NavPath(params NavPoint[] points)
        {
            NavPoints = new List<NavPoint>();
            Points = new List<Vector2>();
            IsValid = true;

            foreach (var t in points)
            {
                Add(t);
            }
        }

        public NavPath(params Vector2[] points)
        {
            NavPoints = new List<NavPoint>();
            Points = new List<Vector2>();
            IsValid = true;

            foreach (var t in points)
            {
                Add(t);
            }
        }

        public void Add(NavPoint point)
        {
            NavPoints.Add(point);
            Points.Add(point.Position);
        }

        public void Add(Vector2 point)
        {
            NavPoints.Add(new NavPoint(point));
            Points.Add(point);
        }
    }
}
