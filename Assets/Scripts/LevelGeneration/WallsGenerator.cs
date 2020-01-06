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
        protected Dictionary<WallCorner, WallCorner> cornerPairs = new Dictionary<WallCorner, WallCorner>();
        private List<WallCorner> cornersSorted = new List<WallCorner>();

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

        protected void CreateWallBetweenPoints(WallCorner pointA, WallCorner pointB)
        {
            var position = pointA.position + ((pointB.position - pointA.position) / 2);
            var wall = InstantiateWall(position);

            var wallScale = new Vector2(Vector2.Distance(pointA.position, pointB.position), 1f);
            wall.transform.localScale = wallScale;
            wall.transform.right = (pointB.position - wall.transform.position).normalized;
        }

    }
}
