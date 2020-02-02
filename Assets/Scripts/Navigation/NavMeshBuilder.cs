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
        protected float density = 1f;

        [SerializeField]
        private bool autoActivate = false;

        protected List<List<NavCell>> cellLines = new List<List<NavCell>>();

        protected List<float> xAxes = new List<float>();
        protected List<float> yAxes = new List<float>();

        protected Transform thisTransform;

        #endregion

        #region Properties

        private float Step { get => 1f / density; }
        

        #endregion

        private void Awake()
        {
            thisTransform = transform;
        }

        private void Start()
        {
            if (autoActivate)
            {
                BuildDelayed();
            }
        }

        protected virtual void Update()
        {
            if (ShowDebug)
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
            GetAllObstacles();
            InitializeGridAxes();
            CreateGrid();
            CreateCells();
        }

        protected virtual void InitializeGridAxes()
        {
            float boundX = buildArea.bounds.extents.x;
            float boundY = buildArea.bounds.extents.y;

            for (float x = -boundX; x <= boundX; x += Step)
            {
                xAxes.Add(x);
            }

            for (float y = -boundY; y <= boundY; y += Step)
            {
                yAxes.Add(y);
            }
        }

        protected void CreateGrid()
        {
            var origin = thisTransform.position.ToVector2();

            foreach (var x in xAxes)
            {
                var line = new List<NavPoint>();

                foreach (var y in yAxes)
                {
                    var point = CreatePoint(origin + new Vector2(x, y));
                    line.Add(point);
                }

                pointLines.Add(line);
            }
        }

        protected void CreateCells()
        {
            int cellX = -1;
            int cellY = -1;

            for (int i = 0; i < pointLines.Count - 1; i++)
            {
                var line = new List<NavCell>();
                cellX = -1;

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

                    if (cell.Triangles.Count > 0)
                    {
                        triangles = triangles.Concat(cell.Triangles).ToList();
                    }

                    foreach(var p in cell.Points)
                    {
                        points.AddUniq(p);
                    }

                    NavCell leftCell = new NavCell();
                    NavCell bottomCell = new NavCell();

                    if (cellX > 1 && cellY >= 0)
                    {
                        leftCell = cellLines[cellY][cellX - 1];
                    }

                    if (cellX >= 1 && cellY > 0)
                    {
                        bottomCell = cellLines[cellY - 1][cellX];
                    }

                    line.Add(cell);
                    cell.MergeEdges(leftCell, bottomCell);
                    cellX++;
                }

                cellLines.Add(line);
                cellY++;
            }
        }

        protected NavCell CreateCell(NavPoint a, NavPoint b, NavPoint c, NavPoint d)
        {
            var cell = new NavCell(a, b, c, d);
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
