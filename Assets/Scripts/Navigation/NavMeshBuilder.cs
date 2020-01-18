using System.Collections;
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
            CreateBounds(buildArea);
            CreateObstaclesBounds();
        }

        protected void CreateTriangle(NavPoint a, NavPoint b, NavPoint c)
        {
            var ab = new NavEdge(a, b);
            var ac = new NavEdge(a, c);
            var bc = new NavEdge(b, c);

            var triangle = new NavTriangle(ab, ac, bc);

            triangles.Add(triangle);
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
