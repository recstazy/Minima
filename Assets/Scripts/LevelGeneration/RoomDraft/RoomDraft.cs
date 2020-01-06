using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.LevelGeneration
{
    public class RoomDraft : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private CornerPoints corners;

        [SerializeField]
        private ExitPoints exits;

        [SerializeField]
        private WallsGenerator wallsGenerator;

        #endregion

        #region Properties

        public CornerPoints Corners { get => corners; }
        public ExitPoints Exits { get => exits; }
        public WallsGenerator WallsGenerator { get => wallsGenerator; }

        #endregion

    }
}
