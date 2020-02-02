using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.Navigation
{
    public class NavAgent : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private bool showDebug = false;

        private NavPathFinder finder;
        private Transform thisTransform;
        private NavPath path;

        #endregion

        #region Properties

        private NavPathFinder Finder
        {
            get
            {
                if (finder == null)
                {
                    finder = FindObjectOfType<NavPathFinder>();
                }

                return finder;
            }
        }

        #endregion

        private void Awake()
        {
            thisTransform = transform;
        }

        public NavPath GetPath(Vector2 target)
        {
            path = Finder.FindPath(thisTransform.position, target);
            return path;
        }

        private void Update()
        {
            if (showDebug && path.IsValid)
            {
                for (int i = 0; i < path.NavPoints.Count - 1; i++)
                {
                    Debug.DrawLine(path.Points[i], path.Points[i + 1], Color.green, Time.deltaTime);
                }
            }
        }
    }
}
