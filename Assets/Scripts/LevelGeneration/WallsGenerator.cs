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

        private RoomDraft roomDraft;
        private Dictionary<Transform, Transform> cornerPairs = new Dictionary<Transform, Transform>();

        #endregion

        public void CreateWalls(RoomDraft roomDraft)
        {
            this.roomDraft = roomDraft;
            ConnectCorners();
        }

        private GameObject InstantiateWall(Vector2 position)
        {
            var wall = Instantiate(wallPrefab, position, Quaternion.identity, transform);
            return wall;
        }

        private void ConnectCorners()
        {
            FindCornerPairs();

            foreach(var p in cornerPairs)
            {
                CreateWallBetweenCorners(p.Key, p.Value);
            }
        }

        private void FindCornerPairs()
        {
            foreach (var c in roomDraft.Corners.Corners)
            {
                var nearestCorner = GetNearestCorner(c);

                if (nearestCorner != null)
                {
                    cornerPairs.Add(c, nearestCorner);
                }
            }
        }

        private Transform GetNearestCorner(Transform corner)
        {
            float minDistance = 0f;
            Transform nearestCorner = null;

            foreach (var c in roomDraft.Corners.Corners)
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

        private bool CornerPairExists(Transform cornerA, Transform cornerB)
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

        private void CreateWallBetweenCorners(Transform cornerA, Transform cornerB)
        {
            var position = cornerA.position + ((cornerB.position - cornerA.position) / 2);

            InstantiateWall(position);
        }
    }
}
