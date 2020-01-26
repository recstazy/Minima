using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.Navigation
{
    public class NavBuildManager : MonoBehaviour
    {
        #region Fields

        #endregion

        #region Properties

        public List<NavMeshBuilderBase> Builders { get; private set; } = new List<NavMeshBuilderBase>();

        #endregion

        public void BuildNavigation()
        {
            ExecuteNextFrame(() => BuildImmediatley());
        }

        public void AddBuilder(NavMeshBuilderBase builder)
        {
            Builders.Add(builder);
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
