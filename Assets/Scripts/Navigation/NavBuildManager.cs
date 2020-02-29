using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.Navigation
{
    public class NavBuildManager : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private bool showPoints = false;

        [SerializeField]
        private bool showDebugMesh = false;

        [SerializeField]
        private int pointsPerObject = 2;

        [SerializeField]
        private float collapsePointsTreshold = 0.3f;

        [SerializeField]
        private NavMeshBuilder builder;

        #endregion

        #region Properties

        public List<NavMeshBuilder> Builders { get; private set; } = new List<NavMeshBuilder>();

        #endregion

        public void BuildNavigation()
        {
            Builders.Add(builder);
            ExecuteNextFrame(() => BuildImmediatley());
        }

        public void AddBuilder(NavMeshBuilder builder)
        {
            //Builders.Add(builder);
            //builder.ShowPoints = showPoints;
            //builder.ShowDebug = showDebugMesh;
            
            //if (builder is NavBuilderDynamicAxes)
            //{
            //    var b = builder as NavBuilderDynamicAxes;
            //    b.CollapsePointsTreshhold = collapsePointsTreshold;
            //    b.PointsPerObject = pointsPerObject;
            //}
        }

        public void CreateChunkConnections()
        {
            foreach (var b in Builders)
            {
                b.CreateChunkConnections();
            }
        }

        protected void BuildImmediatley()
        {
            DateTime startTime = DateTime.Now;

            //foreach (var b in Builders)
            //{
            //    b.BuildNavMesh();
            //}

            //CreateChunkConnections();

            builder.BuildNavMesh();

            TimeSpan timeElapsed = DateTime.Now - startTime;

            //int pointsCount = 0;
            //foreach (var b in Builders)
            //{
            //    pointsCount += b.PointsCount;
            //}

            int pointsCount = builder.GetPointsCount();

            Debug.Log("NavMesh building took " + timeElapsed + ", NavPoints count = " + pointsCount);
        }

        protected void ExecuteNextFrame(Action method)
        {
            StartCoroutine(WaitNextFrameAndExecute(method));
        }

        private IEnumerator WaitNextFrameAndExecute(Action method)
        {
            yield return new WaitForEndOfFrame();
            method?.Invoke();
        }
    }
}
