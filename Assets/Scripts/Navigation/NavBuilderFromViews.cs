using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

namespace Minima.Navigation
{
    public class NavBuilderFromViews : NavMeshBuilderBase
    {
        #region Fields

        [SerializeField]
        [Tooltip("How many nearest points should be checked for connection when building")]
        private int nearestPointsCount = 5;

        private Dictionary<NavPoint, NavPoint[]> invisiblePoints = new Dictionary<NavPoint, NavPoint[]>();
        private NavPointView[] views;
        private NavEdge[] edges = new NavEdge[0];

        #endregion

        #region Properties

        #endregion

        public override void BuildNavMeshImmediately()
        {
            GetViews();
            CreatePoints();
            ConnectAllPoints();
            RemoveViews();
        }

        public override int GetPointsCount()
        {
            return points.Length;
        }

        public override NavPoint[] GetPoints()
        {
            return points;
        }

        public override NavPoint GetNearestPoint(Vector2 position)
        {
            return base.GetNearestPoint(position);
        }

        protected override void DrawEdges()
        {
            foreach (var e in edges)
            {
                Debug.DrawLine(e.Start.Position, e.End.Position, Color.blue);
            }
        }

        private void CreatePoints()
        {
            var positions = views.Select(p => p.transform.position);
            points = positions.Select(p => new NavPoint(p)).ToArray();
        }

        private void ConnectAllPoints()
        {
            foreach (var p in points)
            {
                var copy = points.ToArray();
                var comparer = new NavPointDistanceComparer(p.Position);
                Array.Sort(copy, comparer);

                var count = copy.Length < nearestPointsCount ? copy.Length : nearestPointsCount;

                var nearest = copy.Take(count);

                foreach (var n in nearest)
                {
                    if (!invisiblePoints.ContainsKey(p) || !invisiblePoints[p].Contains(n))
                    {
                        bool visible = Helpers.CheckVisibility(p.Position, n.Position);

                        if (visible)
                        {
                            CreateEdge(p, n);
                        }
                        else
                        {
                            AddToInvisible(p, n);
                        }
                    }
                }
            }
        }

        private void GetViews()
        {
            views = FindObjectsOfType<NavPointView>();
        }

        private void RemoveViews()
        {
            if (!ShowPoints)
            {
                foreach (var v in views)
                {
                    Destroy(v.gameObject);
                }
            }
        }

        private void CreateEdge(NavPoint a, NavPoint b)
        {
            if (!b.ConnectedPoints.Contains(a))
            {
                var edge = new NavEdge(a, b);
                edges = edges.ConcatOne(edge);
            }
        }

        private void AddToInvisible(NavPoint a, NavPoint b)
        {
            AddInvisible(a, b);
            AddInvisible(b, a);
        }

        private void AddInvisible(NavPoint key, NavPoint newInvisible)
        {
            if (invisiblePoints.ContainsKey(key))
            {
                if (!invisiblePoints[key].Contains(newInvisible))
                {
                    invisiblePoints[key] = invisiblePoints[key].ConcatOne(newInvisible);
                }
            }
            else
            {
                invisiblePoints.Add(key, new NavPoint[] { newInvisible });
            }
        }
    }
}
