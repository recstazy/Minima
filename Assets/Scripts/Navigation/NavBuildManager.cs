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

        #endregion

        #region Properties

        public List<NavMeshBuilder> Builders { get; private set; } = new List<NavMeshBuilder>();

        #endregion

        public void BuildNavigation()
        {
            ExecuteNextFrame(() => BuildImmediatley());
        }

        public void AddBuilder(NavMeshBuilder builder)
        {
            Builders.Add(builder);
            builder.ShowPoints = showPoints;
            builder.ShowDebug = showDebugMesh;
        }

        protected void BuildImmediatley()
        {
            DateTime startTime = DateTime.Now;

            foreach (var b in Builders)
            {
                b.BuildNavMesh();
            }

            TimeSpan timeElapsed = DateTime.Now - startTime;
            Debug.Log("NavMesh building took " + timeElapsed);
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
