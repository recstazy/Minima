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
                CreateBounds(o);
            }
        }

        protected void CreateBounds(Collider2D collider)
        {
            float x = collider.bounds.extents.x;
            float y = collider.bounds.extents.y;

            CreatePoint(collider.bounds.center + new Vector3(x, y));
            CreatePoint(collider.bounds.center + new Vector3(-x, y));
            CreatePoint(collider.bounds.center + new Vector3(x, -y));
            CreatePoint(collider.bounds.center + new Vector3(-x, -y));
        }

        protected NavPoint CreatePoint(Vector2 position)
        {
            var clampedPosition = ClampToBounds(position);
            var point = new NavPoint(clampedPosition);
            points.Add(point);
            Instantiate(pointPrefab, point.Position, Quaternion.identity, this.transform);
            return point;
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
