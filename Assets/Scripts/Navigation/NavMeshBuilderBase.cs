using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Minima.Navigation
{
    public class NavMeshBuilderBase : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private bool showDebug = false;

        [SerializeField]
        private bool showPoints = false;

        protected NavPoint[] points;

        #endregion

        #region Properties

        public bool ShowPoints { get => showPoints; set => showPoints = value; }
        public bool ShowDebug { get => showDebug; set => showDebug = value; }
        public NavPoint[] Points { get => GetPoints(); }

        #endregion

        protected virtual void Update()
        {
            if (ShowDebug)
            {
                DrawEdges();
            }
        }

        public void BuildNavMesh()
        {
            var startTime = DateTime.Now;

            ExecuteNextFrame(() => BuildNavMeshImmediately());

            Debug.Log("MeshBuilding took " + (DateTime.Now - startTime));
        }

        public virtual void BuildNavMeshImmediately()
        {
        }

        public virtual NavPoint[] GetPoints()
        {
            return points;
        }

        public virtual int GetPointsCount()
        {
            return points.Length;
        }

        public virtual NavPoint GetNearestPoint(Vector2 position)
        {
            return GetNearestPoint(position, points);
        }

        protected NavPoint GetNearestPoint(Vector2 position, NavPoint[] searchField)
        {
            return searchField
                .Aggregate((p, next) =>
                Vector2.Distance(position, p.Position) < Vector2.Distance(position, next.Position) ? p : next);
        }

        protected virtual void DrawEdges()
        {
            foreach (var p in points)
            {
                foreach (var connections in points.Select(point => point.ConnectedEdges))
                {
                    foreach (var c in connections)
                    {
                        Debug.DrawLine(c.Start.Position, c.End.Position, Color.blue);
                    }
                }
            }
        }

        protected void ExecuteNextFrame(System.Action method)
        {
            StartCoroutine(WaitNextFrameAndExecute(method));
        }

        private IEnumerator WaitNextFrameAndExecute(System.Action method)
        {
            yield return new WaitForEndOfFrame();
            method?.Invoke();
        }
    }
}
