using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace Minima.Navigation
{
    public delegate void PathFoundHandler(NavPath path);

    public class NavPathFinder
    {
        #region Fields

        private NavBuildManager buildManager;
        private static Queue<Task> tasks = new Queue<Task>();

        #endregion

        #region Properties

        #endregion

        /// <summary>
        /// Provide the build manager to give finder navigation info
        /// </summary>
        public NavPathFinder(NavBuildManager buildManager)
        {
            this.buildManager = buildManager;
        }

        public async void FindPathAsync(Vector2 origin, Vector2 target, PathFoundHandler callback)
        {
            await Task.Run(() =>
            {
                var path = FindPath(origin, target);
                callback?.Invoke(path);
            });
        }

        /// <summary>
        /// Find path from origin to target
        /// </summary>
        public NavPath FindPath(Vector2 origin, Vector2 target)
        {
            var path = new NavPath(origin);

            var containingChunk = FindBuilder(origin);

            if (containingChunk != null)
            {
                var startPoint = containingChunk.GetNearestTriangle(origin).ClosestVertex(target);
                var endPoint = containingChunk.GetNearestTriangle(target).ClosestVertex(origin);

                IPathFindAlgorithm dijkstra = new Dijkstra(containingChunk.Points);
                path += dijkstra.FindPath(startPoint, endPoint);
            }

            path.Add(target);

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
    }
}
