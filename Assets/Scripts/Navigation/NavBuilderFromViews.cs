using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Minima.Navigation
{
    public class NavBuilderFromViews : NavMeshBuilderBase
    {
        #region Fields

        private Dictionary<NavPoint, NavPoint[]> invisiblePoints = new Dictionary<NavPoint, NavPoint[]>();
        private NavEdge[] edges = new NavEdge[0];

        #endregion

        #region Properties

        #endregion

        public override void BuildNavMeshImmediately()
        {
            CreatePoints();
            ConnectAllPoints();
        }

        public override int GetPointsCount()
        {
            return points.Length;
        }

        public override NavPoint[] GetPoints()
        {
            return points;
        }

        private void CreatePoints()
        {
            var views = FindObjectsOfType<NavPointView>();
            var positions = views.Select(p => p.transform.position);
            points = positions.Select(p => new NavPoint(p)).ToArray();
        }

        private void ConnectAllPoints()
        {
            foreach (var p in points)
            {
                foreach (var n in points)
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

        protected override void DrawEdges()
        {
            foreach (var e in edges)
            {
                Debug.DrawLine(e.Start.Position, e.End.Position, Color.blue);
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
