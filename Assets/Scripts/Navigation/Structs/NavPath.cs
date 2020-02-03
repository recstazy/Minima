using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.Navigation
{
    public struct NavPath
    {
        #region Properties

        public NavPoint[] NavPoints { get; set; }
        public Vector2[] Points { get; set; }
        public bool IsValid { get; private set; }
        public int Length { get => Points.Length; }

        #endregion

        public NavPath(params NavPoint[] points)
        {
            NavPoints = new NavPoint[0];
            Points = new Vector2[0];
            IsValid = true;

            foreach (var t in points)
            {
                Add(t);
            }
        }

        public NavPath(params Vector2[] points)
        {
            NavPoints = new NavPoint[0];
            Points = new Vector2[0];
            IsValid = true;

            foreach (var t in points)
            {
                Add(t);
            }
        }

        public void Add(NavPoint point)
        {
            NavPoints = NavPoints.ConcatOne(point);
            Points = Points.ConcatOne(point.Position);
        }

        public void Add(Vector2 point)
        {
            NavPoints = NavPoints.ConcatOne(new NavPoint(point));
            Points = Points.ConcatOne(point);
        }
    }
}
