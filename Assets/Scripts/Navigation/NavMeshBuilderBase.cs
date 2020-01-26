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
        protected List<Collider2D> obstacles = new List<Collider2D>();

        protected List<NavTriangle> triangles = new List<NavTriangle>();
        protected List<List<NavPoint>> pointLines = new List<List<NavPoint>>();
        

        #endregion

        #region Properties

        #endregion

        public virtual void BuildNavMesh()
        {
        }

        public bool IsPointInBounds(Vector2 point)
        {
            return buildArea.OverlapPoint(point);
        }

        public NavTriangle GetContainingTriangle(Vector2 point)
        {
            var trianglesList = triangles.ToList();
            var comparer = new TriangleDistanceComparer(point);
            trianglesList.Sort(comparer);

            return trianglesList[0];
        }

        protected NavPoint CreatePoint(Vector2 position, bool instantiatePoint = false)
        {
            var point = new NavPoint(position);
            point.Activated = CheckPointActivation(point);
            points.Add(point);

            if (instantiatePoint)
            {
                InstantiatePoint(point);
            }

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

        protected virtual void GetAllObstacles()
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

        protected void InstantiatePoint(NavPoint point)
        {
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

            var sprite = pointGO.GetComponent<SpriteRenderer>();
            point.Sprite = sprite;
            point.Sprite.color = color;
        }
    }
}
