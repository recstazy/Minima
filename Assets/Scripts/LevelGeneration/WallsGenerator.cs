using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Minima.LevelGeneration
{
    public class WallsGenerator : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private GameObject wallPrefab;

        protected RoomDraft roomDraft;

        #endregion

        public virtual void Initialize(RoomDraft roomDraft)
        {
            this.roomDraft = roomDraft;
        }

        public virtual void CreateWalls()
        {
            ConnectCorners(roomDraft.Corners);
        }

        protected GameObject InstantiateWall(Vector2 position)
        {
            var wall = Instantiate(wallPrefab, position, Quaternion.identity, transform);
            return wall;
        }

        protected virtual void ConnectCorners(List<WallCorner> corners)
        {
        }

        protected virtual void CreateWallBetweenCorners(WallCorner cornerA, WallCorner cornerB)
        {
            var position = cornerA.position + ((cornerB.position - cornerA.position) / 2);
            var wall = InstantiateWall(position);

            var wallScale = new Vector2(Vector2.Distance(cornerA.position, cornerB.position), 1f);
            wall.transform.localScale = wallScale;
            wall.transform.right = (cornerB.position - wall.transform.position).normalized;
        }

    }
}
