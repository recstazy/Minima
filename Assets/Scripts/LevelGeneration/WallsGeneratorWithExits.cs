using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.LevelGeneration
{
    public class WallsGeneratorWithExits : WallsGenerator
    {
        #region Fields

        protected List<Transform> cornerPoints = new List<Transform>();

        #endregion

        #region Properties

        #endregion

        public override void Initialize(RoomDraft roomDraft)
        {
            base.Initialize(roomDraft);
            cornerPoints = roomDraft.Corners.Corners.Concat(roomDraft.Exits.Exits).ToList();
        }

        public override void CreateWalls()
        {
            ConnectCorners(cornerPoints);
        }
    }
}
