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

        [SerializeField]
        private bool randomizeExits;

        #endregion

        #region Properties

        public List<Transform> Exits { get => exits; }

        #endregion

        public void Initialize()
        {
            if (randomizeExits)
            {
                OffsetExits();
            }
        }

        private void OffsetExits()
        {

        }

        private void DragExit(Transform exit)
        {

        }

    }
}
