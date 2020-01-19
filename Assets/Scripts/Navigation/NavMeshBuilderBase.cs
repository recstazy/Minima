using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.Navigation
{
    public class NavMeshBuilderBase : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private GameObject pointPrefab;

        [SerializeField]
        protected Collider2D buildArea;

        [SerializeField]
        protected bool showDebug = false;

        protected List<NavPoint> points = new List<NavPoint>();
        protected List<NavEdge> edges = new List<NavEdge>();

        #endregion

        #region Properties

        #endregion

        public virtual void BuildNavMesh()
        {
        }

        protected void CreateObstaclesBounds()
        {
            var obstacles = GetAllObstacles();

            foreach (var o in obstacles)
            {
                CreateBounds(o, 0.15f, true);
            }
        }

        protected void CreateBounds(Collider2D collider, float addToExtents = 0f, bool createEdges = false)
        {
            float x = collider.bounds.extents.x + addToExtents;
            float y = collider.bounds.extents.y + addToExtents;

            var a = CreatePoint(collider.bounds.center + new Vector3(x, y));
            var b = CreatePoint(collider.bounds.center + new Vector3(-x, y));
            var c = CreatePoint(collider.bounds.center + new Vector3(-x, -y));
            var d = CreatePoint(collider.bounds.center + new Vector3(x, -y));

            if (createEdges)
            {
                CreateEdge(a, b);
                CreateEdge(b, c);
                CreateEdge(c, d);
                CreateEdge(d, a);
            }
        }

        protected NavPoint CreatePoint(Vector2 position)
        {
            var clampedPosition = ClampToBounds(position);
            var point = new NavPoint(clampedPosition);
            points.Add(point);
            Instantiate(pointPrefab, point.Position, Quaternion.identity, this.transform);
            return point;
        }

        protected NavEdge CreateEdge(NavPoint a, NavPoint b, out bool success)
        {
            var edge = CreateEdge(a, b);
            success = edge.IsValid;

            return edge;
        }

        protected NavEdge CreateEdge(NavPoint a, NavPoint b)
        {
            var edge = new NavEdge(a, b);

            var hits = new List<RaycastHit2D>();
            bool visible = StaticHelpers.CheckVisibility(a.Position, b.Position, out hits);

            if (!visible)
            {
                return new NavEdge();
            }

            var except = a.ConnectedEdges.Concat(b.ConnectedEdges).ToList();
            var checkedEdges = edges.Except(except).ToList();

            foreach (var e in checkedEdges)
            {
                if (edge.Intersects(e))
                {
                    return new NavEdge();
                }
            }

            edges.Add(edge);
            a.ConnectedEdges.Add(edge);
            b.ConnectedEdges.Add(edge);

            return edge;
        }

        protected Vector2 ClampToBounds(Vector2 point)
        {
            Vector2 bounds = buildArea.bounds.center + buildArea.bounds.extents;

            if (Mathf.Abs(point.x) > bounds.x)
            {
                point.x = bounds.x * (point.x / Mathf.Abs(point.x)); // multiply to (1 or -1)
            }
            
            if (Mathf.Abs(point.y) > bounds.y)
            {
                point.y = bounds.y * (point.y / Mathf.Abs(point.y));
            }

            return point;
        }

        protected List<NavPoint> GetClosestPoints(NavPoint origin, int count, params NavPoint[] except)
        {
            var distanceComparer = new NavPointDistanceComparer(origin);
            var pointsTemp = points.ToList();

            pointsTemp.Remove(origin);
            pointsTemp = pointsTemp.Except(except).ToList();
            pointsTemp.Sort(distanceComparer);

            var toRemove = new List<NavPoint>();

            foreach (var p in pointsTemp)
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

        protected List<Collider2D> GetAllObstacles()
        {
            List<Collider2D> obstacles = new List<Collider2D>();

            var filter = new ContactFilter2D();
            filter.useLayerMask = true;
            filter.SetLayerMask(LayerMask.GetMask("Obstacles"));

            buildArea.OverlapCollider(filter, obstacles);

            return obstacles;
        }
    }
}
