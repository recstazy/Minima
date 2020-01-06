using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.LevelGeneration
{
    public class CornerPoints : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private List<Transform> corners = new List<Transform>();

        #endregion

        #region Properties

        public List<Transform> Corners { get => corners; }

        #endregion
    }
}
