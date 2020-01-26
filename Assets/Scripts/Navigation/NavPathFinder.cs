using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.Navigation
{
    public class NavPathFinder : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private NavBuildManager buildManager;

        #endregion

        #region Properties

        #endregion

        public NavPath FindPath(Vector2 origin, Vector2 target)
        {
            var path = new NavPath();
            var builder = FindBuilder(origin);

            var lastTriangle = builder.GetContainingTriangle(origin);
            var triangle = lastTriangle.GetClosestConnection(origin);
            bool isVisible = false;

            do
            {
                triangle = triangle.GetClosestConnection(origin, lastTriangle);
                var center = StaticHelpers.GetTriangleCenter(triangle.A.Position, triangle.B.Position, triangle.C.Position);
                isVisible = StaticHelpers.CheckVisibility(center, origin);
                path.Points.Add(center);
            }
            while (!isVisible);

            return path;
        }

        private NavMeshBuilderBase FindBuilder(Vector2 point)
        {
            foreach (var b in buildManager.Builders)
            {
                if (b.IsPointInBounds(point))
                {
                    return b;
                }
            }

            return null;
        }
    }
}
