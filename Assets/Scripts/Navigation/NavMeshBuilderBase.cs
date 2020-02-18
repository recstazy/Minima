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
        private bool showDebug = false;

        [SerializeField]
        private bool showPoints = false;

        protected List<Collider2D> obstacles = new List<Collider2D>();
        protected NavPoint[][] pointLines = new NavPoint[0][];

        #endregion

        #region Properties

        public bool ShowPoints { get => showPoints; set => showPoints = value; }
        public bool ShowDebug { get => showDebug; set => showDebug = value; }

        #endregion

        public virtual void BuildNavMesh()
        {
        }

        public bool IsPointInBounds(Vector2 point)
        {
            return buildArea.OverlapPoint(point);
        }

        protected NavPoint CreatePoint(Vector2 position)
        {
            var point = new NavPoint(position);
            point.Activated = CheckPointActivation(point);

            if (ShowPoints)
            {
                InstantiatePoint(point);
            }

            return point;
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

            var view = pointGO.GetComponent<NavPointView>();
            point.View = view;
            point.View.SetColor(color);
        }
    }
}
