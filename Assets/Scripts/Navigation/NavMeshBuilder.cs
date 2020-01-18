using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.Navigation
{
    public class NavMeshBuilder : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private GameObject pointPrefab;

        [SerializeField]
        private float pointsPerMeter = 2f;

        [SerializeField]
        private bool showDebug = false;

        [SerializeField]
        private Vector2 start;

        [SerializeField]
        private Vector2 end;

        private List<NavTriangle> triangles = new List<NavTriangle>();
        private List<List<NavPoint>> points = new List<List<NavPoint>>();

        #endregion

        #region Properties

        private float step { get => 1f / pointsPerMeter; }

        #endregion

        private void Start()
        {
            BuildNavMesh();
        }

        void Update()
        {
            if (showDebug)
            {
                DrawEdges();
            }
        }

        public void BuildNavMesh()
        {
            CreatePoints();
            CreateEdges();
        }

        private void CreateEdges()
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                var lineAB = points[i];
                var lineCD = points[i + 1];

                for (int j = 0; j < lineAB.Count - 1; j++)
                {
                    var a = lineAB[j];
                    var b = lineAB[j + 1];
                    var c = lineCD[j];
                    var d = lineCD[j + 1];

                    CreateSquareTriangulated(a, b, c, d);
                }
            }
        }

        private void CreateSquareTriangulated(NavPoint a, NavPoint b, NavPoint c, NavPoint d)
        {
            var ab = new NavEdge(a, b);
            var ac = new NavEdge(a, c);
            var bc = new NavEdge(b, c);
            var cd = new NavEdge(c, d);
            var bd = new NavEdge(b, d);

            var triangleA = new NavTriangle(ab, ac, bc);
            var triangleB = new NavTriangle(cd, bd, bc);

            triangles.Add(triangleA);
            triangles.Add(triangleB);
        }

        private void CreatePoints()
        {
            for (float y = start.y; y <= end.x; y += step)
            {
                CreatePointsLine(y);
            }
        }

        private List<NavPoint> CreatePointsLine(float y)
        {
            List<NavPoint> newPoints = new List<NavPoint>();

            for (float x = start.x; x <= end.x; x += step)
            {
                newPoints.Add(CreatePoint(new Vector2(x, y)));
            }

            points.Add(newPoints);

            return newPoints;
        }

        private NavPoint CreatePoint(Vector2 position)
        {
            var point = new NavPoint(position);
            Instantiate(pointPrefab, position, Quaternion.identity, this.transform);
            return point;
        }

        private void DrawEdges()
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
