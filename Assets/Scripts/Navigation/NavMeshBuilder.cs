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
        protected List<NavEdge> edges = new List<NavEdge>();

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
            CreateBounds(buildArea);
            CreateObstaclesBounds();

            CreateTriangles();
        }

        void CreateTriangles()
        {
            var firstPoints = GetClosestPoints(points[0], 3);
            CreateTriangle(firstPoints[0], firstPoints[1], firstPoints[2]);
            Debug.Log(triangles.Count);
        }

        protected void CreateTriangle(NavPoint a, NavPoint b, NavPoint c)
        {
            var ab = new NavEdge(a, b);
            var ac = new NavEdge(a, c);
            var bc = new NavEdge(b, c);

            var triangle = new NavTriangle(ab, ac, bc);

            triangles.Add(triangle);
        }

        protected List<NavPoint> GetClosestPoints(NavPoint origin, int count)
        {
            var distanceComparer = new NavPointDistanceComparer(origin);
            var pointsTemp = points.ToList();

            pointsTemp.Remove(origin);
            pointsTemp.Sort(distanceComparer);

            var closest = pointsTemp.Take(count).ToList();

            foreach (var p in closest)
            {
                Debug.Log(Vector2.Distance(origin.Position, p.Position));
            }

            return closest;
        }

        protected void DrawEdges()
        {
            foreach (var t in triangles)
            {
                Debug.DrawLine(t.A.Position, t.B.Position, Color.white, Time.deltaTime);
                Debug.DrawLine(t.B.Position, t.C.Position, Color.white, Time.deltaTime);
                Debug.DrawLine(t.A.Position, t.C.Position, Color.white, Time.deltaTime);
            }
        }
    }
}
