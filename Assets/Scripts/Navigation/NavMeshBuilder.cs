using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.Navigation
{
    public class NavMeshBuilder : NavMeshBuilderBase
    {
        #region Fields

        [SerializeField]
        int preBuildIterations = 2;

        protected List<NavTriangle> triangles = new List<NavTriangle>();

        #endregion

        #region Properties

        #endregion

        void Start()
        {
            BuildNavMesh();
        }

        protected virtual void Update()
        {
            if (showDebug)
            {
                DrawEdges();
            }
        }

        public override void BuildNavMesh()
        {
            CreateBounds(buildArea, createEdges: true);
            CreateObstaclesBounds();

            CreateTriangles();
        }

        protected void CreateTriangles()
        {
            bool success;
            int iterations = 0;

            do
            {
                success = false;
                var edgesTemp = edges.ToList();

                foreach (var e in edgesTemp)
                {
                    var complete = CreateTriangle(e);
                    if (complete)
                    {
                        success = true;
                    }
                }
                iterations++;
            }
            while (success && iterations < preBuildIterations);

            AddMissingEdges();
            CleanUpPoints();
        }

        private void AddMissingEdges()
        {
            foreach (var origin in points.ToList())
            {
                var connected = origin.ConnectedPoints;
                connected.Remove(origin);

                foreach (var nextPoint in connected)
                {
                    var nextConnected = nextPoint.ConnectedPoints;
                    nextConnected.RemoveAll(x => x == origin || x == nextPoint);

                    foreach (var thirdPoint in nextConnected)
                    {
                        if (thirdPoint.ConnectedPoints.Contains(origin))
                        {
                            continue;
                        }

                        CreateEdge(origin, thirdPoint);

                        if (origin.ConnectedPoints.Count > 100)
                        {
                            throw new System.Exception("NavMeshBuilder: Origin.ConnectedPoints.Count > 100");
                        }
                    }
                }
            }
        }

        private void CleanUpPoints()
        {
            foreach (var p in points.ToList())
            {
                if (p.ConnectedEdges.Count == 0)
                {
                    var point = GetClosestPoints(p, 1);
                    var edge = CreateEdge(p, point[0]);

                    if (edge.IsValid)
                    {
                        var thirdPoints = GetClosestPoints(edge.End.ConnectedPoints, p, 1);
                        var thirdEdge = CreateEdge(thirdPoints[0], p);

                        if (thirdEdge.IsValid)
                        {
                        }
                        else
                        {
                            edges.Remove(edge);
                            points.Remove(p);
                        }
                    }
                    else
                    {
                        points.Remove(p);
                    }
                }
            }
        }

        protected bool CreateTriangle(NavEdge edge)
        {
            var point = GetClosestPoint(edge);
            return CreateTriangle(point, edge.Start, edge.End);
        }

        protected void CreateTriangle(NavPoint origin)
        {
            var firstPoints = GetClosestPoints(origin, 2);

            if (firstPoints.Count < 2)
            {
                return;
            }

            CreateTriangle(origin, firstPoints[0], firstPoints[1]);
        }

        protected bool CreateTriangle(NavPoint a, NavPoint b, NavPoint c)
        {
            var ab = CreateEdge(a, b);
            var ac = CreateEdge(a, c);
            var bc = CreateEdge(b, c);

            if (!ab.IsValid || !ac.IsValid || !bc.IsValid)
            {
                return false;
            }

            return true;
        }

        protected NavPoint GetClosestPoint(NavEdge edge)
        {
            var closest = GetClosestPoints(edge.Start, 3, edge.End);

            var comparer = new EdgeDistanceComparer(edge);

            closest.Sort(comparer);

            return closest[0];
        }

        protected void DrawEdges()
        {
            foreach (var e in edges)
            {
                Debug.DrawLine(e.Start.Position, e.End.Position);
            }
        }
    }
}
