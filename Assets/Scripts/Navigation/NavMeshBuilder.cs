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
        protected List<NavCell> navCells = new List<NavCell>();
        protected List<List<NavPoint>> pointLines = new List<List<NavPoint>>();

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
            CreateSquares();

            System.TimeSpan timeElapsed = System.DateTime.Now - startTime;
            Debug.Log("NavMesh building took " + timeElapsed);
        }

        protected void CreateGrid()
        {
            float boundX = buildArea.bounds.extents.x;
            float boundY = buildArea.bounds.extents.y;

            for (float x = -boundX; x <= boundX; x += Step)
            {
                var line = new List<NavPoint>();

                for (float y = -boundY; y <= boundY; y += Step)
                {
                    var point = CreatePoint(new Vector2(x, y));
                    line.Add(point);
                }

                pointLines.Add(line);
            }
        }

        protected void CreateSquares()
        {
            for (int i = 0; i < pointLines.Count - 1; i++)
            {
                for (int j = 0; j < pointLines[i].Count - 1; j++)
                {
                    var a = pointLines[i][j];
                    var b = pointLines[i][j + 1];
                    var c = pointLines[i + 1][j + 1];
                    var d = pointLines[i + 1][j];

                    var cell = CreateCell(a, b, c, d);

                    if (cell.Edge.IsValid)
                    {
                        edges.Add(cell.Edge);
                    }

                    //var debugPoint = CreatePoint(Vector2.Lerp(a.Position, c.Position, 0.5f));
                    //debugPoint.Sprite.color = Color.red;
                }
            }
        }

        protected NavCell CreateCell(NavPoint a, NavPoint b, NavPoint c, NavPoint d)
        {
            var cell = new NavCell(a, b, c, d);
            navCells.Add(cell);
            return cell;
        }

        protected void DrawEdges()
        {
            foreach (var e in edges)
            {
                Debug.DrawLine(e.Start.Position, e.End.Position, Color.red);
            }
        }

        private IEnumerator DelayedBuild()
        {
            yield return new WaitForEndOfFrame();

            BuildNavMesh();
        }
    }
}
