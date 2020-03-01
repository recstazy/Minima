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
        public NavPoint[] Except { get; set; }
        public bool IsValid { get; private set; }
        public int Length { get => Points.Length; }

        #endregion

        public NavPath(params NavPoint[] points)
        {
            NavPoints = new NavPoint[0];
            Points = new Vector2[0];
            Except = new NavPoint[0];
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
            Except = new NavPoint[0];
            IsValid = true;

            foreach (var t in points)
            {
                Add(t);
            }
        }

        public void InitializeEmpty()
        {
            NavPoints = new NavPoint[0];
            Points = new Vector2[0];
            Except = new NavPoint[0];
            IsValid = true;
        }

        public void Exclude(params NavPoint[] points)
        {
            Except = Except.ConcatUniq(points);
            NavPoints = NavPoints.Except(Except).ToArray();
            Points = Points.Except(Except.Select(p => p.Position)).ToArray();
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

        public void Add(NavPoint[] points)
        {
            NavPoints = NavPoints.Concat(points).ToArray();
            Points = Points.Concat(points.Select(p => p.Position)).ToArray();
        }

        public static NavPath operator+(NavPath a, NavPath b)
        {
            var result = new NavPath();

            result.Points = a.Points.Concat(b.Points).ToArray();
            result.NavPoints = a.NavPoints.Concat(b.NavPoints).ToArray();
            result.Except = a.Except.Concat(b.Except).ToArray();
            result.IsValid = true;

            return result;
        }
    }
}
