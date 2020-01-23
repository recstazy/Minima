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
        [Range(1, 10)]
        private int density = 1;

        protected List<NavTriangle> triangles = new List<NavTriangle>();

        #endregion

        #region Properties

        private float Step { get => 1f / density; }

        #endregion

        void Start()
        {
            //BuildNavMesh();
        }

        protected virtual void Update()
        {
            if (showDebug)
            {
                DrawEdges();
            }
        }

        /// <summary>
        /// Because colliders doesn't update their bounds until new frame
        /// </summary>
        public void BuildDelayed()
        {
            StartCoroutine(DelayedBuild());
        }

        public override void BuildNavMesh()
        {
            System.DateTime startTime = System.DateTime.Now;

            GetAllObstacles();
            CreateGrid();

            System.TimeSpan timeElapsed = System.DateTime.Now - startTime;
            Debug.Log("NavMesh building took " + timeElapsed);
        }

        protected void CreateGrid()
        {
            float boundX = buildArea.bounds.extents.x;
            float boundY = buildArea.bounds.extents.y;

            for (float x = -boundX; x <= boundX; x += Step)
            {
                for (float y = -boundY; y <= boundY; y += Step)
                {
                    var point = CreatePoint(new Vector2(x, y));
                }
            }
        }

        protected void DrawEdges()
        {
            foreach (var e in edges)
            {
                Debug.DrawLine(e.Start.Position, e.End.Position);
            }
        }

        private IEnumerator DelayedBuild()
        {
            yield return new WaitForEndOfFrame();

            BuildNavMesh();
        }
    }
}
