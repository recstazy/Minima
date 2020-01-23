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

        [SerializeField]
        protected float boundOffset = 0.15f;

        protected List<NavPoint> points = new List<NavPoint>();
        protected List<NavEdge> edges = new List<NavEdge>();
        protected List<Collider2D> obstacles = new List<Collider2D>();

        #endregion

        #region Properties

        #endregion

        public virtual void BuildNavMesh()
        {
        }

        protected NavPoint CreatePoint(Vector2 position)
        {
            var point = new NavPoint(position);
            point.Activated = CheckPointActivation(point);
            points.Add(point);
            var pointGO = Instantiate(pointPrefab, point.Position, Quaternion.identity, this.transform);

            Color color;

            if (point.Activated)
            {
                color = Color.white;
            }
            else
            {
                color = Color.black;
            }

            pointGO.GetComponent<SpriteRenderer>().color = color;

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

            edges.Add(edge);
            a.ConnectedEdges.Add(edge);
            b.ConnectedEdges.Add(edge);

            return edge;
        }

        protected List<NavPoint> GetClosestPoints(NavPoint origin, int count, params NavPoint[] except)
        {
            return GetClosestPoints(points, origin, count, except);
        }

        protected List<NavPoint> GetClosestPoints(List<NavPoint> sourcePoints, NavPoint origin, int count, params NavPoint[] except)
        {
            var distanceComparer = new NavPointDistanceComparer(origin);
            var pointsTemp = sourcePoints.ToList();

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

        protected NavPoint GetClosestPoint(NavEdge edge)
        {
            var closest = GetClosestPoints(edge.Start, 3, edge.End);

            var comparer = new EdgeDistanceComparer(edge);

            closest.Sort(comparer);

            return closest[0];
        }

        protected void GetAllObstacles()
        {
            if (obstacles.Count() == 0)
            {
                var filter = new ContactFilter2D();
                filter.useLayerMask = true;
                filter.SetLayerMask(LayerMask.GetMask("Obstacles"));

                buildArea.OverlapCollider(filter, obstacles);
            }
        }

        protected bool CheckPointActivation(NavPoint point)
        {
            foreach (var o in obstacles)
            {
                bool overlap = o.OverlapPoint(point.Position);

                if (overlap)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
