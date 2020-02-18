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

        [SerializeField]
        private NavMeshBuilder navBuilder;

        [SerializeField]
        private LineRenderer lineRenderer;

        #endregion

        #region Properties

        #endregion

        /// <summary>
        /// Find path from origin to target
        /// </summary>
        public NavPath FindPath(Vector2 origin, Vector2 target)
        {
            var path = new NavPath(origin);

            var containingChunk = navBuilder;

            if (containingChunk != null)
            {
                var startPoint = containingChunk.GetNearestTriangle(origin).ClosestVertex(target);
                var endPoint = containingChunk.GetNearestTriangle(target).ClosestVertex(origin);

                IPathFindAlgorithm dijkstra = new Dijkstra(containingChunk.Points);
                path += dijkstra.FindPath(startPoint, endPoint);
            }

            path.Add(target);

            RenderLinesBetweenPoints(path.Points);

            return path;
        }

        private void RenderLinesBetweenPoints(Vector2[] points)
        {
            var points3 = new Vector3[points.Length];

            for (int i = 0; i < points3.Length; i++)
            {
                points3[i] = points[i].ToVector3();
            }

            lineRenderer.positionCount = points3.Length;
            lineRenderer.SetPositions(points3);
        }

        private NavPath FindPathInRoom(NavMeshBuilder builder, Vector2 origin, Vector2 target)
        {
            var path = new NavPath(origin);

            return path;
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
