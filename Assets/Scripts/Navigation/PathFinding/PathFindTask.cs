using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

namespace Minima.Navigation
{
    public class PathFindTask
    {
        public delegate void PathFindTaskConplete(PathFindTask task);
        public event PathFindTaskConplete OnComplete;

        #region Properties

        public Vector2 Origin { get; set; }
        public Vector2 Target { get; set; }

        public PathFoundHandler Callback { get; set; }
        public NavPathFinder Finder { get; set; }

        #endregion

        public PathFindTask(Vector2 origin, Vector2 target, PathFoundHandler callback, NavPathFinder finder)
        {
            Origin = origin;
            Target = target;
            Callback = callback;
            Finder = finder;
        }

        public async void Run()
        {
            var path = await Task.Run(() => Finder.FindPath(Origin, Target));
            Callback?.Invoke(path);
            OnComplete?.Invoke(this);
        }
    }
}
