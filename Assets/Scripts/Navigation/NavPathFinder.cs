using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.Navigation
{
    public class NavPathFinder : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private NavBuildManager buildManager;

        #endregion

        #region Properties

        #endregion

        public NavPath FindPath(Vector2 origin, Vector2 target)
        {
            return new NavPath();
        }
    }
}
