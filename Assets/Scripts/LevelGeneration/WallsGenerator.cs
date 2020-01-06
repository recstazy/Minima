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
            ConnectCorners(roomDraft.Corners.Corners);
        }

        protected GameObject InstantiateWall(Vector2 position)
        {
            var wall = Instantiate(wallPrefab, position, Quaternion.identity, transform);
            return wall;
        }

        protected void ConnectCorners(List<WallCorner> corners)
        {
            SortCorners(corners, corners[0]);

            foreach(var p in cornerPairs)
            {
                CreateWallBetweenPoints(p.Key, p.Value);
            }
        }

        protected WallCorner GetNearestCorner(WallCorner corner, List<WallCorner> corners)
        {
            float minDistance = 0f;
            WallCorner nearestCorner = null;

            foreach (var c in corners)
            {
                if (!CustomConnectionPredicate(c, corner))
                {
                    continue;
                }

                if (c != corner && !CornerPairExists(c, corner))
                {
                    var distance = Vector2.Distance(c.position, corner.position);

                    if (distance < minDistance || nearestCorner == null)
                    {
                        minDistance = distance;
                        nearestCorner = c;
                    }
                }
            }

            return nearestCorner;
        }

        protected virtual bool CustomConnectionPredicate(WallCorner cornerA, WallCorner cornerB)
        {
            return true;
        }

        protected bool CornerPairExists(WallCorner cornerA, WallCorner cornerB)
        {
            if (cornerPairs.ContainsKey(cornerA))
            {
                if (cornerPairs[cornerA] == cornerB)
                {
                    return true;
                }
            }
            else if (cornerPairs.ContainsKey(cornerB))
            {
                if (cornerPairs[cornerB] == cornerA)
                {
                    return true;
                }
            }

            return false;
        }

        protected void CreateWallBetweenPoints(WallCorner pointA, WallCorner pointB)
        {
            var position = pointA.position + ((pointB.position - pointA.position) / 2);
            var wall = InstantiateWall(position);

            var wallScale = new Vector2(Vector2.Distance(pointA.position, pointB.position), 1f);
            wall.transform.localScale = wallScale;
            wall.transform.right = (pointB.position - wall.transform.position).normalized;
        }

        private void SortCorners(List<WallCorner> corners, WallCorner startCorner)
        {
            if (startCorner.NearestCorner != null)
            {
                return;
            }

            startCorner.NearestCorner = GetNearestCorner(startCorner, corners);
            cornerPairs.Add(startCorner, startCorner.NearestCorner);
            cornersSorted.Add(startCorner.NearestCorner);
            SortCorners(corners, startCorner.NearestCorner);
        }
    }
}
