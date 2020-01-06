using System.Collections;
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
        protected Dictionary<Transform, Transform> cornerPairs = new Dictionary<Transform, Transform>();

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

        protected void ConnectCorners(List<Transform> corners)
        {
            FindCornerPairs(corners);

            foreach(var p in cornerPairs)
            {
                CreateWallBetweenPoints(p.Key, p.Value);
            }
        }

        protected void FindCornerPairs(List<Transform> corners)
        {
            foreach (var c in corners)
            {
                var nearestCorner = GetNearestCorner(c, corners);

                if (nearestCorner != null)
                {
                    cornerPairs.Add(c, nearestCorner);
                }
            }
        }

        protected Transform GetNearestCorner(Transform corner, List<Transform> corners)
        {
            float minDistance = 0f;
            Transform nearestCorner = null;

            foreach (var c in corners)
            {
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

        protected bool CornerPairExists(Transform cornerA, Transform cornerB)
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

        protected void CreateWallBetweenPoints(Transform pointA, Transform pointB)
        {
            var position = pointA.position + ((pointB.position - pointA.position) / 2);
            var wall = InstantiateWall(position);

            var wallScale = new Vector2(Vector2.Distance(pointA.position, pointB.position), 1f);
            wall.transform.localScale = wallScale;
            wall.transform.right = (pointB.position - wall.transform.position).normalized;
        }
    }
}
