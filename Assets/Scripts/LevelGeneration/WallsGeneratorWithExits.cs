using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.LevelGeneration
{
    public class WallsGeneratorWithExits : WallsGenerator
    {
        #region Fields

        protected List<WallCorner> cornerPoints = new List<WallCorner>();

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

        protected override bool CustomConnectionPredicate(WallCorner cornerA, WallCorner cornerB)
        {
            if (cornerA.IsExit && cornerB.IsExit)
            {
                return false;
            }

            return true;
        }
    }
}
