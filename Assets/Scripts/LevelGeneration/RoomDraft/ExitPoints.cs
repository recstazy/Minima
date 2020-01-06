using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.LevelGeneration
{
    public class ExitPoints : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private List<Transform> exits = new List<Transform>();

        #endregion

        #region Properties

        public List<Transform> Exits { get => exits; }

        #endregion
    }
}
