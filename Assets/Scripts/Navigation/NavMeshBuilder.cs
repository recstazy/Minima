using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.Navigation
{
    public class NavMeshBuilder : NavMeshBuilderBase
    {
        #region Fields

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
            var edgesTemp = edges.ToList();

            foreach(var e in edgesTemp)
            {
                CreateTriangle(e);
            }
        }

        protected void CreateTriangle(NavEdge edge)
        {
            var point = GetClosestPoint(edge);
            CreateTriangle(point, edge.Start, edge.End);
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

        protected void CreateTriangle(NavPoint a, NavPoint b, NavPoint c)
        {
            bool success = false;
            var ab = CreateEdge(a, b, out success);
            var ac = CreateEdge(a, c, out success);
            var bc = CreateEdge(b, c, out success);

            var triangle = new NavTriangle(ab, ac, bc);

            triangles.Add(triangle);
        }

        protected List<NavPoint> GetClosestPoints(NavPoint origin, int count, params NavPoint[] except)
        {
            var distanceComparer = new NavPointDistanceComparer(origin);
            var pointsTemp = points.ToList();

            pointsTemp.Remove(origin);
            pointsTemp = pointsTemp.Except(except).ToList();
            pointsTemp.Sort(distanceComparer);

            var toRemove = new List<NavPoint>();

            foreach(var p in pointsTemp)
            {
                if (!StaticHelpers.CheckVisibility(origin.Position, p.Position))
                {
                    toRemove.Add(p);
                }
            }

            pointsTemp = pointsTemp.Except(toRemove).ToList();
            var closest = pointsTemp.Take(count).ToList();

            return closest;
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
