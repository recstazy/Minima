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

        private PathFindManager finder;
        private NavMeshBuilderBase navMeshBuilder;
        private Transform thisTransform;
        private NavPath path;
        private PathFoundHandler ownerCallback;

        #endregion

        #region Properties

        #endregion

        private void Awake()
        {
            navMeshBuilder = FindObjectOfType<NavMeshBuilderBase>();
            finder = FindObjectOfType<PathFindManager>();
            thisTransform = transform;
        }

        public void GetPath(Vector2 target, PathFoundHandler callback)
        {
            ownerCallback = callback;
            finder.FindPath(thisTransform.position, target, PathFoundCallBack);
        }

        private void PathFoundCallBack(NavPath path)
        {
            this.path = path;
            ownerCallback?.Invoke(path);
        }

        private void Update()
        {
            if (showDebug && path.IsValid)
            {
                for (int i = 0; i < path.Length - 1; i++)
                {
                    var color = Color.Lerp(Color.green, Color.red, i / (float)10f);
                    Debug.DrawLine(path.Points[i], path.Points[i + 1], color, Time.deltaTime);
                }

                for (int i = 0; i < path.Except.Length - 1; i++)
                {
                    Debug.DrawLine(path.Except[i].Position, path.Except[i + 1].Position, Color.white, Time.deltaTime);
                }
            }
        }
    }
}
