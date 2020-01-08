using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.LevelGeneration
{
    public class WallsGeneratorWithExits : WallsGenerator
    {
        #region Fields

        #endregion

        #region Properties

        protected List<WallCorner> CornerPoints
        {
            get
            {
                return roomDraft.Corners.Concat(roomDraft.Exits).ToList();
            }
        }

        #endregion

        public override void Initialize(RoomDraft roomDraft)
        {
            base.Initialize(roomDraft);
        }

        public override void CreateWalls()
        {
            ConnectCorners(CornerPoints);
        }

        protected override void ConnectCorners(List<WallCorner> corners)
        {
            foreach (var c in corners)
            {
                CreateWallBetweenCorners(c, c.NextCorner);
            }
        }
    }
}
