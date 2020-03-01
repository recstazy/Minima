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

        private NavMeshBuilderBase navBuilder;

        #endregion

        #region Properties

        #endregion

        /// <summary>
        /// Provide the build manager to give finder navigation info
        /// </summary>
        public NavPathFinder(NavMeshBuilderBase navMeshBuider)
        {
            navBuilder = navMeshBuider;
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

            if (navBuilder != null)
            {
                var startPoint = navBuilder.GetNearestPoint(origin);
                var endPoint = navBuilder.GetNearestPoint(target);

                IPathFindAlgorithm dijkstra = new Dijkstra(navBuilder.Points);
                path += dijkstra.FindPath(startPoint, endPoint);
            }

            path.Add(target);

            return path;
        }
    }
}
