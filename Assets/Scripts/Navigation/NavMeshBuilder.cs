using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.Navigation
{
    /// <summary>
    /// Use it to test how (O^n) algorithms makes your game suffer
    /// </summary>
    public class NavMeshBuilder : NavMeshBuilderBase
    {
        #region Fields

        [SerializeField]
        int preBuildIterations = 1;

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
            System.DateTime startTime = System.DateTime.Now;

            

            System.TimeSpan timeElapsed = System.DateTime.Now - startTime;
            Debug.Log("NavMesh building took " + timeElapsed);
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
