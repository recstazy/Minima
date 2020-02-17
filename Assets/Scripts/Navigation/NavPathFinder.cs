using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

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
            var path = new NavPath(origin);
            var originBuilder = FindBuilder(origin);
            var targetBuilder = FindBuilder(target);

            if (originBuilder == null)
            {
                return path;
            }

            path = FindPathInRoom(originBuilder, origin, target);

            if (originBuilder != targetBuilder)
            {
                path.Add(target);
            }
            
            return path;
        }

        private NavPath FindPathInRoom(NavMeshBuilder builder, Vector2 origin, Vector2 target)
        {
            var path = new NavPath(origin);

            var startPoint = builder.GetNearestTriangle(origin).ClosestVertex(target);
            var endPoint = builder.GetNearestTriangle(target).ClosestVertex(origin);

            path.Add(startPoint);
            AddNextPoints(ref path, startPoint, endPoint);
            path.Add(target);

            return path;
        }

        private void AddNextPoints(ref NavPath path, NavPoint point, NavPoint target)
        {
            if (path.Length > 25)
            {
                Debug.Log("Path length > 25");
                return;
            }

            if (point == target)
            {
                return;
            }

            var nextPoint = point.ClosestVertex(target.Position, path.NavPoints.Concat(path.Except).ToArray());

            if (!nextPoint.IsValid)
            {
                path.Exclude(point);

                if (path.Length != 0)
                {
                    AddNextPoints(ref path, path.NavPoints.LastOrDefault(), target);
                }
                
                return;
            }

            if (!Helpers.CheckVisibility(point.Position, nextPoint.Position))
            {
                path.Exclude(nextPoint);

                if (path.Length != 0)
                {
                    AddNextPoints(ref path, path.NavPoints.LastOrDefault(), target);
                }

                return;
            }

            path.Add(nextPoint);
            AddNextPoints(ref path, nextPoint, target);
        }

        private NavMeshBuilder FindBuilder(Vector2 point)
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

        private void FindBuilderPath(ref NavPath path, NavMeshBuilder startBuilder, NavMeshBuilder targetBuilder)
        {
            var builders = buildManager.Builders.Except(path.Builders).ToArray();
            var comparer = new NavBuilderCostComparer(startBuilder.transform.position, targetBuilder.transform.position);
            Array.Sort(builders, comparer);

            var nextBuilder = builders.LastOrDefault();

            if (nextBuilder == null)
            {
                Debug.LogError("First builder in path find is null");
                return;
            }

            path.AddBuilder(nextBuilder);

            if (nextBuilder == targetBuilder)
            {
                return;
            }

            FindBuilderPath(ref path, nextBuilder, targetBuilder);
        }
    }
}
