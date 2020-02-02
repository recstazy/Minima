using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

        // Trying to implement A*
        public NavPath FindPath(Vector2 origin, Vector2 target)
        {
            var path = new NavPath(origin);
            var originBuilder = FindBuilder(origin);
            var targetBuilder = FindBuilder(target);

            if (originBuilder == null)
            {
                return path;
            }

            var startPoint = originBuilder.GetNearestTriangle(origin).ClosestVertex(target);
            var endPoint = targetBuilder.GetNearestTriangle(target).ClosestVertex(origin);

            path.Add(startPoint);
            AddNextPoints(ref path, startPoint, endPoint);
            path.Add(target);

            return path;
        }

        private void AddNextPoints(ref NavPath path, NavPoint point, NavPoint target)
        {
            if (path.NavPoints.Count > 100)
            {
                Debug.Log("Path length > 100");
                return;
            }

            if (point == target)
            {
                return;
            }

            var nextPoint = point.ClosestVertex(target.Position, path.NavPoints.ToArray());

            if (!nextPoint.IsValid)
            {
                return;
            }

            path.Add(nextPoint);
            AddNextPoints(ref path, nextPoint, target);
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
