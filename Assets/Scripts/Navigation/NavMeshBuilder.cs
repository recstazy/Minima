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
        protected NavCell[] cells = new NavCell[0];

        protected List<float> xAxes = new List<float>();
        protected List<float> yAxes = new List<float>();

        protected Transform thisTransform;

        private ChunkConnection leftConnection = new ChunkConnection(Vector2.left);
        private ChunkConnection rightConnection = new ChunkConnection(Vector2.right);
        private ChunkConnection topConnection = new ChunkConnection(Vector2.up);
        private ChunkConnection bottomConnection = new ChunkConnection(Vector2.down);

        private List<List<NavCell>> chunkConnections = new List<List<NavCell>>();
        private NavPoint[] points;

        #endregion

        #region Properties

        public int PointsCount
        {
            get
            {
                int count = 0;
                foreach (var c in cells)
                {
                    count += c.Points.Length;
                }

                return count;
            }
        }

        public NavPoint[] Points
        {
            get
            {
                if (points == null)
                {
                    points = new NavPoint[0];

                    foreach (var c in cells)
                    {
                        points = points.ConcatUniq(c.Points);
                    }
                }

                return points;
            }
        }

        private float Step { get => 1f / density; }

        protected ChunkConnection LeftConnection { get => leftConnection; set => leftConnection = value; }
        protected ChunkConnection RightConnection { get => rightConnection; set => rightConnection = value; }
        protected ChunkConnection TopConnection { get => topConnection; set => topConnection = value; }
        protected ChunkConnection BottomConnection { get => bottomConnection; set => bottomConnection = value; }

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

        public void CreateChunkConnections()
        {
            SetConnections();

            var connections = new ChunkConnection[] { RightConnection, LeftConnection, TopConnection, BottomConnection };

            foreach (var c in connections)
            {
                if (c.IsValid && !c.IsConnected)
                {
                    ConnectChunks(c);
                }
            }
        }

        public NavTriangle GetNearestTriangle(Vector2 point)
        {
            var cell = GetNearestCell(point);
            return cell.GetNearestTriangle(point);
        }

        public void AddCells(params NavCell[] cells)
        {
            this.cells = this.cells.Concat(cells).ToArray();
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
                var line = new NavPoint[0];

                foreach (var y in yAxes)
                {
                    var point = CreatePoint(origin + new Vector2(x, y));
                    line = line.ConcatOne(point);
                }

                pointLines = pointLines.ConcatOne(line);
            }
        }

        protected void CreateCells()
        {
            for (int i = 0; i < pointLines.Length - 1; i++)
            {
                var line = new List<NavCell>();

                for (int j = 0; j < pointLines[i].Length - 1; j++)
                {
                    var a = pointLines[i][j];
                    var b = pointLines[i][j + 1];
                    var c = pointLines[i + 1][j + 1];
                    var d = pointLines[i + 1][j];

                    var cell = CreateCell(a, b, c, d);

                    cells = cells.ConcatOne(cell);
                    line.Add(cell);
                }

                cellLines.Add(line);
            }
        }

        protected void ConnectChunks(ChunkConnection connection)
        {
            if (connection.Direction == Vector2.right)
            {
                ConnectChunksHorisontal(connection.Builder, this);
                connection.Builder.BottomConnection.SetConnected(true);
            }
            else if (connection.Direction == Vector2.left)
            {
                ConnectChunksHorisontal(this, connection.Builder);
                connection.Builder.TopConnection.SetConnected(true);
            }

            connection.IsConnected = true;
        }

        private void ConnectChunksHorisontal(NavMeshBuilder right, NavMeshBuilder left)
        {
            var rightCells = right.cellLines.First().ToArray();
            var leftCells = left.cellLines.Last().ToArray();

            var minIndex = Mathf.Min(rightCells.Length, leftCells.Length);
            float middle = minIndex / 2f;
            int centerIndex = Mathf.FloorToInt(middle);

            var line = new NavCell[0];

            for (int i = centerIndex; i >= 0; i--)
            {
                var rightCell = rightCells[i];
                var leftCell = leftCells[i];

                var cell = CreateCell(leftCell.D, leftCell.C, rightCell.A, rightCell.B);
                line = line.ConcatOne(cell);
            }

            for (int i = centerIndex; i < minIndex; i++)
            {
                var rightCell = rightCells[i];
                var leftCell = leftCells[i];

                var cell = CreateCell(leftCell.D, leftCell.C, rightCell.A, rightCell.B);
                line = line.ConcatOne(cell);
            }

            right.cellLines.Insert(0, line.ToList());
            left.cellLines.Add(line.ToList());
            right.AddCells(line);
            left.AddCells(line);
        }

        protected NavCell CreateCell(NavPoint a, NavPoint b, NavPoint c, NavPoint d)
        {
            var cell = new NavCell(a, b, c, d);
            return cell;
        }

        protected void DrawEdges()
        {
            foreach (var c in cells)
            {
                foreach (var e in c.Edges)
                {
                    Debug.DrawLine(e.Start.Position, e.End.Position, Color.red);
                }
            }

            foreach (var line in chunkConnections)
            {
                foreach (var c in line)
                {
                    foreach (var e in c.Edges)
                    {
                        Debug.DrawLine(e.Start.Position, e.End.Position, Color.blue);
                    }
                }
            }
        }

        private NavCell GetNearestCell(Vector2 point)
        {
            var cells = this.cells.Where(c => c.ActivationAvg >= 0.9f).ToArray();
            var comparer = new CellDistanceComparer(point);
            System.Array.Sort(cells, comparer);

            return cells.FirstOrDefault();
        }

        private IEnumerator DelayedBuild()
        {
            yield return new WaitForEndOfFrame();

            BuildNavMesh();
        }

        private void SetConnections()
        {
            if (!RightConnection.TriedConnect)
            {
                if (RaycastBuilder(ref rightConnection))
                {
                    RightConnection.Builder.LeftConnection.SetBuilder(this);
                    RightConnection.Builder.LeftConnection.SetTried(true);
                }

                RightConnection.SetTried(true);
            }

            if (!LeftConnection.TriedConnect)
            {
                if (RaycastBuilder(ref leftConnection))
                {
                    LeftConnection.Builder.RightConnection.SetBuilder(this);
                    LeftConnection.Builder.RightConnection.SetTried(true);
                }

                LeftConnection.SetTried(true);
            }

            if (!TopConnection.TriedConnect)
            {
                if (RaycastBuilder(ref topConnection))
                {
                    TopConnection.Builder.BottomConnection.SetBuilder(this);
                    TopConnection.Builder.BottomConnection.SetTried(true);
                }

                TopConnection.SetTried(true);
            }

            if (!BottomConnection.TriedConnect)
            {
                if (RaycastBuilder(ref bottomConnection))
                {
                    BottomConnection.Builder.TopConnection.SetBuilder(this);
                    BottomConnection.Builder.TopConnection.SetTried(true);
                }

                BottomConnection.SetTried(true);
            }
        }

        private bool RaycastBuilder(ref ChunkConnection connection)
        {
            var filter = new ContactFilter2D();
            filter.useTriggers = true;
            filter.SetLayerMask(LayerMask.GetMask("Navigation"));
            var result = new List<RaycastHit2D>();
            Physics2D.Raycast(thisTransform.position, connection.Direction, filter, result, buildArea.bounds.size.x);

            result.RemoveAll(r => r.collider == buildArea);

            if (result.Count > 0)
            {
                connection.Builder = result.First().collider.GetComponentInParent<NavMeshBuilder>();
                return true;
            }

            return false;
        }
    }
}
