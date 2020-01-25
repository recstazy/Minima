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
        [Range(0.1f, 10f)]
        private float density = 1f;

        protected List<NavTriangle> triangles = new List<NavTriangle>();
        protected List<NavCell> navCells = new List<NavCell>();
        protected List<List<NavPoint>> pointLines = new List<List<NavPoint>>();

        protected List<List<NavCell>> cellLines = new List<List<NavCell>>();
        protected Transform thisTransform;

        #endregion

        #region Properties

        private float Step { get => 1f / density; }

        #endregion

        private void Awake()
        {
            thisTransform = transform;
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
            CreateCells();

            System.TimeSpan timeElapsed = System.DateTime.Now - startTime;
            Debug.Log("NavMesh building took " + timeElapsed);
        }

        protected void CreateGrid()
        {
            float boundX = buildArea.bounds.extents.x;
            float boundY = buildArea.bounds.extents.y;
            var origin = thisTransform.position.ToVector2();

            for (float x = -boundX; x <= boundX; x += Step)
            {
                var line = new List<NavPoint>();

                for (float y = -boundY; y <= boundY; y += Step)
                {
                    var point = CreatePoint(origin + new Vector2(x, y));
                    line.Add(point);
                }

                pointLines.Add(line);
            }
        }

        protected void CreateCells()
        {
            for (int i = 0; i < pointLines.Count - 1; i++)
            {
                var line = new List<NavCell>();

                for (int j = 0; j < pointLines[i].Count - 1; j++)
                {
                    var a = pointLines[i][j];
                    var b = pointLines[i][j + 1];
                    var c = pointLines[i + 1][j + 1];
                    var d = pointLines[i + 1][j];

                    var cell = CreateCell(a, b, c, d);

                    if (cell.Edges.Count > 0)
                    {
                        edges = edges.Concat(cell.Edges).ToList();
                    }

                    foreach(var p in cell.Points)
                    {
                        points.AddUniq(p);
                    }

                    line.Add(cell);
                }

                cellLines.Add(line);
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
