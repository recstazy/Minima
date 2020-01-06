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
            cornerPoints = roomDraft.Corners.Concat(roomDraft.Exits).ToList();
        }

        public override void CreateWalls()
        {
            ConnectCorners(cornerPoints);
        }

        protected override void ConnectCorners(List<WallCorner> corners)
        {
            foreach (var c in cornerPoints)
            {
                CreateWallBetweenPoints(c, c.NextCorner);
            }
        }
    }
}
